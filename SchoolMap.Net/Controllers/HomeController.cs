using System.Web.Mvc;

namespace SchoolMap.Net.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}