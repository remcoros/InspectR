using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using InspectR.Data;

namespace InspectR.Core
{
    public interface IRequestCache
    {
        void Store(InspectorInfo inspector, RequestInfo request);
        IEnumerable<RequestInfo> Get(InspectorInfo inspector);
        void RemoveAll(InspectorInfo inspector);
    }

    public class RequestCache : IRequestCache
    {
        protected Cache Cache { get; set; }

        public RequestCache()
        {
            Cache = HttpContext.Current.Cache;
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
            
            //using (var context = new InspectRContext())
            //{
            //    context.Requests.Add(new RequestInfoEntry(inspector, request));
            //    context.SaveChanges();
            //}
        }

        private IList<RequestInfo> GetInternal(InspectorInfo inspector)
        {
            if (inspector == null) throw new ArgumentNullException("inspector");
            var requests = Cache["inspectR" + inspector] as IList<RequestInfo>;
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