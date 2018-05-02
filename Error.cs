namespace Arta.Infrastructure
{
    public class Error
    {
        public string ErrorDescription { get; set; }
    }

    public class ErrorWithErrorCode
    {
        public string ErrorDescription { get; set; }
        public string ErrorCode { get; set; }
    }
}
