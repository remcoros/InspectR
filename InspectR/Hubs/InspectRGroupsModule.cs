using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace InspectR.Hubs
{
    public class InspectRGroupsModule : HubPipelineModule
    {
        private static readonly ConcurrentDictionary<string, string[]> _groups = new ConcurrentDictionary<string, string[]>();

        public override Func<HubDescriptor, IRequest, IEnumerable<string>, IEnumerable<string>> BuildRejoiningGroups(Func<HubDescriptor, IRequest, IEnumerable<string>, IEnumerable<string>> rejoiningGroups)
        {
            return (hubDescriptor, request, groups) =>
                {
                    string[] thegroups;
                    _groups.TryGetValue(request.QueryString["connectionId"], out thegroups);
                    return groups;
                };
        }

        public static void OnReconnected(Hub hub)
        {
            string[] groups;
            _groups.TryGetValue(hub.Context.ConnectionId, out groups);
            if (groups != null)
            {
                foreach (var key in groups)
                {
                    hub.Groups.Add(hub.Context.ConnectionId, key);
                }
            }
            
        }

        public static void OnDisconnected(InspectRHub hub)
        {
            string[] val;
            _groups.TryRemove(hub.Context.ConnectionId, out val);             
        }

        public static void StartInspect(InspectRHub hub, string uniquekey)
        {
            var connectionId = hub.Context.ConnectionId;
            hub.Groups.Add(connectionId, uniquekey);
            _groups.AddOrUpdate(connectionId, new[] { uniquekey },
                                (key, current) => current.Concat(new[] { uniquekey }).ToArray());
        }
    }
}