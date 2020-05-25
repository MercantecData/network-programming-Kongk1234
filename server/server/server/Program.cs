using System;
using System.Net;
using System.Text;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Threading;

namespace TestServer
{
    class Program
    {

        public static void server()
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

                serverMsg = Encoding.UTF8.GetString(buffer, 0, numb);

            }
        }

        public static string serverMsg;
        static void Main(string[] args)
        {
            Task task = Task.Run((Action)server);



            TcpClient client = new TcpClient();

            Console.WriteLine("Port:");
            int port = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Ip:");
            string ipaddress = Console.ReadLine();
            IPAddress ip = IPAddress.Parse(ipaddress);
            IPEndPoint endpoint = new IPEndPoint(ip, port);

            client.Connect(endpoint);
            int i = 0;
            while (true)
            {
                serverMsg = serverMsg;

                if (i == 3)
                {
                    NetworkStream stream = client.GetStream();

                    Console.WriteLine("Skriv din besked:");
                    string text = Console.ReadLine();
                    byte[] bytes = Encoding.UTF8.GetBytes(text);

                    stream.Write(bytes, 0, bytes.Length);
                    Thread.Sleep(1000);
                    Console.WriteLine(serverMsg);
                    i = 0;
                }
                else
                {
                    i++;
                }

            }


        }


    }
}
