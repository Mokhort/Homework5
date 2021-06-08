using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static int remotePort;
        static IPAddress ipAddress;
        static Socket listeningSocket;
        static UdpClient client;
        static bool flag = true;
        static IPEndPoint serverEP;
        static void Main(string[] args)
        {
            ipAddress = IPAddress.Loopback;
            remotePort = 8080;
            client = new UdpClient();
            serverEP = new IPEndPoint(ipAddress, remotePort);
            Console.WriteLine("Enter the num: 1 - First request, 2 - Second request, 3 - Exit");
            listeningSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                while (flag)
                {
                    string cmd = Console.ReadLine();
                    switch (cmd)
                    {
                        case "1":
                             Console.WriteLine("You choisen 1 request");
                             Request1(cmd);
                        break;
                        case "2":
                             Console.WriteLine("You choisen 2 request");
                             Request1(cmd);
                        break;
                        case "3":
                            Console.WriteLine("Closing programm...");
                            flag = false;
                            Close();
                            break;
                        default:
                            Console.WriteLine("Incorrect value, try again");
                            break;
                    }
                }
        }
        public static async void Request1(string cmd)
        {
            Console.WriteLine("req in progress...");
            string ans = await SendAndReceiveCommand(cmd);
            Console.WriteLine(ans);
        }

        static async Task<string> SendAndReceiveCommand(string command)
        {
            byte[] commandDatagram = Encoding.UTF8.GetBytes(command);
            await client.SendAsync(commandDatagram, commandDatagram.Length, serverEP);
            UdpReceiveResult result = await client.ReceiveAsync();
            string reply = Encoding.UTF8.GetString(result.Buffer);
            return reply;
        }

        private static void Close()
        {
            if (listeningSocket != null)
            {
                listeningSocket.Shutdown(SocketShutdown.Both);
                listeningSocket.Close();
                listeningSocket = null;
            }

            Console.WriteLine("Server closed!");
        }
    }
}