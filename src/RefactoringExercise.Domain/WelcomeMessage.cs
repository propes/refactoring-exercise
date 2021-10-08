using System;

namespace RefactoringExercise.Domain
{
	public class WelcomeMessage
	{
		public Guid Id { get; set; }
		public Guid PatientId { get; set; }
		public Guid ServiceId { get; set; }
		public int ProviderId { get; set; }
		public bool WelcomeMessageSent { get; set; }
		public string Reason { get; set; }
		public string SentBy { get; set; }
		public DateTime? SentDateTime { get; set; }
		public DateTime CreatedDateTime { get; set; }
	}
}
