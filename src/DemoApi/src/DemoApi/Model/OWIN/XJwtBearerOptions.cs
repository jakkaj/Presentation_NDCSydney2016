// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Builder;

namespace DemoApi.Model.OWIN
{
    /// <summary>
    /// Options class provides information needed to control Bearer Authentication middleware behavior
    /// </summary>
    public class XJwtBearerOptions : JwtBearerOptions
    {
        /// <summary>
        /// Creates an instance of bearer authentication options with default values.
        /// </summary>
        public XJwtBearerOptions() : base()
        {
            AuthenticationScheme = "Bearer";
            AutomaticAuthenticate = true;
            AutomaticChallenge = true;
        }
        
    }
}
