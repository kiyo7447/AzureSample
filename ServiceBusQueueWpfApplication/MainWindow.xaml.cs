using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ServiceBusWpfApplication
{
	/// <summary>
	/// MainWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void button_Click(object sender, RoutedEventArgs e)
		{
			string connectionString = new Properties.Settings().ConnectionString;

			var namespaceManager =
				NamespaceManager.CreateFromConnectionString(connectionString);

			if (!namespaceManager.QueueExists("TestQueue2"))
			{
				namespaceManager.CreateQueue("TestQueue2");
			}
		}

		private void button1_Click(object sender, RoutedEventArgs e)
		{


			// Configure queue settings.
			QueueDescription qd = new QueueDescription("TestQueue");
			qd.MaxSizeInMegabytes = 5120;
			qd.DefaultMessageTimeToLive = new TimeSpan(0, 1, 0);

			// Create a new queue with custom settings.
			string connectionString = new Properties.Settings().ConnectionString;

			var namespaceManager =
				NamespaceManager.CreateFromConnectionString(connectionString);

			if (!namespaceManager.QueueExists("TestQueue"))
			{
				namespaceManager.CreateQueue(qd);
			}
		}

		private void button2_Click(object sender, RoutedEventArgs e)
		{
			// Create a new queue with custom settings.
			string connectionString = new Properties.Settings().ConnectionString;

			var l = System.Environment.TickCount;

			QueueClient Client =
				QueueClient.CreateFromConnectionString(connectionString, "TestQueue");

			Client.Send(new BrokeredMessage());


			for (int i = 0; i < 100; i++)
			{
				// Create message, passing a string message for the body.
				BrokeredMessage message = new BrokeredMessage("Test message " + i);

				// Set some addtional custom app-specific properties.
				message.Properties["TestProperty"] = "TestValue";
				message.Properties["Message number"] = i;

				// Send message to the queue.
				Client.Send(message);
			}

			

			//処理時間=11469ms    100message
			Debug.WriteLine("処理時間=" + (Environment.TickCount - l));

		}
	}
}
