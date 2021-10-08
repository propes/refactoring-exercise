using System;

namespace RefactoringExercise.Domain
{
	public class Service
	{
		public Guid Id { get; set; }
		public int ProviderId { get; set; }
		public string ServiceName { get; set; }
		public string UrlPostfix { get; set; }
		public string Specialty { get; set; }
		public string ServiceInformation { get; set; }
		public string ThankyouMessage { get; set; }
		public int BookingTeamId { get; set; }
		public bool EligibilityListEnabled { get; set; }
		public int? EligibilityListTimeFrameMonths { get; set; }
		public string WelcomeMessage { get; set; }
		public string OffBoardingMessage { get; set; }
		public bool IsActive { get; set; }
		public EligibilityListType EligibilityListType { get; set; }
		public Guid? PatientListId { get; set; }
		public DateTime CreatedDateTime { get; set; }
		public string CreatedBy { get; set; }
		public bool WelcomeMessageEnabled { get; set; }
		public bool OffBoardingMessageEnabled { get; set; }
		public bool RequestExpiresPatient { get; set; }
		public bool HasPatientBulkUploadsInProgress { get; set; }
	}
}