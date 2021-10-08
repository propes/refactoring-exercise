using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RefactoringExercise.DataAccess.Repositories;
using RefactoringExercise.DataAccess.Repositories.National;
using RefactoringExercise.Domain;
using RefactoringExercise.Models;
using RefactoringExercise.Models.PatientMessaging;
using RefactoringExercise.Services.Extensions;

namespace RefactoringExercise.Services.PatientMessaging
{
	public interface IWelcomeMessagesService
	{
		Task SendWelcomeMessages(List<EligibilityListEntryWithOnBoardingInfo> entries, List<ServicesDetailsQueryResponse> services);
	}

	public class WelcomeMessagesService : IWelcomeMessagesService
	{
		private readonly IBus _bus;
		private readonly IPatientMessagingContentCreator _patientMessagingContentCreator;
		private readonly IWelcomeMessagesRepository _welcomeMessagesRepository;
		private readonly INationalProviderRepository _providerRepository;
		private readonly INationalPatientRepository _patientRepository;
		private readonly INationalPatientContactRepository _patientContactRepository;
		private readonly INationalBookingTeamRepository _nationalBookingTeamRepository;

		public WelcomeMessagesService(
			IBus bus,
			IPatientMessagingContentCreator patientMessagingContentCreator,
			IWelcomeMessagesRepository welcomeMessagesRepository,
			INationalProviderRepository providerRepository,
			INationalPatientRepository patientRepository,
			INationalPatientContactRepository patientContactRepository, 
			INationalBookingTeamRepository nationalBookingTeamRepository)
		{
			_bus = bus;
			_patientMessagingContentCreator = patientMessagingContentCreator;
			_welcomeMessagesRepository = welcomeMessagesRepository;
			_providerRepository = providerRepository;
			_patientRepository = patientRepository;
			_patientContactRepository = patientContactRepository;
			_nationalBookingTeamRepository = nationalBookingTeamRepository;
		}

		public async Task AddWelcomeMessage(Guid patientId, Guid serviceId, string sentBy, int providerId, string reason, bool sent = false)
		{
			var welcomeMessage = new WelcomeMessage
			{
				Id = Guid.NewGuid(),
				PatientId = patientId,
				ServiceId = serviceId,
				ProviderId = providerId,
				WelcomeMessageSent = sent,
				Reason = reason,
				SentBy = sentBy,
				SentDateTime = sent ? DomainTime.UtcNow.ToZonedTime() : (DateTime?) null,
				CreatedDateTime = DomainTime.UtcNow
			};

			await _welcomeMessagesRepository.CreateWelcomeMessageAsync(welcomeMessage);
		}

		public async Task SendWelcomeMessages(List<EligibilityListEntryWithOnBoardingInfo> entries, List<ServicesDetailsQueryResponse> services)
		{
			entries.ForEach(e => e.SetServiceAndBookingTeamIdFromServicesDetailsQueryResponse(services));
			var entriesThatNeedToBeWelcomed = entries.Where(e => e.ShouldSendWelcomeMessage()).ToList();

			if (!entriesThatNeedToBeWelcomed.Any())
			{
				return;
			}

			// Filter out services that don't have any entries to be welcomed
			var filteredServices = services.Where(s => entriesThatNeedToBeWelcomed.Any(e => e.ServiceId == s.Id)).ToList();

			var providers = await _providerRepository.GetProvidersByIds(filteredServices.Select(s => s.ProviderId).DefaultIfEmpty().Distinct().ToList());
			var providerDetails = providers.Select(p => p.ToProviderDetails()).ToList();
			var bookingTeams = await _nationalBookingTeamRepository.GetByIds(filteredServices.Select(s => s.MarsBookingTeamId).DefaultIfEmpty().Distinct().ToList());

			entriesThatNeedToBeWelcomed.ForEach(e => e.SetProviderDetailsAndBookingTeam(providerDetails, bookingTeams));

			await HydrateEligibilityListEntriesWithPatientNationalData(entriesThatNeedToBeWelcomed);
			await HydrateEligibilityListEntriesWithPatientContactNationalData(entriesThatNeedToBeWelcomed);

			entriesThatNeedToBeWelcomed = FilterOutEntriesWithErrors(entriesThatNeedToBeWelcomed);

			entriesThatNeedToBeWelcomed.ForEach(e => SendWelcomeMessageFromOnBoardingAgent(e));
		}

		private async Task HydrateEligibilityListEntriesWithPatientContactNationalData(List<EligibilityListEntryWithOnBoardingInfo> entries)
		{
			var patientsThatHaveValidContacts = await _patientContactRepository.PatientsThatHaveValidContacts(entries.Select(e => e.PatientId).DefaultIfEmpty().Distinct().ToList());

			entries.ForEach(e => e.SetPatientContactNationalData(patientsThatHaveValidContacts));
		}

		private async Task SendWelcomeMessageFromOnBoardingAgent(EligibilityListEntryWithOnBoardingInfo entry)
		{
			var sendMessageWithReason = entry.GetSendMessageWithReason();

			if (sendMessageWithReason.SendMessage)
			{
				var welcomeMessageContent = new WelcomeMessageContent
				{
					UrlPostfix = entry.Service.UrlPostfix,
					Message = entry.Service.WelcomeMessage,
					Specialty = entry.Service.Specialty,
					FirstName = entry.FirstName,
					LastName = entry.LastName,
					StartDate = entry.StartDate,
					EndDate = entry.EndDate,
					PatientId = entry.PatientId,
					BookingTeamContact = entry.BookingTeam.PhoneNumber
				};

				var messageProperties = await _patientMessagingContentCreator.CreateWelcomeMessageProperties(welcomeMessageContent, entry.ProviderDetails);
				_bus.Send(new SendPatientMessage
				{
					ProviderId = entry.ProviderId,
					PatientId = entry.PatientId,
					MessageProperties = messageProperties,
					IncludeBrandingInfo = false,
					Timestamp = DomainTime.UtcNow
				});
			}

			if (entry.ShouldUpdateWelcomeMessageLog(sendMessageWithReason))
			{
				await AddWelcomeMessage(entry.PatientId, entry.ServiceId, "PatientsOnBoardingAgent", entry.ProviderId, sendMessageWithReason.Reason, sendMessageWithReason.SendMessage);
			}

			PublishOnBoardPatientReportingEvent(entry, messageSent: sendMessageWithReason.SendMessage);
		}

		private List<EligibilityListEntryWithOnBoardingInfo> FilterOutEntriesWithErrors(List<EligibilityListEntryWithOnBoardingInfo> entries)
		{
			var validEntries = new List<EligibilityListEntryWithOnBoardingInfo>();

			entries.ForEach(e =>
			{
				var errors = e.BuildErrorMessage();

				if (errors.Count != 0)
				{
					PublishOnBoardPatientReportingEvent(e, errors);
				}
				else
				{
					validEntries.Add(e);
				}
			});

			return validEntries;
		}

		private async Task HydrateEligibilityListEntriesWithPatientNationalData(List<EligibilityListEntryWithOnBoardingInfo> entries)
		{
			var nationalPatients = await _patientRepository.GetPatientByIds(entries.Select(e => e.PatientId).DefaultIfEmpty().Distinct().ToList());

			entries.ForEach(e => e.SetPatientNationalData(nationalPatients));
		}

		private void PublishOnBoardPatientReportingEvent(EligibilityListEntryWithOnBoardingInfo entry, Dictionary<string, object> errors = null, bool messageSent = false)
		{
			var reportingEvent = new DynamicReportingEvent
			{
				Category = "AppointmentRequestService",
				Description = "Patient on boarded by the agent",
				Payload = new Dictionary<string, object>
				{
					{ "ProviderId", entry.ProviderId },
					{ "ServiceId", entry.ServiceId },
					{ "ServiceName", entry.Service?.ServiceName },
					{ "PatientId", entry.PatientId },
					{ "MessageSent", messageSent },
					{ "Success", errors == null }
				}
			};

			if (errors != null)
			{
				foreach (var (key, value) in errors)
				{
					reportingEvent.Payload.Add(key, value);
				}
			}

			_bus.Publish(reportingEvent);
		}
	}
}
