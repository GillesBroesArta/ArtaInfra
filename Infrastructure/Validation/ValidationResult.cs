using System.Net;

namespace Arta.Infrastructure.Validation
{
    public class ValidationResult
    {
        public bool IsSuccessful { get; }
        public bool IsFailure => !IsSuccessful;
        public string ErrorMessage { get; }
        public string ErrorCode { get; set; }

        public HttpStatusCode HttpStatusCode { get; }
        public string Error;

        internal ValidationResult(bool isSuccessful, HttpStatusCode httpStatusCode, string errorMessage, string errorCode)
        {
            IsSuccessful = isSuccessful;
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
            HttpStatusCode = httpStatusCode;
        }

        public static ValidationResult Ok()
        {
            return new ValidationResult(true, HttpStatusCode.OK, null, null);
        }

        public static ValidationResult Forbidden(string errorMessage, string errorCode)
        {
            return new ValidationResult(false, HttpStatusCode.Forbidden, errorMessage, errorCode);
        }

        public static ValidationResult Unauthorized(string errorMessage, string errorCode)
        {
            return new ValidationResult(false, HttpStatusCode.Unauthorized, errorMessage, errorCode);
        }

        public static ValidationResult BadRequest(string errorMessage, string errorCode)
        {
            return new ValidationResult(false, HttpStatusCode.BadRequest, errorMessage, errorCode);
        }

        public static ValidationResult NotFound(string errorMessage, string errorCode)
        {
            return new ValidationResult(false, HttpStatusCode.NotFound, errorMessage, errorCode);
        }

        public static ValidationResult InternalServerError(string errorMessage, string errorCode)
        {
            return new ValidationResult(false, HttpStatusCode.InternalServerError, errorMessage, errorCode);
        }

        public static ValidationResult NotAcceptable(string errorMessage, string errorCode)
        {
            return new ValidationResult(false, HttpStatusCode.NotAcceptable, errorMessage, errorCode);
        }
    }
}