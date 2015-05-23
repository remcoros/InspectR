namespace InspectR.Controllers.RequestLogger
{
    using System.Web;
    using System.Web.Routing;

    using InspectR.Core;
    using InspectR.Core.RequestLogger;
    using InspectR.Data;

    public class InspectRHandlerHttpHandler : IHttpHandler
    {
        private readonly RequestContext _requestContext;

        public InspectRHandlerHttpHandler(RequestContext requestContext)
        {
            _requestContext = requestContext;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            var requestCache = new RequestCache();
            var requestLogger = new RequestLogger(requestCache, new DefaultRequestCollector());

            InspectorInfo inspectorInfo;
            using (var dbContext = new InspectRContext())
            {
                var service = new InspectRService(dbContext);
                var id = _requestContext.RouteData.Values["id"] as string;

                inspectorInfo = service.GetInspectorInfoByKey(id);

                // TODO: check private
                // ..
            }

            if (inspectorInfo == null)
            {
                throw new HttpException(404, "404 - Inspector Not found");
            }

            requestLogger.LogRequest(inspectorInfo);

            // TODO: random response? :)
            context.Response.Write("ok");
        }
    }
}
