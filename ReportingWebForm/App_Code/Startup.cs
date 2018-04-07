using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ReportingWebForm.Startup))]
namespace ReportingWebForm
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
