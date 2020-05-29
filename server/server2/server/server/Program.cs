using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            bool sevs = true;
            while (sevs)
            {
                Console.Write("Hvis du vil være server så skriv: server\n");
                Console.Write("Hvis du vil være client så skriv: client\n");
                Console.Write("Hvis du vil lukke programmet så skriv: exit\n");
                string test = Console.ReadLine();
                if (test == "server")
                {
                    Server server = new Server();
                }
                else if (test == "client")
                {
                    Client client = new Client();
                }
                else if (test == "exit")
                {
                    sevs = false;
                }
                else
                {
                    Console.WriteLine("Fjols");
                }
            }
        }
    }
    
    public class Client
    {
        public Client()
        {
            TcpClient client = new TcpClient();
            Console.WriteLine("port");
            int port = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("ipadresse");
            string ipaddress = Console.ReadLine();
            IPAddress ip = IPAddress.Parse(ipaddress);
            IPEndPoint endPoint = new IPEndPoint(ip, port);

            client.Connect(endPoint);
            NetworkStream stream = client.GetStream();
            Console.WriteLine("Write your message");

            while (true)
            {
                sendMessage(stream);
                receiveMessage(stream);
            }
        }
        public async void sendMessage(NetworkStream stream)
        {
            string text = Console.ReadLine();
            byte[] buffer = Encoding.UTF8.GetBytes(text);
            stream.Write(buffer, 0, buffer.Length);

            if (text == "clear" || text == "Clear")
            {
                Console.Clear();
            }
           
            else
            {
                byte[] bytes = Encoding.UTF8.GetBytes(text);
                await stream.WriteAsync(bytes, 0, bytes.Length);
            }
        }
        public async void receiveMessage(NetworkStream stream)
        {
            while (true)
            {
                byte[] buffer = new byte[256];
                int numberBytesread = await stream.ReadAsync(buffer, 0, 256);
                string recieveMessage = Encoding.UTF8.GetString(buffer, 0, numberBytesread);

                Console.WriteLine("\n" + recieveMessage);
            }
        }   
    }
}
