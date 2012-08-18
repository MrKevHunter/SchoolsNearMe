using System.Collections.Generic;

namespace SchoolsNearMe.Controllers.Api
{
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