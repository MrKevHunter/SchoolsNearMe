using System;
using System.Collections.Generic;
using Raven.Client;
using SchoolsNearMe.Models;

namespace SchoolsNearMe.Services
{
    public interface ISchoolQuery
    {
        IEnumerable<School> GetSchools(MapBoundries mapBoundries, IDocumentSession ravenSession, int overallOfstedRating, List<string> schoolTypes = null);
    }

    public class SchoolQueryLogger : ISchoolQuery
    {
        private readonly ISchoolQuery _schoolQuery;

        public SchoolQueryLogger(ISchoolQuery schoolQuery)
        {
            _schoolQuery = schoolQuery;
        }

        public IEnumerable<School> GetSchools(MapBoundries mapBoundries, IDocumentSession ravenSession, int overallOfstedRating, List<string> schoolTypes = null)
        {
            var result = _schoolQuery.GetSchools(mapBoundries, ravenSession, overallOfstedRating,schoolTypes);
            string id = DateTime.Now.ToString("yyyyMMdd");
            var dailyQueryData = ravenSession.Load<DailyQueryData>(id);
            if (dailyQueryData==null)
            {
                dailyQueryData = new DailyQueryData
                {
                    TotalQueries = 1,
                    Id = id
                };
                ravenSession.Store(dailyQueryData);
            }
            else
            {
                dailyQueryData.TotalQueries++;
            }
            ravenSession.SaveChanges();
            return result;
        }
    }

    public class DailyQueryData
    {
        public string Id { get; set; }
        public int TotalQueries { get; set; }
    }
}