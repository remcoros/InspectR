using System.Web.Mvc;
using System.Web.Routing;

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

            routes.MapRoute(
                name: "log",
                url: "{id}",
                defaults: new { controller = "InspectR", action = "Log", inspect = false }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "InspectR", action = "Index", id = UrlParameter.Optional }
                );
        }
    }
}