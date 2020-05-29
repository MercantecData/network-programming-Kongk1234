using System;
using System.Collections.Generic;
using System.Text;

namespace client
{
    public class Server
    {
        List<server> clients = new List<server>();
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
                    foreach (TcpClient client1 in clients)
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
}
