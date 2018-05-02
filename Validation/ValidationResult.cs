
namespace Arta.Infrastructure.Validation
{
    public class ValidationResult
    {
        public bool IsSuccessful { get; }
        public bool IsFailure => !IsSuccessful;
        public string ErrorMessage { get; }
        public string ErrorCode { get; set; }

        public int HttpStatusCode { get; }
        public string Error;

        internal ValidationResult(bool isSuccessful, int httpStatusCode, string errorMessage, string errorCode)
        {
            IsSuccessful = isSuccessful;
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
            HttpStatusCode = httpStatusCode;
        }

        public static ValidationResult Ok()
        {
            return new ValidationResult(true, (int)System.Net.HttpStatusCode.OK, null, null);
        }

        public static ValidationResult Forbidden(string errorMessage, string errorCode = null)
        {
            return new ValidationResult(false, (int)System.Net.HttpStatusCode.Forbidden, errorMessage, errorCode);
        }

        public static ValidationResult Unauthorized(string errorMessage, string errorCode = null)
        {
            return new ValidationResult(false, (int)System.Net.HttpStatusCode.Unauthorized, errorMessage, errorCode);
        }

        public static ValidationResult BadRequest(string errorMessage, string errorCode = null)
        {
            return new ValidationResult(false, (int)System.Net.HttpStatusCode.BadRequest, errorMessage, errorCode);
        }

        public static ValidationResult NotFound(string errorMessage, string errorCode = null)
        {
            return new ValidationResult(false, (int)System.Net.HttpStatusCode.NotFound, errorMessage, errorCode);
        }

        public static ValidationResult InternalServerError(string errorMessage, string errorCode = null)
        {
            return new ValidationResult(false, (int)System.Net.HttpStatusCode.InternalServerError, errorMessage, errorCode);
        }

        public static ValidationResult NotAcceptable(string errorMessage, string errorCode = null)
        {
            return new ValidationResult(false, (int)System.Net.HttpStatusCode.NotAcceptable, errorMessage, errorCode);
        }

        public static ValidationResult ValidationError(string errorMessage, string errorCode = null)
        {
            return new ValidationResult(false, 460, errorMessage, errorCode);
        }

    }
}