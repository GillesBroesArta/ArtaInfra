using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Arta.Infrastructure.Logging
{
    public class ApiLogger : IApiLogger
    {
        private readonly ILogger<ApiLogger> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly JsonSerializerSettings _settings;

        public ApiLogger(ILogger<ApiLogger> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _settings = new JsonSerializerSettings();
            _settings.NullValueHandling = NullValueHandling.Ignore;
        }

        public void LogInformation<T>(T message, LoggingType? loggingType = null) 
        {
            Log(new { message, loggingType = loggingType != null ? loggingType.ToString() : null, traceIdentifier = _httpContextAccessor.HttpContext.TraceIdentifier});
        }

        public void LogException(Exception ex)
        {
            _logger.LogCritical(ex.ToString());
        }

        public void LogException(string message, Exception ex)
        {
            var logging = new { message, exception = ex.ToString(), traceIdentifier = _httpContextAccessor.HttpContext.TraceIdentifier };
            _logger.LogCritical($"{JsonConvert.SerializeObject(logging, _settings)}");
        }
        
        public void LogError(string error)
        {
            _logger.LogError(new { error, traceIdentifier = _httpContextAccessor.HttpContext.TraceIdentifier }.ToString());
        }

        private void Log<T>(T contents)
        {
            var logging = $"{JsonConvert.SerializeObject(contents, _settings)}";
            _logger.LogInformation(logging);
        }
    }
}
