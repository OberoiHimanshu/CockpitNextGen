using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(B_Web_App.Startup))]
namespace B_Web_App
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
