using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ServidorSocket
{
    class Servidor
    {
        static void Main(string[] args)
        {
            StartServer();
        }

        public static void StartServer()
        {
            // Establecer el endpoint para el socket
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, 5000); // <--Cambiar

            // Cambio requerido para conectarme desde otro equipo :
            // IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse("192.168.1.100"), 5000); //<-- funciona con otros equipos
            //De esta manera, el servidor estará escuchando conexiones en la dirección IP 192.168.1.100 en el puerto 5000.
          
            
            
            // Crear un socket TCP/IP
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // Vincular el socket a un endpoint y escuchar por conexiones
            listener.Bind(endpoint);
            listener.Listen(2);

            Console.WriteLine("Servidor socket iniciado. Esperando conexiones...");

            try
            {
                while (true)
                {
                    // Esperar por una conexión
                    Socket handler = listener.Accept();

                    Console.WriteLine("Conexión aceptada desde " + handler.RemoteEndPoint.ToString());

                    // Iniciar un nuevo hilo para manejar la conexión del cliente
                    Thread thread = new Thread(() => HandleClient(handler));
                    thread.Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public static void HandleClient(Socket handler)
        {
            try
            {
                byte[] buffer = new byte[1024];
                string data = null;

                while (true)
                {
                    int bytesRec = handler.Receive(buffer);
                    data += Encoding.ASCII.GetString(buffer, 0, bytesRec);
                    Console.WriteLine("Mensaje recibido del cliente: " + data);

                    if (data.IndexOf("<EOF>") > -1)
                    {
                        break;
                    }

                    byte[] response = Encoding.ASCII.GetBytes("Respuesta del servidor: " + data);
                    handler.Send(response);
                }

                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}