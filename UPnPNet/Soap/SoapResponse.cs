using System.Collections.Generic;

namespace UPnPNet.Soap
{
	public class SoapResponse
	{
		public IDictionary<string, string> Values { get; set; } = new Dictionary<string, string>();
	}
}
