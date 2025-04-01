using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class Client
{
    static void Main()
    {
        Console.Title = " Client ";
        Console.Write("Entrez l'adresse IP du serveur: ");
        TcpClient client = new TcpClient(Console.ReadLine(), 1234);
        Console.WriteLine("Connecté au serveur.");

        NetworkStream stream = client.GetStream();
        Thread receiveThread = new Thread(() => ReceiveMessages(stream));
        receiveThread.Start();

        while (true)
        {
            string message = Console.ReadLine();
            byte[] buffer = Encoding.ASCII.GetBytes(message);
            stream.Write(buffer, 0, buffer.Length);
        }
    }

    static void ReceiveMessages(NetworkStream stream)
    {
        byte[] buffer = new byte[1024];
        int bytesRead;

        while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
        {
            string receivedMessage = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            Console.WriteLine(receivedMessage + "\n");
            
        }
    }
}