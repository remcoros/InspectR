using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InspectR.Core;
using InspectR.Data;
using InspectR.Helpers;
using InspectR.Models;

namespace InspectR.Controllers
{
    public class InspectRController : Controller
    {
        private InspectRService _service;

        public InspectRController()
        {
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create(bool isprivate)
        {
            var inspector = _service.CreateInspector(isprivate);

            return Redirect(Url.InspectR().Inspect(inspector.UniqueKey));
        }

        public ActionResult Inspect(string id)
        {
            InspectorInfo inspectorInfo = _service.GetInspectorInfoByKey(id);

            if (inspectorInfo == null)
                return HttpNotFound();

            if (User != null)
            {
                var user = User.Identity.Name;
                if (!string.IsNullOrEmpty(user))
                {
                    _service.AddInspectorToUser(user, inspectorInfo);
                }
            }

            return View("Inspect", new InspectRViewModel() {
                InspectorInfo = inspectorInfo,
            });
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var dbContext = (InspectRContext)HttpContext.Items["InspectRContext"];
            _service = new InspectRService(dbContext);

            if (User != null)
            {
                var username = User.Identity.Name;
                if (!string.IsNullOrEmpty(username))
                {
                    // todo: map a dto
                    ViewBag.UserProfile = dbContext.GetUserProfile(username);
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
