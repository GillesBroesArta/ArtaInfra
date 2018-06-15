using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arta.Sessions.Api.ArtaInfra.Middleware
{
    public class Request
    {
        public string Method { get; set; }
        public string Url { get; set; }
        public string Body { get; set; }
        public string Client { get; set; }
    }
}
