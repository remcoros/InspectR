using System.Web;
using System.Web.Mvc;
using InspectR.Data;

namespace InspectR.Controllers
{
    public interface IRequestCollector
    {
        void Collect(RequestInfo info, InspectorInfo inspector, HttpContextBase controller);
    }
}