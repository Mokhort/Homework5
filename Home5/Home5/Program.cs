using Homework3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Home5
{
    class Program
    {
		static UdpClient server;
		const int port = 8080;
		static bool flag;
        static Socket listeningSocket;
        static List<IPEndPoint> clients = new List<IPEndPoint>();
		static Context context = new Context();
		public static Queue<UdpReceiveResult> queue = new Queue<UdpReceiveResult>();

		static async Task Listener_run()
		{
			string reply;
			while (flag)
			{

				UdpReceiveResult commandDatagram = await server.ReceiveAsync();
				queue.Enqueue(commandDatagram);
				string command = Encoding.UTF8.GetString(commandDatagram.Buffer);

				bool flag_queue = queue.TryDequeue(out commandDatagram);
				if (flag_queue)
				{
					reply = await Request_run(command);
					Console.WriteLine(reply);
					byte[] replyDatagram = Encoding.UTF8.GetBytes(reply);
					await server.SendAsync(replyDatagram, replyDatagram.Length, commandDatagram.RemoteEndPoint);
				}
			}
		}
		static async Task<string> Request_run( string result){

			string answer = "";	
			
						if (result == "1")
						{
						Console.WriteLine("get res 1 ");
						Console.WriteLine(result);
							IQueryable<Passanger> passanger = from ps in context.Passanger select ps;
							List<Passanger> list_passanger = passanger.ToList();
							foreach (Passanger passangers in list_passanger) {
								Console.WriteLine($"Name - {passangers.Name} LastName - {passangers.LastName} Email - {passangers.Email}");
						        answer += $"Name - {passangers.Name} LastName - {passangers.LastName} Email - {passangers.Email}\n";
					        }

				        }
						else if (result == "2")
						{
						    Console.WriteLine("get res 2 ");
						    IQueryable<Flight> flight = from ps in context.Flight select ps;
							List<Flight> list_flight = flight.ToList();
					        foreach (Flight flights in list_flight) {
						          Console.WriteLine($"Name - {flights.Name} Price - {flights.Cost}");
						          answer += $"Name - {flights.Name} Price - {flights.Cost}\n";
							}
						}
			//context.Dispose();
			return answer;
		}

		static async Task Listener()
		{
			await Task.Run(() => Listener_run());
		}

		static void add_notes() {
			context.Passanger.Add(new Passanger()
			{
				Name = "Yorick",
				LastName = "Ivanov",
				Email = "kyky@mail.ru"
			});
			context.Passanger.Add(new Passanger()
			{
				Name = "Ilya",
				LastName = "Semenov",
				Email = "semy@mail.ru"
			});
			context.Flight.Add(new Flight()
			{
				Name = "YTY2",
				Cost = 27000
			});
			context.Flight.Add(new Flight()
			{
				Name = "OGO3",
				Cost = 5600
			});
			context.SaveChanges();
		}

		static async Task Main(string[] args)
        {
		    server = new UdpClient(port);
			Console.WriteLine("Server start");
		    context.Database.EnsureCreated();
			add_notes();
			context.SaveChanges();
			listeningSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
			flag = true;
			Task listener = Listener();
			Console.ReadLine();
			await listener;
		}
    }
}
