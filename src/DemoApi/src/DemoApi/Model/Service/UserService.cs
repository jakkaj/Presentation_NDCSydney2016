using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DemoApi.Model.Contract;

namespace DemoApi.Model.Service
{
    public class UserService : IUserService
    {
        public string Name { get; set; }
        public string Id { get; set; }

        public async Task SetupUser(IEnumerable<Claim> claims)
        {
            var surname = claims.FirstOrDefault(_ => _.Type == ClaimTypes.Surname)?.Value;
            var firstName = claims.FirstOrDefault(_ => _.Type == ClaimTypes.GivenName)?.Value;
            var id = claims.FirstOrDefault(_ => _.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value;

            Name = $"{firstName} {surname}";
            Id = id;
        }
    }
}
