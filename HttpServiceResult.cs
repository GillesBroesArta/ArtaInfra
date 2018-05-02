namespace Arta.Infrastructure
{
    public class HttpServiceResult<TValue>
    {
        public bool IsSuccessful { get; }
        public bool IsFailure => !IsSuccessful;
        public string ErrorMessage { get; }
        public TValue Value { get; }
        public string ErrorCode { get; }
        public int HttpStatusCode { get; }

        internal HttpServiceResult(TValue value, bool isSuccessful, string errorMessage, string errorCode, int httpStatusCode)
        {
            Value = value;
            IsSuccessful = isSuccessful;
            ErrorMessage = errorMessage;
            ErrorCode = errorCode;
            HttpStatusCode = httpStatusCode;
        }

        public static HttpServiceResult<TValue> Ok(TValue value, int httpStatusCode)
        {
            return new HttpServiceResult<TValue>(value, true, null, null, httpStatusCode);
        }

        public static HttpServiceResult<TValue> Fail(string errorMessage, string errorCode, int httpStatusCode)
        {
            return new HttpServiceResult<TValue>(default(TValue), false, errorMessage, errorCode, httpStatusCode);
        }
    }

    public class HttpServiceResult
    {
        public bool IsSuccessful { get; }
        public bool IsFailure => !IsSuccessful;
        public string ErrorMessage { get; }
        public string ErrorCode { get; }
        public int HttpStatusCode { get; }

        internal HttpServiceResult(bool isSuccessful, string errorMessage, string errorCode, int httpStatusCode)
        {
            IsSuccessful = isSuccessful;
            ErrorMessage = errorMessage;
            ErrorCode = errorCode;
            HttpStatusCode = httpStatusCode;
        }

        public static HttpServiceResult Ok(int httpStatusCode)
        {
            return new HttpServiceResult(true, null, null, httpStatusCode);

        }

        public static HttpServiceResult Fail(string errorMessage, string errorCode, int httpStatusCode)
        {
            return new HttpServiceResult(false, errorMessage, errorCode, httpStatusCode);
        }
    }
}