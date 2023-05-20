using System.Web.Mvc;

namespace DaiPhucVinh.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("")]
    public class HomeController : BaseController
    {
        [Route("")]
        public ActionResult Index()
        {
            return View();
        }
    }
}
