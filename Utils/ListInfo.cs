using System.Collections.Generic;

namespace ArtaInfra.Utils
{
    public class ListInfo : ActionLinks
    {
        public int Size { get; set; } = 0;
        public int Count { get; set; } = 0;
        public bool HasMore { get; set; } = false;
    }

    public class ActionLinks
    {
        public Dictionary<string, string> Links { get; set; } = new Dictionary<string, string>();
    }
}
