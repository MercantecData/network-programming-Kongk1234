using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

public class Server{
    public Server(){
        //laver en specifik port og ip adresse 
        int port = 420;
        IPAddress ip = IPAddress.Any;
        IPEndPoint endpoint = new IPEndPoint(ip, port);

        TcpListener listener = new TcpListener(endpoint);

        Task<NetworkStream> stream = ClientConnect(listener);
        NetworkStream stream1 = stream.Result;

        //laver en random med 2002 mulige numre
        Random random = new Random();
        int Numb = random.Next(0, 2001);
        System.Console.WriteLine(Numb);
        while (true)
        {
            RecieveMessage(stream1, Numb);
        }   

    }

    //Her connecte jeg klienter og skriver hvilke forhold der er omkring tallene 
    public async Task<NetworkStream> ClientConnect(TcpListener listener)
    {
        listener.Start();

        TcpClient client = await listener.AcceptTcpClientAsync();

        NetworkStream stream = client.GetStream();
        string message = "The number is between 0-2001";
        ServerMessage(stream, message);
        return stream;
    }

    //Her kan serveren sende en besked
    public void ServerMessage(NetworkStream stream, string mes)
    {
        byte[] bytes = Encoding.UTF8.GetBytes("Server: " + mes);
        stream.Write(bytes, 0, bytes.Length);
    }
    
    //Her modtager den beskeder
    public async void RecieveMessage(NetworkStream stream, int NumberToGuess)
    {
        byte[] buffer = new byte[1000];
        
        bool run = true;

        while(run){
            int number = await stream.ReadAsync(buffer, 0, buffer.Length);
            string message = Encoding.UTF8.GetString(buffer, 0, number);
            int guess;

            bool intParse = Int32.TryParse(message, out guess);
            //Her svare den tilbage omkring om talet er højere eller lavere 
            if(intParse){
                if (guess != NumberToGuess){
                    if (guess <= NumberToGuess){
                        string higher = "The number is higher";
                        ServerMessage(stream, higher);
                    }
                    else if(guess >= NumberToGuess){
                        string lower = "The number is lower";
                        ServerMessage(stream, lower);
                    }
                }
                else{
                    string right = "You guessed the number";
                    ServerMessage(stream, right);
                    run = false;
                }
            }else{
                string error = "It has to be a number";
                ServerMessage(stream, error);
            }
        }
    }
}