using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Arta.Infrastructure.Logging
{
    public class CommandLogger : ICommandLogger
    {
        private readonly ILogger<CommandLogger> _logger;

        public CommandLogger(ILogger<CommandLogger> logger)
        {
            _logger = logger;
        }

        public void LogCommand(string category, string user, string uid, string description, string updatedValue, string originalValue)
        {            
            Log("Command", new {  category, user, description, updatedValue, originalValue });
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
