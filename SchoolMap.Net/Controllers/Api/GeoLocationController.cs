using System.IO;
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

        ///// <summary>
        ///// http://stackoverflow.com/questions/1069103/how-to-get-my-own-ip-address-in-c
        ///// </summary>
        ///// <returns></returns>
        //private string GetPublicIp()
        //{
        //    string ipaddress = "";
        //    WebRequest request = WebRequest.Create("http://checkip.dyndns.org/");
        //    using (WebResponse response = request.GetResponse())
        //    using (StreamReader stream = new StreamReader(response.GetResponseStream()))
        //    {
        //        ipaddress = stream.ReadToEnd();
        //    }

        //    //Search for the ip in the html
        //    int first = ipaddress.IndexOf("Address: ") + 9;
        //    int last = ipaddress.LastIndexOf("</body>");
        //    ipaddress = ipaddress.Substring(first, last - first);

        //    return ipaddress;
        //}

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