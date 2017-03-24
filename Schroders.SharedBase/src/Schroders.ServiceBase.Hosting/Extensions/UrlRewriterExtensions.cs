

using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.Rewrite.Internal;
using Microsoft.Extensions.Configuration;

namespace Schroders.ServiceBase.Hosting.Extensions
{
    public static class UrlRewriterExtensions
    {
        public static IApplicationBuilder UseRewriterToForceHttpsRedirect(this IApplicationBuilder app, IConfigurationRoot configuration, RewriteOptions rewriteOptions = null)
        {
            rewriteOptions = rewriteOptions ?? new RewriteOptions();

            var section = configuration.GetSection(nameof(HostingConfiguration));
            var config = new HostingConfiguration();
            section.Bind(config);

            if (config.ForceHttpsRedirect)
            {
                rewriteOptions.Rules.Insert(0, new RedirectToHttpsRule
                {
                    SSLPort = config.HttpsPort,
                    StatusCode = 301
                });
            }

            if (rewriteOptions.Rules.Any())
            {
                app.UseRewriter(rewriteOptions);
            }

            return app;
        }
    }
}