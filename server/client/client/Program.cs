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
                    server server = new server();
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

    public class server
    {
        List<TcpClient> clients = new List<TcpClient>();
        public server()
        {
            int port = 420;
            IPAddress ip = IPAddress.Any;
            IPEndPoint localendpoint = new IPEndPoint(ip, port);

            TcpListener listner = new TcpListener(localendpoint);

            listner.Start();
            acceptClient(listner);
            Console.WriteLine("Mågegeden knepper bæltedyret");
            while (true)
            {
                string text = Console.ReadLine();
                byte[] buffer = Encoding.UTF8.GetBytes(text);
                foreach (TcpClient client in clients)
                {
                    client.GetStream().Write(buffer, 0, buffer.Length);
                }
            }
        }

        public async void recieveMessage(NetworkStream stream)
        {
            while (true)
            {
                byte[] buffer = new byte[256];
                int numberOfBytesRead = await stream.ReadAsync(buffer, 0, 256);
                string receivedMessage = Encoding.UTF8.GetString(buffer, 0, numberOfBytesRead);
                Console.WriteLine("\n" + receivedMessage);
                foreach (TcpClient client in clients)
                {
                    string text = receivedMessage;
                    byte[] buffer1 = Encoding.UTF8.GetBytes(text);
                    foreach(TcpClient client1 in clients)
                    {
                        client.GetStream().Write(buffer1, 0, buffer1.Length);
                    }
                }
            }
        }
        public async void acceptClient(TcpListener listener)
        {
            bool isRunning = true;
            while (isRunning)
            {
                TcpClient client = await listener.AcceptTcpClientAsync();
                clients.Add(client);
                NetworkStream stream = client.GetStream();
                int numb =
                recieveMessage(stream);
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
