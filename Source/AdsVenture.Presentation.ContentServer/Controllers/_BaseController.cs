using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using Bardock.Utils.Web.Mvc;

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
	}
}