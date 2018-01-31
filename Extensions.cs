using System;
using System.Linq;
using System.Net;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Remotion.Linq.Parsing.Structure;

namespace Arta.Infrastructure
{
    public static class Extensions
    {
        public static bool IsSuccessStatusCode(this HttpStatusCode statusCode) => ((int)statusCode >= 200) && ((int)statusCode <= 299);
    }
}
