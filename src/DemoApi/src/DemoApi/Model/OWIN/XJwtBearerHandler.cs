// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Http.Features.Authentication;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using TokenFunctionHelper;

namespace DemoApi.Model.OWIN
{
    internal class XJwtBearerHandler : AuthenticationHandler<XJwtBearerOptions>
    {
        /// <summary>
        /// Searches the 'Authorization' header for a 'Bearer' token. If the 'Bearer' token is found, it is validated using <see cref="TokenValidationParameters"/> set in the options.
        /// </summary>
        /// <returns></returns>
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string token = null;

            AuthenticateResult result = null;
            List<Exception> validationFailures = null;


            try
            {
                string authorization = Request.Headers["Authorization"];

                // If no authorization header found, nothing to process further
                if (string.IsNullOrEmpty(authorization))
                {
                    return AuthenticateResult.Skip();
                }

                if (authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    token = authorization.Substring("Bearer ".Length).Trim();
                }

                // If no token found, no further work possible
                if (string.IsNullOrEmpty(token))
                {
                    return AuthenticateResult.Skip();
                }

                //validate the token

                var remoteAuthResult =
                    await
                        TokenValidator.Validate(Options.ValiationEndpoint, token, Options.RsaPublicKey,
                            Options.ClaimsIssuer, Options.Audience);

                if (remoteAuthResult.IsValid)
                {
                    var claims = new List<Claim>();

                    foreach (var claim in remoteAuthResult.Claims)
                    {
                        claims.Add(new Claim(claim.Key, claim.Value));
                    }

                    var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));

                    var ticket = new AuthenticationTicket(principal, new AuthenticationProperties(),
                        Options.AuthenticationScheme);

                    if (Options.SaveToken)
                    {
                        ticket.Properties.StoreTokens(new[]
                        {
                            new AuthenticationToken {Name = "access_token", Value = token}
                        });
                    }

                    return AuthenticateResult.Success(ticket);
                }

                var authenticationFailedContext = new AuthenticationFailedContext(Context, Options)
                {
                    Exception = new RemoteValidationFail(remoteAuthResult.FailReason)
                };

                await Options.Events.AuthenticationFailed(authenticationFailedContext);

                if (authenticationFailedContext.CheckEventResult(out result))
                {
                    return result;
                }

                return AuthenticateResult.Fail(authenticationFailedContext.Exception);
            }
            catch (Exception ex)
            {
                return AuthenticateResult.Fail(ex);
            }
        }


        protected override async Task<bool> HandleUnauthorizedAsync(ChallengeContext context)
        {
            var authResult = await HandleAuthenticateOnceAsync();

            var eventContext = new JwtBearerChallengeContext(Context, Options, new AuthenticationProperties(context.Properties))
            {
                AuthenticateFailure = authResult?.Failure,
            };

            // Avoid returning error=invalid_token if the error is not caused by an authentication failure (e.g missing token).
            if (Options.IncludeErrorDetails && eventContext.AuthenticateFailure != null)
            {
                eventContext.Error = "invalid_token";
                eventContext.ErrorDescription = CreateErrorDescription(eventContext.AuthenticateFailure);
            }

            Response.StatusCode = 401;

            if (string.IsNullOrEmpty(eventContext.Error) &&
                string.IsNullOrEmpty(eventContext.ErrorDescription) &&
                string.IsNullOrEmpty(eventContext.ErrorUri))
            {
                Response.Headers.Append(HeaderNames.WWWAuthenticate, Options.Challenge);
            }
            else
            {
                // https://tools.ietf.org/html/rfc6750#section-3.1
                // WWW-Authenticate: Bearer realm="example", error="invalid_token", error_description="The access token expired"
                var builder = new StringBuilder(Options.Challenge);
                if (Options.Challenge.IndexOf(" ", StringComparison.Ordinal) > 0)
                {
                    // Only add a comma after the first param, if any
                    builder.Append(',');
                }
                if (!string.IsNullOrEmpty(eventContext.Error))
                {
                    builder.Append(" error=\"");
                    builder.Append(eventContext.Error);
                    builder.Append("\"");
                }
                if (!string.IsNullOrEmpty(eventContext.ErrorDescription))
                {
                    if (!string.IsNullOrEmpty(eventContext.Error))
                    {
                        builder.Append(",");
                    }

                    builder.Append(" error_description=\"");
                    builder.Append(eventContext.ErrorDescription);
                    builder.Append('\"');
                }
                if (!string.IsNullOrEmpty(eventContext.ErrorUri))
                {
                    if (!string.IsNullOrEmpty(eventContext.Error) ||
                        !string.IsNullOrEmpty(eventContext.ErrorDescription))
                    {
                        builder.Append(",");
                    }

                    builder.Append(" error_uri=\"");
                    builder.Append(eventContext.ErrorUri);
                    builder.Append('\"');
                }

                Response.Headers.Append(HeaderNames.WWWAuthenticate, builder.ToString());
            }

            return false;
        }

        private static string CreateErrorDescription(Exception authFailure)
        {
            IEnumerable<Exception> exceptions;
            if (authFailure is AggregateException)
            {
                var agEx = authFailure as AggregateException;
                exceptions = agEx.InnerExceptions;
            }
            else
            {
                exceptions = new[] { authFailure };
            }

            var messages = new List<string>();

            foreach (var ex in exceptions)
            {
                // Order sensitive, some of these exceptions derive from others
                // and we want to display the most specific message possible.
                if (ex is RemoteValidationFail)
                {

                    messages.Add($"Validation server rejected token: {ex.Message}");
                }
                else
                {
                    messages.Add($"Random exception we don't particularly handle: {ex.Message}");
                }
               
            }

            return string.Join("; ", messages);
        }

        protected override Task HandleSignOutAsync(SignOutContext context)
        {
            throw new NotSupportedException();
        }

        protected override Task HandleSignInAsync(SignInContext context)
        {
            throw new NotSupportedException();
        }
    }
}
