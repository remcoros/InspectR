using System.Collections.Generic;
using InspectR.Data;

namespace InspectR.Models
{
    public class InspectRViewModel
    {
        public InspectorInfo InspectorInfo { get; set; }

        public IEnumerable<RequestInfo> RecentRequests { get; set; }
    }
}