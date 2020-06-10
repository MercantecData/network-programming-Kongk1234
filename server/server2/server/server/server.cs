using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    //Laver en class med en tcp client og en string og includer dem i min clientconnect
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

    //Laver min server og laver en liste til clients
    class Server
    {
        List<ClientConnect> clients = new List<ClientConnect>();
        //giver en port og definere et brugerinput til ip adresse og gør så den acceptere klienter 
        public Server()
        {
            int port = 420;
            IPAddress ip = IPAddress.Any;
            IPEndPoint localendpoint = new IPEndPoint(ip, port);

            TcpListener listner = new TcpListener(localendpoint);

            listner.Start();
            acceptClient(listner);
            Console.WriteLine("Started");
            //Laver et while loop der gør man kan skrive til hinanden 
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

        //Her accepetere jeg klienter og tager den første besked de skriver 
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

        //her modtager jeg beskeder og laver commands hvis man skriver noget specefikt
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
                    if (aclient.client == client)
                    {
                        Name = aclient.name;
                    }

                }
                if (receivedMessage == "måge")
                {
                    foreach (ClientConnect client2 in clients)
                    {
                        string Måge = "Måger (Larinae) er en underfamilie af mågefuglene. \n De er udbredt i alle verdensdele og er store eller mellemstore fugle, der er knyttet til vandet. \n Danmark er med sine mange lavvandede strande et godt sted for måger. \n Fødderne er forsynet med svømmehud, vingerne er lange og slanke, og flugten er let og ubesværet med rolige vingeslag. \n Fjerdragten er ret ensartet farvet, hyppigst gråblå på oversiden og hvid på undersiden. \n Mange måger yngler i kolonier og færdes også udenfor yngletiden i flokke. \n Føden er meget variabel, men består oftest af småfisk eller krebsdyr.";
                        byte[] buffer1 = Encoding.UTF8.GetBytes(Måge);
                        client2.client.GetStream().Write(buffer1, 0, buffer1.Length);
                    }

                }
                else if (receivedMessage.Contains("fugl") || receivedMessage.Contains("fugle") || receivedMessage.Contains("Fugl") || receivedMessage.Contains("Fugle"))
                {
                    foreach (ClientConnect client2 in clients)
                    {
                        string fugl = "Hvis du til fuglekending, selv er et fjog, så spørg Poul Hansen ornitolog";
                        byte[] buffer1 = Encoding.UTF8.GetBytes(fugl);
                        client2.client.GetStream().Write(buffer1, 0, buffer1.Length);
                    }

                }
                //Hvis man skriver change i chatten, har man mulighed for at ændre sit brugernavn
                else if (receivedMessage.Contains("change"))
                {
                    string[] newName = receivedMessage.Split(":");
                    foreach (ClientConnect client2 in clients)
                    {
                        if (client2.name == Name)
                        {
                            client2.name = newName[1];
                        }
                    }
                }
                else
                {
                    //Når man skriver en besked tager man den første besked og sætter den sammen med de resterende beskeder 
                    foreach (ClientConnect clienten in clients)
                    {
                        string Mes = Name + ":" + receivedMessage;
                        byte[] buffer1 = Encoding.UTF8.GetBytes(Mes);
                        clienten.client.GetStream().Write(buffer1, 0, buffer1.Length);
                    }
                }
            }
        }

    }
}
