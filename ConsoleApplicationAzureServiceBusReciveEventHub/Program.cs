using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.ServiceBus.Messaging;
using System.Diagnostics;

namespace ConsoleApplicationAzureServiceBusReciveEventHub
{
	class Program
	{
		//Event Hubsの使用
		//https://azure.microsoft.com/ja-jp/documentation/articles/event-hubs-csharp-ephcs-getstarted/


		static void Main(string[] args)
		{
			Properties.Settings settings = new Properties.Settings();

			//string eventHubConnectionString = "{Event Hub connection string}";
			//string eventHubName = "{Event Hub name}";
			string eventHubName = settings.EventHubName;
			string eventHubConnectionString = settings.EventHubConnectionString;

			//string storageAccountName = "{storage account name}";
			//string storageAccountKey = "{storage account key}";
			string storageAccountName = settings.StorageAccountName;
			string storageAccountKey = settings.StorageAccountKey;

			string storageConnectionString = string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}", storageAccountName, storageAccountKey);

			string eventProcessorHostName = Guid.NewGuid().ToString();
			EventProcessorHost eventProcessorHost = new EventProcessorHost(eventProcessorHostName, eventHubName, EventHubConsumerGroup.DefaultGroupName, eventHubConnectionString, storageConnectionString);
			Console.WriteLine("Registering EventProcessor...");
			var options = new EventProcessorOptions();
			options.ExceptionReceived += (sender, e) => { Console.WriteLine(e.Exception); };
			eventProcessorHost.RegisterEventProcessorAsync<SimpleEventProcessor>(options).Wait();

			Console.WriteLine("Receiving. Press enter key to stop worker.");
			Console.ReadLine();
			eventProcessorHost.UnregisterEventProcessorAsync().Wait();
		}
	}
}
