using System;
using System.Configuration;
using System.Net;
using IdentityServer3.AccessTokenValidation;
using Microsoft.Owin;
using Owin;

namespace ID.Api
{
    public class Startup
    {
        /// <summary>
        ///     Configurations the specified application.
        /// </summary>
        /// <param name="app">The application.</param>
        public void Configuration(IAppBuilder app)
        {
            var hostUrl = ConfigurationManager.AppSettings["Identity.Host.Url"];

            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

            try
            {
                app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
                {
                    Authority = hostUrl,
                    EnableValidationResultCache = true,
                    ValidationResultCacheDuration = TimeSpan.FromMinutes(10)
                });
            }
            catch (Exception ex)
            {
            }
        }
    }
}