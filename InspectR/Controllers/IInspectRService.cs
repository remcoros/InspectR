using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using InspectR.Data;

namespace InspectR.Controllers
{
    public interface IInspectRService
    {
        InspectorInfo Create(bool isprivate);

        InspectorInfo GetInspectorInfo(Guid id);

        InspectorInfo GetInspectorInfoByKey(string uniquekey);

        void LogRequest(string id);
        
        IEnumerable<RequestInfo> GetRecentRequests(Guid id);
        void ClearRecentRequests(Guid id);
    }
}