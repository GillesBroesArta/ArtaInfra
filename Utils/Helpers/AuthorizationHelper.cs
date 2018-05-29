using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Arta.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Primitives;

namespace ArtaInfra.Utils.Helpers
{
    public static class AuthorizationHelper
    {   
        private static bool hasClaimWithValue(HttpContextAccessor httpContextAccessor, string type, string value) {
            return httpContextAccessor.HttpContext.User.Claims.Any(c => c.Type == type && c.Value.Equals(value, StringComparison.InvariantCultureIgnoreCase));
        }
        public static bool isCspAdmin(HttpContextAccessor httpContextAccessor, string csp)
        {
            return hasClaimWithValue(httpContextAccessor, "isCspAdmin", csp);
        }
        public static bool isCspSales(HttpContextAccessor httpContextAccessor, string csp)
        {
            return hasClaimWithValue(httpContextAccessor, "isCspSales", csp);
        }
        public static bool isCspPlatformConfiguration(HttpContextAccessor httpContextAccessor, string csp)
        {
            return hasClaimWithValue(httpContextAccessor, "isCspPlatformConfiguration", csp);
        }
        public static bool isCspPartnerSupport(HttpContextAccessor httpContextAccessor, string csp)
        {
            return hasClaimWithValue(httpContextAccessor, "isCspPartnerSupport", csp);
        }
    }        
}
