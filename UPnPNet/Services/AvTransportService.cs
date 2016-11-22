using System.Collections.Generic;
using System.Threading.Tasks;
using UPnPNet.Models;

namespace UPnPNet.Services
{
	public class AvTransportService : UPnPServiceControl
	{
		public enum PlayMode
		{
			Normal,
			RepeatAll,
			RepeatOne,
			ShuffleNoRepeat,
			Shuffle,
			ShuffleRepeatOne
		}

		private IReadOnlyDictionary<PlayMode, string> playModeMap = new Dictionary<PlayMode, string>
		{
			{PlayMode.Normal, "NORMAL"},
			{PlayMode.RepeatAll, "REPEAT_ALL" },
			{PlayMode.RepeatOne, "REPEAT_ONE" },
			{PlayMode.ShuffleNoRepeat, "SHUFFLE_NOREPEAT"},
			{PlayMode.Shuffle, "SHUFFLE" },
			{PlayMode.ShuffleRepeatOne, "SHUFFLE_REPEAT_ONE" }
		};

		public AvTransportService(UPnPService service) : base(service)
		{

		}

		public Task Stop(int instanceId)
		{
			return SendAction("Stop", new Dictionary<string, string> { { "InstanceId", instanceId.ToString() } });
		}

		public Task Play(int instanceId, int speed)
		{
			return SendAction("Play", new Dictionary<string, string>
			{
				{ "InstanceId", instanceId.ToString()},
				{ "Speed", speed.ToString() }
			});
		}

		public Task Pause(int instanceId)
		{
			return SendAction("Pause", new Dictionary<string, string> { { "InstanceId", instanceId.ToString() } });
		}

		public Task Next(int instanceId)
		{
			return SendAction("Next", new Dictionary<string, string> { { "InstanceId", instanceId.ToString() } });
		}

		public Task Previous(int instanceId)
		{
			return SendAction("Next", new Dictionary<string, string> { { "InstanceId", instanceId.ToString() } });
		}

		public Task SetPlayMode(int instanceId, PlayMode playMode)
		{
			return SendAction("Next", new Dictionary<string, string>
			{
				{ "InstanceId", instanceId.ToString() },
				{ "NewPlayMode", playModeMap[playMode] }
			});
		}
	}
}
