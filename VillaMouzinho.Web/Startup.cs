using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(VillaMouzinho.Web.Startup))]
namespace VillaMouzinho.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
