using System.Web;
using InspectR.Data;

namespace InspectR.Core
{
    public interface IRequestCollector
    {
        void Collect(RequestInfo info, InspectorInfo inspector, HttpContextBase controller);
    }
}