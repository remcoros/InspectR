namespace InspectR.Core
{
    using System.Collections.Generic;

    using InspectR.Data;

    public interface IRequestCache
    {
        void Store(InspectorInfo inspector, RequestInfo request);

        IEnumerable<RequestInfo> Get(InspectorInfo inspector);

        void RemoveAll(InspectorInfo inspector);
    }
}
