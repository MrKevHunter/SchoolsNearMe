using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Xml;

namespace SchoolsNearMe.DataImporter
{
    //A summary of status information returned in the response when you check 
    // job status. 

    public class BingGeoCoder
    {
        private static string CreateJob(string dataFilePath, string dataFormat, string key, string description)
        {
            //Define parameters for the HTTP request
            //
            // The 'Content-Type' header of the HTTP Request must be "text/plain" or "application/xml"
            // depending on the input data format.
            //
            string contentType = "text/plain";
            if (dataFormat.Equals("xml", StringComparison.OrdinalIgnoreCase))
                contentType = "application/xml";

            var queryStringBuilder = new StringBuilder();

            //
            // The 'input'(input format) and 'key' (Bing Maps Key) parameters are required.
            //
            queryStringBuilder.Append("input=").Append(Uri.EscapeUriString(dataFormat));
            queryStringBuilder.Append("&");
            queryStringBuilder.Append("key=").Append(Uri.EscapeUriString(key));

            if (!String.IsNullOrEmpty(description))
            {
                //
                // The 'description' parameter is optional.
                //
                queryStringBuilder.Append("&");
                queryStringBuilder.Append("description=").Append(Uri.EscapeUriString(description));
            }

            //Build the HTTP URI that will upload and create the geocode dataflow job
            var uriBuilder = new UriBuilder("http://spatial.virtualearth.net");
            uriBuilder.Path = "/REST/v1/dataflows/geocode";
            uriBuilder.Query = queryStringBuilder.ToString();

            //Include the data to geocode in the HTTP request
            using (FileStream dataStream = File.OpenRead(dataFilePath))
            {
                var request = (HttpWebRequest) WebRequest.Create(uriBuilder.Uri);

                //
                // The HTTP method must be 'POST'.
                //
                request.Method = "POST";
                request.ContentType = contentType;

                using (Stream requestStream = request.GetRequestStream())
                {
                    var buffer = new byte[16384];
                    int bytesRead = dataStream.Read(buffer, 0, buffer.Length);
                    while (bytesRead > 0)
                    {
                        requestStream.Write(buffer, 0, bytesRead);

                        bytesRead = dataStream.Read(buffer, 0, buffer.Length);
                    }
                }

                //Submit the HTTP request and check if the job was created successfully. 
                using (var response = (HttpWebResponse) request.GetResponse())
                {
                    //
                    // If the job was created successfully, the status code should be
                    // 201 (Created) and the 'Location' header should contain a URL
                    // that defines the location of the new dataflow job. You use this 
                    // URL with the Bing Maps Key to query the status of your job.
                    //
                    if (response.StatusCode != HttpStatusCode.Created)
                        throw new Exception("An HTTP error status code was encountered when creating the geocode job.");

                    string dataflowJobLocation = response.GetResponseHeader("Location");
                    if (String.IsNullOrEmpty(dataflowJobLocation))
                        throw new Exception(
                            "The 'Location' header is missing from the HTTP response when creating a goecode job.");

                    return dataflowJobLocation;
                }
            }
        }


        //Checks the status of a dataflow job and defines the URLs to use to download results when the job is completed.
        //Parameters: 
        //   dataflowJobLocation: The URL to use to check status for a job.
        //   key: The Bing Maps Key for this job. The same key is used to create the job and download results.  
        //Return value: A DownloadDetails object that contains the status of the geocode dataflow job (Completed, Pending, Aborted).
        //              When the status is set to Completed, DownloadDetails also contains the links to download the results
        private static DownloadDetails CheckStatus(string dataflowJobLocation, string key)
        {
            var statusDetails = new DownloadDetails();
            statusDetails.jobStatus = "Pending";

            //Build the HTTP Request to get job status
            var uriBuilder = new UriBuilder(dataflowJobLocation + @"?key=" + key + "&output=xml");
            var request = (HttpWebRequest) WebRequest.Create(uriBuilder.Uri);

            request.Method = "GET";

            //Submit the request and read the response to get job status and to retrieve the links for 
            //  downloading the job results
            //Note: The following conditional statements make use of the fact that the 'Status' field will  
            //  always appear after the 'Link' fields in the HTTP response.
            using (var response = (HttpWebResponse) request.GetResponse())
            {
                if (response.StatusCode != HttpStatusCode.OK)
                    throw new Exception("An HTTP error status code was encountered when checking job status.");

                using (Stream receiveStream = response.GetResponseStream())
                {
                    var reader = new XmlTextReader(receiveStream);
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            if (reader.Name.Equals("Status"))
                            {
                                //return job status
                                statusDetails.jobStatus = reader.ReadString();

                                return (statusDetails);
                            }
                            if (reader.Name.Equals("Link"))
                            {
                                //Set the URL location values for retrieving 
                                // successful and failed job results
                                reader.MoveToFirstAttribute();
                                if (reader.Value.Equals("output"))
                                {
                                    reader.MoveToNextAttribute();
                                    if (reader.Value.Equals("succeeded"))
                                    {
                                        statusDetails.suceededlink = reader.ReadString();
                                    }
                                    else if (reader.Value.Equals("failed"))
                                    {
                                        statusDetails.failedlink = reader.ReadString();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return (statusDetails);
        }

        //Downloads job results to files names Success.txt (successfully geocoded results) and 
        //   Failed.txt (info about spatial data that was not geocoded successfully).
        //Parameters: 
        //   statusDetails: Inclues job status and the URLs to use to download all geocoded results.
        //   key: The Bing Maps Key for this job. The same key is used to create the job and get job status.   

        private static void DownloadResults(DownloadDetails statusDetails, string key)
        {
            //Write the results for data that was geocoded successfully to a file named Success.xml
            if (statusDetails.suceededlink != null && !statusDetails.suceededlink.Equals(String.Empty))
            {
                //Create a request to download successfully geocoded data. You must add the Bing Maps Key to the 
                //  download location URL provided in the response to the job status request.
                var successUriBuilder = new UriBuilder(statusDetails.suceededlink + @"?key=" + key);
                var request1 = (HttpWebRequest) WebRequest.Create(successUriBuilder.Uri);

                request1.Method = "GET";

                using (var response = (HttpWebResponse) request1.GetResponse())
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception("An HTTP error status code was encountered when downloading results.");

                    using (Stream receiveStream = response.GetResponseStream())
                    {
                        var successfile = new StreamWriter("Success.txt");
                        using (var r = new StreamReader(receiveStream))
                        {
                            string line;
                            while ((line = r.ReadLine()) != null)
                            {
                                successfile.Write(line);
                            }
                        }
                        successfile.Close();
                    }
                }
            }

            //If some spatial data could not be geocoded, write the error information to a file called Failed.xml
            if (statusDetails.failedlink != null && !statusDetails.failedlink.Equals(String.Empty))
            {
                //Create an HTTP request to download error information. You must add the Bing Maps Key to the 
                //  download location URL provided in the response to the job status request.
                var failedUriBuilder = new UriBuilder(statusDetails.failedlink + @"?key=" + key);
                var request2 = (HttpWebRequest) WebRequest.Create(failedUriBuilder.Uri);

                request2.Method = "GET";

                using (var response = (HttpWebResponse) request2.GetResponse())
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception("An HTTP error status code was encountered when downloading results.");

                    using (Stream receiveStream = response.GetResponseStream())
                    {
                        var failedfile = new StreamWriter("Failed.txt");
                        using (var r = new StreamReader(receiveStream))
                        {
                            string line;
                            while ((line = r.ReadLine()) != null)
                            {
                                failedfile.Write(line);
                            }
                        }
                        failedfile.Close();
                    }
                }
            }
        }


        //
        // Sample command-line:
        // GeocodeDataFlowExample.exe <dataFilePath> <dataFormat> <key> [<description>]
        //
        // Where:
        // <dataFilePath> is a path to the data file containing entities to geocode.
        // <dataFormat> is one of these types: xml, csv, tab, pipe.
        // <key> is a Bing Maps Key from http://www.bingmapsportal.com.
        // <description> is an optional description for the dataflow job.
        //    


        public void GeocodeLocations(string path)
        {
            string dataFilePath = path;
            string dataFormat = "xml";
            string key = "Anjjq6kccKkjvnHb9j79xArp8aInLfOHo0Irv87DxglGgZwSisIxnoyJXsKf-rPa";
            string description = null;

            try
            {

                string dataflowJobLocation = CreateJob(dataFilePath, dataFormat, key, description);
                Console.WriteLine("Dataflow Job Location: {0}", dataflowJobLocation);

                //Continue to check the dataflow job status until the job has completed
                DownloadDetails statusDetails = new DownloadDetails();
                do
                {
                    statusDetails = CheckStatus(dataflowJobLocation, key);
                    Console.WriteLine("Dataflow Job Status: {0}", statusDetails.jobStatus);
                    if (statusDetails.jobStatus == "Aborted")
                        throw new Exception("Job was aborted due to an error.");
                    Thread.Sleep(30000); //Get status every 30 seconds
                }
                while (statusDetails.jobStatus.Equals("Pending"));

                //When the job is completed, get the results
                //Two files are created to record the results:
                //  Success.xml contains the data that was successfully geocoded
                //  Failed.mxl contains the data that could not be geocoded

                DownloadResults(statusDetails, key);

            }

            catch (Exception e)
            {
                Console.WriteLine("Exception :" + e.Message);
            }

        }

        /*      static void Main(string[] args)
        {
            string dataFilePath = args[0];
            string dataFormat = args[1];
            string key = args[2];
            string description = null;

            try
            {

                if (args.Length > 3)
                    description = args[3];


                string dataflowJobLocation = CreateJob(dataFilePath, dataFormat, key, description);
                Console.WriteLine("Dataflow Job Location: {0}", dataflowJobLocation);

                //Continue to check the dataflow job status until the job has completed
                DownloadDetails statusDetails = new DownloadDetails();
                do
                {
                    statusDetails = CheckStatus(dataflowJobLocation, key);
                    Console.WriteLine("Dataflow Job Status: {0}", statusDetails.jobStatus);
                    if (statusDetails.jobStatus== "Aborted")
                        throw new Exception("Job was aborted due to an error.");
                    Thread.Sleep(30000); //Get status every 30 seconds
                }
                while (statusDetails.jobStatus.Equals("Pending")); 

                //When the job is completed, get the results
                //Two files are created to record the results:
                //  Success.xml contains the data that was successfully geocoded
                //  Failed.mxl contains the data that could not be geocoded

                DownloadResults(statusDetails, key);

            }

            catch (Exception  e)
            {
                Console.WriteLine("Exception :" + e.Message);
            }
         }*/
    }
}