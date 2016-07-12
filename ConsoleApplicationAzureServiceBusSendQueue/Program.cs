using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.ServiceBus.Messaging;
using System.Threading;

namespace ConsoleApplicationSendAzureEvnetHubQueue
{
	class Program
	{
		//Service Bus キューの使用
		//https://azure.microsoft.com/ja-jp/documentation/articles/service-bus-dotnet-get-started-with-queues/

		static void Main(string[] args)
		{
			Properties.Settings settings = new Properties.Settings();

			//var connectionString = "{connectionString}";
			//var queueName = "{queueName}";
			var connectionString = settings.QueueConnectionString;
			var queueName = settings.QueueName;

			var client = QueueClient.CreateFromConnectionString(connectionString, queueName);
			for (int i = 0; i < 1000; i++) {
				var guid = Guid.NewGuid().ToString();
				var message = new BrokeredMessage($"This is a test message! datetime={DateTime.Now.ToString()}, message={guid}");
				client.Send(message);
				var s = message.GetBody<string>();

				Console.Out.WriteLine($"message={s}");
				Thread.Sleep(200);
			}
			Console.In.ReadLine();
		}
	}
}
