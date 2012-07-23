using System;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;
using SchoolMap.Net.Models;
using SchoolMap.Net.Models.Indexes;

namespace SchoolMap.Net.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetSchools(decimal northEastLat, decimal northEastLong, decimal southWestLat, decimal southWestLong, int ofstedRating)
        {
            var schools = RavenSession.Query<School>().Customize(x => x.WaitForNonStaleResultsAsOfNow()).Where(
                x =>
                x.Location.Latitude >= southWestLat && x.Location.Latitude <= northEastLat &&
                x.Location.Longitude <= northEastLong && x.Location.Longitude >= southWestLong 
                && x.OfstedRating != null 
                && x.OfstedRating.OverallEffectiveness <= ofstedRating
                );

            return new JsonResult()
                       {JsonRequestBehavior = JsonRequestBehavior.AllowGet, Data = JsonConvert.SerializeObject(schools)};
        }
    }
}
