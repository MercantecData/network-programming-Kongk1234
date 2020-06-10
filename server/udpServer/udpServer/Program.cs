using System.Net.Sockets;
using System.Net;
using System.Text;
using System;

namespace udpServer
{
    class Program
    {
        static void Main(string[] args)
        {
            //Kalder min funktion og hvis der bliver connected til den specifikke ip og port skriver den min string text
            recieve();
            UdpClient client = new UdpClient();
            string text = "Er du til fuglekending, selv er et fjog, så spørg Poul Hansen ornitolog";
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5000);
            client.Send(bytes, bytes.Length, endpoint);
            Console.ReadLine();
        }
        //Her modtager den min besked fra tidligere og sender tilbage at den er modtaget
        public static async void recieve()
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5000);
            UdpClient client = new UdpClient(endPoint);
            UdpReceiveResult result = await client.ReceiveAsync(); 
            byte[] buffer = result.Buffer;
            string text = Encoding.UTF8.GetString(buffer);
            Console.WriteLine("Recieved: " + text);
        }
    }
}
