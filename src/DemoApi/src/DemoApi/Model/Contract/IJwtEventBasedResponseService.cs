using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace DemoApi.Model.Contract
{
    public interface IJwtEventBasedResponseService
    {
        Task OnMessageReceived(MessageReceivedContext context);
    }
}