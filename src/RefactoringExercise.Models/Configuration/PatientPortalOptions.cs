namespace RefactoringExercise.Models.Configuration
{
	/// <summary>
	/// Patient portal configuration settings
	/// </summary>
	public class PatientPortalOptions : SiteOptions
	{
		/// <summary>
		/// The configuration section to bind to for patient portal options
		/// </summary>
		public const string ConfigurationSectionName = "PatientPortal";
	}
}