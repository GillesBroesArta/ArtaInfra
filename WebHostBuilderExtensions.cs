using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.PlatformAbstractions;
using Serilog.Context;

namespace Arta.Sessions.Api.ArtaInfra
{
    public static class WebHostBuilderExtensions
    {

        public static IWebHostBuilder RegisterService(this IWebHostBuilder webhostBuilder)
        {

            var serviceId = Guid.NewGuid().ToString();
            var serviceName = PlatformServices.Default.Application.ApplicationName;
            var geoLocation = Environment.GetEnvironmentVariable("GEOLOCATION");
            Environment.SetEnvironmentVariable("SERVICEID", serviceId);
            Environment.SetEnvironmentVariable("SERVICENAME", serviceName);

            var vars = Environment.GetEnvironmentVariables();

            using (var containerInfo = File.AppendText("/opt/containerinfo/containerinfo"))
            {
                containerInfo.WriteLine($"{DateTime.Now}-{serviceName}-{geoLocation}-{serviceId}");
            }

            return webhostBuilder;
        }
    }

}
