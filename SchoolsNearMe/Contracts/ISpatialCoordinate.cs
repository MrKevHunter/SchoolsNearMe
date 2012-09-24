namespace SchoolsNearMe.Contracts
{
    public interface ISpatialCoordinate
    {
        decimal Latitude { get; set; }
        decimal Longitude { get; set; }

        bool NotSet();
    }
}