using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TokenFunctionHelper.Contract
{
    public interface IUserService
    {
        Task SetupUser(IEnumerable<Claim> claims);
        string Name { get; set; }
        string Id { get; set; }
    }
}