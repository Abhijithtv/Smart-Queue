using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SQ.Common.Library.Helpers
{
    public class NetworkHelper
    {
        private  int Port { get; set; }
        public IPAddress IP_Adress { get; set; }
        public IPEndPoint iPEndPoint { get; set; }
        public Socket Socket { get; set; }

        public NetworkHelper()
        {
            IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
            IP_Adress = ipHost.AddressList[0];
            Port = 1111;
        }
        public  NetworkHelper CreateIpEndPoint()
        {
            iPEndPoint = new IPEndPoint(IP_Adress,Port );
            return this;
        }

        public Socket CreateSocket()
        {
            if (iPEndPoint == null) throw new Exception("create an endpoint first");
            Socket = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
            return Socket;
        }

        public void ServerStart()
        {
            if (iPEndPoint == null) throw new Exception("create an endpoint first");
            if (Socket == null) throw new Exception("create a socekt first");
            Socket.Bind(iPEndPoint);
            Socket.Listen(10);
            Console.WriteLine($"Server started at ip ={IP_Adress} and port={Port}");
        }
    }
}
