using System.Linq;
using System.Net;
using System.Web;
using SchoolsNearMe.Contracts;
using SchoolsNearMe.Models;

namespace SchoolsNearMe.Controllers.Api
{
    public class GeoLocationController : BaseApiController
    {
        private ILocationService LocationService { get; set; }

        public GeoLocationController(ILocationService locationService)
        {
            LocationService = locationService;
        }

        // get: /api/geolocation
        public Coordinate Get()
        {
            string ipaddress = GetIP4Address();
            var parsed = IPAddress.Parse(ipaddress);
            var coordinates = LocationService.GetLocationByIpAddress(parsed);

            if (coordinates.NotSet())
            {
                throw new HttpException((int)HttpStatusCode.NotFound, "Location unable to be found");
            }

            return coordinates;
        }

        /// <summary>
        /// Originally based on http://www.aspnet-answers.com/microsoft/ASP-NET/30078410/request-object.aspx
        /// </summary>
        /// <returns></returns>
        private static string GetIP4Address()
        {
            var strIP4Address = string.Empty;

            foreach (IPAddress ip in Dns.GetHostAddresses(HttpContext.Current.Request.UserHostAddress).Where(ip => ip.AddressFamily.ToString() == "InterNetwork"))
            {
                strIP4Address = ip.ToString();
                break;
            }

            if (string.IsNullOrWhiteSpace(strIP4Address))
            {
                foreach (IPAddress ip in Dns.GetHostAddresses(Dns.GetHostName()).Where(ip => ip.AddressFamily.ToString() == "InterNetwork"))
                {
                    strIP4Address = ip.ToString();
                    break;
                }
            }

            return strIP4Address;
        }
    }
}