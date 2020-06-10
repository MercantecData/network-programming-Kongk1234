using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class Client
{
    //laver det muligt for klienten at skrive en port og ipadresse
    public Client()
    {
        TcpClient client = new TcpClient();

        Console.WriteLine("Port:");
        int port = Convert.ToInt32(Console.ReadLine());

        Console.WriteLine("Ip:");
        string ipAddress = Console.ReadLine();
        IPAddress ip = IPAddress.Parse(ipAddress);
        IPEndPoint endpoint = new IPEndPoint(ip, port);

        client.Connect(endpoint);
        //her gør jeg så de kan skrive beskeder
        NetworkStream stream = client.GetStream();
        RecieveMessage(stream);
        while (true)
        {
            WriteMessage(stream);
        }
            
    }
    //Her kan klienten skrive beskeder
    public async void WriteMessage(NetworkStream stream)
    {
        string text = Console.ReadLine();
        byte[] bytes = Encoding.UTF8.GetBytes(text);
        await stream.WriteAsync(bytes, 0, bytes.Length);
    }
    //Her kan klienten modtager beskeder
    public async void RecieveMessage(NetworkStream stream)
    {
         byte[] buffer = new byte[1024];

         while(true){
            int numb = await stream.ReadAsync(buffer, 0, buffer.Length);
            string mes = Encoding.UTF8.GetString(buffer, 0, numb);
            Console.WriteLine("\n" + mes);
         }
    }
}