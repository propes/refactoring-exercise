using System.Threading.Tasks;
using RefactoringExercise.Models.PatientMessaging;

namespace RefactoringExercise.Services.PatientMessaging
{
	public interface IPatientMessagingContentCreator
	{
		public Task<MessageProperties> CreateWelcomeMessageProperties(WelcomeMessageContent welcomeMessageContent, ProviderDetails providerDetails);
	}
}