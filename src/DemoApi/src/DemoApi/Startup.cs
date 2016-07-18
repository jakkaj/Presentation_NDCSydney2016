using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using DemoApi.Model.Entity;
using DemoApi.Model.Filters;
using DemoApi.Model.OWIN;
using DemoApi.Model.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace DemoApi
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsEnvironment("Development"))
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }
        public IContainer ApplicationContainer { get; private set; }
        // This method gets called by the runtime. Use this method to add services to the container
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Add framework services.

            services.Configure<SigningSettings>(Configuration.GetSection("SigningSettings"));

            var justBeSignedIn = new AuthorizationPolicyBuilder()
                .RequireClaim("iss", "CentralAuthHost")
                .AddAuthenticationSchemes("Bearer")
                .Build();

            services.AddMvc(_ =>
            {
                _.Filters.Add(new XAuthorizeFilter(justBeSignedIn));
            });

            var builder = new ContainerBuilder();

            builder.RegisterAssemblyTypes(typeof(SampleService).GetTypeInfo().Assembly)
                .Where(t => t.Name.EndsWith("Service") || t.Name.EndsWith("Repo"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.Populate(services);
            this.ApplicationContainer = builder.Build();

            return new AutofacServiceProvider(this.ApplicationContainer);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            ILoggerFactory loggerFactory,
            IOptions<SigningSettings> signingSettings)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseBrowserLink();

            app.XUseJwtBearerAuthentication(new XJwtBearerOptions
            {
                ValiationEndpoint = signingSettings.Value.ValidationEndpoint,
                Audience = signingSettings.Value.TokenAllowedAudience,
                ClaimsIssuer = signingSettings.Value.TokenValidIssuer,
                RsaPublicKey = signingSettings.Value.RSAPublic,
                AuthenticationScheme = "Bearer"
            });

            app.UseMiddleware<CustomMiddleware>();

            app.UseMvc();

        }
    }
}
