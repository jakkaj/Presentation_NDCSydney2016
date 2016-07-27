namespace TokenFunctionHelper.Entity
{
    public class SigningSettings
    {
        public string TokenValidIssuer { get; set; }
        public string TokenAllowedAudience { get; set; }

        public string RSAPublic { get; set; }
        public string ValidationEndpoint { get; set; }
    }
}
