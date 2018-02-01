using System.Net;

namespace Arta.Infrastructure
{
    public class ApiResult<TValue>
    {
        public bool IsSuccessful { get; }
        public bool IsFailure => !IsSuccessful;
        public string ErrorMessage { get; }

        public int HttpStatusCode { get; }
        public TValue Value { get; }
        public string ErrorCode { get; }

        internal ApiResult(TValue value, bool isSuccessful, int httpStatusCode, string errorMessage, string errorCode = null)
        {
            Value = value;
            IsSuccessful = isSuccessful;
            ErrorMessage = errorMessage;
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

        public static ApiResult<TValue> Forbidden(string errorMessage)
        {
            return new ApiResult<TValue>(default(TValue), false, (int)System.Net.HttpStatusCode.Forbidden, errorMessage);
        }

        public static ApiResult<TValue> Unauthorized(string errorMessage)
        {
            return new ApiResult<TValue>(default(TValue), false, (int)System.Net.HttpStatusCode.Unauthorized, errorMessage);
        }

        public static ApiResult<TValue> BadRequest(string errorMessage)
        {
            return new ApiResult<TValue>(default(TValue), false, (int)System.Net.HttpStatusCode.BadRequest, errorMessage);
        }

        public static ApiResult<TValue> BadRequest(string errorMessage, string errorCode)
        {
            return new ApiResult<TValue>(default(TValue), false, (int)System.Net.HttpStatusCode.BadRequest, errorMessage, errorCode);
        }
        public static ApiResult<TValue> NotFound(string errorMessage)
        {
            return new ApiResult<TValue>(default(TValue), false, (int)System.Net.HttpStatusCode.NotFound, errorMessage);
        }

        public static ApiResult<TValue> InternalServerError(string errorMessage)
        {
            return new ApiResult<TValue>(default(TValue), false, (int)System.Net.HttpStatusCode.InternalServerError, errorMessage);
        }

        public static ApiResult<TValue> ValidationError(string errorMessage, string errorCode)
        {
            return new ApiResult<TValue>(default(TValue), false, 460, errorMessage, errorCode);
        }

        public static ApiResult<TValue> Fail(int httpStatusCode, string errorMessage, string errorCode, TValue result = default(TValue))
        {
            return new ApiResult<TValue>(result, false, httpStatusCode, errorMessage);
        }

        public static ApiResult<TValue> Translate(ApiResult result, TValue value)
        {
            return new ApiResult<TValue>(value, result.IsSuccessful, result.HttpStatusCode, result.ErrorMessage);
        }
    }

    public class ApiResult
    {
        public bool IsSuccessful { get; }
        public bool IsFailure => !IsSuccessful;
        public string ErrorMessage { get; }

        public int HttpStatusCode { get; }
        public string ErrorCode { get; }

        internal ApiResult(bool isSuccessful, int httpStatusCode, string errorMessage, string errorCode = null)
        {
            IsSuccessful = isSuccessful;
            ErrorMessage = errorMessage;
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

        public static ApiResult Forbidden(string errorMessage)
        {
            return new ApiResult(false, (int)System.Net.HttpStatusCode.Forbidden, errorMessage);
        }

        public static ApiResult Unauthorized(string errorMessage)
        {
            return new ApiResult(false, (int)System.Net.HttpStatusCode.Unauthorized, errorMessage);
        }

        public static ApiResult BadRequest(string errorMessage)
        {
            return new ApiResult(false, (int)System.Net.HttpStatusCode.BadRequest, errorMessage);
        }

        public static ApiResult NotFound(string errorMessage)
        {
            return new ApiResult(false, (int)System.Net.HttpStatusCode.NotFound, errorMessage);
        }

        public static ApiResult InternalServerError(string errorMessage)
        {
            return new ApiResult(false, (int)System.Net.HttpStatusCode.InternalServerError, errorMessage);
        }

        public static ApiResult Fail(int httpStatusCode, string errorMessage, string errorCode = null)
        {
            return new ApiResult(false, httpStatusCode, errorMessage);
        }

        public static ApiResult ValidationError(string errorMessage, string errorCode)
        {
            return new ApiResult(false, 460, errorMessage, errorCode);
        }

        public static ApiResult Translate<TValue>(ApiResult<TValue> result)
        {
            return new ApiResult(result.IsSuccessful, result.HttpStatusCode, result.ErrorMessage);
        }
    }
}