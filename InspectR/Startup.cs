using InspectR;

using Microsoft.Owin;

[assembly: OwinStartup(typeof(Startup))]
namespace InspectR
{
    using Owin;

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            this.ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}

namespace InspectR.Net45
{
    public static class Dummy { }
}