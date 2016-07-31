using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TokenFunctionHelper.Contract;

namespace TokenFunctionHelper.Service
{
    public class UserService : IUserService
    {
        
        public UserService()
        {
            Debug.WriteLine("*************** setup");
        }


        public string Name { get; set; }
        public string Id { get; set; }

        public async Task SetupUser(IEnumerable<Claim> claims)
        {
            var surname = claims.FirstOrDefault(_ => _.Type == ClaimTypes.Surname)?.Value;

            if (string.IsNullOrWhiteSpace(surname))
            {
                return;
            }

            var firstName = claims.FirstOrDefault(_ => _.Type == ClaimTypes.GivenName)?.Value;
            var id = claims.FirstOrDefault(_ => _.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value;

            Name = $"{firstName} {surname}";
            Id = id;
        }
    }
}
