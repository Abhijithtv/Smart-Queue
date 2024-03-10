
using SQ.Common.Library.Handlers;
using SQ.Common.Library.Helpers;
using SQ.Service.API.GroupService;
using SQ.Service.API.Interfaces;
using SQ.Service.API.MessageService;
using System.Net.Sockets;
using System.Text;

namespace SQ.WebApi
{
    public class Program
    {
        public static int Count = 0;
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<IGenerateGroupService, GroupGenerateService>();
            builder.Services.AddScoped<IPublishService,PublishService>();  
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            ThreadHandler.SocketPoolInit();

            app.Run();
        }

        static void ConnectAsync()
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
