using System;
using System.Collections.Generic;
using System.Linq;
using Raven.Client;
using SchoolsNearMe.Models;

namespace SchoolsNearMe.Services
{
    public class SchoolQuery : ISchoolQuery
    {
        #region ISchoolQuery Members

        public IEnumerable<School> GetSchools(MapBoundries mapBoundries, IDocumentSession ravenSession,
                                              int overallOfstedRating, List<string> schoolTypes = null)
        {
            string query = string.Format("OfstedRating.OverallEffectiveness:[1 TO {0}]", overallOfstedRating);
            query += string.Format(" AND Location.Latitude:[{0} TO {1}]",
                                   Math.Min(mapBoundries.SouthWestLat, mapBoundries.NorthEastLat),
                                   Math.Max(mapBoundries.SouthWestLat, mapBoundries.NorthEastLat));
            query += string.Format(" AND Location.Longitude:[{0} TO {1}]",
                                   Math.Max(mapBoundries.NorthEastLong, mapBoundries.SouthWestLong),
                                   Math.Min(mapBoundries.NorthEastLong, mapBoundries.SouthWestLong));
            if (schoolTypes != null && schoolTypes.Count > 0)
            {
                if (schoolTypes.Count == 1)
                {
                    query += string.Format(" AND TypeOfEstablishment:\"{0}\"", schoolTypes[0]);
                }
                else
                {
                    query += string.Format(" AND (");
                    query +=
                        string.Join(" OR ", schoolTypes.Select(x => string.Format("TypeOfEstablishment:\"{0}\"", x))) +
                        ")";
                }
            }


            List<School> schools =
                ravenSession.Advanced.LuceneQuery<School>().WaitForNonStaleResultsAsOfNow().Skip(0).Take(100).Where(
                    query).ToList();

            return schools;
        }

        #endregion
    }
}