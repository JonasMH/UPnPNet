using System.Collections.Generic;

namespace UPnPNet
{
	public struct ActionArgument
	{
		public enum ArgumentDirection
		{
			In,
			Out
		}

		public string Name { get; set; }
		public ArgumentDirection Direction { get; set; }
		public string RelatedStateVariable { get; set; }
	}

	public class UPnPAction
	{
		public string Name { get; set; }
		public IList<ActionArgument> Arguments { get; set; } = new List<ActionArgument>();

		public override string ToString()
		{
			return Name;
		}
	}
}
