using Arta.Sessions.Api.ArtaInfra.Middleware;
using Microsoft.AspNetCore.Builder;

namespace Arta.Infrastructure.Middleware { 
    public static class Extensions
    {
        public static IApplicationBuilder UseRequestResponseLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestResponseLoggingMiddleware>();
        }

        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlerMiddleware>();
        }

        public static IApplicationBuilder UseGenericFailureToApiResponseMapper(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GenericFailureToApiResponseMapper>();
        }
    }
}