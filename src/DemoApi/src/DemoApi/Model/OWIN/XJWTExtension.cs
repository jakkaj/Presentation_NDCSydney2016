using System;
using Microsoft.AspNetCore.Builder;

namespace DemoApi.Model.OWIN
{
    public static class XJWTExtension
    {
        public static IApplicationBuilder XUseJwtBearerAuthentication(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            return app.UseMiddleware<XJwtBearerMiddleware>();
        }
    }
}