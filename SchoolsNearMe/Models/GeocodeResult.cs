using System;

namespace SchoolsNearMe.Models
{
    public class GeocodeResult
    {
        private GeocodeResult()
        {
        }

        public GeocodeReturnCode ReturnCode { get; set; }
        public decimal Accuracy { get; set; }
        public Coordinate Location { get; set; }

        public static GeocodeResult CreateResultFromGoogleCsv(string[] geocodeInfo)
        {
            if (geocodeInfo.Length== 3)
            {
                return new GeocodeResult(){ReturnCode = GetReturnCode(geocodeInfo[0])};
            }
            var result = new GeocodeResult
                {
                    Location = new Coordinate(Convert.ToDecimal(geocodeInfo[2]), Convert.ToDecimal(geocodeInfo[3])),
                    ReturnCode = GetReturnCode(geocodeInfo[0]),
                    Accuracy = Convert.ToDecimal(geocodeInfo[1])
                };
            return result;
        }

        private static GeocodeReturnCode GetReturnCode(string returnCode)
        {
            if (returnCode == "200")
            {
                return GeocodeReturnCode.Success;
            }
            if (returnCode == "602")
            {
                return GeocodeReturnCode.UnknownAddress;
            }
            if (returnCode == "620")
            {
                return GeocodeReturnCode.TooManyAttempts;
            }
            return GeocodeReturnCode.Unknown;
        }
    }
}