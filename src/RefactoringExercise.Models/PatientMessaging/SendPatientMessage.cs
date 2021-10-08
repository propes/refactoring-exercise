using System;
using RefactoringExercise.Domain;

namespace RefactoringExercise.Models.PatientMessaging
{
	public class SendPatientMessage : BusItemBase, ICommand
	{
		public int ProviderId { get; set; }

		public Guid PatientId { get; set; }

		public MessageProperties MessageProperties { get; set; }

		public bool IncludeBrandingInfo { get; set; }
	}
}