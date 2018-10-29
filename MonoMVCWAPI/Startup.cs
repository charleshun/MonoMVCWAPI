using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MonoMVCWAPI.Startup))]
namespace MonoMVCWAPI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
