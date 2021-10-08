using RefactoringExercise.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RefactoringExercise.DataAccess.Repositories
{
	public interface IServicesRepository
	{
		Task<IReadOnlyList<ServicesDetailsQueryResponse>> GetActiveServicesWithWelcomeMessageEnabled();
	}
}
