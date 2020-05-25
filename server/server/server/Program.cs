using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace server
{
    class Program
    {
            static void Main(string[] args)
            {
                int port = 420;
                IPAddress ip = IPAddress.Any;
                IPEndPoint endpoint = new IPEndPoint(ip, port);

                TcpListener listener = new TcpListener(endpoint);
                listener.Start();

                Console.WriteLine("Awaiting Clients");

                TcpClient client = listener.AcceptTcpClient();

                NetworkStream stream = client.GetStream();

            byte[] buffer = new byte[256];
            while (true)
            {
                int numb = stream.Read(buffer, 0, buffer.Length);

                string mes = Encoding.UTF8.GetString(buffer, 0, numb);

                Console.WriteLine(mes);
            }
            }
        
    }
}
