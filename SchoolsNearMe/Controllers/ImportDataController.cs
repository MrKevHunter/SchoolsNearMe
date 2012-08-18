using System.Web.Mvc;
using Raven.Client.Embedded;
using SchoolsNearMe.Models;

namespace SchoolsNearMe.Controllers
{
    public class ImportDataController : Controller
    {
        //
        // GET: /ImportData/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ImportData()
        {
            DumperStats dumperStats = new RavenDbDumper((EmbeddableDocumentStore) MvcApplication.Store).Import(Server.MapPath("~/App_data/Dump of Default, Jul 29 2012 18-38.raven.dump.raven.dump"));
            return View(dumperStats);
        }
    }
}
