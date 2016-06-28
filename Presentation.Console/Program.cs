using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Presentation.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            int port = 1900;
            string host = "239.255.255.250";

            IPAddress multicastAddress = IPAddress.Parse(host);

            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, 2000);
            IPEndPoint multicastEndPoint = new IPEndPoint(multicastAddress, port);
            
            //Prepare receive socket
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            socket.Bind(localEndPoint);
            socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, new MulticastOption(multicastAddress, IPAddress.Any));
            socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastTimeToLive, 2);
            socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastLoopback, true);
            
            string request = "M-SEARCH * HTTP/1.1\r\nHOST:239.255.255.250:1900\r\nMAN:\"ssdp: discover\"\r\nST:ssdp:all\r\nMX:3\r\n\r\n";

            socket.SendTo(Encoding.ASCII.GetBytes(request), SocketFlags.None, multicastEndPoint);

            System.Console.WriteLine("Request sent");

            byte[] buffer = new byte[10000];
            while (true)
            {
                if (socket.Available > 0)
                {
                    int receivedBytes = socket.Receive(buffer, SocketFlags.None);

                    if (receivedBytes > 0)
                    {
                        System.Console.WriteLine(Encoding.UTF8.GetString(buffer, 0, receivedBytes));
                    }
                }
            }
        }
    }
}
