
install-package Microsoft.Owin.Host.SystemWeb
install-package IdentityServer3
install-package Thinktecture.IdentityModel.Owin.ResourceAuthorization.Mvc
install-package IdentityServer3.AccessTokenValidation

Install-Package Microsoft.AspNet.WebApi.Cors -Version 5.2.3
------

1. Install above packages
2. Add Startup.cs file
3. Add Identity Host Url Key
4. Add ScopeAuthorizeAttribute to implement custom authorization
5. Decorate your controller/action with [Authorize] and [ScopeAuthorize("wyd")] attributes

------

Sending request to Web API

1. Request headerr should have Authorization key
[{"key":"Authorization","value":"Bearer {access_totken}","description":""}]

