using System.Net;
using System.Web;
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
        /// http://www.aspnet-answers.com/microsoft/ASP-NET/30078410/request-object.aspx
        /// </summary>
        /// <returns></returns>
        private static string GetIP4Address()
        {
            string strIP4Address = string.Empty;

            foreach (IPAddress ip in Dns.GetHostAddresses(HttpContext.Current.Request.UserHostAddress))
            {
                if (ip.AddressFamily.ToString() == "InterNetwork")
                {
                    strIP4Address = ip.ToString();
                    break;
                }
            }
            if (strIP4Address != string.Empty)
            {
                return strIP4Address;
            }
            foreach (IPAddress ip in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (ip.AddressFamily.ToString() == "InterNetwork")
                {
                    strIP4Address = ip.ToString();
                    break;
                }
            }
            return strIP4Address;
        }
    }
}