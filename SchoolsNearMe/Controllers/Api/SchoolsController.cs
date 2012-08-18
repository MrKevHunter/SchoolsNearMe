using System.Collections.Generic;
using System.Linq;
using SchoolsNearMe.Models;
using SchoolsNearMe.Services;

namespace SchoolsNearMe.Controllers.Api
{
    public class SchoolsController : BaseApiController
    {
        private ISchoolQuery SchoolQuery { get; set; }

        public SchoolsController(ISchoolQuery schoolQuery)
        {
            SchoolQuery = schoolQuery;
        }

        public IEnumerable<School> Post(SchoolSearchParameters parameters) 
        {
            IEnumerable<School> schools = SchoolQuery.GetSchools(new MapBoundries(parameters.NorthEastLat, parameters.NorthEastLong, parameters.SouthWestLat, parameters.SouthWestLong),
                                                        RavenSession, parameters.OfstedRating, parameters.SchoolTypes);

            return schools.ToList();
        }
    }
}