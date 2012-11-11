using System.Collections.Generic;
using InspectR.Data;

namespace InspectR.Core
{
    public interface IRequestCache
    {
        void Store(InspectorInfo inspector, RequestInfo request);
        IEnumerable<RequestInfo> Get(InspectorInfo inspector);
        void RemoveAll(InspectorInfo inspector);
    }
}