using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InspectR.Data;
using InspectR.Filters;
using InspectR.Helpers;
using InspectR.Models;

namespace InspectR.Controllers
{
    [InitializeSimpleMembership]
    public class InspectRController : Controller
    {
        private IInspectRService _inspectR = new DefaultInspectRService(new HttpContextWrapper(System.Web.HttpContext.Current));

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create(bool isprivate)
        {
            InspectorInfo inspectorInfo = _inspectR.Create(isprivate);

            return Redirect(Url.InspectR().Inspect(inspectorInfo.UniqueKey));
        }

        public ActionResult Inspect(string id)
        {
            InspectorInfo inspectorInfo = _inspectR.GetInspectorInfoByKey(id);
            IEnumerable<RequestInfo> recentRequests = _inspectR.GetRecentRequests(inspectorInfo.Id);

            if (inspectorInfo == null)
                return HttpNotFound();

            return View("Inspect", new InspectRViewModel()
                {
                    InspectorInfo = inspectorInfo,
                    RecentRequests = recentRequests
                });
        }

        [AcceptVerbs(HttpVerbs.Delete | HttpVerbs.Get | HttpVerbs.Head | HttpVerbs.Options | HttpVerbs.Patch | HttpVerbs.Post | HttpVerbs.Put)]
        public ActionResult Log(string id)
        {
            if (Request.QueryString.ToString().ToLowerInvariant() == "inspect")
            {
                return Inspect(id);
            }

            InspectorInfo inspectorInfo = _inspectR.GetInspectorInfoByKey(id);

            if (inspectorInfo == null)
                return HttpNotFound();

            // TODO: check private

            _inspectR.LogRequest(id);

            return Json(new
            {
                result = "ok"
            }, JsonRequestBehavior.AllowGet);            
        }
    }
}
