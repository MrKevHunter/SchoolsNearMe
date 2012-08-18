using SchoolsNearMe.Models;

namespace SchoolsNearMe.Controllers.Api
{
    public class AddressGeocodeController : BaseApiController
    {
        private readonly IGeocode _geocode;

        public AddressGeocodeController(IGeocode geocode)
        {
            _geocode = geocode;
        }


        public Coordinate Post(AddressSearch  address)
        {
           
            GeocodeResult geocodeResult = _geocode.GetCoordinates(address.AddressSearchText);
            if (geocodeResult.ReturnCode==GeocodeReturnCode.Success)
            {
                return geocodeResult.Location;
            }
            return new Coordinate(0,0);
        }

    }
}