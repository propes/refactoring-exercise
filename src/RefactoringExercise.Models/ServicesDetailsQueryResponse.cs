using RefactoringExercise.Domain;

namespace RefactoringExercise.Models
{
	public class ServicesDetailsQueryResponse :  ServicesQueryResponse
	{
		public int MarsBookingTeamId { get; set; }
		public string ServiceInformation { get; set; }
		public string WelcomeMessage { get; set; }
		public bool WelcomeMessageEnabled { get; set; }
		public string OffBoardingMessage { get; set; }
		public bool OffBoardingMessageEnabled { get; set; }
		public bool HasPatientBulkUploadsInProgress { get; set; }
		public int ProviderId { get; set; }

		public Service MapToDomain()
		{
			return new Service
			{
				Id = Id,
				ProviderId = ProviderId,
				ServiceName = ServiceName,
				UrlPostfix = UrlPostfix,
				Specialty = Specialty,
				ServiceInformation = ServiceInformation,
				ThankyouMessage = ThankyouMessage,
				EligibilityListEnabled = EligibilityListEnabled,
				EligibilityListTimeFrameMonths = EligibilityListTimeFrameMonths,
				WelcomeMessage = WelcomeMessage,
				OffBoardingMessage = OffBoardingMessage,
				IsActive = IsActive,
				EligibilityListType = EligibilityListType,
				PatientListId = PatientListId,
				CreatedBy = CreatedBy,
				WelcomeMessageEnabled = WelcomeMessageEnabled,
				OffBoardingMessageEnabled = OffBoardingMessageEnabled,
				RequestExpiresPatient = RequestExpiresPatient,
				HasPatientBulkUploadsInProgress = HasPatientBulkUploadsInProgress
			};
		}
	}
}
