using System;
using System.Collections.Generic;
using RefactoringExercise.Domain;

namespace RefactoringExercise.Models.PatientMessaging
{
	public class MessageProperties
	{
		public SendTemplatedEmail EmailTemplate { get; set; }

		public string SmsMessage { get; set; }

		public Guid LoggingCorrelationId { get; set; }

		public string MessageCategory { get; set; }
	}

	public class SendTemplatedEmail : BusItemBase, ICommand, IBusItem
	{
		public SendTemplatedEmail() => this.Version = 1;

		public string TemplateIdentifier { get; set; }

		public string Subject { get; set; }

		public string FromEmail { get; set; }

		public string FromName { get; set; }

		public List<string> Addressees { get; set; }

		public string ReplyTo { get; set; }

		public int ProviderId { get; set; }

		public string MessageCategory { get; set; }

		public int Version { get; set; }
	}
}