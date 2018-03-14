using System;

namespace Arta.Infrastructure.Logging
{
    public interface IEventLogger
    {
        void LogInformation(string message, Guid? id);
        void LogException(Exception ex, Guid? id);
        void LogException(string message, Exception ex, Guid? id);
        void LogError(string error, Guid? id);
    }
}