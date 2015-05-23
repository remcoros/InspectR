namespace InspectR.Core.RequestLogger
{
    using InspectR.Data;

    public interface IRequestCollector
    {
        void Collect(RequestInfo info, InspectorInfo inspector);
    }
}
