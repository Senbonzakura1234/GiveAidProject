using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GiveAidCharity.Startup))]
namespace GiveAidCharity
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
