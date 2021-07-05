using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LoginApp.Startup))]
namespace LoginApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
