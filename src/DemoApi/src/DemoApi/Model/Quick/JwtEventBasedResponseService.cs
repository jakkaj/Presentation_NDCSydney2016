using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DemoApi.Model.Contract;
using DemoApi.Model.Entity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.Extensions.Options;
using TokenFunctionHelper;

namespace DemoApi.Model.Quick
{
    public class JwtEventBasedResponseService : IJwtEventBasedResponseService
    {
        private readonly SigningSettings _settings;

        public JwtEventBasedResponseService(IOptions<SigningSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task OnMessageReceived(MessageReceivedContext context)
        {
            string token = null;

            string authorization = context.Request.Headers["Authorization"];

            // If no authorization header found, nothing to process further
            if (string.IsNullOrEmpty(authorization))
            {
                context.State = EventResultState.Skipped;
                return;
            }

            if (authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                token = authorization.Substring("Bearer ".Length).Trim();
            }

            var remoteAuthResult =
                     await
                         TokenValidator.Validate(_settings.ValidationEndpoint, token, _settings.RSAPublic,
                             _settings.TokenValidIssuer, _settings.TokenAllowedAudience);

            if (remoteAuthResult.IsValid)
            {
                var claims = new List<Claim>();

                foreach (var claim in remoteAuthResult.Claims)
                {
                    claims.Add(new Claim(claim.Key, claim.Value));
                }

                var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));

                var ticket = new AuthenticationTicket(principal, new AuthenticationProperties(),
                    "Bearer");
                
                
                ticket.Properties.StoreTokens(new[]
                {
                    new AuthenticationToken {Name = "access_token", Value = token}
                });

                context.Ticket = ticket;

                context.State = EventResultState.HandledResponse;
            }
            else
            {
                context.State = EventResultState.Continue;
            }
        }
    }
}
