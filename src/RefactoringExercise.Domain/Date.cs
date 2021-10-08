using System;

namespace RefactoringExercise.Domain
{
	public class Date
	{
		private DateTime _value;

		public DateTime Value
		{
			get => _value;

			set => _value = value.GetDatePart();
		}

		public static Date CreateFromDate(DateTime? value)
		{
			if (value.HasValue)
			{
				return new Date
				{
					Value = value.Value
				};
			}

			return null;
		}
	}
}