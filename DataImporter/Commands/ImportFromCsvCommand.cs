using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LumenWorks.Framework.IO.Csv;
using Raven.Client;
using Raven.Client.Document;
using SchoolMap.Net.Models;

namespace SchoolMap.Net.DataImporter.Commands
{
    internal class ImportFromCsvCommand : IRavenCommand
    {
        #region IRavenCommand Members

        public void Execute(DocumentStore store)
        {
            decimal pos = 0;
            var csvReader =
                new CsvReader(new StreamReader(File.OpenRead("C:\\coding\\SchoolMap.Net\\Collateral\\extract.csv")),
                              true);
            decimal total = csvReader.Count();
            csvReader =
                new CsvReader(new StreamReader(File.OpenRead("C:\\coding\\SchoolMap.Net\\Collateral\\extract.csv")),
                              true);
            List<School> batch = new List<School>();
            foreach (var line in csvReader)
            {
                using (IDocumentSession session = store.OpenSession())
                {
                    string id = line[31];
                    School school = session.Query<School>().SingleOrDefault(x => x.Id == id);
                    if (school == null)
                    {
                        school = new School();
                        SetSchoolProperties(line, school);
                        school.Id = id;
                    }
                    else
                    {
                        SetSchoolProperties(line, school);
                    }
                    if (batch.Count == 30)
                    {
                        var savingSession = store.OpenSession();
                        batch.ForEach(x=>savingSession.Store(x));
                        savingSession.SaveChanges();
                        batch.Clear();
                    }
                    batch.Add(school);
                }
                Console.Write("\r{0}%    ", Math.Round((++pos/total)*100, 2));
            }
        }

        private static void SetSchoolProperties(string[] line, School school)
        {
            school.SchoolName = line[3];
            school.Street = line[25];
            school.Town = line[28];
            school.SchoolType = line[29];
            school.Website = line[32];
            school.PostCode = line[17];
            school.IsSchoolClosed = line[6] == "Closed";
        }

        #endregion
    }
}