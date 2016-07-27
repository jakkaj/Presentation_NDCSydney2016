using Newtonsoft.Json;

namespace TokenFunctionHelper.TokenStuff
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
