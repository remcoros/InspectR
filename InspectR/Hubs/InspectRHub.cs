using System.Collections.Generic;
using System.Web;
using InspectR.Controllers;
using InspectR.Data;
using Microsoft.AspNet.SignalR.Hubs;

namespace InspectR.Hubs
{
    public class InspectRHub : Hub
    {
        private IInspectRService _service = new DefaultInspectRService(new HttpContextWrapper(HttpContext.Current));

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
            IEnumerable<RequestInfo> recentRequests = _service.GetRecentRequests(inspectorInfo.Id);

            return recentRequests;
        }

        public void ClearRecentRequests(string inspector)
        {
            InspectorInfo inspectorInfo = _service.GetInspectorInfoByKey(inspector);
            _service.ClearRecentRequests(inspectorInfo.Id);            
        }

        public override System.Threading.Tasks.Task OnConnected()
        {
            return base.OnConnected();
        }
    }
}