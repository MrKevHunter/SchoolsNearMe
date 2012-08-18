using System.Net;
using System.Web.Mvc;
using Raven.Abstractions.Data;
using Raven.Abstractions.Smuggler;
using Raven.Smuggler;

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