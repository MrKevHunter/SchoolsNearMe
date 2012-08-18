using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using LumenWorks.Framework.IO.Csv;
using Raven.Client;
using Raven.Client.Document;
using SchoolsNearMe.Models;

namespace SchoolsNearMe.DataImporter.Commands
{
    internal class ImportFromCsvCommand : IRavenCommand
    {
        #region IRavenCommand Members

        public void Execute(DocumentStore store)
        {
            Stopwatch sw = Stopwatch.StartNew();
            decimal pos = 0;
            var csvReader =
                new CsvReader(new StreamReader(File.OpenRead("C:\\coding\\SchoolMap.Net\\Collateral\\extract.csv")),
                              true);
            decimal total = csvReader.Count();
            csvReader =
                new CsvReader(new StreamReader(File.OpenRead("C:\\coding\\SchoolMap.Net\\Collateral\\extract.csv")),
                              true);
            var batch = new List<School>();
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
                        batch.ForEach(savingSession.Store);
                        savingSession.SaveChanges();
                        batch.Clear();
                    }
                    batch.Add(school);
                }
                Console.Write("\r{0}%    ", Math.Round((++pos/total)*100, 2));
            }
            sw.Stop();
            Console.WriteLine("Completed in {0} minutes");
        }

        private static void SetSchoolProperties(string[] line, School school)
        {
            school.SchoolName = line[3];
            school.Street = line[25];
            school.Town = line[28];
            school.SchoolType = line[29];
            school.Website = line[32];
            school.PostCode = line[17];
            school.TypeOfEstablishment = SetTypeOfEstablishment(line[36]);

            school.IsSchoolClosed = line[6] == "Closed";
        }

        private static TypeOfEstablishment SetTypeOfEstablishment(string input)
        {
            if(input=="Nursery")
            {
                return TypeOfEstablishment.Nursery;
            }
            if (input == "16 Plus")
            {
                return TypeOfEstablishment.SixteenPlus;
            }
            if(input=="Primary" || input == "Middle Deemed Primary")
            {
                return TypeOfEstablishment.Primary;
            }
            if (input=="Secondary" || input == "Middle Deemed Secondary")
            {
                  return TypeOfEstablishment.Secondary;
            }
            return TypeOfEstablishment.NotApplicable;
        }

        #endregion
    }
}