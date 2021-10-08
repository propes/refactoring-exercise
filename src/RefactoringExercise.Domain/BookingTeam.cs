using System.Collections.Generic;

namespace RefactoringExercise.Domain
{
	public class BookingTeam
	{
		public int Id { get; set; }
		public int ProviderId { get; set; }
		public int MarsBookingTeamId { get; set; }
		public string AdministrativeName { get; set; }
		public string PhoneNumber { get; set; }
		public IList<string> Emails { get; set; }
	}
}
