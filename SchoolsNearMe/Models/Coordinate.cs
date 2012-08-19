using System;

namespace SchoolsNearMe.Models
{
    public struct Coordinate : ISpatialCoordinate, IEquatable<Coordinate>
    {
        private decimal _latitude;
        private decimal _longitude;

        public Coordinate(decimal latitude, decimal longitude)
        {
            _latitude = latitude;
            _longitude = longitude;
        }

        public decimal Latitude
        {
            get { return _latitude; }
            set { _latitude = value; }
        }

        public decimal Longitude
        {
            get { return _longitude; }
            set { _longitude = value; }
        }

        public bool Equals(Coordinate other)
        {
            return _latitude == other._latitude && _longitude == other._longitude;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Coordinate && Equals((Coordinate) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (_latitude.GetHashCode()*397) ^ _longitude.GetHashCode();
            }
        }

        public static bool operator ==(Coordinate left, Coordinate right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Coordinate left, Coordinate right)
        {
            return !left.Equals(right);
        }

        public bool NotSet()
        {
            return ((Latitude == 0) && (Longitude == 0));
        }
    }
}