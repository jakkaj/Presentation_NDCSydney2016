using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TokenFunctionHelper.Contract;

namespace DemoApi.Model.OWIN
{
    public class CustomMiddleware
    {
        private readonly RequestDelegate _next;
       

        public CustomMiddleware(RequestDelegate next)
        {
            _next = next;
            
        }

        public async Task Invoke(HttpContext context, IUserService userService)
        {
            var auth = context.User;

            if (auth == null)
            {
                await _next.Invoke(context);
                return;
            }

            var userClaims = auth.Claims;

            await userService.SetupUser(userClaims);

            await _next.Invoke(context);
        }
    }
}
