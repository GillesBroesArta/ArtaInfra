using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Arta.Infrastructure;
using Microsoft.AspNetCore.Http;
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

        public static string GetForwardedPathHeader(this IHttpContextAccessor httpContextAccessor)
        {
            StringValues stringValues;
            return httpContextAccessor.HttpContext.Request.Headers.TryGetValue("X-Forwarded-Path", out stringValues) ? stringValues.FirstOrDefault() : "";
        }

        public static string GetSelfLink(this IHttpContextAccessor httpContextAccessor)
        {
            var baseLink = GetBaseLink(httpContextAccessor);
            var urlPath = GetForwardedPathHeader(httpContextAccessor);
            var queryString = httpContextAccessor.HttpContext.Request.QueryString;
            return $"{baseLink}{urlPath}{queryString}";
        }
        
        public static string GetBaseLink(this IHttpContextAccessor httpContextAccessor)
        {
            var baseUrl = $"{GetForwardedHostHeader(httpContextAccessor).RemoveTrailingSlash()}";
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
            return GetFullLink(httpContextAccessor, partner, null, reference);
        }

        public static string GetFullLink(IHttpContextAccessor httpContextAccessor, string partner, string csp, string reference)
        {
            var cspRef = string.IsNullOrEmpty(csp) ? "" : $"/csps/{csp}";
            var partnerRef = string.IsNullOrEmpty(partner) ? "" : $"/partners/{partner}";
            return $"{GetBaseLink(httpContextAccessor)}{cspRef}{partnerRef}/{reference}";
        }

        public static string GetFullCspLink(IHttpContextAccessor httpContextAccessor, string csp, string reference)
        {
            return GetFullLink(httpContextAccessor, null, csp, reference);
        }

        public static Dictionary<string, string> AddLink(this Dictionary<string, string> links, IHttpContextAccessor httpContextAccessor,string linkName, string partner, string reference)
        {
            return AddLink(links, httpContextAccessor, linkName, partner, null, reference);
        }

        public static Dictionary<string, string> AddLink(this Dictionary<string, string> links, IHttpContextAccessor httpContextAccessor, string linkName, string partner, string csp, string reference)
        {
            linkName = ToFirstCapitalCase(linkName);
            if (links == null) links = new Dictionary<string, string>();
            links.Add($"{linkName}", GetFullLink(httpContextAccessor, partner, csp, reference));
            return links;
        }

        public static Dictionary<string, string> AddCspLink(this Dictionary<string, string> links, IHttpContextAccessor httpContextAccessor, string linkName, string csp, string reference)
        {
            return AddLink(links, httpContextAccessor, linkName, null, csp, reference);
        }

        public static Dictionary<string, string> AddPagingLink(this Dictionary<string, string> links, IHttpContextAccessor httpContextAccessor,string linkName, int offset, int limit)
        {
            linkName = ToFirstCapitalCase(linkName); 
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

        private static string ToFirstCapitalCase(string input)
        {
            var converted = input;
            if (!string.IsNullOrEmpty(input))
            {
                converted = char.ToUpper(input[0]) + input.Substring(1);
            }
            return converted;
        }

    }
}