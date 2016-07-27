using ExtensionGoo.Standard.Extensions;
using System.Threading.Tasks;

namespace TokenFunctionHelper.TokenStuff
{
    public static class TokenValidator
    {
        //https://jwtparse.azurewebsites.net/api/JWTWithRsaValidator?code=r3gqblabmyfju0c6zbhknwyiimhdrdn1yld7
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
