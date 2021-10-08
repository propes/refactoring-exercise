using System.Collections.Generic;
using System.Threading.Tasks;
using RefactoringExercise.Models.National;

namespace RefactoringExercise.DataAccess.Repositories.National
{
	public interface INationalProviderRepository
	{
		public Task<List<NationalProvider>> GetProvidersByIds(List<int> providerIds);
	}
}
