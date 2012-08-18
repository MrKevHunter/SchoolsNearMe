using System;
using System.Net;
using Raven.Client.Document;
using Raven.Imports.SignalR;
using SchoolsNearMe.Models;

namespace SchoolsNearMe.DataImporter.Commands
{
    class UploadRavenItem : IRavenCommand
    {
        public void Execute(DocumentStore store)
        {
            var ravenEach = new RavenEach();
            ravenEach.RavenForEach<School>(store,Upload);
        }

        private void Upload(School obj)
        {
            try
            {
                using (var webClient = new WebClient())
                {
                    webClient.UseDefaultCredentials = true;
                    string stringify = new JsonNetSerializer().Stringify(obj);
                    var result = webClient.UploadString(new Uri(new
                                                                    Uri("http://www.schoolsnearme.co.uk"), "/api/SchoolsUpload"), "POST", stringify);
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error encountered while stopping indexing", e);
            }
        }
    }
}