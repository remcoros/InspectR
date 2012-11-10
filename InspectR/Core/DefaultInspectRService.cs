using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using InspectR.Core;
using InspectR.Data;
using InspectR.Hubs;
using Microsoft.AspNet.SignalR;

namespace InspectR.Controllers
{
    public class DefaultInspectRService : IInspectRService
    {
        private readonly Func<HttpContextWrapper> _httpContext;
        private IRequestCollector collector = new DefaultRequestCollector();
        private IRequestCache _requestCache;

        protected HttpContextBase HttpContext { get { return _httpContext(); } }

        public DefaultInspectRService(IRequestCache requestCache, Func<HttpContextWrapper> httpContext)
        {
            _requestCache = requestCache;
            _httpContext = httpContext;
        }

        public InspectorInfo CreateInspector(bool isprivate)
        {
            var inspector = new InspectorInfo()
                {
                    IsPrivate = isprivate
                };

            using (var context = new InspectRContext())
            {
                context.Inspectors.Add(inspector);
                context.SaveChanges();
            }

            return inspector;
        }

        public InspectorInfo GetInspectorInfo(Guid id)
        {
            using (var context = new InspectRContext())
            {
                return context.Inspectors.Find(id);
            }            
        }

        public InspectorInfo GetInspectorInfoByKey(string uniquekey)
        {
            using (var context = new InspectRContext())
            {
                return context.Inspectors.FirstOrDefault(x => x.UniqueKey == uniquekey);
            }
        }

        public void LogRequest(InspectorInfo inspector)
        {
            if (inspector == null) throw new ArgumentNullException("inspector");

            var request = CreateRequestInfo(inspector);

            _requestCache.Store(inspector, request);
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
            collector.Collect(info, inspector, HttpContext);
            return info;
        }
    }
}