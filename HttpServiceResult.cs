namespace Arta.Infrastructure
{
    public class HttpServiceResult<TValue>
    {
        public bool IsSuccessful { get; }
        public bool IsFailure => !IsSuccessful;
        public string ErrorDescription { get; }
        public TValue Value { get; }
        public string ErrorCode { get; }
        public int HttpStatusCode { get; }

        internal HttpServiceResult(TValue value, bool isSuccessful, string errorDescription, string errorCode, int httpStatusCode)
        {
            Value = value;
            IsSuccessful = isSuccessful;
            ErrorDescription = errorDescription;
            ErrorCode = errorCode;
            HttpStatusCode = httpStatusCode;
        }

        public static HttpServiceResult<TValue> Ok(TValue value, int httpStatusCode)
        {
            return new HttpServiceResult<TValue>(value, true, null, null, httpStatusCode);
        }

        public static HttpServiceResult<TValue> Fail(string errorDescription, string errorCode, int httpStatusCode)
        {
            return new HttpServiceResult<TValue>(default(TValue), false, errorDescription, errorCode, httpStatusCode);
        }
    }

    public class HttpServiceResult
    {
        public bool IsSuccessful { get; }
        public bool IsFailure => !IsSuccessful;
        public string ErrorDescription { get; }
        public string ErrorCode { get; }
        public int HttpStatusCode { get; }

        internal HttpServiceResult(bool isSuccessful, string errorDescription, string errorCode, int httpStatusCode)
        {
            IsSuccessful = isSuccessful;
            ErrorDescription = errorDescription;
            ErrorCode = errorCode;
            HttpStatusCode = httpStatusCode;
        }

        public static HttpServiceResult Ok(int httpStatusCode)
        {
            return new HttpServiceResult(true, null, null, httpStatusCode);

        }

        public static HttpServiceResult Fail(string errorDescription, string errorCode, int httpStatusCode)
        {
            return new HttpServiceResult(false, errorDescription, errorCode, httpStatusCode);
        }
    }
}