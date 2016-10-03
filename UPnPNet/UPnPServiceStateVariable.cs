using System.Collections.Generic;

namespace UPnPNet
{
	public class UPnPServiceStateVariable
	{
		public string Name { get; set; }
		public string DataType { get; set; }
		public bool SendEvent { get; set; }
		public IList<string> AllowedValues { get; set; } = new List<string>();
	}
}
