using System.Collections.Generic;
using System.Linq;
using SchoolMap.Net.Models;
using SchoolMap.Net.Services;

namespace SchoolMap.Net.Controllers.Api
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

    public class SchoolSearchParameters
    {
        public SchoolSearchParameters()
        {
            SchoolTypes = new List<string>();
        }

        public decimal NorthEastLat { get; set; }
        public decimal NorthEastLong { get; set; }
        public decimal SouthWestLat { get; set; }
        public decimal SouthWestLong { get; set; }
        public int OfstedRating { get; set; }
        public List<string> SchoolTypes { get; set; } 
    }
}