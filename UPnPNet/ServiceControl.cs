using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

namespace UPnPNet
{
    public class ServiceControl
    {
        private UPnPService _service;

        public ServiceControl(UPnPService service)
        {
            _service = service;
        }

        public bool SendAction(UPnPAction action, IDictionary<string, string> arguments)
        {
            string argumentStr = arguments.Aggregate("", (current, c) => current + $"<{c.Key}>{c.Value}</{c.Key}>");

            string body =
                "<?xml version=\"1.0\"?>\r\n" +
                "<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\">\r\n" +
                    "<s:Body>\r\n" +
                        $"<u:{action.Name} xmlns:u=\"{_service.Type}\">\r\n" +
                            argumentStr +
                        $"</u:{action.Name}>\r\n" +
                    "</s:Body>\r\n" +
                "</s:Envelope>\r\n";

            HttpClient client = new HttpClient();

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_service.BaseUrl + _service.ControlUrl);
            request.Method = "POST";

            request.Headers.Add("SOAPACTION", _service.Type + "#" + action.Name);

            byte[] buffer = Encoding.UTF8.GetBytes(body);

            request.ContentType = "text/xml; charset =\"utf-8\"";
            request.ContentLength = buffer.Length;

            Stream dataStream = request.GetRequestStream();
            dataStream.Write(buffer, 0, buffer.Length);
            dataStream.Close();

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return response.StatusCode == HttpStatusCode.OK;
            }
        }
    }
}
