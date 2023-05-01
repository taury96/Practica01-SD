using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ClienteSocket
{
    class Cliente
    {
        static void Main(string[] args)
        {
            StartClient();
        }

        public static void StartClient()
        {
            // Establecer el endpoint para el socket
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5000); //<- cambiar 

            //ESta para conectarme a la dirección del servidor : 
            //IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse("192.168.0.100"), 5000);//<-- elegir la dirección IP del servidor
            /*Esto hará que el cliente intente conectarse al servidor en la dirección IP especificada. 
             * Si el servidor está correctamente configurado y en ejecución en esa dirección IP, la conexión se establecerá correctamente.*/


// Crear un socket TCP/IP
Socket sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

try
{
    // Conectar el socket al endpoint remoto
    sender.Connect(endpoint);

    Console.WriteLine("Conexión establecida con el servidor.");

    // Enviar datos al servidor
    string message = "";
    while (message != "x")
    {
        Console.WriteLine("Escriba un mensaje para enviar al servidor:");
        message = Console.ReadLine();
        byte[] data = Encoding.ASCII.GetBytes(message);
        sender.Send(data);

        // Recibir la respuesta del servidor
        byte[] buffer = new byte[1024];
        int bytesRec = sender.Receive(buffer);
        string response = Encoding.ASCII.GetString(buffer, 0, bytesRec);
        Console.WriteLine("Respuesta recibida del servidor: " + response);
    }

    // Cerrar el socket
    sender.Shutdown(SocketShutdown.Both);
    sender.Close();
}
catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
}
}
}
}
