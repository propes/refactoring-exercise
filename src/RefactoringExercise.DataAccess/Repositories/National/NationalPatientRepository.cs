using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RefactoringExercise.Models.National;

namespace RefactoringExercise.DataAccess.Repositories.National
{
	public interface INationalPatientRepository
	{
		Task<IReadOnlyList<NationalPatient>> GetPatientByIds(IReadOnlyList<Guid> patientIds);
	}
}
