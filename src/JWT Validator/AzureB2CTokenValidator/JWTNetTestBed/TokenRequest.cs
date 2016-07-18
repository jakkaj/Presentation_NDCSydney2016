using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace JWTNetTestBed
{
    public class TokenRequest
    {
        [JsonProperty("token")]
        public string Token { get; set; }
        [JsonProperty("rsaKey")]
        public string RsaKey { get; set; }
        [JsonProperty("issuer")]
        public string Issuer { get; set; }
        [JsonProperty("audience")]
        public string Audience { get; set; }
    }
}
