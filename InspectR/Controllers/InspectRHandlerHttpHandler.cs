using System.Web;
using System.Web.Routing;
using InspectR.Core;
using InspectR.Data;

namespace InspectR.Controllers
{
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
            var requestLogger = new RequestLogger(requestCache, new DefaultRequestCollector());
            var inspectR = new DefaultInspectRService();

            var id = _requestContext.RouteData.Values["id"] as string;

            InspectorInfo inspectorInfo = inspectR.GetInspectorInfoByKey(id);

            if (inspectorInfo == null)
                throw new HttpException(404, "404 - Inspector Not found");

            // TODO: check private

            requestLogger.LogRequest(inspectorInfo);

            context.Response.Write("ok");
        }
    }
}