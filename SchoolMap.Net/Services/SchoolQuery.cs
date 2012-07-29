using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Raven.Client;
using Raven.Client.Linq;
using SchoolMap.Net.Models;

namespace SchoolMap.Net.Services
{
    public class SchoolQuery : ISchoolQuery
    {
        #region ISchoolQuery Members

        public IEnumerable<School> GetSchools(MapBoundries mapBoundries, IDocumentSession ravenSession,
                                              int overallOfstedRating, List<string> schoolTypes = null)
        {
            string query = string.Format("OfstedRating.OverallEffectiveness:[1 TO {0}]",overallOfstedRating);
            query += string.Format(" AND Location.Latitude:[{0} TO {1}]",
                                   Math.Min(mapBoundries.SouthWestLat, mapBoundries.NorthEastLat),
                                   Math.Max(mapBoundries.SouthWestLat, mapBoundries.NorthEastLat));
            query += string.Format(" AND Location.Longitude:[{0} TO {1}]",
                                   Math.Max(mapBoundries.NorthEastLong, mapBoundries.SouthWestLong),
                                   Math.Min(mapBoundries.NorthEastLong, mapBoundries.SouthWestLong));
            if (schoolTypes!= null && schoolTypes.Count > 0)
            {
                if (schoolTypes.Count == 1)
                {
                    query += string.Format(" AND TypeOfEstablishment:\"{0}\"", schoolTypes[0]);
                }
                else
                {
                    query += string.Format(" AND (");

                    query += string.Join(" OR ", schoolTypes.Select(x => string.Format("TypeOfEstablishment:\"{0}\"", x))) + ")";
                }
            }
                
            
            List<School> schools = ravenSession.Advanced.LuceneQuery<School>().WaitForNonStaleResultsAsOfNow().Skip(0).Take(100).Where(query).ToList();

            /*        Expression<Func<School, bool>> schoolsInLocationWithOfstedAndMiniumum = x =>
                                                                                    x.OfstedRating != null &&
                                                                                    x.OfstedRating.OverallEffectiveness <= overallOfstedRating
                                                                                    &&
                                                                                    x.Location.Latitude >=
                                                                                    mapBoundries.SouthWestLat
                                                                                    &&
                                                                                    x.Location.Latitude <=
                                                                                    mapBoundries.NorthEastLat
                                                                                    &&
                                                                                    x.Location.Longitude <=
                                                                                    mapBoundries.NorthEastLong
                                                                                    &&
                                                                                    x.Location.Longitude >=
                                                                                    mapBoundries.SouthWestLong;


            return ravenSession.Query<School>()
                .Customize(x => x.WaitForNonStaleResultsAsOfNow())
                .Where(schoolsInLocationWithOfstedAndMiniumum)
                .ToList();*/
            return schools;
        }

        #endregion
    }
}