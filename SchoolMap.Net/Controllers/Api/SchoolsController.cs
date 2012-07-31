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

        //public IEnumerable<School> Get(double northEastLat = 51.54508757438882, double northEastLong = -0.9241957720337268,
        //    double southWestLat = 51.512675601327466, double southWestLong = -1.0466761644897815, int ofstedRating = 4, List<string> schoolTypes = null)
        public IEnumerable<School> Post(SchoolSearchParameters parameters) 
        {
            IEnumerable<School> schools = SchoolQuery.GetSchools(new MapBoundries(parameters.NorthEastLat, parameters.NorthEastLong, parameters.SouthWestLat, parameters.SouthWestLong),
                                                        RavenSession, parameters.OfsteadRating, parameters.SchoolTypes);

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
        public int OfsteadRating { get; set; }
        public List<string> SchoolTypes { get; set; } 
    }
}