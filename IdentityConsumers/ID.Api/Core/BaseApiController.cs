using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using Claim = System.IdentityModel.Claims.Claim;

namespace ID.Api.Core
{
    public abstract class BaseApiController : ApiController
    {
        protected BaseApiController()
        {
            var cp = (ClaimsPrincipal)HttpContext.Current.User;

            LoggedInUserClaims = cp?.Claims.ToList();

        }

        public List<System.Security.Claims.Claim> LoggedInUserClaims { get; }
    }
}