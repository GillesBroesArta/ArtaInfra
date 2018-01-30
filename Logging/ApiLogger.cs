namespace AArta.nfrastructure.Logging
{
    public interface IApiLogger
    {
        void LogInformation(string message, LoggingType loggingType);
        void LogError(string message, LoggingType loggingType);
    }
}