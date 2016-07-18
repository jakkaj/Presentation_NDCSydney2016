using System.Threading.Tasks;
using AzureFunctionsToolkit.Portable.Extensions;

namespace JWTNetTestBed
{
    public static class JWTValidateAgainstAzureFunction
    {
        public static async Task<TokenResult> Validate(string endPoint, string token, string rsaPublicKey, string issuer, string audience)
        {
            var entityToSend = new TokenRequest
            {
                Token = token,
                Audience = audience,
                Issuer = issuer,
                RsaKey = rsaPublicKey
            };

            var result = await endPoint.PostAndParse<TokenResult, TokenRequest>(entityToSend);

            return result;
        } 
    }
}
