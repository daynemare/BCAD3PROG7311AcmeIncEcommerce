using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AcmeIncEcommerce.Startup))]
namespace AcmeIncEcommerce
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
