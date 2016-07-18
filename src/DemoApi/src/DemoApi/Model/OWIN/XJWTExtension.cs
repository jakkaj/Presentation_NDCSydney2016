using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;

namespace DemoApi.Model.OWIN
{
    public static class XJWTExtension
    {
        public static IApplicationBuilder XUseJwtBearerAuthentication(this IApplicationBuilder app, XJwtBearerOptions options)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            return app.UseMiddleware<XJwtBearerMiddleware>(Options.Create(options));
        }
    }
}