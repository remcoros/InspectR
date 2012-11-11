using System;
using System.Collections.Generic;
using InspectR.Data;

namespace InspectR.Core
{
    public interface IInspectRService
    {
        InspectorInfo CreateInspector(bool isprivate);

        void AddInspectorToUser(string userName, InspectorInfo info);
    }
}