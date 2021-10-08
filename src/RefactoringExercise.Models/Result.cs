namespace RefactoringExercise.Models
{
    public class Result
    {
        public bool Succeeded { get; }

        public string ErrorSummary { get; set; }

        public Result(bool succeeded)
        {
            Succeeded = succeeded;
        }
    }

    public class Result<T> : Result
    {
        public T Value { get; }

        public Result(bool succeeded, T value) : base(succeeded)
        {
            Value = value;
        }
    }
}