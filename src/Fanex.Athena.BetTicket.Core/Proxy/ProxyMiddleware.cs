// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Proxy
{
    /// <summary>
    /// Proxy Middleware
    /// </summary>
    public class ProxyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly SharedProxyOptions _options;

        public ProxyMiddleware(RequestDelegate next, IOptions<SharedProxyOptions> options)
        {
            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _next = next;
            _options = options.Value;
        }

        public Task Invoke(HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (!_options.ShouldProxyRequest.Invoke(context.Request))
            {
                return _next.Invoke(context);
            }

            if (!_options.ValidateRequest.Invoke(context))
            {
                return _options.OnFailedValidation(context);
            }

            var uri = _options.PrepareUri(context.Request);
            return context.ProxyRequest(uri);
        }
    }
}