using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Lvgaga.Startup))]
namespace Lvgaga
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
