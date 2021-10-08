using System.Threading.Tasks;
using RefactoringExercise.Models;
using RefactoringExercise.Models.PatientMessaging;

namespace RefactoringExercise.Services.PatientMessaging
{
	public interface ILinkShortenerApiService
	{
		Task<Result<CreateShortLinkResult>> CreateShortLink(CreateShortLinkCommand command);
	}
}