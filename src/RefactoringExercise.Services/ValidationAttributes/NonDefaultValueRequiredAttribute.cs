using System;

namespace RefactoringExercise.Services.ValidationAttributes
{
	public class NonDefaultValueRequiredAttribute : System.ComponentModel.DataAnnotations.ValidationAttribute
	{
		public NonDefaultValueRequiredAttribute()
			: base("{0} requires a non-default value")
		{
		}

		public override bool IsValid(object value)
		{
			var type = value.GetType();
			return !type.IsValueType || !value.Equals(Activator.CreateInstance(type));
		}
	}
}