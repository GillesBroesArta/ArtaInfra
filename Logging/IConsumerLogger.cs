using System;

namespace Arta.Infrastructure.Logging
{
    public interface IConsumerLogger
    {
        void LogInformation(string message, Guid id);
        void LogError(string message, Guid id);
    }
}