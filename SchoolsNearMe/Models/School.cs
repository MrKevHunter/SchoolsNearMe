using System;
using System.Collections.Generic;

namespace SchoolsNearMe.Models
{
    public class School : IEquatable<School>
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
                const string httpPrefix = "http://";
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

        public bool Equals(School other)
        {
            return this.Id.Equals(other.Id);
        }

        private sealed class IdEqualityComparer : IEqualityComparer<School>
        {
            public bool Equals(School x, School y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return string.Equals(x.Id, y.Id);
            }

            public int GetHashCode(School obj)
            {
                return obj.Id.GetHashCode();
            }
        }
        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        private static readonly IEqualityComparer<School> IdComparerInstance = new IdEqualityComparer();

        public static IEqualityComparer<School> IdComparer
        {
            get { return IdComparerInstance; }
        }

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