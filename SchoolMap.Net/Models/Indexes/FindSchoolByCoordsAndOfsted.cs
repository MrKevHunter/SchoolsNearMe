using System.Linq;
using Raven.Client.Indexes;

namespace SchoolMap.Net.Models.Indexes
{
    public class FindSchoolByCoordsAndOfsted : AbstractIndexCreationTask<School>
    {
        public FindSchoolByCoordsAndOfsted()
        {
            Map = schools => from school in schools
                             where school.OfstedRating != null 
                             select new
                                 {
                                     school.OfstedRating.OverallEffectiveness,
                                     school.Location,
                                     school.Location.Latitude,
                                     school.Location.Longitude
                                 };
        }
    }
}