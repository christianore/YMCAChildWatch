using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(ChildWatchEmployee.Startup))]

namespace ChildWatchEmployee
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
