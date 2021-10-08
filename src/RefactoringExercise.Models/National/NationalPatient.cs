using System;

namespace RefactoringExercise.Models.National
{
	public class NationalPatient
	{
		public Guid PatientId { get; set; }
		public int ProviderId { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string NhsNumber { get; set; }
		public string Mrn { get; set; }
		public DateTime? DateOfDeath { get; set; }
		public bool IsArchived { get; set; }
	}
}
