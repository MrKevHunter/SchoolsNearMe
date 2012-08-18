namespace SchoolsNearMe.Models
{
    public interface ISpatialCoordinate
    {
        decimal Latitude { get; set; }
        decimal Longitude { get; set; }

        bool NotSet();
    }
}