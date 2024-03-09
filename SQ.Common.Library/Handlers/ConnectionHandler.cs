using SQ.Common.Library.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SQ.Common.Library.Handlers
{
    public class Connection
    {
        private  Socket producerSocket;
        private  static Connection _connection;
        private  NetworkHelper _networkHelper;
        private Connection()
        {
            _networkHelper = new NetworkHelper();
            producerSocket = _networkHelper.CreateIpEndPoint().CreateSocket();
            
        }

        public static Connection GetInstance()
        {
            if(_connection == null)
            {
                _connection = new Connection();
            }
            return _connection;
        }

        public Connection Connect()
        {
            if (!producerSocket.Connected)
            {
                producerSocket.Connect(_networkHelper.iPEndPoint);
            }
            return this;
        }

        public void CloseConnection()
        {
            producerSocket.Disconnect(false);
            producerSocket.Shutdown(SocketShutdown.Both);
            producerSocket.Close();
        }
    }
    public  class ConnectionHandler
    {
        public static void GetConnnection() 
        {
            var connection = Connection.GetInstance().Connect();
        }

        public static void CloseConnnection()
        {
            Connection.GetInstance().CloseConnection();
        }



        public void lot()
        {
            NetworkHelper networkHelper = new NetworkHelper();
            var client = networkHelper.CreateIpEndPoint().CreateSocket();
            client.Connect(networkHelper.iPEndPoint);

            while (!client.Connected) { }
            Console.WriteLine("Connected to Server");

            while (true)
            {
                Console.WriteLine("Enter the msg to send");
                var msg = Console.ReadLine();
                if (msg == null) { msg = "Empty msg"; }
                if (msg.Equals("exit")) { break; }

                var msgBytes = Encoding.UTF8.GetBytes(msg);
                client.Send(msgBytes, SocketFlags.None);

                byte[] buffer = new byte[256];
                var serverMsgSize = client.Receive(buffer, SocketFlags.None);
                var serverMsg = Encoding.UTF8.GetString(buffer, 0, serverMsgSize);
                Console.WriteLine("Resp from server : " + serverMsg);
            }
            client.Disconnect(false);
            client.Shutdown(SocketShutdown.Both);
            client.Close();
        }

    }
}
