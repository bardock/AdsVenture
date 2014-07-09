using System;
using System.Web;
using System.Web.Mvc;
using AdsVenture.Core.Exceptions;

namespace AdsVenture.Presentation.ContentServer.Controllers
{
    [AllowAnonymous()]
    public class ExceptionsEmulatorController : Controller
    {
        public ActionResult HttpException401()
        {
            throw new HttpException(401, "Unauthorized");
        }
        public ActionResult HttpException404()
        {
            throw new HttpException(404, "Not found");
        }
        public ActionResult HttpException500()
        {
            throw new HttpException(500, "Server error");
        }
        public ActionResult BusinessException()
        {
            throw new BusinessException("Business error");
        }
        public ActionResult EntityNotFoundException()
        {
            throw new EntityNotFoundException("Specified entity was not found");
        }
        public ActionResult UnauthorizedException()
        {
            throw new UnauthorizedException("Unauthorized");
        }
        public ActionResult Exception()
        {
            throw new Exception("Unknown exception");
        }
    }
}