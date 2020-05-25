using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace client
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpClient client = new TcpClient();

            Console.WriteLine("Port:");
            int port = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Ip:");
            string ipaddress = Console.ReadLine();
            IPAddress ip = IPAddress.Parse(ipaddress);
            IPEndPoint endpoint = new IPEndPoint(ip, port);

            client.Connect(endpoint);
            while (true)
            {

                NetworkStream stream = client.GetStream();

                Console.WriteLine("Skriv din besked:");
                string text = Console.ReadLine();


                byte[] bytes = Encoding.UTF8.GetBytes(text);

                stream.Write(bytes, 0, bytes.Length);
            }

            client.Close();
        }
    }
}
