using System.Web.Mvc;
using SchoolsNearMe.Models;

namespace SchoolsNearMe.Controllers
{
    public class ContactController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

/*        [HttpPost]
        public ActionResult Index(ContactUs contactUs)
        {
            if (!ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(contactUs.From))
                {
                    ModelState.AddModelError("From","You must enter an email address");
                }
                if (string.IsNullOrEmpty(contactUs.Body))
                {
                    ModelState.AddModelError("Body", "You must enter a message");
                }
                return View();
            }
            return RedirectToAction("ContactConfirm");
        }*/

        public ActionResult ContactConfirm()
        {
            return View();
        }
    }
}