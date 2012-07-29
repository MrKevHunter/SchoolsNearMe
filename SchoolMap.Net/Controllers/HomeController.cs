using System.Collections.Generic;
using System.Web.Mvc;
using Newtonsoft.Json;
using SchoolMap.Net.Models;
using SchoolMap.Net.Services;

namespace SchoolMap.Net.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ISchoolQuery _schoolQuery;

        public HomeController(ISchoolQuery schoolQuery)
        {
            _schoolQuery = schoolQuery;
        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetSchools(decimal northEastLat, decimal northEastLong, decimal southWestLat,
                                     decimal southWestLong, int ofstedRating, List<string> schoolTypes)
        {
            IEnumerable<School> schools = _schoolQuery.GetSchools(new MapBoundries(northEastLat, northEastLong, southWestLat, southWestLong), RavenSession, ofstedRating, schoolTypes);



            return new JsonResult
                {JsonRequestBehavior = JsonRequestBehavior.AllowGet, Data = JsonConvert.SerializeObject(schools)};
        }
    }
}