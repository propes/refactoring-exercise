using System.Collections.Generic;
using System.Threading.Tasks;
using RefactoringExercise.Domain;

namespace RefactoringExercise.DataAccess.Repositories.National
{
	public interface INationalBookingTeamRepository
	{
		Task<List<BookingTeam>> GetByIds(List<int> ids);
	}
}
