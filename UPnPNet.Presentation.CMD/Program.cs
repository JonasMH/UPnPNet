using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UPnPNet;
using UPnPNet.Discovery;

namespace Presentation.CMD
{
    class Program
    {
        static void Main(string[] args)
        {
            UPnPDiscovery upnPDiscovery = new UPnPDiscovery {SearchTarget = new DiscoverySearchTargetRootDevices()};


            Task<IList<UPnPDevice>> task = upnPDiscovery.SearchAsync();

            IList<UPnPDevice> result = task.Result;

            UPnPDevice upnPDevice = result.First();

            Console.WriteLine($"Found devices: " + result.Count);

            Console.WriteLine(upnPDevice.UniqueServiceName);
            Console.WriteLine(upnPDevice.UUID);
            Console.WriteLine(upnPDevice.Location);
            Console.WriteLine("Targets:");

            upnPDevice.LoadDescription();

            UPnPService avtransportService = upnPDevice.Services.First(x => x.Type.Contains("AVTransport"));

            avtransportService.LoadDescription();


            UPnPAction playAction = avtransportService.Actions.First(x => x.Name == "Play");
            UPnPAction stopAction = avtransportService.Actions.First(x => x.Name == "Stop");
            UPnPAction pauseAction = avtransportService.Actions.First(x => x.Name == "Pause");

            ServiceControl control = new ServiceControl(avtransportService);

            while (true)
            {
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.A:
                        control.SendAction(stopAction, new Dictionary<string, string>() { { "InstanceID", "0" } });
                        break;
                    case ConsoleKey.S:
                        control.SendAction(playAction, new Dictionary<string, string>() { { "InstanceID", "0" }, { "Speed", "1" } });
                        break;
                    case ConsoleKey.D:
                        control.SendAction(pauseAction, new Dictionary<string, string>() { { "InstanceID", "0" } });
                        break;
                }
            }
        }
    }
}
