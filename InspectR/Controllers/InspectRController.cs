using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InspectR.Core;
using InspectR.Data;
using InspectR.Filters;
using InspectR.Helpers;
using InspectR.Models;

namespace InspectR.Controllers
{
    [InitializeSimpleMembership]
    public class InspectRController : Controller
    {
        private IRequestCache _requestCache;
        private IInspectRService _inspectR;

        public InspectRController()
        {
            _requestCache = new RequestCache();
            _inspectR = new DefaultInspectRService(_requestCache, ()=>new HttpContextWrapper(System.Web.HttpContext.Current));
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create(bool isprivate)
        {
            InspectorInfo inspectorInfo = _inspectR.CreateInspector(isprivate);

            return Redirect(Url.InspectR().Inspect(inspectorInfo.UniqueKey));
        }

        public ActionResult Inspect(string id)
        {
            InspectorInfo inspectorInfo = _inspectR.GetInspectorInfoByKey(id);
            var recentRequests = _requestCache.Get(inspectorInfo).OrderByDescending(x => x.DateCreated).Take(20);

            if (inspectorInfo == null)
                return HttpNotFound();

            return View("Inspect", new InspectRViewModel()
                {
                    InspectorInfo = inspectorInfo,
                    RecentRequests = recentRequests
                });
        }

        //[AcceptVerbs(HttpVerbs.Delete | HttpVerbs.Get | HttpVerbs.Head | HttpVerbs.Options | HttpVerbs.Patch | HttpVerbs.Post | HttpVerbs.Put)]
        //public ActionResult Log(string id)
        //{
        //    if (Request.QueryString.ToString().ToLowerInvariant().StartsWith("inspect"))
        //    {
        //        return Inspect(id);
        //    }

        //    InspectorInfo inspectorInfo = _inspectR.GetInspectorInfoByKey(id);

        //    if (inspectorInfo == null)
        //        return HttpNotFound();

        //    // TODO: check private

        //    _inspectR.LogRequest(inspectorInfo);

        //    return Json(new
        //    {
        //        result = "ok"
        //    }, JsonRequestBehavior.AllowGet);            
        //}
    }
}
