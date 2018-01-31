using System;
using Microsoft.Extensions.Logging;

namespace Arta.Infrastructure.Logging
{
    public class ConsumerLogger : IConsumerLogger
    {
        private readonly ILogger<ConsumerLogger> _logger;

        public ConsumerLogger(ILogger<ConsumerLogger> logger)
        {
            _logger = logger;
        }

        public void LogInformation(string message, Guid id)
        {
            var fileLogging = $"[{id}] - {message}";

            _logger.LogInformation(fileLogging);
        }

        public void LogError(string message, Guid id)
        {
            var fileLogging = $"[{id}] - {message}";

            _logger.LogError(fileLogging);
        }
    }
}
