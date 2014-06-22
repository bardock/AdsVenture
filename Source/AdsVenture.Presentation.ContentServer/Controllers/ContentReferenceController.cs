using Bardock.Utils.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdsVenture.Presentation.ContentServer.Controllers
{
    public class ContentReferenceController : BaseController
    {
        public ActionResult SampleFluid()
        {
            return View();
        }

        public ActionResult SampleFluidExternal()
        {
            return View();
        }
    }
}
