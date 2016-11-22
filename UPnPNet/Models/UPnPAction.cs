using System.Collections.Generic;

namespace UPnPNet.Models
{
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
