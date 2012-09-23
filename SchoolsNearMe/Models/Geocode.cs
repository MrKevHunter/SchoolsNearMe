using System;
using System.Linq;
using System.Net;
using System.Web;
using SchoolsNearMe.Contracts;

namespace SchoolsNearMe.Models
{
    public class Geocode : IGeocode
    {
        private const string _googleUri = "http://maps.google.com/maps/geo?q=";
        private const string _googleKey = "AIzaSyDWVbVQzp7brCHZzomUT2hFsEjvAPK8xc8";
        private const string _outputType = "csv"; // Available options: csv, xml, kml, json

        private  Uri GetGeocodeUri(string address)
        {
            address = HttpUtility.UrlEncode(address);
            return new Uri(String.Format("{0}{1}&output={2}&key={3}", _googleUri, address, _outputType, _googleKey));
        }

        /// <summary>
        /// Gets a Coordinate from a address.
        /// </summary>
        /// <param name="address">An address.
        /// <remarks>
        /// <example>1600 Amphitheatre Parkway Mountain View, CA 94043</example>
        /// </remarks>
        /// </param>
        /// <returns>A spatial coordinate that contains the latitude and longitude of the address.</returns>
        public  GeocodeResult GetCoordinates(string address)
        {
            var client = new WebClient();
            Uri uri = GetGeocodeUri(address);

            /* The first number is the status code, 
         * the second is the accuracy, 
         * the third is the latitude, 
         * the fourth one is the longitude.
         */
            string[] geocodeInfo = client.DownloadString(uri).Split(',');
            GeocodeResult resultFromGoogleCsv = GeocodeResult.CreateResultFromGoogleCsv(geocodeInfo);
            while (resultFromGoogleCsv.ReturnCode == GeocodeReturnCode.UnknownAddress)
            {
                address = string.Join(",", address.Split(',').Skip(1).ToArray());
                if (string.IsNullOrWhiteSpace(address))
                {
                    break;
                }
                uri = GetGeocodeUri(address);
                geocodeInfo = client.DownloadString(uri).Split(',');
                resultFromGoogleCsv = GeocodeResult.CreateResultFromGoogleCsv(geocodeInfo);
            }
            return resultFromGoogleCsv;
        }
    }
}