using System.Web.Mvc;
using SchoolsNearMe.Models;
using SchoolsNearMe.Services;

namespace SchoolsNearMe.Controllers
{
    public class ContactController : Controller
    {
        private readonly ISendEmail _sendEmail;

        public ContactController(ISendEmail sendEmail)
        {
            _sendEmail = sendEmail;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(ContactUs contactUs)
        {
            if (!ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(contactUs.From))
                {
                    ModelState.AddModelError("From", "You must enter an email address");
                }
                if (string.IsNullOrEmpty(contactUs.Body))
                {
                    ModelState.AddModelError("Body", "You must enter a message");
                }
                return View();
            }
            _sendEmail.Send(contactUs.From,contactUs.Body);
            return RedirectToAction("ContactConfirm");
        }

        public ActionResult ContactConfirm()
        {
            return View();
        }
    }
}