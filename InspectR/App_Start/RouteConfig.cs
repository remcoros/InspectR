using System.Web.Mvc;
using System.Web.Routing;
using InspectR.Controllers;
using InspectR.Controllers.RequestLogger;
using InspectR.Core;
using InspectR.Core.RequestLogger;

namespace InspectR.App_Start
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "create",
                url: "create",
                defaults: new { controller = "InspectR", action = "Create", isprivate = false }
            );

            //routes.MapRoute(
            //    name: "inspect",
            //    url: "inspect/{id}",
            //    defaults: new {controller = "InspectR", action = "Inspect"}
            //    );

            routes.Add("log", new Route("{id}", new RouteValueDictionary(new
                {
                    // Altough this action does not exists, need these default values for nice url's
                    controller = "InspectR",
                    action = "Log"
                }), new InspectRRouteHandler()));

            //routes.MapRoute(
            //    name: "log",
            //    url: "{id}",
            //    defaults: new { controller = "InspectR", action = "Log", inspect = false }
            //);

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "InspectR", action = "Index", id = UrlParameter.Optional }
                );
        }
    }
}