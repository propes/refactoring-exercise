using RefactoringExercise.DataAccess.Repositories;
using System.Linq;
using System.Threading.Tasks;
using RefactoringExercise.Services.PatientMessaging;

namespace RefactoringExercise.Services
{
	public class EligibilityListService
	{
		private readonly IEligibilityListRepository _eligibilityListRepository;
		private readonly IWelcomeMessagesService _welcomeMessageService;
		private readonly IServicesRepository _servicesRepository;

		public EligibilityListService(IEligibilityListRepository eligibilityListRepository,
			IWelcomeMessagesService welcomeMessageService,
			IServicesRepository servicesRepository)
		{
			_eligibilityListRepository = eligibilityListRepository;
			_welcomeMessageService = welcomeMessageService;
			_servicesRepository = servicesRepository;
		}

		public async Task OnBoardPatients()
		{
			var services = await _servicesRepository.GetActiveServicesWithWelcomeMessageEnabled();
			if (!services.Any())
			{
				return;
			}

			var entries = await _eligibilityListRepository.GetEligibilityListEntriesWithOnBoardingInfo(services.Select(s => s.Id).DefaultIfEmpty().ToList());
			
			await _welcomeMessageService.SendWelcomeMessages(entries, services.ToList());
		}
	}
}
