using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Arta.Infrastructure;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Arta.Sessions.Api.ArtaInfra.Middleware
{
    public class GenericFailureToApiResponseMapper
    {

        private readonly RequestDelegate _next;

        public GenericFailureToApiResponseMapper(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var currentBody = context.Response.Body;

            using (var memoryStream = new MemoryStream())
            {
                //set the current response to the memorystream.
                context.Response.Body = memoryStream;

                try
                {
                    await _next(context);
                }
                catch (Exception e)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                }
                
                //reset the body 
                context.Response.Body = currentBody;
                memoryStream.Seek(0, SeekOrigin.Begin);

                var readToEnd = new StreamReader(memoryStream).ReadToEnd();
                var objResult = JsonConvert.DeserializeObject(readToEnd);

                if (context.Response.StatusCode > 400 && objResult == null)
                {
                    var result = StausCodeMapper.Map((HttpStatusCode)context.Response.StatusCode);
                    
                    //Ideally this should be retrieved from System.Net.Mime.Mediatypes but application/json does not yet exist
                    //So manually added the exact type with charset from an example response that's set by asp.net core (used the else part
                    //of this if to see what asp.net core uses by default)
                    //See: https://github.com/dotnet/corefx/issues/24895
                    context.Response.ContentType = "application / json; charset = utf - 8";
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(result));

                }
                else
                {
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(objResult));
                }

            }
        }

    }

    public static class StausCodeMapper
    {

        private static readonly Dictionary<HttpStatusCode, ErrorWithErrorCode> StatusCodeMap = new Dictionary<HttpStatusCode, ErrorWithErrorCode>
        {
            { HttpStatusCode.Unauthorized, new ErrorWithErrorCode{ErrorCode = "000001", ErrorDescription = "You have no access to this resource."} },
            { HttpStatusCode.NotFound, new ErrorWithErrorCode{ErrorCode = "000002", ErrorDescription = "Resource not found."} },
            { HttpStatusCode.InternalServerError, new ErrorWithErrorCode{ErrorCode = "000003", ErrorDescription = "Something went wrong. Please try again in a few minutes or contact your support team."} },
        };

        public static ErrorWithErrorCode Map(HttpStatusCode statusCode)
        {
            return StatusCodeMap.GetValueOrDefault(statusCode, null);
        }
    }
}
