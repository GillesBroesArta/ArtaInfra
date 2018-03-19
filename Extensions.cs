using System;
using System.Linq;
using System.Net;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using Remotion.Linq.Parsing.Structure;

namespace Arta.Infrastructure
{
    public static class Extensions
    {
        public static bool IsSuccessStatusCode(this HttpStatusCode statusCode) => ((int)statusCode >= 200) && ((int)statusCode <= 299);

        public static bool IsValidJson(this string json)
        {
            try
            {
                JsonConvert.DeserializeObject(json);
            }
            catch (JsonReaderException)
            {
                //Catch only specific exception thrown on invalid json
                //bubble other deserialize exceptions to result in a http 500 status
                return false;
            }

            return true;
        }

        public static string RemoveTrailingSlash(this string value) => value.EndsWith("/") ? value.TrimEnd('/') : value;

    }
}
