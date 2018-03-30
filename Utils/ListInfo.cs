using System.Collections.Generic;

namespace ArtaInfra.Utils
{
    public class ListInfo : ActionLinks
    {
        public int Size { get; set; }
        public int Count { get; set; }
        public bool HasMore { get; set; }
    }

    public class ActionLinks
    {
        public Dictionary<string, string> Links { get; set; } = new Dictionary<string, string>();
    }
}
