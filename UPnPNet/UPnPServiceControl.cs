using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UPnPNet.Models;
using UPnPNet.Soap;

namespace UPnPNet
{
	public class UPnPServiceControl
	{
		private readonly UPnPService _service;

		public UPnPServiceControl(UPnPService service)
		{
			_service = service;
		}

		public async Task<IDictionary<string, string>> SendAction(string action, IDictionary<string, string> arguments)
		{
			SoapClient client = new SoapClient()
			{
				BaseAddress = new Uri(_service.BaseUrl)
			};

			SoapRequest request = new SoapRequest()
			{
				Action = action,
				ServiceType = _service.Type,
				Arguments = arguments,
				ControlUrl = _service.ControlUrl
			};
			
			return (await client.SendAsync(request)).Values;
		}
	}
}
