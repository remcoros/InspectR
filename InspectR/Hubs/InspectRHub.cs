using System.Collections.Generic;
using System.Linq;
using System.Web;
using InspectR.Controllers;
using InspectR.Core;
using InspectR.Data;
using Microsoft.AspNet.SignalR.Hubs;

namespace InspectR.Hubs
{
    public class InspectRHub : Hub
    {
        private IRequestCache _requestCache;
        private IInspectRService _service;

        public InspectRHub()
        {
            // REVIEW: there must be a nicer way...
            _requestCache = new RequestCache();
            _service = new DefaultInspectRService(_requestCache, ()=>new HttpContextWrapper(HttpContext.Current));            
        }
        public void StartInspect(string inspector)
        {
            var info = _service.GetInspectorInfoByKey(inspector);
            if (info == null)
                return;

            Groups.Add(Context.ConnectionId, info.UniqueKey);
        }

        public IEnumerable<RequestInfo> GetRecentRequests(string inspector)
        {
            InspectorInfo inspectorInfo = _service.GetInspectorInfoByKey(inspector);
            var recentRequests = _requestCache.Get(inspectorInfo).OrderByDescending(x=>x.DateCreated).Take(20);
            return recentRequests;
        }

        public void ClearRecentRequests(string inspector)
        {
            InspectorInfo inspectorInfo = _service.GetInspectorInfoByKey(inspector);
            _requestCache.RemoveAll(inspectorInfo);
        }

        public override System.Threading.Tasks.Task OnConnected()
        {
            return base.OnConnected();
        }
    }
}