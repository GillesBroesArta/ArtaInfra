namespace Arta.Infrastructure
{
    public class Error
    {
        public string ErrorMessage { get; set; }
    }

    public class ErrorWithErrorCode
    {
        public string ErrorMessage { get; set; }
        public string ErrorCode { get; set; }
    }
}