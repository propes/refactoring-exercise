using System;

namespace RefactoringExercise.Domain
{
	public static class DateTimeExtensions
	{
		private const string UKLocalTime = "GMT Standard Time";

		public static DateTime ToZonedTime(this DateTime date, string timezoneId = UKLocalTime)
		{
			var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timezoneId);

			var result = TimeZoneInfo.ConvertTime(new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, DateTimeKind.Utc), timeZoneInfo);

			return result;
		}

		public static DateTime GetDatePart(this DateTime dateWithTime)
		{
			return new DateTime(dateWithTime.Year, dateWithTime.Month, dateWithTime.Day);
		}
	}
}