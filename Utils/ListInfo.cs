using System.Collections.Generic;
using Arta.Subscriptions.Api.ArtaInfra.Utils.Helpers;
using Newtonsoft.Json;

namespace ArtaInfra.Utils
{
    public class ListInfo : ActionLinks
    {
        public int Size { get; set; } = 0;
        public int Count { get; set; } = 0;

        [JsonConverter(typeof(BooleanYesNoConverter))]
        public bool HasMore { get; set; } = false;
    }

    public class ActionLinks
    {
        public Dictionary<string, string> Links { get; set; } = new Dictionary<string, string>();
    }
}
