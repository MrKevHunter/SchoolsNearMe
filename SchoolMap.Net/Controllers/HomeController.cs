using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using Newtonsoft.Json;
using SchoolMap.Net.Models;

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

            Expression<Func<School, bool>> schoolsInLocationWithOfstedAndMiniumum = x => 
                x.OfstedRating != null 
                && x.OfstedRating.OverallEffectiveness <= ofstedRating 
                && x.Location.Latitude >= southWestLat 
                && x.Location.Latitude <= northEastLat 
                && x.Location.Longitude <= northEastLong 
                && x.Location.Longitude >= southWestLong;

            var schools = RavenSession.Query<School>()
                .Customize(x=>x.WaitForNonStaleResultsAsOfNow())
                .Where(schoolsInLocationWithOfstedAndMiniumum)
				.Take(100000)
                .ToList();


            return new JsonResult()
                       {JsonRequestBehavior = JsonRequestBehavior.AllowGet, Data = JsonConvert.SerializeObject(schools)};
        }
    }
}
