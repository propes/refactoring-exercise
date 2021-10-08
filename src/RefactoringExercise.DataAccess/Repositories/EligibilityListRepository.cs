using System;
using RefactoringExercise.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace RefactoringExercise.DataAccess.Repositories
{
	public interface IEligibilityListRepository
	{
		Task<List<EligibilityListEntryWithOnBoardingInfo>> GetEligibilityListEntriesWithOnBoardingInfo(List<Guid> serviceIds);
	}
}
