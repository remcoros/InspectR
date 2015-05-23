namespace InspectR.Core.RequestLogger
{
    using System;

    using InspectR.Data;
    using InspectR.Hubs;

    using Microsoft.AspNet.SignalR;

    public class RequestLogger
    {
        private readonly IRequestCache _cache;

        private readonly IRequestCollector _collector;

        public RequestLogger(IRequestCache cache, IRequestCollector collector)
        {
            _cache = cache;
            _collector = collector;
        }

        public void LogRequest(InspectorInfo inspector)
        {
            if (inspector == null)
            {
                throw new ArgumentNullException("inspector");
            }

            var request = CreateRequestInfo(inspector);

            _cache.Store(inspector, request);
            BroadcastRequestInfo(inspector, request);
        }

        private void BroadcastRequestInfo(InspectorInfo inspector, RequestInfo request)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<InspectRHub>();
            context.Clients.Group(inspector.UniqueKey).requestLogged(inspector, request);
        }

        private RequestInfo CreateRequestInfo(InspectorInfo inspector)
        {
            var info = new RequestInfo();
            _collector.Collect(info, inspector);
            return info;
        }
    }
}
