// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Net.Http;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace DemoApi.Model.OWIN
{
    /// <summary>
    /// Bearer authentication middleware component which is added to an HTTP pipeline. This class is not
    /// created by application code directly, instead it is added by calling the the IAppBuilder UseJwtBearerAuthentication
    /// extension method.
    /// </summary>
    public class XJwtBearerMiddleware : AuthenticationMiddleware<XJwtBearerOptions>
    {
        /// <summary>
        /// Bearer authentication component which is added to an HTTP pipeline. This constructor is not
        /// called by application code directly, instead it is added by calling the the IAppBuilder UseJwtBearerAuthentication 
        /// extension method.
        /// </summary>
        public XJwtBearerMiddleware(
            RequestDelegate next,
            ILoggerFactory loggerFactory,
            UrlEncoder encoder,
            IOptions<XJwtBearerOptions> options)
            : base(next, options, loggerFactory, encoder)
        {
            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            if (encoder == null)
            {
                throw new ArgumentNullException(nameof(encoder));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (Options.Events == null)
            {
                Options.Events = new JwtBearerEvents();
            }
        }

        /// <summary>
        /// Called by the AuthenticationMiddleware base class to create a per-request handler. 
        /// </summary>
        /// <returns>A new instance of the request handler</returns>
        protected override AuthenticationHandler<XJwtBearerOptions> CreateHandler()
        {
            return new XJwtBearerHandler();
        }
    }
}
