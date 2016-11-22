using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Xml.Linq;

namespace UPnPNet.Gena
{
	public class GenaNotifyResponse
	{
		public string NTS { get; set; }
		public string SubscriptionId { get; set; }
		public int SequencyNumber { get; set; }
		public IDictionary<string, string> Values { get; set; } = new Dictionary<string, string>();
	}

	public interface IGenaNotifyParser
	{
		GenaNotifyResponse Parse(IDictionary<string, string> headers, string body);
	}

	public class GenaNotifyParser : IGenaNotifyParser
	{
		public GenaNotifyResponse Parse(IDictionary<string, string> headers, string body)
		{
			headers = new Dictionary<string, string>(headers, StringComparer.OrdinalIgnoreCase);

			GenaNotifyResponse response = new GenaNotifyResponse
			{
				NTS = headers["NTS"],
				SubscriptionId = headers["SID"],
				SequencyNumber = int.Parse(headers["SEQ"]),
				Values = ParseBody(body)
			};
			
			return response;
		}

		private IDictionary<string, string> ParseBody(string body)
		{
			XDocument document = XDocument.Parse(body);

			return document.Root.Elements()
					.Where(x => x.Name.LocalName == "property")
					.SelectMany(x => x.Elements())
					.ToDictionary(x => x.Name.LocalName, x => x.Value);
		}
	}
}
