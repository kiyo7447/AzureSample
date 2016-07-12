using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
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

namespace StorageQueueWpfApplication
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

		/// <summary>
		/// エンキュー
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button_Click(object sender, RoutedEventArgs e)
		{

			var constr = new Properties.Settings().StorageConnectionString;
			CloudStorageAccount storageAccount = CloudStorageAccount.Parse(constr);



			// Create the queue client
			CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();



			// Retrieve a reference to a queue
			CloudQueue queue = queueClient.GetQueueReference("taikin");

			// Create the queue if it doesn't already exist
			queue.CreateIfNotExists();

		}

		private void button1_Click(object sender, RoutedEventArgs e)
		{
			var constr = new Properties.Settings().StorageConnectionString;
			CloudStorageAccount storageAccount = CloudStorageAccount.Parse(constr);

			// Create the queue client.
			CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

			// Retrieve a reference to a queue.
			CloudQueue queue = queueClient.GetQueueReference("taikin");

			// Create the queue if it doesn't already exist.
			queue.CreateIfNotExists();



			// Create a message and add it to the queue.
			CloudQueueMessage message = new CloudQueueMessage("Hello, World" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
			queue.AddMessage(message);

		}

		private void button2_Click(object sender, RoutedEventArgs e)
		{
			var constr = new Properties.Settings().StorageConnectionString;
			CloudStorageAccount storageAccount = CloudStorageAccount.Parse(constr);

			// Create the queue client
			CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

			// Retrieve a reference to a queue
			CloudQueue queue = queueClient.GetQueueReference("taikin");

			// Get the next message
			CloudQueueMessage retrievedMessage = queue.GetMessage();

			//Queueにはデータが入っていない。
			if (retrievedMessage == null)
			{
				Console.WriteLine("データがみつかりません。");
				return;
			}

			Console.WriteLine(retrievedMessage.AsString);


			//Process the message in less than 30 seconds, and then delete the message
			queue.DeleteMessage(retrievedMessage);


			
		}

		private void button3_Click(object sender, RoutedEventArgs e)
		{
			var constr = new Properties.Settings().StorageConnectionString;
			CloudStorageAccount storageAccount = CloudStorageAccount.Parse(constr);



			// Create the queue client
			CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();



			// Retrieve a reference to a queue
			CloudQueue queue = queueClient.GetQueueReference("taikin2");

			var l = System.Environment.TickCount;

			// Create the queue if it doesn't already exist
			var task = queue.CreateIfNotExistsAsync();

			task.Wait();

			//4109ms
			Console.WriteLine("add queue complete 処理時間：" + (System.Environment.TickCount - l));
		}

		private void button4_Click(object sender, RoutedEventArgs e)
		{

			var constr = new Properties.Settings().StorageConnectionString;
			CloudStorageAccount storageAccount = CloudStorageAccount.Parse(constr);

			// Create the queue client.
			CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

			// Retrieve a reference to a queue.
			CloudQueue queue = queueClient.GetQueueReference("taikin");

			// Create the queue if it doesn't already exist.
			queue.CreateIfNotExists();

			var l = System.Environment.TickCount;

			for (var c = 0; c < 100; c++)
			{
				// Create a message and add it to the queue.
				CloudQueueMessage message = new CloudQueueMessage("Hello, World:" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + ":" + c);
				queue.AddMessage(message);
			}

			//13672msの処理時間がかかった
			Debug.WriteLine("100Queueのエンキューの処理時間=" + (System.Environment.TickCount - l));
		}
	}
}
