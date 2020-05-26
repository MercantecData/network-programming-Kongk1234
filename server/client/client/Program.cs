using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace client
{
    class Program
    {
        static void main()
        {
            Client client = new Client();
            Console.WriteLine(client);
        }
    }

    public class Client
    {
        public Client()
        {
            TcpClient client = new TcpClient();

            int port = 420;
            string ipaddress = Console.ReadLine();
            IPAddress ip = IPAddress.Parse(ipaddress);
            IPEndPoint endPoint = new IPEndPoint(ip, port);

            client.Connect(endPoint);
            NetworkStream stream = client.GetStream();
        }
        public async void receiveMessage(NetworkStream stream)
        {
            byte[] buffer = new byte[256];
            int numberBytesread = await stream.ReadAsync(buffer, 0, 256);
            string recieveMessage = Encoding.UTF8.GetString(buffer, 0, numberBytesread);

            Console.Write("\n" + recieveMessage);
        }
    }
}
