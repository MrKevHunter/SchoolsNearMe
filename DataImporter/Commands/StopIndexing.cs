using System;
using System.Net;
using Raven.Client.Document;

namespace SchoolMap.Net.DataImporter.Commands
{
    internal class StopIndexing : IRavenCommand
    {
        public void Execute(DocumentStore store)
        {

            try
            {
                using (var webClient = new WebClient())
                {
                    webClient.UseDefaultCredentials = true;
                    var result = webClient.UploadString(new Uri(new
                                                                    Uri("http://localhost:8080"), "/admin/stopindexing"), "POST", "");
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error encountered while stopping indexing", e);
            }

        }
    }
}