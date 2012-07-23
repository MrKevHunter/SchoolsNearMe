using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SchoolMap.Net.Models;

namespace SchoolMap.Net.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetSchools(decimal northEastLat, decimal northEastLong, decimal southWestLat, decimal southWestLong)
        {
           var schools = WebApiApplication.CurrentSession.Query<School>().Where(
                x =>
                x.Location.Latitude >= southWestLat && x.Location.Latitude <= northEastLat &&
                x.Location.Longitude <= northEastLong && x.Location.Longitude >= southWestLong);
            foreach (var school in schools)
            {
                Console.WriteLine(school);
            }
            return null;
        }
    }
}
