using SchoolsNearMe.Contracts;
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

        public Coordinate Post(AddressSearch address)
        {           
            GeocodeResult geocodeResult = _geocode.GetCoordinates(address.AddressSearchText);
            return (geocodeResult.ReturnCode==GeocodeReturnCode.Success) ? geocodeResult.Location : new Coordinate(0,0);
        }
    }
}