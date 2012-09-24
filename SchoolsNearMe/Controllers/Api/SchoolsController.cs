using System.Collections.Generic;
using SchoolsNearMe.Contracts;
using SchoolsNearMe.Models;
using SchoolsNearMe.Services;

namespace SchoolsNearMe.Controllers.Api
{
    public class SchoolsController : BaseApiController
    {
        private readonly ISchoolShifterService _schoolShifterService;
        public SchoolsController(ISchoolQuery schoolQuery,ISchoolShifterService schoolShifterService)
        {
            _schoolShifterService = schoolShifterService;
            SchoolQuery = schoolQuery;
        }

        private ISchoolQuery SchoolQuery { get; set; }

        // post: /api/schools
        public IEnumerable<School> Post(SchoolSearchParameters parameters)
        {
             var schools =   SchoolQuery.GetSchools(
                    new MapBoundries(parameters.NorthEastLat, parameters.NorthEastLong, parameters.SouthWestLat,
                                     parameters.SouthWestLong),
                    RavenSession, parameters.OfstedRating, parameters.SchoolTypes);
            return _schoolShifterService.Shift(schools);
        }
    }
}