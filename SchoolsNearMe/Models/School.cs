using System;

namespace SchoolsNearMe.Models
{
    public class School
    {
        private string _website;
        public string SchoolName { get; set; }

        public string PostCode { get; set; }

        public string Id { get; set; }

        public string Street { get; set; }

        public string Town { get; set; }

        public TypeOfEstablishment TypeOfEstablishment { get; set; }

        public string Website
        {
            get
            {
                string httpPrefix = "http://";
                if (_website.StartsWith(httpPrefix,StringComparison.OrdinalIgnoreCase))
                {
                    return _website;
                }
                return httpPrefix + _website;
            }
            set { _website = value; }
        }

        public Coordinate Location { get; set; }

        public OfstedRating OfstedRating { get; set; }

        public bool IsSchoolClosed { get; set; }

        public string SchoolType { get; set; }

        public override string ToString()
        {
            return SchoolName + ", " + Town;
        }

        public string GetAddress()
        {
            return string.Format("{0}, {1}, {2}, {3}", SchoolName, Street, Town, PostCode);
        }
    }
}