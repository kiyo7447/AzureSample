using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;
using Microsoft.ServiceBus.Messaging;

namespace ConsoleApplicationSendAzureServiceBus
{
	class Program
	{
		//Event Hubsの使用
		//https://azure.microsoft.com/ja-jp/documentation/articles/event-hubs-csharp-ephcs-getstarted/

	

		//static string eventHubName = "{EventHubName}";
		//static string eventHubConnectionString = "{EventHubConnectionString}";

		static void Main(string[] args)
		{


			Console.WriteLine("Press Ctrl-C to stop the sender process");
			Console.WriteLine("Press Enter to start now");
			Console.ReadLine();
			SendingRandomMessages();
		}

		static void SendingRandomMessages()
		{
			Properties.Settings settings = new Properties.Settings();

			string eventHubName = settings.EventHubName;
			string eventHubConnectionString = settings.EventHubConnectionString;

			var eventHubClient = EventHubClient.CreateFromConnectionString(eventHubConnectionString, eventHubName);
//			var eventHubClient = EventHubClient.CreateFromConnectionString(connectionString, eventHubName);
			while (true)
			{
				try
				{
					var message = Guid.NewGuid().ToString();
					Console.WriteLine("{0} > Sending message: {1}", DateTime.Now, message);
					eventHubClient.Send(new EventData(Encoding.UTF8.GetBytes(message)));
					
				}
				catch (Exception exception)
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("{0} > Exception: {1}", DateTime.Now, exception.Message);
					//Console.WriteLine("{0} > Exception: {1}", DateTime.Now, exception.ToString());
					Console.ResetColor();
				}

				Thread.Sleep(200);
			}
		}
	}
}
