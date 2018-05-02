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

        internal HttpServiceResult(TValue value, bool isSuccessful, string ErrorDescription, string errorCode, int httpStatusCode)
        {
            Value = value;
            IsSuccessful = isSuccessful;
            ErrorDescription = ErrorDescription;
            ErrorCode = errorCode;
            HttpStatusCode = httpStatusCode;
        }

        public static HttpServiceResult<TValue> Ok(TValue value, int httpStatusCode)
        {
            return new HttpServiceResult<TValue>(value, true, null, null, httpStatusCode);
        }

        public static HttpServiceResult<TValue> Fail(string ErrorDescription, string errorCode, int httpStatusCode)
        {
            return new HttpServiceResult<TValue>(default(TValue), false, ErrorDescription, errorCode, httpStatusCode);
        }
    }

    public class HttpServiceResult
    {
        public bool IsSuccessful { get; }
        public bool IsFailure => !IsSuccessful;
        public string ErrorDescription { get; }
        public string ErrorCode { get; }
        public int HttpStatusCode { get; }

        internal HttpServiceResult(bool isSuccessful, string ErrorDescription, string errorCode, int httpStatusCode)
        {
            IsSuccessful = isSuccessful;
            ErrorDescription = ErrorDescription;
            ErrorCode = errorCode;
            HttpStatusCode = httpStatusCode;
        }

        public static HttpServiceResult Ok(int httpStatusCode)
        {
            return new HttpServiceResult(true, null, null, httpStatusCode);

        }

        public static HttpServiceResult Fail(string ErrorDescription, string errorCode, int httpStatusCode)
        {
            return new HttpServiceResult(false, ErrorDescription, errorCode, httpStatusCode);
        }
    }
}