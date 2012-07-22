namespace SchoolMap.Net.Models
{
    public struct Coordinate : ISpatialCoordinate
    {
        private decimal _latitude;
        private decimal _longitude;

        public Coordinate(decimal latitude, decimal longitude)
        {
            _latitude = latitude;
            _longitude = longitude;
        }

        #region ISpatialCoordinate Members

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

        public bool NotSet()
        {
            return ((Latitude == 0) && (Longitude == 0));
        }

        #endregion
    }
}