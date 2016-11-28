using System;
using System.Collections.Generic;
using System.Linq;
using UPnPNet.Models;

namespace UPnPNet.Services
{
	public abstract class UPnPLastChangeServiceControl<T> : UPnPServiceControl
	{
		public event EventHandler<T> OnLastChangeEvent;
		public T LastChangeEvent { get; private set; }
		public UPnPLastChangeEventParser EventParser { get; set; } = new UPnPLastChangeEventParser();

		protected UPnPLastChangeServiceControl(UPnPService service, Func<IList<UPnPLastChangeValue>, T> eventCreator) : base(service)
		{
			OnNewEvent += (sender, args) =>
			{
				LastChangeEvent = eventCreator(EventParser.Parse(args.Values.FirstOrDefault(x => x.Key == "LastChange").Value));
				OnLastChangeEvent?.Invoke(this, LastChangeEvent);
			};
		}
	}
}
