using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using SchoolsNearMe.Models;

namespace SchoolsNearMe.Services
{
    public class HostIpLocationService : ILocationService
    {
        private static Dictionary<string, Coordinate> cachedIps = new Dictionary<string, Coordinate>();

        private const string DefaultLocationXml = @"<?xml version=""1.0"" encoding=""ISO-8859-1"" ?>
                    <HostipLookupResultSet version=""1.0.0"" xmlns=""http://www.hostip.info/api"" xmlns:gml=""http://www.opengis.net/gml"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xsi:schemaLocation=""http://www.hostip.info/api/hostip-1.0.0.xsd"">
                     <gml:description>This is the Hostip Lookup Service</gml:description>
                     <gml:name>hostip</gml:name>
                     <gml:boundedBy>
                        <gml:Null>inapplicable</gml:Null>
                     </gml:boundedBy>
                     <gml:featureMember>
                        <Hostip>
                         <gml:name>Sugar Grove, IL</gml:name>
                         <countryName>UNITED STATES</countryName>
                         <countryAbbrev>US</countryAbbrev>
                         <!-- Co-ordinates are available as lng,lat -->
                         <ipLocation>
                            <gml:PointProperty>
                             <gml:Point srsName=""http://www.opengis.net/gml/srs/epsg.xml#4326"">
                                <gml:coordinates>0,0</gml:coordinates>
                             </gml:Point>
                            </gml:PointProperty>
                         </ipLocation>
                        </Hostip>
                     </gml:featureMember>
                    </HostipLookupResultSet>";

        private string GetLocationInformation(string ipAddress)
        {
            string result = string.Empty;
            if (ipAddress == "127.0.0.1")
            {
                result = DefaultLocationXml;
            }
            else
            {
                // go off into the world and get some xml
                bool issue = false;
                try
                {
                    using (var w = new WebClient())
                    {
                        result = w.DownloadString(String.Format("http://api.hostip.info/?ip={0}&position=true", ipAddress));
                    }
                }
                catch (WebException)
                {
                    issue = true;
                }
                catch (NotSupportedException)
                {
                    issue = true;
                }

                if (issue)
                {
                    result = DefaultLocationXml;
                }
            }

            return result;
        }

        public Coordinate GetLocationByIpAddress(IPAddress ipAddress)
        {
            Coordinate result = new Coordinate();
            string ip = ipAddress.ToString();
            if (!cachedIps.ContainsKey(ip))
            {
                string r = GetLocationInformation(ip);

                var xmlResponse = XDocument.Parse(r);
                var gml = (XNamespace)"http://www.opengis.net/gml";
                var ns = (XNamespace)"http://www.hostip.info/api";

                try
                {
                    result = (from x in xmlResponse.Descendants(ns + "Hostip")
                              select new Coordinate
                              {
                                  Longitude = decimal.Parse(x.Descendants(gml + "coordinates").Single().Value.Split(',')[0]),
                                  Latitude = decimal.Parse(x.Descendants(gml + "coordinates").Single().Value.Split(',')[1])
                              }).SingleOrDefault();
                }
                catch (NullReferenceException)
                {
                    //Looks like we didn't get what we expected.
                }
                if (!result.NotSet())
                {
                    cachedIps.Add(ip, result);
                }
            }
            else
            {
                result = cachedIps[ip];
            }
            return result;
        }
    }
}