using SQ.Common.Library.Handlers;
using SQ.Common.Library.Helpers;
using System.Net.Sockets;
using System.Text;

namespace ConsoleApp1
{
    internal class Program
    {

        static Dictionary<string, Socket> MemberSockets;
        static async Task Main(string[] args)
        {
            NetworkHelper networkHelper = new NetworkHelper();
            var server = networkHelper.CreateIpEndPoint().CreateSocket();
            networkHelper.ServerStart();
            Console.WriteLine("listening for connection");
            MemberSockets = new Dictionary<string, Socket>();
            Task  task1 = GetConnectionAsync(server);
            Task task2 = SendMsgAsync();


            //error in it
            /*server.Shutdown(SocketShutdown.Both);*/
          
            while (!task1.IsCompleted && !task2.IsCompleted) { }
            server.Close();
        }

        static async Task GetConnectionAsync(Socket server)
        {
            do
            {
                Socket client = await server.AcceptAsync();
                
                    Console.WriteLine("Connection Established Successfully: count-" + MemberSockets.Count);
                    try
                    {
                            byte[] bytes = new byte[256];
                            int numByte = await client.ReceiveAsync(bytes);

                            var recvMsg = Encoding.ASCII.GetString(bytes, 0, numByte);

                            var msgBytes = Encoding.UTF8.GetBytes("Got your msg");
                            await client.SendAsync(msgBytes, SocketFlags.None);
                            MemberSockets.Add(recvMsg, client);
                    }

                    catch (Exception ex)
                    {
                        Console.WriteLine("error: " + ex.Message);
                    }
                    
                    // client.Shutdown(SocketShutdown.Both);
                



            } while (true);
        }

        static async Task SendMsgAsync()
        {
            while (true)
            {

                await Task.Delay(1000);
                foreach (var member in MemberSockets)
            {
                    if (!member.Value.Connected)
                    {
                        Console.WriteLine("member disconnected" + member.Key);
                        continue;
                    }
                    Console.WriteLine("member connected" + member.Key);
                var msgBytes = Encoding.UTF8.GetBytes("i got your back");
                await member.Value.SendAsync(msgBytes, SocketFlags.None);
            }
            }
        }
    }
}
