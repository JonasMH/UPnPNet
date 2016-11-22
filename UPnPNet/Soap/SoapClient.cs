using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace UPnPNet.Soap
{
	public interface IHttpHandler
	{
		Task<HttpResponseMessage> SendAsync(HttpRequestMessage message);
	}

	public class HttpHandler : IHttpHandler
	{
		public Task<HttpResponseMessage> SendAsync(HttpRequestMessage message)
		{
			HttpClientHandler handler = new HttpClientHandler
			{
				AutomaticDecompression = System.Net.DecompressionMethods.None
			};
			HttpClient client = new HttpClient(handler);
			client.DefaultRequestHeaders.Clear();

			return client.SendAsync(message);
		}
	}

	public class SoapClient
	{
		public Uri BaseAddress { get; set; }
		public IHttpHandler HttpHandler { get; set; } = new HttpHandler();
		public ISoapResponseParser SoapResponseParser { get; set; } = new SoapResponseParser();

		public async Task<SoapResponse> SendAsync(SoapRequest request)
		{
			HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, new Uri(BaseAddress, request.ControlUrl));

			message.Headers.Add("SOAPACTION", request.ServiceType + "#" + request.Action);
			message.Content = new StringContent(request.GetBody(), Encoding.UTF8, "text/xml");

			HttpResponseMessage responseMsg = await HttpHandler.SendAsync(message);

			byte[] response = await responseMsg.Content.ReadAsByteArrayAsync();

			return SoapResponseParser.ParseResponse(Encoding.UTF8.GetString(response));
		}
	}
}