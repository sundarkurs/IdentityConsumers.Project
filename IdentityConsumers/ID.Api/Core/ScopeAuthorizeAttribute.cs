#region Namespace

using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Controllers;

#endregion


namespace ID.Api.Core
{
    public class ScopeAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly string[] _scopes;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ScopeAuthorizeAttribute" /> class.
        /// </summary>
        /// <param name="scopes">The scopes.</param>
        /// <exception cref="System.ArgumentNullException">scopes</exception>
        public ScopeAuthorizeAttribute(params string[] scopes)
        {
            if (scopes == null)
            {
                throw new ArgumentNullException(nameof(scopes));
            }


            _scopes = scopes;
        }

        /// <summary>
        ///     Indicates whether the specified control is authorized.
        /// </summary>
        /// <param name="actionContext">The context.</param>
        /// <returns>
        ///     true if the control is authorized; otherwise, false.
        /// </returns>
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var principal = actionContext.ControllerContext.RequestContext.Principal as ClaimsPrincipal;

            if (principal == null)
            {
                return false;
            }

            var grantedScopes = principal.Claims.Where(s => s.Type == "scope").Select(v => v.Value).ToList();

            foreach (var scope in _scopes)
            {
                if (grantedScopes.Contains(scope, StringComparer.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        ///     Processes requests that fail authorization.
        /// </summary>
        /// <param name="actionContext">The context.</param>
        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            HttpResponseMessage response;
            if (actionContext.RequestContext.Principal != null &&
                actionContext.RequestContext.Principal.Identity.IsAuthenticated)
            {
                response = actionContext.Request.CreateErrorResponse(HttpStatusCode.Forbidden, "insufficient_scope");
                response.Headers.Add("WWW-Authenticate", "Bearer error=\"insufficient_scope\"");
            }
            else
            {
                // If you get unauthorized response, check your ids3 endpoint, whether it maches the website ids3 endpoint.
                response = actionContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Unauthorized");
            }

            actionContext.Response = response;
        }
    }
}