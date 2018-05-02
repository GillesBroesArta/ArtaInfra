namespace Arta.Infrastructure
{
    public class Result<TValue>
    {
        public bool IsSuccessful { get; }
        public bool IsFailure => !IsSuccessful;
        public string ErrorDescription { get; }
        public TValue Value { get; }

        internal Result(TValue value, bool isSuccessful, string errorDescription)
        {
            Value = value;
            IsSuccessful = isSuccessful;
            ErrorDescription = errorDescription;
        }

        public static Result<TValue> Ok(TValue value)
        {
            return new Result<TValue>(value, true, null);
        }

        public static Result<TValue> Fail(string errorDescription)
        {
            return new Result<TValue>(default(TValue), false, errorDescription);
        }
    }

    public class Result
    {
        public bool IsSuccessful { get; }
        public bool IsFailure => !IsSuccessful;
        public string ErrorDescription{ get; }

        internal Result(bool isSuccessful, string errorDescription)
        {
            IsSuccessful = isSuccessful;
            ErrorDescription = errorDescription;
        }

        public static Result Ok()
        {
            return new Result(true, null);
        }

        public static Result Fail(string errorDescription)
        {
            return new Result(false, errorDescription);
        }
    }
}
