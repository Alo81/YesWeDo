using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SmashUltimateEditor;
using SmashUltimateEditor.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;

namespace YesWeDo.Helpers
{
    class NetworkHelper
    {

        public static async void UpdateCheck()
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpWebRequest options = (HttpWebRequest)WebRequest.Create(Defs.gitReleasesUrl);

                options.Headers["User-Agent"] = "request";

                var response = await options.GetResponseAsync();


                if (((HttpWebResponse)response).StatusCode == HttpStatusCode.OK)
                {
                    var serializer = new JsonSerializer();

                    using (var sr = new StreamReader(response.GetResponseStream()))
                    using (var jsonTextReader = new JsonTextReader(sr))
                    {
                        JArray jsonResult = (JArray)serializer.Deserialize(jsonTextReader);
                        var latestRelease = jsonResult.First;
                        var latestReleaseDate = DateTime.Parse(latestRelease["created_at"].ToString());     // In UTC time, so we should compare at UTC.
                        var currentProgramDate = new FileInfo(System.Reflection.Assembly.GetEntryAssembly().Location).LastWriteTimeUtc;     //Created date actually maps to when it was unzipped.  Modified should be more accurate to when the program was actually made.  

                        if (latestReleaseDate > currentProgramDate)
                        {
                            if (UiHelper.PopUpQuestion("There is a new version available.  Would you like to download the latest release?"))
                            {
                                var latestReleaseUrl = latestRelease["html_url"].ToString();
                                Process.Start(new ProcessStartInfo(latestReleaseUrl) { UseShellExecute = true });
                            }
                        }
                    }
                }
            }
            catch
            {
            }
        }

        public static void DownloadParamLabels(string fileLocation)
        {
            using (var client = new WebClient())
            {
                client.DownloadFile(Defs.paramLabelsGitUrl, fileLocation);
            }
        }
    }
}
