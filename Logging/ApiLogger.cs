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

        public ApiLogger(ILogger<ApiLogger> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public void LogInformation(string message, LoggingType? loggingType = null) 
        {
            Log("ApiInformation", new { message, loggingType = loggingType != null ? loggingType.ToString() : null, traceIdentifier = _httpContextAccessor.HttpContext.TraceIdentifier});
        }

        public void LogException(Exception ex)
        {
            LogException(null, ex);
        }

        public void LogException(string message, Exception ex)
        {
            Log("ApiError", new { message, exception = ex.ToString(),  traceIdentifier = _httpContextAccessor.HttpContext.TraceIdentifier });
        }
        
        public void LogError(string error)
        {
            Log("ApiError", new { error,  traceIdentifier = _httpContextAccessor.HttpContext.TraceIdentifier });
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
