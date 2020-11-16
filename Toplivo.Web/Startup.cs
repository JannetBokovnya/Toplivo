using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Toplivo.Web.Startup))]
namespace Toplivo.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
