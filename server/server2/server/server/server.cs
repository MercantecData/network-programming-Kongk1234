using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Server
{

    public class ClientConnect
    {
        public TcpClient client;
        public string name;
        public ClientConnect(TcpClient client, string name)
        {
            this.client = client;
            this.name = name;
        }
    }

    class Server
    {
        List<ClientConnect> clients = new List<ClientConnect>();
        public Server()
        {
            int port = 420;
            IPAddress ip = IPAddress.Any;
            IPEndPoint localendpoint = new IPEndPoint(ip, port);

            TcpListener listner = new TcpListener(localendpoint);

            listner.Start();
            acceptClient(listner);
            Console.WriteLine("Started");
            while (true)
            {
                string text = Console.ReadLine();
                byte[] buffer = Encoding.UTF8.GetBytes(text);
                foreach (ClientConnect client in clients)
                {
                    client.client.GetStream().Write(buffer, 0, buffer.Length);
                }
            }
        }

        public async void acceptClient(TcpListener listener)
        {
            bool isRunning = true;
            byte[] bytes = new byte[256];
            while (isRunning)
            {
                TcpClient client = await listener.AcceptTcpClientAsync();
                NetworkStream stream = client.GetStream();
                int numb = stream.Read(bytes, 0, bytes.Length);
                string name = Encoding.UTF8.GetString(bytes, 0, numb);
                clients.Add(new ClientConnect(client, name));
                Console.WriteLine("Welcome: " + name);
                recieveMessage(client);

            }

        }

        public async void recieveMessage(TcpClient client)
        {
            byte[] buffer = new byte[256];
            while (true)
            {
                NetworkStream stream = client.GetStream();
                int numberOfBytesRead = await stream.ReadAsync(buffer, 0, 256);
                string receivedMessage = Encoding.UTF8.GetString(buffer, 0, numberOfBytesRead);
                string Name = "";
                Console.WriteLine("\n" + receivedMessage);
                foreach (ClientConnect aclient in clients)
                {
                   if(aclient.client == client)
                   {
                        Name = aclient.name;
                   }
                   
                }
                foreach(ClientConnect clienten in clients)
                {
                    string Mes = Name + ":" + receivedMessage;
                    byte[] buffer1 = Encoding.UTF8.GetBytes(Mes);
                    clienten.client.GetStream().Write(buffer1, 0, buffer1.Length);
                }
            }
        }

    }
}
