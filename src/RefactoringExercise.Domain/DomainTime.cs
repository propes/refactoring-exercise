using System;

namespace RefactoringExercise.Domain
{
    public static class DomainTime
    {
        private static DateTime _overrideDate = DateTime.MinValue;

        public static DateTime UtcNow => !(DomainTime._overrideDate == DateTime.MinValue) ? DomainTime._overrideDate.ToUniversalTime() : DateTime.UtcNow;

        public static DateTime Now => !(DomainTime._overrideDate == DateTime.MinValue) ? DomainTime._overrideDate : DateTime.Now;

        public static DateTime Today => !(DomainTime._overrideDate == DateTime.MinValue) ? DomainTime._overrideDate.Date : DateTime.Today;

        public static DateTime MaxValue => DateTime.MaxValue;

        public static DateTime MinValue => DateTime.MinValue;

        public static void Set(DateTime overrideDate) => DomainTime._overrideDate = overrideDate;

        public static void Reset() => DomainTime._overrideDate = DateTime.MinValue;
    }
}