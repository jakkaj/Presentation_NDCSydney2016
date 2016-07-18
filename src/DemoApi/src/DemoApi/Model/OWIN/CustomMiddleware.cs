using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoApi.Model.Contract;
using Microsoft.AspNetCore.Http;

namespace DemoApi.Model.OWIN
{
    public class CustomMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IUserService _userService;

        public CustomMiddleware(RequestDelegate next, IUserService userService)
        {
            _next = next;
            _userService = userService;
        }

        public async Task Invoke(HttpContext context)
        {
            var auth = context.User;

            if (auth == null)
            {
                return;
            }

            var userClaims = auth.Claims;

            if (userClaims?.Count() == 0)
            {
                return;
            }

            await _userService.SetupUser(userClaims);

            await _next.Invoke(context);
        }
    }
}
