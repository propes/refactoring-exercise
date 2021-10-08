using System;
using System.Collections.Generic;
using System.Linq;
using RefactoringExercise.Domain;
using RefactoringExercise.Models.National;
using RefactoringExercise.Models.PatientMessaging;

namespace RefactoringExercise.Models
{
	public class EligibilityListEntryWithOnBoardingInfo
	{
		public Guid ServiceId { get; set; }
		public Guid PatientId { get; set; }
		public int ProviderId { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public DateTime? RequestCreatedDateTime { get; set; } 
		public DateTime? RequestReviewedDateTime { get; set; }
		public bool PatientHasBeenSentWelcomeMessage { get; set; }
		public string WelcomeMessageReason { get; set; }

		public ProviderDetails ProviderDetails { get; private set; }
		public Service Service { get; private set; }
		public BookingTeam BookingTeam { get; private set; }

		// Hydrated from National
		public bool IsArchived { get; private set; }
		public DateTime? DateOfDeath { get; private set; }
		public bool PatientExistsInNational { get; private set; }
		public bool HasValidContact { get; private set; }

		public int? BookingTeamId { get; private set; }

		// Internal flags used to check external data needed into model has been set
		private bool NationalPatientContactDataSet { get; set; }
		private bool NationalPatientDataSet { get; set; }
		private bool ServiceDataSet { get; set; }
		private bool ProviderAndBookingTeamDataSet { get; set; }

		public bool ShouldSendWelcomeMessage()
		{
			if (!ServiceDataSet)
			{
				throw new Exception("You need to set service data before calling this method");
			}
			
			return Service.IsActive
				&& Service.WelcomeMessageEnabled
				&& Service.EligibilityListEnabled
				&& !PatientHasBeenSentWelcomeMessage
				&& StartDate.Date <= DomainTime.UtcNow.Date
				&& (!EndDate.HasValue || EndDate.Value.Date >= DomainTime.UtcNow.Date)
				&& PatientEligibility.IsPatientEligible(
					PatientEligibility.GetEligibilityStatus(
						Service.EligibilityListEnabled,
						Service.RequestExpiresPatient,
						StartDate,
						EndDate,
						RequestCreatedDateTime.HasValue,
						RequestReviewedDateTime.HasValue));
		}

		public SendMessageWithReason GetSendMessageWithReason()
		{
			if (!NationalPatientContactDataSet || !NationalPatientDataSet)
			{
				throw new Exception("You need to set national patient and contact data before calling this method");
			}

			if (IsArchived || DateOfDeath.HasValue)
			{
				return new SendMessageWithReason("Patient record is inactive");
			}

			if (!HasValidContact)
			{
				return new SendMessageWithReason("Patient does not have valid contact");
			}

			return new SendMessageWithReason("Welcome message sent", true);
		}

		public Dictionary<string, object> BuildErrorMessage()
		{
			if (!(NationalPatientDataSet && ServiceDataSet && ProviderAndBookingTeamDataSet))
			{
				throw new Exception("You need set patient, service, provider and booking team data before calling this method");
			}

			var errors = new Dictionary<string, object>();
			if (BookingTeam == null)
			{
				errors.Add("BookingTeamError", $"Booking team with id {BookingTeamId} could not be found");
			}

			if (ProviderDetails == null)
			{
				errors.Add("ProviderError", $"Provider with id {ProviderId} could not be found");
			}

			if (Service == null)
			{
				errors.Add("ServiceError", $"Service with id {ServiceId} could not be found");
			}

			if (!PatientExistsInNational)
			{
				errors.Add("PatientError", $"Patient with id {PatientId} could not be found");
			}

			return errors;
		}

		public bool ShouldUpdateWelcomeMessageLog(SendMessageWithReason sendMessageWithReason)
		{
			return sendMessageWithReason.SendMessage || WelcomeMessageReason != sendMessageWithReason.Reason;
		}

		public void SetServiceAndBookingTeamIdFromServicesDetailsQueryResponse(List<ServicesDetailsQueryResponse> services)
		{
			ServiceDataSet = true;
			
			var service = services.FirstOrDefault(s => s.Id == ServiceId);

			if (service != null)
			{
				Service = service.MapToDomain();
				BookingTeamId = service.MarsBookingTeamId;
			}
		}

		public void SetProviderDetailsAndBookingTeam(List<ProviderDetails> providers, List<BookingTeam> bookingTeams)
		{
			// This is required because we hydrate the booking team id from the service
			if (!ServiceDataSet)
			{
				throw new Exception("You need to set service data before calling this method");
			}
			
			ProviderAndBookingTeamDataSet = true;

			SetProviderDetails(providers);
			SetBookingTeam(bookingTeams);
		}

		public void SetPatientNationalData(IReadOnlyList<NationalPatient> nationalPatients)
		{
			NationalPatientDataSet = true;
			
			var nationalPatient = nationalPatients.FirstOrDefault(n => n.PatientId == PatientId);

			if (nationalPatient != null)
			{
				PatientExistsInNational = true;
				IsArchived = nationalPatient.IsArchived;
				DateOfDeath = nationalPatient.DateOfDeath;
			}
		}

		public void SetPatientContactNationalData(IReadOnlyList<Guid> patientsWithValidContact)
		{
			NationalPatientContactDataSet = true;
			HasValidContact = patientsWithValidContact.Any(p => p == PatientId);
		}

		private void SetProviderDetails(List<ProviderDetails> providers)
		{
			ProviderDetails = providers.FirstOrDefault(p => p.ProviderId == ProviderId);
		}

		private void SetBookingTeam(List<BookingTeam> bookingTeams)
		{
			BookingTeam = Service == null ? null : bookingTeams.FirstOrDefault(b => b.Id == BookingTeamId);
		}
	}

	public class SendMessageWithReason
	{
		public SendMessageWithReason(string reason, bool sendMessage = false)
		{
			Reason = reason;
			SendMessage = sendMessage;
		}

		public bool SendMessage { get; }
		public string Reason { get; }
	}
}
