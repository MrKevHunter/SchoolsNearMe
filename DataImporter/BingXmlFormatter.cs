using System.Collections.Generic;
using System.Xml.Linq;
using SchoolsNearMe.Models;

namespace SchoolsNearMe.DataImporter
{
    public class BingXmlFormatter
    {
        private int _id;

        public string CreateBingGeocodeXml(IEnumerable<School> schools)
        {
            var xml = new XElement("GeocodeFeed");
            foreach (School school in schools)
            {
                xml.Add(SchoolToGeocodeEntity(school));
                _id++;
            }
            return xml.ToString();
        }
        
        private XElement SchoolToGeocodeEntity(School school)
        {
            XNamespace ns = "http://schemas.microsoft.com/search/local/2010/5/geocode";
            return new XElement(ns + "GeocodeEntity",
                                new XAttribute("Id", _id),
                                new XElement("GeocodeRequest", new XAttribute("Culture", "en-GB")),
                                new XElement("Address", new XAttribute("AddressLine", school.Street),
                                new XAttribute("Locality", school.Town),
                                new XAttribute("PostalCode", school.PostCode)));
        }
    }
}