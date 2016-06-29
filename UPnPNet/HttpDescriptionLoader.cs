using System.IO;
using System.Net;
using System.Net.Http;

namespace UPnPNet
{
    public class HttpDescriptionLoader : IDescriptionLoader
    {
        public string LoadDescription(string url)
        {
            HttpClient client = new HttpClient();
            
            string xml = string.Empty;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                xml = reader.ReadToEnd();
            }

            return xml;
        }
    }
}