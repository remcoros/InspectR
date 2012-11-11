using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using InspectR.App_Start;
using InspectR.Data;

namespace InspectR
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : System.Web.HttpApplication
    {
        public WebApiApplication()
        {
			BeginRequest += (sender, args) =>
			{
                HttpContext.Current.Items["InspectRContext"] = new InspectRContext();
			};

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

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
        }
    }
}