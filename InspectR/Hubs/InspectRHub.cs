using System;
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
        private InspectRContext _dbContext;
        private IInspectRService _service;

        public InspectRHub()
        {
            _requestCache = new RequestCache();
            _dbContext = new InspectRContext();
            _service = new InspectRService(_dbContext);
        }

        public override System.Threading.Tasks.Task OnReconnected()
        {
            InspectRGroupsModule.OnReconnected(this);
            return base.OnReconnected();
        }

        public override System.Threading.Tasks.Task OnDisconnected()
        {
            InspectRGroupsModule.OnDisconnected(this);
            return base.OnDisconnected();
        }

        public InspectorInfo StartInspect(string inspector)
        {
            var info = _dbContext.GetInspectorInfoByKey(inspector);
            if (info == null)
                return null;

            InspectRGroupsModule.StartInspect(this, info.UniqueKey);
            
            return info;
        }

        public InspectRUserProfile GetUserProfile()
        {
            if (Context.User != null)
            {
                var username = Context.User.Identity.Name;
                if (!string.IsNullOrEmpty(username))
                {
                    // todo: map a dto
                    return _dbContext.GetUserProfile(username);
                }
            }

            return null;
        }

        public void RemoveInspectorFromUserProfile(Guid inspectorId)
        {
            if (Context.User != null)
            {
                var username = Context.User.Identity.Name;
                if (!string.IsNullOrEmpty(username))
                {
                    _service.RemoveInspectorFromUser(username, inspectorId);                    
                }
            }            
        }

        public IEnumerable<RequestInfo> GetRecentRequests(string inspector)
        {
            InspectorInfo inspectorInfo = _dbContext.GetInspectorInfoByKey(inspector);
            var recentRequests = _requestCache.Get(inspectorInfo).OrderByDescending(x => x.DateCreated).Take(20);
            return recentRequests;
        }

        public void ClearRecentRequests(string inspector)
        {
            InspectorInfo inspectorInfo = _dbContext.GetInspectorInfoByKey(inspector);
            _requestCache.RemoveAll(inspectorInfo);
        }

        public void SetTitle(Guid id, string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return;
            }

            InspectorInfo inspectorInfo = _dbContext.GetInspectorInfo(id);
            inspectorInfo.Title = title;
            _dbContext.SaveChanges();
        }

        public override System.Threading.Tasks.Task OnConnected()
        {
            return base.OnConnected();
        }
    }
}