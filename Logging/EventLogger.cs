using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Arta.Infrastructure.Logging
{
    public class EventLogger : IEventLogger
    {
        private readonly ILogger<EventLogger> _logger;
        private readonly JsonSerializerSettings _settings;

        public EventLogger(ILogger<EventLogger> logger)
        {
            _logger = logger;
            _settings = new JsonSerializerSettings();
            _settings.NullValueHandling = NullValueHandling.Ignore;
        }

        public void LogInformation(string message, Guid? id)
        {
            Log(new { message, id });
        }

        public void LogException(Exception ex, Guid? id)
        {
            LogException(null, ex, id);
        }

        public void LogException(string message, Exception ex, Guid? id)
        {

            var logging = new {message, exception = ex.ToString(), id};
            _logger.LogCritical($"{JsonConvert.SerializeObject(logging, _settings)}");
        }
        
        public void LogError(string error, Guid? id)
        {
            var logging = new {error, id};
            _logger.LogError($"{JsonConvert.SerializeObject(logging, _settings)}");
        }

        private void Log<T>(T contents)
        {
            var logging = $"{JsonConvert.SerializeObject(contents, _settings)}";
            _logger.LogInformation(logging);
        }
    }
}
