using System.Collections.Generic;
using Raven.Client;
using SchoolsNearMe.Models;

namespace SchoolsNearMe.Services
{
    public interface ISchoolQuery
    {
        IEnumerable<School> GetSchools(MapBoundries mapBoundries, IDocumentSession ravenSession, int overallOfstedRating, List<string> schoolTypes = null);
    }

    public class MapBoundries
    {
        public MapBoundries(decimal northEastLat, decimal northEastLong, decimal southWestLat, decimal southWestLong)
        {
            NorthEastLat = northEastLat;
            NorthEastLong = northEastLong;
            SouthWestLat = southWestLat;
            SouthWestLong = southWestLong;
        }

        public decimal NorthEastLat { get; private set; }

        public decimal NorthEastLong { get; private set; }

        public decimal SouthWestLat { get; private set; }

        public decimal SouthWestLong { get; private set; }
    }
}