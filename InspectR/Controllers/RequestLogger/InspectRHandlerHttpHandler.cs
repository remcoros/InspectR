using System.Web;
using System.Web.Routing;
using InspectR.Core;
using InspectR.Core.RequestLogger;
using InspectR.Data;

namespace InspectR.Controllers.RequestLogger
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
            var requestLogger = new Core.RequestLogger.RequestLogger(requestCache, new DefaultRequestCollector());
            var dbContext = new InspectRContext();

            var id = _requestContext.RouteData.Values["id"] as string;

            InspectorInfo inspectorInfo = dbContext.GetInspectorInfoByKey(id);

            // TODO: check private
            // ..

            dbContext.Dispose();

            if (inspectorInfo == null)
                throw new HttpException(404, "404 - Inspector Not found");

            requestLogger.LogRequest(inspectorInfo);

            // TODO: random response? :)
            context.Response.Write("ok");
        }
    }
}