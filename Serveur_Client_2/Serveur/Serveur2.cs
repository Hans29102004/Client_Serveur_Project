using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

internal class Program
{
    private static List<TcpClient> clients = new List<TcpClient>();

    static void Main()
    {  
        Console.Title = "Serveur" ;
        string adresse;
        Console.WriteLine("Entrer l'addresse IP de votre serveur ou enter OK pour utiliser l'address Local 127.0.0.1");
        string a =Console.ReadLine();
        string DefaultAddress ="127.0.0.1";
        if( a == "OK" || a == "ok" || a =="oK" || a == "Ok"  ){
            adresse = DefaultAddress;
            Console.Clear();
        }
        else{
            adresse = a;
            Console.Clear();
        }
        TcpListener serveur = new TcpListener(IPAddress.Parse(adresse), 1234);
        serveur.Start();
        Console.WriteLine("Serveur démarré sur " + IPAddress.Parse(adresse));

        while (true)
        {
            TcpClient client = serveur.AcceptTcpClient();
            clients.Add(client);
            Console.WriteLine("Nouvelle connexion entrante ........");
            Thread clientThread = new Thread(() => HandleClient(client));
            clientThread.Start();
        }
    }

    private static void HandleClient(TcpClient client)
    {
        NetworkStream stream = client.GetStream();
        byte[] buffer = new byte[1024];
        int bytesRead;
        string pseudo = "";

        try
        {
            // Envoi de la demande de pseudo
            byte[] prompt = Encoding.ASCII.GetBytes("Entrez votre pseudo :  ");
            stream.Write(prompt, 0, prompt.Length);

            // Réception du pseudo
            bytesRead = stream.Read(buffer, 0, buffer.Length);
            pseudo = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            Console.WriteLine(pseudo + "  est connecté.");

            // Confirmation de connexion
            prompt = Encoding.ASCII.GetBytes("Vous etes connecte . Vous pouvez commencer a chatter....... \n \n \n");
            stream.Write(prompt, 0, prompt.Length);

            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                BroadcastMessage(pseudo + "         :          " + message, client);
            }
        }
        catch (Exception)
        {
            // Gestion de la déconnexion inattendue
        }
        finally
        {
            client.Close();
            clients.Remove(client);
            Console.WriteLine(pseudo + " déconnecté.");
            BroadcastMessage(pseudo + " s'est déconnecté.", null); // Notifier tous les clients
        }
    }

    private static void BroadcastMessage(string message, TcpClient? sender)
    {
        byte[] buffer = Encoding.ASCII.GetBytes(message);
        foreach (var client in clients)
        {
            if (client != sender)
            {
                NetworkStream stream = client.GetStream();
                stream.Write(buffer, 0, buffer.Length);
            }
        }
    }
}
