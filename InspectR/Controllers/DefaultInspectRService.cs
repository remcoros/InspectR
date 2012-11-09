using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using InspectR.Data;
using InspectR.Hubs;
using Microsoft.AspNet.SignalR;

namespace InspectR.Controllers
{
    public class DefaultInspectRService : IInspectRService
    {
        private readonly HttpContextBase _httpContext;
        private IRequestCollector collector = new DefaultRequestCollector();

        public DefaultInspectRService(HttpContextBase httpContext)
        {
            _httpContext = httpContext;
        }

        public InspectorInfo Create(bool isprivate)
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

        public void LogRequest(string id)
        {
            InspectorInfo inspector = GetInspectorInfoByKey(id);
            if (inspector == null)
            {
                return;
            }

            var request = CreateRequestInfo(inspector);

            StoreRequestInfo(inspector, request);
            BroadcastRequestInfo(inspector, request);
        }

        public IEnumerable<RequestInfo> GetRecentRequests(Guid id)
        {
            var cache = _httpContext.Cache; // TODO: cache somewhere else
            var requests = cache["inspectR" + id] as IList<RequestInfo>;
            if (requests == null)
            {
                return null;
            }
            
            return requests.OrderByDescending(x=>x.DateCreated).Take(20);
        }

        public void ClearRecentRequests(Guid id)
        {
            var cache = _httpContext.Cache; // TODO: cache somewhere else
            cache.Remove("inspectR" + id);
        }

        private void BroadcastRequestInfo(InspectorInfo inspector, RequestInfo request)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<InspectRHub>();
            context.Clients.Group(inspector.UniqueKey).requestLogged(inspector, request);
        }

        private void StoreRequestInfo(InspectorInfo inspector, RequestInfo request)
        {
            var cache = _httpContext.Cache; // TODO: cache somewhere else
            var requests = cache["inspectR" + inspector.Id] as IList<RequestInfo>;
            if (requests == null)
            {
                requests = new List<RequestInfo>();
            }
            requests.Add(request);
            cache.Insert("inspectR" + inspector.Id, requests, null, Cache.NoAbsoluteExpiration, TimeSpan.FromDays(1));
            
            //using (var context = new InspectRContext())
            //{
            //    context.Requests.Add(new RequestInfoEntry(inspector, request));
            //    context.SaveChanges();
            //}
        }

        private RequestInfo CreateRequestInfo(InspectorInfo inspector)
        {
            var info = new RequestInfo();
            collector.Collect(info, inspector, _httpContext);
            return info;
        }
    }
}