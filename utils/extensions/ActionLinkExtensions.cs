using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;

namespace ArtaInfra.Utils.Extensions
{
    public static class ActionLinkExtensions
    {
        public static string GetSelfLink(this IHttpContextAccessor httpContextAccessor)
        {
            return httpContextAccessor.HttpContext?.Request?.GetDisplayUrl();
        }

        public static Dictionary<string, string> AddSelfLink(this Dictionary<string, string> links, IHttpContextAccessor httpContextAccessor)
        {
            if (links == null) links = new Dictionary<string, string>();
            links.Add("URL", $"{httpContextAccessor.GetSelfLink()}");
            return links;
        }

        public static Dictionary<string, string> AddLink(this Dictionary<string, string> links, IHttpContextAccessor httpContextAccessor, string linkName, string partner, string reference)
        {
            if (links == null) links = new Dictionary<string, string>();
            var partnerRef = string.IsNullOrEmpty(partner) ? "" : $"partners/{partner}";
            links.Add($"{linkName}", $"{httpContextAccessor.HttpContext?.Request?.Scheme}://{httpContextAccessor.HttpContext?.Request?.Host}{httpContextAccessor.HttpContext?.Request?.PathBase}/{partnerRef}/{reference}");
            return links;
        }

        public static Dictionary<string, string> AddLink(this Dictionary<string, string> links, string linkName, string link)
        {
            if (links == null) links = new Dictionary<string, string>();
            links.Add($"{linkName}", link);
            return links;
        }

        public static Dictionary<string, string> AddPagingLink(this Dictionary<string, string> links, IHttpContextAccessor httpContextAccessor, string linkName, int offset, int limit)
        {
            if (links == null) links = new Dictionary<string, string>();
            var link = httpContextAccessor.HttpContext?.Request?.GetDisplayUrl();

            if (link != null)
            {
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
            }

            return links;
        }
    }
}
