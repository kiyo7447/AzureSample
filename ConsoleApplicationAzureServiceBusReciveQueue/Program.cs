using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.ServiceBus.Messaging;

namespace ConsoleApplicationAzureEventHubReciveQueue
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

			client.OnMessage(message =>
			{
				Console.WriteLine(String.Format("Message body: {0}", message.GetBody<String>()));
				Console.WriteLine(String.Format("Message id: {0}", message.MessageId));
			});

			Console.ReadLine();
		}
	}
}
