using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using InspectR.Data;

namespace InspectR.Core
{
    public class RequestCache : IRequestCache
    {
        protected Cache Cache
        {
            get { return HttpContext.Current.Cache; }
        }

        public RequestCache()
        {
        }

        public void Store(InspectorInfo inspector, RequestInfo request)
        {
            var requests = GetInternal(inspector);

            // keep only last 50 requests for now, cleanup at 100+
            if (requests.Count >= 100)
            {
                requests = requests.Skip(requests.Count - 50).Take(50).ToList();
            }
            requests.Add(request);
            Cache.Insert("inspectR" + inspector.Id, requests, null, Cache.NoAbsoluteExpiration, TimeSpan.FromDays(1));
        }

        private IList<RequestInfo> GetInternal(InspectorInfo inspector)
        {
            if (inspector == null) throw new ArgumentNullException("inspector");
            var requests = Cache["inspectR" + inspector.Id] as IList<RequestInfo>;
            if (requests == null)
            {
                requests = new List<RequestInfo>();
            }
            return requests;
        }

        public IEnumerable<RequestInfo> Get(InspectorInfo inspector)
        {
            return GetInternal(inspector);
        }

        public void RemoveAll(InspectorInfo inspector)
        {
            Cache.Remove("inspectR" + inspector.Id);
        }
    }
}