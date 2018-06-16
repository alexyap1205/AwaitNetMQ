using System;
using NetMQ;
using NetMQ.Sockets;

namespace testNetCore2
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var server = new ResponseSocket("@tcp://localhost:5556")) // bind
            {
                Console.WriteLine("SERVER: Waiting for message");
                // Receive the message from the server socket
                string m1 = server.ReceiveFrameString();
                Console.WriteLine("SERVER: From Client: {0}", m1);

                Console.WriteLine("SERVER: Press ENTER to send back");
                Console.ReadLine();
                // Send a response back from the server
                server.SendFrame("Hi Back");
                Console.WriteLine("SERVER: sent back");

                Console.ReadLine();
            }
        }
    }
}
