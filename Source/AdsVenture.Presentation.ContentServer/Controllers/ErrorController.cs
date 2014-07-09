using Bardock.Utils.Web.Mvc;
using System.Web.Mvc;

namespace AdsVenture.Presentation.ContentServer.Controllers
{
    [AllowAnonymous()]
    public class ErrorController : _BaseController
    {
        [ActionName("403")]
        public ActionResult Forbidden()
        {
            return View();
        }

        [ActionName("404")]
        public ActionResult NotFound()
        {
            return View();
        }

        public ActionResult Unknown()
        {
            return View();
        }
    }
}