using System.Collections.Generic;
using System.Linq;

namespace UPnPNet.Services
{
	public class UPnPLastChangeValue
	{
		public string Key { get; set; }
		public IDictionary<string, string> Attributes { get; set; } = new Dictionary<string, string>();

		public override string ToString()
		{
			return Key;
		}
	}

	public class UPnPLastChangeEvent
	{
		public UPnPLastChangeEvent(IList<UPnPLastChangeValue> values)
		{
			Values = values;
		}

		protected string TryGetValue(string key)
		{
			UPnPLastChangeValue value = Values.FirstOrDefault(x => x.Key == key);

			return value?.Attributes["val"];
		}

		protected int? TryGetIntValue(string key)
		{
			int value;

			if (int.TryParse(TryGetValue(key), out value))
			{
				return value;
			}

			return null;
		}

		protected bool? TryGetBoolValue(string key)
		{
			switch (TryGetValue(key))
			{
				case "0":
					return false;
				case "1":
					return true;
				default:
					return null;
			}
		}

		public IList<UPnPLastChangeValue> Values { get; protected set; }
	}

	public class UPnPServiceEvent
	{
		public UPnPServiceEvent(IDictionary<string, string> values)
		{
			Values = values;
		}

		public IDictionary<string, string> Values { get; protected set; }
	}
}