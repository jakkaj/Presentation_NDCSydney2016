using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace TokenFunctionHelper.Contract
{
    public interface IJwtEventBasedResponseService
    {
        Task OnMessageReceived(MessageReceivedContext context);
    }
}