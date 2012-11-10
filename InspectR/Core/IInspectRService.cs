using System;
using System.Collections.Generic;
using InspectR.Data;

namespace InspectR.Core
{
    public interface IInspectRService
    {
        InspectorInfo CreateInspector(bool isprivate);

        InspectorInfo GetInspectorInfo(Guid id);

        InspectorInfo GetInspectorInfoByKey(string uniquekey);

        void LogRequest(InspectorInfo inspector);
    }
}