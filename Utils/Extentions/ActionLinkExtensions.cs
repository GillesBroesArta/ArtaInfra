using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Arta.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Primitives;

namespace ArtaInfra.Utils.Extensions
{
    public static class ActionLinkExtensions
    {

        public static string GetForwardedProtoHeader(this IHttpContextAccessor httpContextAccessor)
        {
            StringValues stringValues;
            return httpContextAccessor.HttpContext.Request.Headers.TryGetValue("X-Forwarded-Proto", out stringValues) ? stringValues.FirstOrDefault() : httpContextAccessor.HttpContext.Request.Scheme.ToString();
        }

        public static string GetForwardedHostHeader(this IHttpContextAccessor httpContextAccessor)
        {
            StringValues stringValues;
            return httpContextAccessor.HttpContext.Request.Headers.TryGetValue("X-Forwarded-Host", out stringValues) ? stringValues.FirstOrDefault() : httpContextAccessor.HttpContext.Request.Host.ToString();
        }

        public static string GetForwardedUriHeader(this IHttpContextAccessor httpContextAccessor)
        {
            StringValues stringValues;
            return httpContextAccessor.HttpContext.Request.Headers.TryGetValue("X-Forwarded-Uri", out stringValues) ? stringValues.FirstOrDefault() : httpContextAccessor.HttpContext.Request.Path.ToString();
        }

        public static string GetSelfLink(this IHttpContextAccessor httpContextAccessor)
        {
            var urlPath = GetForwardedUriHeader(httpContextAccessor);
            return $"{GetBaseLink(httpContextAccessor)}{urlPath}{httpContextAccessor.HttpContext.Request.QueryString}";
        }
        
        public static string GetBaseLink(this IHttpContextAccessor httpContextAccessor)
        {
            var baseUrl = GetForwardedHostHeader(httpContextAccessor);
            var protocol = GetForwardedProtoHeader(httpContextAccessor);
            return $"{protocol}://{baseUrl.RemoveTrailingSlash()}";
        }

        public static Dictionary<string, string> AddSelfLink(this Dictionary<string, string> links, IHttpContextAccessor httpContextAccessor)
        {
            if (links == null ) links = new Dictionary<string, string>();
            links.Add("URL",$"{httpContextAccessor.GetSelfLink()}");
            return links;
        }

        public static string GetFullLink(IHttpContextAccessor httpContextAccessor, string partner, string reference)
        {
            var partnerRef = string.IsNullOrEmpty(partner) ? "" : $"/partners/{partner}";
            return $"{GetBaseLink(httpContextAccessor)}{partnerRef}/{reference}";
        }


        public static string GetFullCspLink(IHttpContextAccessor httpContextAccessor, string csp, string reference)
        {
            var cspRef = string.IsNullOrEmpty(csp) ? "" : $"/csps/{csp}";
            return $"{GetBaseLink(httpContextAccessor)}{cspRef}/{reference}";
        }

        public static Dictionary<string, string> AddLink(this Dictionary<string, string> links, IHttpContextAccessor httpContextAccessor,string linkName, string partner, string reference)
        {
            linkName = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(linkName);
            if (links == null ) links = new Dictionary<string, string>();
            links.Add($"{linkName}", GetFullLink(httpContextAccessor, partner, reference));
            return links;
        }

        public static Dictionary<string, string> AddCspLink(this Dictionary<string, string> links, IHttpContextAccessor httpContextAccessor, string linkName, string csp, string reference)
        {
            linkName = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(linkName);
            if (links == null) links = new Dictionary<string, string>();
            links.Add($"{linkName}", GetFullCspLink(httpContextAccessor, csp, reference));
            return links;
        }

        public static Dictionary<string, string> AddPagingLink(this Dictionary<string, string> links, IHttpContextAccessor httpContextAccessor,string linkName, int offset, int limit)
        {
            linkName = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(linkName); 
            if (links == null ) links = new Dictionary<string, string>();
            var link = GetSelfLink(httpContextAccessor);

            if (link.Contains("offset"))
            {
                //Update Offset
                var offsetRegex = @"offset=\d+";
                link = Regex.Replace(link, offsetRegex, $"offset={offset}");                
            }
            else
            {
                //Add filter sign for get all
                link = !link.Contains("?") ? $"{link}?offset={offset}" : $"{link}&offset={offset}";
            }

            if (link.Contains("limit"))
            {
                //Update limit
                var limitRegex = @"limit=\d+";
                link = Regex.Replace(link, limitRegex, $"limit={limit}");
            }
            else link = !link.Contains("?") ? $"{link}?limit={limit}" : $"{link}&limit={limit}";
            links.Add($"{linkName}", $"{link}");
            return links;
        }
    }        
}