using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace InspectR.Core
{
    public class InspectRRouteHandler : MvcRouteHandler
    {
        protected override IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            string query = requestContext.HttpContext.Request.QueryString.ToString().ToLowerInvariant();
            if (query.StartsWith("inspect"))
            {
                // use mvc pipeline
                requestContext.RouteData.Values["controller"] = "InspectR";
                requestContext.RouteData.Values["action"] = "Inspect";

                return base.GetHttpHandler(requestContext);
            }

            return new InspectRHandlerHttpHandler(requestContext);
        }
    }
}