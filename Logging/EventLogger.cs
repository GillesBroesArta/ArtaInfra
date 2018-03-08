using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Arta.Infrastructure.Logging
{
    public class EventLogger : IEventLogger
    {
        private readonly ILogger<EventLogger> _logger;

        public EventLogger(ILogger<EventLogger> logger)
        {
            _logger = logger;
        }

        public void LogInformation(string message, Guid id)
        {
            Log("EventInformation", new { message, id });
        }

        public void LogException(Exception ex, Guid id)
        {
            LogException(null, ex, id);
        }

        public void LogException(string message, Exception ex, Guid id)
        {
            Log("EventError", new { message, exception = ex.ToString(), id });
        }
        
        public void LogError(string error, Guid id)
        {
            Log("EventError", new { error, id });
        }

        private void Log<T>(string category, T contents)
        {
            var settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            var logging = $"[{category}] {JsonConvert.SerializeObject(contents, settings)}";
            _logger.LogInformation(logging);
        }
    }
}
