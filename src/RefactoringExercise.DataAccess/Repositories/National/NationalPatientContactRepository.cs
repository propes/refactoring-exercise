using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RefactoringExercise.DataAccess.Repositories.National
{
	public interface INationalPatientContactRepository
	{
		public Task<IReadOnlyList<Guid>> PatientsThatHaveValidContacts(List<Guid> patientIds);
	}
}
