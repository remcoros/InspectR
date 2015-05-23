namespace InspectR.Hubs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using InspectR.Core;
    using InspectR.Data;

    using Microsoft.AspNet.SignalR;

    public class InspectRHub : Hub
    {
        private readonly IRequestCache _requestCache;

        private readonly InspectRContext _dbContext;

        private readonly InspectRService _service;

        public InspectRHub()
        {
            _requestCache = new RequestCache();
            _dbContext = new InspectRContext();
            _service = new InspectRService(_dbContext);
        }

        public override Task OnReconnected()
        {
            InspectRGroupsModule.OnReconnected(this);
            return base.OnReconnected();
        }

        /// <summary>
        ///     Called when a connection disconnects from this hub gracefully or due to a timeout.
        /// </summary>
        /// <param name="stopCalled">
        ///     true, if stop was called on the client closing the connection gracefully;
        ///     false, if the connection has been lost for longer than the
        ///     <see cref="P:Microsoft.AspNet.SignalR.Configuration.IConfigurationManager.DisconnectTimeout" />.
        ///     Timeouts can be caused by clients reconnecting to another SignalR server in scaleout.
        /// </param>
        /// <returns>
        ///     A <see cref="T:System.Threading.Tasks.Task" />
        /// </returns>
        public override Task OnDisconnected(bool stopCalled)
        {
            InspectRGroupsModule.StopInspect(this);
            return base.OnDisconnected(stopCalled);
        }

        public InspectorInfo StopInspect(Guid id)
        {
            var info = _dbContext.GetInspectorInfo(id);
            if (info == null)
            {
                return null;
            }

            InspectRGroupsModule.StopInspect(this, info.UniqueKey);

            return info;
        }

        public InspectorInfo StartInspect(string inspector)
        {
            var info = _dbContext.GetInspectorInfoByKey(inspector);
            if (info == null)
            {
                return null;
            }

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

        public IEnumerable<RequestInfo> GetRecentRequests(string uniquekey)
        {
            var inspectorInfo = _dbContext.GetInspectorInfoByKey(uniquekey);
            if (inspectorInfo == null)
            {
                throw new Exception("Can't find inspector");
            }

            var recentRequests = _requestCache.Get(inspectorInfo).OrderByDescending(x => x.DateCreated).Take(20);
            return recentRequests;
        }

        public void ClearRecentRequests(string inspector)
        {
            var inspectorInfo = _dbContext.GetInspectorInfoByKey(inspector);
            if (inspectorInfo == null)
            {
                throw new Exception("Can't find inspector");
            }
            _requestCache.RemoveAll(inspectorInfo);
        }

        public void SetTitle(Guid id, string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return;
            }

            var inspectorInfo = _dbContext.GetInspectorInfo(id);
            inspectorInfo.Title = title;
            _dbContext.SaveChanges();
        }

        public override Task OnConnected()
        {
            return base.OnConnected();
        }
    }
}
