using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ID.AuthorizeClient.Startup))]
namespace ID.AuthorizeClient
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
