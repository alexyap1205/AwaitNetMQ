using System;
using System.Threading.Tasks;
using NetMQ;
using NetMQ.Sockets;
using Newtonsoft.Json;
using Serilog;
using Serilog.Context;
using Serilog.Events;
using Serilog.Formatting.Json;

namespace testNetCore
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //TestSerilog();

            using (var client = new RequestSocket(">tcp://localhost:5556")) // connect
            {
                // Send a message from the client socket
                client.SendFrame("Hello");
                Console.WriteLine("CLIENT: Sent to Server");

                Console.WriteLine("CLIENT: Press enter to receive from server");
                // Receive the response from the client socket

                var task = WaitForResponse(client);

                Console.WriteLine("CLIENT: Press enter to exit");
                Console.ReadLine();

                await task;
            }

            
        }

        private static async Task WaitForResponse(RequestSocket client)
        {
            await Task.Run(() =>
            {
                string m2 = client.ReceiveFrameString();
                Console.WriteLine("CLIENT: From Server: {0}", m2);
            });
        }

        private static void TestSerilog()
        {
            var log = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties}{NewLine}")
                .CreateLogger();

            JobContext context = new JobContext()
            {
                Id = 5
            };

            using (LogContext.PushProperty("@Job", context))
            {
                log.Information("Test");
            }

            ////Console.WriteLine("Hello World!");
            Console.ReadLine();
        }
    }

    class JobContext
    {
        public int Id { get; set; }
    }
}
