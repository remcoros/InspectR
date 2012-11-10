using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using InspectR.Core;
using InspectR.Data;

namespace InspectR.Controllers
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
        
        public class InspectRHandlerHttpHandler : IHttpHandler
        {
            private readonly RequestContext _requestContext;

            public InspectRHandlerHttpHandler(RequestContext requestContext)
            {
                _requestContext = requestContext;
            }

            public bool IsReusable { get { return false; } }

            public void ProcessRequest(HttpContext context)
            {
                var requestCache = new RequestCache();
                var inspectR = new DefaultInspectRService(requestCache, ()=>new HttpContextWrapper(System.Web.HttpContext.Current));
                var id = _requestContext.RouteData.Values["id"] as string;

                InspectorInfo inspectorInfo = inspectR.GetInspectorInfoByKey(id);

                if (inspectorInfo == null)
                    throw new HttpException(404, "404 - Inspector Not found");

                // TODO: check private

                inspectR.LogRequest(inspectorInfo);

                context.Response.Write("ok");
            }
        }
    }
}