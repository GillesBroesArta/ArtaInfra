using System.Net;

namespace Arta.Infrastructure
{
    public class ApiResult<TValue>
    {
        public bool IsSuccessful { get; }
        public bool IsFailure => !IsSuccessful;
        public string ErrorDescription{ get; }

        public int HttpStatusCode { get; }
        public TValue Value { get; }
        public string ErrorCode { get; }

        internal ApiResult(TValue value, bool isSuccessful, int httpStatusCode, string errorDescription, string errorCode = null)
        {
            Value = value;
            IsSuccessful = isSuccessful;
            ErrorDescription = errorDescription;
            HttpStatusCode = httpStatusCode;
            ErrorCode = errorCode;
        }

        public static ApiResult<TValue> Ok(TValue value)
        {
            return new ApiResult<TValue>(value, true, (int)System.Net.HttpStatusCode.OK, null);
        }

        public static ApiResult<TValue> Accepted(TValue value)
        {
            return new ApiResult<TValue>(value, true, (int)System.Net.HttpStatusCode.Accepted, null);
        }

        public static ApiResult<TValue> Created(TValue value)
        {
            return new ApiResult<TValue>(value, true, (int)System.Net.HttpStatusCode.Created, null);
        }

        public static ApiResult<TValue> NoContent()
        {
            return new ApiResult<TValue>(default(TValue), true, (int)System.Net.HttpStatusCode.NoContent, null);
        }

        public static ApiResult<TValue> Forbidden(string ErrorDescription)
        {
            return new ApiResult<TValue>(default(TValue), false, (int)System.Net.HttpStatusCode.Forbidden, ErrorDescription);
        }

        public static ApiResult<TValue> Unauthorized(string ErrorDescription)
        {
            return new ApiResult<TValue>(default(TValue), false, (int)System.Net.HttpStatusCode.Unauthorized, ErrorDescription);
        }

        public static ApiResult<TValue> BadRequest(string ErrorDescription)
        {
            return new ApiResult<TValue>(default(TValue), false, (int)System.Net.HttpStatusCode.BadRequest, ErrorDescription);
        }

        public static ApiResult<TValue> ErrorDuringProcessing(string ErrorDescription)
        {
            return new ApiResult<TValue>(default(TValue), false, 461, ErrorDescription);
        }

        public static ApiResult<TValue> BadRequest(string ErrorDescription, string errorCode = null)
        {
            return new ApiResult<TValue>(default(TValue), false, (int)System.Net.HttpStatusCode.BadRequest, ErrorDescription, errorCode);
        }
        public static ApiResult<TValue> NotFound(string ErrorDescription, string errorCode = null)
        {
            return new ApiResult<TValue>(default(TValue), false, (int)System.Net.HttpStatusCode.NotFound, ErrorDescription, errorCode);
        }

        public static ApiResult<TValue> InternalServerError(string ErrorDescription)
        {
            return new ApiResult<TValue>(default(TValue), false, (int)System.Net.HttpStatusCode.InternalServerError, ErrorDescription);
        }

        public static ApiResult<TValue> NotAcceptable(string ErrorDescription)
        {
            return new ApiResult<TValue>(default(TValue), false, (int)System.Net.HttpStatusCode.NotAcceptable, ErrorDescription);
        }

        public static ApiResult<TValue> ValidationError(string ErrorDescription, string errorCode = null)
        {
            return new ApiResult<TValue>(default(TValue), false, 460, ErrorDescription, errorCode);
        }

        public static ApiResult<TValue> Fail(int httpStatusCode, string ErrorDescription, string errorCode = null, TValue result = default(TValue))
        {
            return new ApiResult<TValue>(result, false, httpStatusCode, ErrorDescription, errorCode);
        }

        public static ApiResult<TValue> Translate(ApiResult result, TValue value)
        {
            return new ApiResult<TValue>(value, result.IsSuccessful, result.HttpStatusCode, result.ErrorDescription);
        }
    }

    public class ApiResult
    {
        public bool IsSuccessful { get; }
        public bool IsFailure => !IsSuccessful;
        public string ErrorDescription { get; }

        public int HttpStatusCode { get; }
        public string ErrorCode { get; }

        internal ApiResult(bool isSuccessful, int httpStatusCode, string errorDescription, string errorCode = null)
        {
            IsSuccessful = isSuccessful;
            ErrorDescription = errorDescription;
            HttpStatusCode = httpStatusCode;
            ErrorCode = errorCode;
        }

        public static ApiResult Ok()
        {
            return new ApiResult(true, (int)System.Net.HttpStatusCode.OK, null);
        }

        public static ApiResult Accepted()
        {
            return new ApiResult(true, (int)System.Net.HttpStatusCode.Accepted, null);
        }

        public static ApiResult Created()
        {
            return new ApiResult(true, (int)System.Net.HttpStatusCode.Created, null);
        }

        public static ApiResult NoContent()
        {
            return new ApiResult(true, (int)System.Net.HttpStatusCode.NoContent, null);
        }

        public static ApiResult Forbidden(string ErrorDescription)
        {
            return new ApiResult(false, (int)System.Net.HttpStatusCode.Forbidden, ErrorDescription);
        }

        public static ApiResult Unauthorized(string ErrorDescription)
        {
            return new ApiResult(false, (int)System.Net.HttpStatusCode.Unauthorized, ErrorDescription);
        }

        public static ApiResult BadRequest(string ErrorDescription)
        {
            return new ApiResult(false, (int)System.Net.HttpStatusCode.BadRequest, ErrorDescription);
        }

        public static ApiResult NotFound(string ErrorDescription, string errorCode = null)
        {
            return new ApiResult(false, (int)System.Net.HttpStatusCode.NotFound, ErrorDescription, errorCode);
        }

        public static ApiResult InternalServerError(string ErrorDescription)
        {
            return new ApiResult(false, (int)System.Net.HttpStatusCode.InternalServerError, ErrorDescription);
        }

        public static ApiResult Fail(int httpStatusCode, string ErrorDescription, string errorCode = null)
        {
            return new ApiResult(false, httpStatusCode, ErrorDescription, errorCode);
        }

        public static ApiResult ValidationError(string ErrorDescription, string errorCode = null)
        {
            return new ApiResult(false, 460, ErrorDescription, errorCode);
        }

        public static ApiResult Translate<TValue>(ApiResult<TValue> result)
        {
            return new ApiResult(result.IsSuccessful, result.HttpStatusCode, result.ErrorDescription);
        }
    }
}
