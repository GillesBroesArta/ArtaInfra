using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Arta.Infrastructure.Logging;

namespace Arta.Infrastructure.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IApiLogger logger)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                logger.LogException(ex);

                var result = JsonConvert.SerializeObject(new Error { ErrorDescription = "An unhandled exception occurred." });
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response.WriteAsync(result);
            }
        }
    }
}