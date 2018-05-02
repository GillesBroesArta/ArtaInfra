namespace Arta.Infrastructure
{
    public class Result<TValue>
    {
        public bool IsSuccessful { get; }
        public bool IsFailure => !IsSuccessful;
        public string ErrorDescription { get; }
        public TValue Value { get; }

        internal Result(TValue value, bool isSuccessful, string ErrorDescription)
        {
            Value = value;
            IsSuccessful = isSuccessful;
            ErrorDescription = ErrorDescription;
        }

        public static Result<TValue> Ok(TValue value)
        {
            return new Result<TValue>(value, true, null);
        }

        public static Result<TValue> Fail(string ErrorDescription)
        {
            return new Result<TValue>(default(TValue), false, ErrorDescription);
        }
    }

    public class Result
    {
        public bool IsSuccessful { get; }
        public bool IsFailure => !IsSuccessful;
        public string ErrorDescription{ get; }

        internal Result(bool isSuccessful, string ErrorDescription)
        {
            IsSuccessful = isSuccessful;
            ErrorDescription = ErrorDescription;
        }

        public static Result Ok()
        {
            return new Result(true, null);
        }

        public static Result Fail(string ErrorDescription)
        {
            return new Result(false, ErrorDescription);
        }
    }
}
