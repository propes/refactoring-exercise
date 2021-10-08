using System;

namespace RefactoringExercise.Domain
{
	public static class PatientEligibility
	{
		public static bool IsPatientEligible(EligibilityStatus eligibilityStatus)
		{
			return eligibilityStatus == EligibilityStatus.Eligible;
		}

		public static EligibilityStatus GetEligibilityStatus(
			bool eligibilityListEnabled, bool requestExpiresPatient, DateTime? startDate, DateTime? endDate, bool hasRequestCreatedDate, bool hasRequestReviewedDate,
			DateTime? startDateToCheck = null, DateTime? endDateToCheck = null)
		{
			if (requestExpiresPatient && hasRequestReviewedDate)
			{
				return EligibilityStatus.OneRequestDone;
			}

			if (hasRequestCreatedDate && !hasRequestReviewedDate)
			{
				return EligibilityStatus.RequestPending;
			}

			if (!eligibilityListEnabled)
			{
				return EligibilityStatus.Eligible;
			}

			var isPatientInEligibilityList = startDate.HasValue;
			if (!isPatientInEligibilityList)
			{
				return EligibilityStatus.PatientNotInEligibilityList;
			}

			startDateToCheck ??= DomainTime.UtcNow.ToZonedTime().GetDatePart();
			endDateToCheck ??= DomainTime.UtcNow.ToZonedTime().GetDatePart();

			startDate = startDate.Value.GetDatePart();
			var eligibilityPeriodStarted = startDate.Value <= startDateToCheck;
			if (!eligibilityPeriodStarted)
			{
				return EligibilityStatus.NotYetEligible;
			}

			if (!endDate.HasValue)
			{
				return EligibilityStatus.Eligible;
			}

			endDate = endDate.Value.GetDatePart();
			if (endDate.Value.Date < endDateToCheck)
			{
				return EligibilityStatus.NoLongerEligible;
			}

			return EligibilityStatus.Eligible;
		}
	}
}
