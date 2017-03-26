using Microsoft.AspNetCore.Mvc;
using UPnPNet.TestServer.Repositories;

namespace UPnPNet.TestServer.Controllers
{
	[Route("api/[controller]")]
	public class DevicesController : Controller
	{
		private readonly DeviceRepository _deviceRepository;

		public DevicesController(DeviceRepository deviceRepository)
		{
			_deviceRepository = deviceRepository;
		}

		[HttpGet]
		public IActionResult Get()
		{
			return Ok(_deviceRepository.Devices);
		}
	}
}
