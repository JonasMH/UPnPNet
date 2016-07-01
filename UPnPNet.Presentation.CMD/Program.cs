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

            foreach (UPnPDevice device in result)
            {
                Console.WriteLine(device.Location);

                UPnPDeviceDescription description = device.LoadDescription();
                string keyValue = description.Properties.Where(x => x.Key == "roomName").Select(x => x.Value).FirstOrDefault();

                if(!string.IsNullOrEmpty(keyValue))
                    Console.WriteLine(keyValue);
            }

            /*while (true)
            {
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.A:
                        control.SendAction("Stop", new Dictionary<string, string>() { { "InstanceID", "0" } });
                        break;
                    case ConsoleKey.S:
                        control.SendAction("Play", new Dictionary<string, string>() { { "InstanceID", "0" }, { "Speed", "1" } });
                        break;
                    case ConsoleKey.D:
                        control.SendAction("Pause", new Dictionary<string, string>() { { "InstanceID", "0" } });
                        break;
                }
            }*/
            Console.WriteLine("Press any key");
            Console.ReadKey();
        }
    }
}
