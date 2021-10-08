namespace RefactoringExercise.Models.PatientMessaging
{
	public class CreateShortLinkCommand
	{
		public string FullUrl { get; set; }
		public string ShortUrlHostname { get; set; }
	}
}