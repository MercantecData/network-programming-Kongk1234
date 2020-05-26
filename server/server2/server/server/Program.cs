using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace server
{
    class Program
    {
        static void Main()
        {
            server test = new server();
            Console.WriteLine(test);
        }
        public class server{
            public server()
            {
                int port = 420;
                IPAddress ip = IPAddress.Any;
                IPEndPoint localendpoint = new IPEndPoint(ip, port);

                TcpListener listner = new TcpListener(localendpoint);

                listner.Start();

                Console.WriteLine("awaiting clients");
                TcpClient client = listner.AcceptTcpClient();

                NetworkStream stream = client.GetStream();
                recieveMessage(stream);
                Console.Write("write your message here");
                string text = Console.ReadLine();
                byte[] buffer = Encoding.UTF8.GetBytes(text);
                stream.Write(buffer, 0, buffer.Length);
                Console.ReadKey();
            }
            public async void recieveMessage(NetworkStream stream)
            {
                while (true)
                {
                    byte[] buffer = new byte[256];
                    int numberOfBytesRead = await stream.ReadAsync(buffer, 0, 256);
                    string receivedMessage = Encoding.UTF8.GetString(buffer, 0, numberOfBytesRead);
                    Console.Write("\n" + receivedMessage);
                }
            }

        }


    }
}
