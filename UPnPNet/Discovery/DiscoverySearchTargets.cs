using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UPnPNet.Discovery
{
	public class DiscoverySearchTargets
	{
		public static IDiscoverySearchTarget All { get; } = new DiscoverySearchTargetAll();
	}
}
