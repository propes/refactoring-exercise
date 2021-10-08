using System;
using RefactoringExercise.Domain;

namespace RefactoringExercise.Models
{
	public class ServicesQueryResponse
	{
		public Guid Id { get; set; }
		public string ServiceName { get; set; }
		public string Specialty { get; set; }
		public string UrlPostfix { get; set; }
		public string ThankyouMessage { get; set; }
		public string BookingTeamName { get; set; }
		public bool EligibilityListEnabled { get; set; }
		public int? EligibilityListTimeFrameMonths { get; set; }
		public bool RequestExpiresPatient { get; set; }
		public EligibilityListType EligibilityListType { get; set; }
		public Guid? PatientListId { get; set; }
		public bool IsActive { get; set; }
		public string CreatedBy { get; set; }
	}
}
