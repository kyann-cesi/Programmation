using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace client
{
    class Program
    {
        // Main Method
        static void Main(string[] args)
        {
            ExecuteClient();
        }


        public static void ExecuteClient()
        {
            try
            {
                IPHostEntry iPHost = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress iPAddr = iPHost.AddressList[0];
                IPEndPoint localEndPoint = new IPEndPoint(iPAddr, 11111);

                // Création TCP/IP Socket
                Socket sender = new Socket(iPAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                try
                {
                    sender.Connect(localEndPoint);


                    Console.WriteLine("Socket connecté à -> {0}", sender.RemoteEndPoint.ToString());


                    byte[] messageSent = Encoding.ASCII.GetBytes("Test Client<EOF>");
                    int byteSent = sender.Send(messageSent);

                    byte[] messageReceived = new byte[1024];

                    int byteRecv = sender.Receive(messageReceived);
                    Console.WriteLine("Message venant du server -> {0}", Encoding.ASCII.GetString(messageReceived, 0, byteRecv));

                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();
                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }
                catch (SocketException se)
                {
                    Console.WriteLine("SocketException : {0}", se.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception : {0}", e.ToString());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

        }
    }
}