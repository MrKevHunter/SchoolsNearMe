using System.Collections.Generic;
using Raven.Client;
using SchoolsNearMe.Models;

namespace SchoolsNearMe.Services
{
    public interface ISchoolQuery
    {
        IEnumerable<School> GetSchools(MapBoundries mapBoundries, IDocumentSession ravenSession, int overallOfstedRating, List<string> schoolTypes = null);
    }
}