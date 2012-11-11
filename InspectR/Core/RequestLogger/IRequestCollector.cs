using InspectR.Data;

namespace InspectR.Core.RequestLogger
{
    public interface IRequestCollector
    {
        void Collect(RequestInfo info, InspectorInfo inspector);
    }
}