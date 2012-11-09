using System.Web;
using System.Web.Routing;
using InspectR.App_Start;
using Microsoft.AspNet.SignalR;

[assembly: PreApplicationStartMethod(typeof(RegisterHubs), "Start")]

namespace InspectR.App_Start
{
    public static class RegisterHubs
    {
        public static void Start()
        {
            // Register the default hubs route: ~/signalr/hubs
            RouteTable.Routes.MapHubs();            
        }
    }
}
