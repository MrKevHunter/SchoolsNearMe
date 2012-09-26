using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Raven.Client.Document;
using SchoolsNearMe.Models;

namespace SchoolsNearMe.DataImporter.Commands
{
    public class CheckProximityCommand : IRavenCommand
    {
        #region DistanceType enum

        public enum DistanceType
        {
            Miles,
            Km
        }

        #endregion

        private DocumentStore _store;
        private List<KeyValuePair<double,School>> _distances = new List<KeyValuePair<double, School>>();
        private List<PostCode> _postCodes;

        #region IRavenCommand Members

        public void Execute(DocumentStore store)
        {
            _postCodes = new List<PostCode>();
            string[] readAllLines = File.ReadAllLines(@"C:\coding\SchoolsNearMe\Collateral\postcodes to Lat Long.csv");
            foreach (string readAllLine in readAllLines.Skip(1))
            {
                string[] items = readAllLine.Split(',');
                var p = new PostCode
                    {
                        PostalCode = items[1],
                        Position =
                            new Position {Latitude = Convert.ToDouble(items[2]), Longitude = Convert.ToDouble(items[3])}
                    };
                _postCodes.Add(p);
            }
            StreamWriter streamWriter = File.CreateText(@"C:\coding\SchoolsNearMe\Collateral\mislocated schools.txt");
            _store = store;

            new RavenEach().RavenForEach<School>(store, CheckDistance);
            foreach (var distance in _distances.Distinct().OrderByDescending(x=>x.Key).Take(100))
            {
                string format = string.Format("{0} - {1} - {2}", distance.Value.Id, distance.Value.SchoolName, distance.Key);
                Console.WriteLine(format);
                streamWriter.WriteLine(format);
            }
            streamWriter.Flush();
            streamWriter.Close();
            Console.WriteLine("Avg:{0}", _distances.Average(x=>x.Key));
            Console.ReadLine();
        }

        #endregion

        private void CheckDistance(School obj)
        {
            string pc;
            if (obj.PostCode.Contains(" "))
            {
                pc = obj.PostCode.Substring(0, obj.PostCode.IndexOf(' '));
            }
            else
            {
                pc = obj.PostCode;
            }
            PostCode singleOrDefault = _postCodes.FirstOrDefault(x => x.PostalCode == pc);
            if (singleOrDefault!=null)
            {
                double distance = Distance(singleOrDefault.Position, new Position() {Latitude = (double) obj.Location.Latitude, Longitude = (double) obj.Location.Longitude}, DistanceType.Km);
                _distances.Add(new KeyValuePair<double, School>(distance, obj));
            }
        }

        public double Distance(Position pos1, Position pos2, DistanceType type)
        {
            double R = (type == DistanceType.Miles) ? 3960 : 6371;

            double dLat = ToRadian(pos2.Latitude - pos1.Latitude);


            double dLon = ToRadian(pos2.Longitude - pos1.Longitude);


            double a = Math.Sin(dLat/2)*Math.Sin(dLat/2) +
                       Math.Cos(ToRadian(pos1.Latitude))*Math.Cos(ToRadian(pos2.Latitude))*
                       Math.Sin(dLon/2)*Math.Sin(dLon/2);
            double c = 2*Math.Asin(Math.Min(1, Math.Sqrt(a)));
            double d = R*c;

            return d;
        }

        /// <summary>
        /// Convert to Radians.
        /// </summary>
        /// <param name=”val”></param>
        /// <returns></returns>
        private double ToRadian(double val)
        {
            return (Math.PI/180)*val;
        }

        #region Nested type: Position

        public class Position
        {
            public double Latitude { get; set; }

            public double Longitude { get; set; }
        }

        #endregion

        #region Nested type: PostCode

        private class PostCode
        {
            public string PostalCode { get; set; }

            public Position Position { get; set; }
        }

        #endregion
    }
}