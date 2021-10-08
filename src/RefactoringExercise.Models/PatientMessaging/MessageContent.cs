using System;

namespace RefactoringExercise.Models.PatientMessaging
{
	public class MessageContent
	{
		public string UrlPostfix { get; set; }
		public string Message { get; set; }
		public string Specialty { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public Guid PatientId { get; set; }
		public string BookingTeamContact { get; set; }
	}
}
