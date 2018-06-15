using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Arta.Infrastructure.Logging;
using Arta.Sessions.Api.ArtaInfra.Middleware;
using Newtonsoft.Json;

namespace Arta.Infrastructure.Middleware
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestResponseLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IApiLogger logger)
        {
            logger.LogInformation(await GetRequestLogging(context), LoggingType.Request);

            var bodyStream = context.Response.Body;
            var responseBodyStream = new MemoryStream();
            context.Response.Body = responseBodyStream;

            await _next(context);

            logger.LogInformation(await GetResponseLogging(context.Response, context.Request.Path, responseBodyStream, bodyStream), LoggingType.Response);
        }

        public async Task<Request> GetRequestLogging(HttpContext context)
        {
            var request = context.Request;
            var requestBodyStream = new MemoryStream();

            await request.Body.CopyToAsync(requestBodyStream);
            requestBodyStream.Seek(0, SeekOrigin.Begin);

            var requestBodyText = await new StreamReader(requestBodyStream).ReadToEndAsync();

            requestBodyText = Regex.Replace(requestBodyText, @"^\s*$\n", string.Empty, RegexOptions.Multiline).TrimEnd();

   
            var requestLog = new Request
            {
                Method = request.Method,
                Url = $"{request.Path}{request.QueryString}",
                Body = requestBodyText,
                Client = context.Connection.RemoteIpAddress.ToString()
            };

            return requestLog;
        }

        public async Task<Response> GetResponseLogging(HttpResponse response, string path, MemoryStream responseBodyStream, Stream bodyStream)
        {
            responseBodyStream.Seek(0, SeekOrigin.Begin);
            var responseBody = new StreamReader(responseBodyStream).ReadToEnd();

            responseBody = Regex.Replace(responseBody, @"^\s*$\n", string.Empty, RegexOptions.Multiline).TrimEnd();

            var body = responseBody.IsValidJson() ? JsonConvert.DeserializeObject(responseBody) : responseBody;

            var responseLog = new Response
            {
                Code = response.StatusCode.ToString(),
                Body = !IsBodyIgnoredFromResponseLogging(path) ? responseBody : "Ignored"
            };

            responseBodyStream.Seek(0, SeekOrigin.Begin);

            if (response.StatusCode != 204)
                await responseBodyStream.CopyToAsync(bodyStream);

            return responseLog;
        }

        private static bool IsBodyIgnoredFromResponseLogging(string path)
        {
            var paths = new List<string>
            {
            };

            return paths.Any(x => x == path);
        }
    }
}