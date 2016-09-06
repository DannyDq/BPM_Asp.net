using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Fiodariane.Startup))]
namespace Fiodariane
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
