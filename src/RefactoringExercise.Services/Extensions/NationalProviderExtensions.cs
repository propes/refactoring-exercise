using RefactoringExercise.Models.National;
using RefactoringExercise.Models.PatientMessaging;

namespace RefactoringExercise.Services.Extensions
{
	public static class NationalProviderExtensions
	{
		public static ProviderDetails ToProviderDetails(this NationalProvider provider)
		{
			return new ProviderDetails
			{
				ProviderId = provider.ProviderId,
				ProviderDescription = provider.Description,
				ProviderLogoFileName = provider.LogoFileName,
				ProviderShortUrlHostname = provider.ShortUrlHostname,
				Alias = provider.Alias
			};
		}
	}
}
