using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoApi.Model.Entity
{
    public class SigningSettings
    {
        public string TokenValidIssuer { get; set; }
        public string TokenAllowedAudience { get; set; }

        public string RSAPublic { get; set; }
        public string ValidationEndpoint { get; set; }
    }
}
