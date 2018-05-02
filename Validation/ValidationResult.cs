
namespace Arta.Infrastructure.Validation
{
    public class ValidationResult
    {
        public bool IsSuccessful { get; }
        public bool IsFailure => !IsSuccessful;
        public string ErrorDescription { get; }
        public string ErrorCode { get; set; }

        public int HttpStatusCode { get; }
        public string Error;

        internal ValidationResult(bool isSuccessful, int httpStatusCode, string ErrorDescription, string errorCode)
        {
            IsSuccessful = isSuccessful;
            ErrorCode = errorCode;
            ErrorDescription = ErrorDescription;
            HttpStatusCode = httpStatusCode;
        }

        public static ValidationResult Ok()
        {
            return new ValidationResult(true, (int)System.Net.HttpStatusCode.OK, null, null);
        }

        public static ValidationResult Forbidden(string ErrorDescription, string errorCode = null)
        {
            return new ValidationResult(false, (int)System.Net.HttpStatusCode.Forbidden, ErrorDescription, errorCode);
        }

        public static ValidationResult Unauthorized(string ErrorDescription, string errorCode = null)
        {
            return new ValidationResult(false, (int)System.Net.HttpStatusCode.Unauthorized, ErrorDescription, errorCode);
        }

        public static ValidationResult BadRequest(string ErrorDescription, string errorCode = null)
        {
            return new ValidationResult(false, (int)System.Net.HttpStatusCode.BadRequest, ErrorDescription, errorCode);
        }

        public static ValidationResult NotFound(string ErrorDescription, string errorCode = null)
        {
            return new ValidationResult(false, (int)System.Net.HttpStatusCode.NotFound, ErrorDescription, errorCode);
        }

        public static ValidationResult InternalServerError(string ErrorDescription, string errorCode = null)
        {
            return new ValidationResult(false, (int)System.Net.HttpStatusCode.InternalServerError, ErrorDescription, errorCode);
        }

        public static ValidationResult NotAcceptable(string ErrorDescription, string errorCode = null)
        {
            return new ValidationResult(false, (int)System.Net.HttpStatusCode.NotAcceptable, ErrorDescription, errorCode);
        }

        public static ValidationResult ValidationError(string ErrorDescription, string errorCode = null)
        {
            return new ValidationResult(false, 460, ErrorDescription, errorCode);
        }

    }
}