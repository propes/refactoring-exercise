using System.Threading.Tasks;
using RefactoringExercise.Domain;

namespace RefactoringExercise.DataAccess.Repositories
{
	public interface IWelcomeMessagesRepository
	{
		Task<bool> CreateWelcomeMessageAsync(WelcomeMessage welcomeMessage);
	}
}
