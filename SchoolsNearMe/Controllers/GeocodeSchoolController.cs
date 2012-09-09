using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SchoolsNearMe.Models;

namespace SchoolsNearMe.Controllers
{
    public class GeocodeSchoolController : BaseController
    {
        private readonly IGeocode _geocode;

        public GeocodeSchoolController(IGeocode geocode)
        {
            _geocode = geocode;
        }

        //
        // GET: /GeocodeSchool/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Update(string txtSchoolUrn)
        {
            var school = RavenSession.Load<School>(txtSchoolUrn);
            string address = school.GetAddress();
            GeocodeResult geocodeResult = _geocode.GetCoordinates(address);
            if (geocodeResult.ReturnCode == GeocodeReturnCode.Success)
            {
                school.Location = geocodeResult.Location;
                RavenSession.Store(school);
                return View("UpdateSchool", school);
            }
         
            return View("UpdateSchool", school);
        }
    }
}
