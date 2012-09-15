using System.Web.Mvc;
using SchoolsNearMe.Models;

namespace SchoolsNearMe.Controllers
{
    public class SchoolDetailController : BaseController
    {
        //
        // GET: /SchoolDetail/

        public ActionResult Detail(int id)
        {
            var school = RavenSession.Load<School>(id.ToString());

            return PartialView(school);
        }

    }
}
