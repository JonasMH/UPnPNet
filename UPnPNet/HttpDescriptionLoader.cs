using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace UPnPNet
{
	public class HttpDescriptionLoader : IDescriptionLoader
	{
		public async Task<string> LoadDescription(string url)
		{
			string xml;

			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
			HttpWebResponse response = (HttpWebResponse) await request.GetResponseAsync();
			
			using (Stream stream = response.GetResponseStream())
			using (StreamReader reader = new StreamReader(stream))
			{
				xml = reader.ReadToEnd();
			}
			
			return xml;
		}
	}
}