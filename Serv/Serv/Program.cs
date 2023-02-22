using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    class Program
    {
        // Main Method
        static void Main(string[] args)
        {
            ExecuteServer();
        }


        public static void ExecuteServer()
        {
            IPHostEntry iPHost = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress iPAddr = iPHost.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(iPAddr, 11111);

            // Création TCP/IP Socket
            Socket listener = new Socket(iPAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(10);

                while (true)
                {
                    Console.WriteLine("Attente de connexion...");

                    Socket clientSocket = listener.Accept();

                    // data buffer
                    byte[] bytes = new Byte[1024];
                    string data = null;

                    while (true)
                    {
                        int numByte = clientSocket.Receive(bytes);

                        data += Encoding.ASCII.GetString(bytes, 0, numByte);

                        if (data.IndexOf("<EOF>") > -1)
                            break;
                    }

                    Console.WriteLine("Text recu -> {0}", data);
                    byte[] message = Encoding.ASCII.GetBytes("Test Server");

                    clientSocket.Send(message);

                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();

                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
