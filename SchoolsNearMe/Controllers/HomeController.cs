using System.Web.Mvc;

namespace SchoolsNearMe.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}