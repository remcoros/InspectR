namespace InspectR
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;

    using InspectR.Data;

    public class MvcApplication : HttpApplication
    {
        public MvcApplication()
        {
            BeginRequest += (sender, args) => { HttpContext.Current.Items["InspectRContext"] = new InspectRContext(); };

            EndRequest += (o, eventArgs) =>
                {
                    var dbcontext = HttpContext.Current.Items["InspectRContext"] as IDisposable;
                    if (dbcontext != null)
                    {
                        dbcontext.Dispose();
                    }
                };
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
