using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using Bardock.Utils.Web.Mvc;
using AdsVenture.Commons.Pagination;

namespace AdsVenture.Presentation.ContentServer.Controllers
{
    //[Authorize]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class _BaseController : BaseController
    {
        //protected SessionUser CurrentUser
        //{
        //    get { return SessionUser.Current; }
        //}

        protected virtual void ThrowHttpNotFound()
        {
            throw new HttpException(404, "NotFound");
        }

        public ActionResult TablePage<T>(PageData<T> page, string viewName = "Index_Table")
        {
            return Json(new
            {
                Html = RenderViewToString(viewName, page.Data),
                TotalRecords = page.TotalRecords
            }, JsonRequestBehavior.AllowGet);
        }
	}
}