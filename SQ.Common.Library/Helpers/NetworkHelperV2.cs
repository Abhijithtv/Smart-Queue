using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SQ.Common.Library.Helpers
{
    public class SocketWrapper 
    {
        public required Socket Socket {  get; set; }
        public IPEndPoint EndPoint { get; set; }

        public List<Socket> Clients { get; set; }

        public Task AsyncAccept { get; set; }

        public bool Abort { get; set; }

    }

    public class NetworkHelperV2
    {
        public static int max_clients = 20;

        public static int max_connection = 1000;

        //public static Queue<int> FreePorts = new Queue<int>();
        //we assign port = 0 . tcp stack will add a port
        //public static Queue<int> LockedPorts =new Queue<int>();

        public static Queue<Socket> freeSocktets = new Queue<Socket>();//maybe dont need it

        public static Dictionary<string, SocketWrapper> SocketServers = new Dictionary<string, SocketWrapper>();

        public static void CreateServerSocket(string groupId)
        {
            if (SocketServers.Count == max_connection)
                throw new Exception("max-servers");

            var serverSocket = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
            var iPEndPoint = new IPEndPoint(GetLocalIp(), 0);

            SocketServers.Add(groupId, new SocketWrapper { Socket = serverSocket, EndPoint = iPEndPoint, Clients = new() ,Abort = false});
        }

        public static void GroupServerStart(string groupId)
        {
            SocketWrapper socketWrapper;
            
            if(!SocketServers.TryGetValue(groupId, out socketWrapper))
            {
                throw new Exception("no socket allocated for groupId");
            }

            if (socketWrapper != null)
            {
                socketWrapper.Socket.Bind(socketWrapper.EndPoint);
                socketWrapper.Socket.Listen(10);
                Console.WriteLine($"Server started at ip ={((IPEndPoint)socketWrapper.Socket.LocalEndPoint).Address} and port={((IPEndPoint)socketWrapper.Socket.LocalEndPoint).Port}");
                socketWrapper.AsyncAccept = GetConnectionAsync(groupId);
            }
        }

        static async Task GetConnectionAsync(string groupId)//todo-exit grp 
        {
            do
            {
                //check if key value exist 
                SocketWrapper socketWrapper;
                if (!SocketServers.TryGetValue(groupId, out socketWrapper))
                {
                    return;
                }

                if (socketWrapper.Abort)//ists not working properly
                {
                    return; //cancel this sockets server from listening to connection.
                }

                if (socketWrapper.Clients.Count == max_clients)
                {
                    return;
                }

                Socket client = await socketWrapper.Socket.AcceptAsync();

                try
                {

                    if (await TryAddClientAsync(socketWrapper.Clients.Count, client))
                    {
                        socketWrapper.Clients.Add(client);
                        SocketServers[groupId] = socketWrapper;
                    }
                    else
                    {
                        client.Dispose();
                    }
                }

                catch (Exception ex)
                {
                    Console.WriteLine("error: " + ex.Message);
                }

            } while (true);
        }

        public static async Task<bool> TryAddClientAsync(int clientCount, Socket client)
        {
            byte[] bytes = new byte[256];
            int numByte = await client.ReceiveAsync(bytes);
            var recvMsg = Encoding.ASCII.GetString(bytes, 0, numByte); //can validate the msg.

            string msg = (clientCount >= max_clients) ? "Max-client-reached" : "Successs";
            var msgBytes = Encoding.UTF8.GetBytes(msg);
            await client.SendAsync(msgBytes, SocketFlags.None);

            return (clientCount < max_clients);
        }

        private static IPAddress GetLocalIp()
        {
            IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
            return ipHost.AddressList[0];
        }

        public static void PreLoader()
        {
            
        }

        public static int TryGetGroupServerPort(string groupId)
        {
            SocketWrapper socketWrapper ;
            if(!SocketServers.TryGetValue(groupId, out socketWrapper)) { throw new Exception("Invalid-Group"); }
            return ((IPEndPoint)socketWrapper.Socket.LocalEndPoint).Port;    
        }

        public static async Task<bool> GroupServerStop(string groupId) //todo: free the thread and pass it to thread pool.
        {
            try
            {
                SocketWrapper socketWrapper;
                if (!SocketServers.TryGetValue(groupId, out socketWrapper)) { throw new Exception("Invalid-Group"); }

                socketWrapper.Abort = true;
                SocketServers[groupId] = socketWrapper;
                
                await Task.Delay(100); 

                foreach (var client in socketWrapper.Clients)
                {
                    client.Shutdown(SocketShutdown.Both);
                    client.Dispose();
                    //client.Close();
                }

                if (socketWrapper.Socket.Connected)
                {
                    socketWrapper.Socket.Shutdown(SocketShutdown.Both);
                }
                socketWrapper.Socket.Dispose();
                socketWrapper.Socket.Close();

                SocketServers.Remove(groupId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }

            return true;
        }


    }
}
