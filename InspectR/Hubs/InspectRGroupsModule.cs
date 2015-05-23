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

        /// <summary>
        /// Wraps a function that determines which of the groups belonging to the hub described by the <see cref="T:Microsoft.AspNet.SignalR.Hubs.HubDescriptor"/>
        ///             the client should be allowed to rejoin.
        ///             By default, clients will rejoin all the groups they were in prior to reconnecting.
        /// </summary>
        /// <param name="rejoiningGroups">A function that determines which groups the client should be allowed to rejoin.</param>
        /// <returns>
        /// A wrapped function that determines which groups the client should be allowed to rejoin.
        /// </returns>
        public override Func<HubDescriptor, IRequest, IList<string>, IList<string>> BuildRejoiningGroups(Func<HubDescriptor, IRequest, IList<string>, IList<string>> rejoiningGroups)
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

        public static void StartInspect(InspectRHub hub, string uniquekey)
        {
            var connectionId = hub.Context.ConnectionId;
            hub.Groups.Add(connectionId, uniquekey);
            _groups.AddOrUpdate(connectionId, new[] { uniquekey },
                                (key, current) => current.Concat(new[] { uniquekey }).ToArray());
        }

        public static void StopInspect(Hub hub)
        {
            string[] val;
            _groups.TryRemove(hub.Context.ConnectionId, out val);
        }

        public static void StopInspect(Hub hub, string uniqueKey)
        {
            var connectionId = hub.Context.ConnectionId;
            string[] groups;
            _groups.TryGetValue(connectionId, out groups);
            if (groups != null && groups.Contains(uniqueKey))
            {
                groups = groups.Where(x => x != uniqueKey).ToArray();
                _groups.AddOrUpdate(connectionId, groups, (key, current) => groups);
            }
        }
    }
}