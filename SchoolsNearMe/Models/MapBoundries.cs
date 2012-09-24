namespace SchoolsNearMe.Services
{
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