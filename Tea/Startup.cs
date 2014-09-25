using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Tea.Startup))]
namespace Tea
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
