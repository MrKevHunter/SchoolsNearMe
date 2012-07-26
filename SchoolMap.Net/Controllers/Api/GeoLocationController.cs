using SchoolMap.Net.Models;
using SchoolMap.Net.Services;

namespace SchoolMap.Net.Controllers.Api
{
    public class GeoLocationController : BaseApiController
    {
        private ILocationService LocationService { get; set; }

        public GeoLocationController(ILocationService locationService)
        {
            LocationService = locationService;
        }

        public Coordinate GetLocation()
        {
            string ipaddress = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            var parsed = System.Net.IPAddress.Parse(ipaddress);
            var coordinates = LocationService.GetLocationByIpAddress(parsed);
            return coordinates;
        }
    }
}