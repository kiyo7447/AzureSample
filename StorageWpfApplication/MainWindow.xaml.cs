using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
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

//blob の手順
//https://azure.microsoft.com/ja-jp/documentation/articles/storage-dotnet-how-to-use-blobs/


//https://abestorage2.blob.core.windows.net/

//Acount  -  Container  -  Blob

//https://abestorage2.blob.core.windows.net/movies/20140510_154631.mp4
//https://abestorage2.blob.core.windows.net/movies


//storage account  abestorage2
//access key  IwD5+vgsdi6x9NJaKcMXlU3RrlNnhS5sMqkKPIm3qtofvJrR9eTfyPY7MRGynvYPoZLC6vq4P+5MUrp5pLWkyQ==

//download
//https://abestorage2.blob.core.windows.net/movies/20140510_154631.mp4

namespace StorageWpfApplication
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
			//このコードは、動作しませんでした。

			var constr = new Properties.Settings().StorageConnectionString;
			CloudStorageAccount storageAccount = CloudStorageAccount.Parse(constr);


			// Create the blob client.
			CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

			// Retrieve a reference to a container.
			CloudBlobContainer container = blobClient.GetContainerReference("movies");

			// Create the container if it doesn't already exist.
			container.CreateIfNotExists();

			container.SetPermissions(
				new BlobContainerPermissions
				{
					PublicAccess =
				BlobContainerPublicAccessType.Blob
				});
		}



		private void button1_Click(object sender, RoutedEventArgs e)
		{
			var constr = new Properties.Settings().StorageConnectionString;

			// Retrieve storage account from connection string.
			CloudStorageAccount storageAccount = CloudStorageAccount.Parse(constr);

			// Create the blob client.
			CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

			// Retrieve reference to a previously created container.
			CloudBlobContainer container = blobClient.GetContainerReference("movies");

			// Loop over items within the container and output the length and URI.
			foreach (IListBlobItem item in container.ListBlobs(null, false))
			{
				if (item.GetType() == typeof(CloudBlockBlob))
				{
					CloudBlockBlob blob = (CloudBlockBlob)item;

					Console.WriteLine("Block blob of length {0}: {1}", blob.Properties.Length, blob.Uri);

				}
				else if (item.GetType() == typeof(CloudPageBlob))
				{
					CloudPageBlob pageBlob = (CloudPageBlob)item;

					Console.WriteLine("Page blob of length {0}: {1}", pageBlob.Properties.Length, pageBlob.Uri);

				}
				else if (item.GetType() == typeof(CloudBlobDirectory))
				{
					CloudBlobDirectory directory = (CloudBlobDirectory)item;

					Console.WriteLine("Directory: {0}", directory.Uri);
				}
			}
		}

		private void button2_Click(object sender, RoutedEventArgs e)
		{
			var constr = new Properties.Settings().StorageConnectionString;

			// Retrieve storage account from connection string.
			CloudStorageAccount storageAccount = CloudStorageAccount.Parse(constr);

			//Create service client for credentialed access to the Blob service.
			CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

			//Get a reference to a container.
			CloudBlobContainer container = blobClient.GetContainerReference("movies");

			//Create the container if it does not already exist. 
			container.CreateIfNotExists();

			//Get a reference to an append blob.
			CloudAppendBlob appendBlob = container.GetAppendBlobReference("20140510_154631.mp4");

			//Create the append blob. Note that if the blob already exists, the CreateOrReplace() method will overwrite it.
			//You can check whether the blob exists to avoid overwriting it by using CloudAppendBlob.Exists().
			appendBlob.CreateOrReplace();

			int numBlocks = 10;

			//Generate an array of random bytes.
			Random rnd = new Random();
			byte[] bytes = new byte[numBlocks];
			rnd.NextBytes(bytes);

			//Simulate a logging operation by writing text data and byte data to the end of the append blob.
			for (int i = 0; i < numBlocks; i++)
			{
				appendBlob.AppendText(String.Format("Timestamp: {0:u} \tLog Entry: {1}{2}",
					DateTime.UtcNow, bytes[i], Environment.NewLine));
			}

			//Read the append blob to the console window.
			Console.WriteLine(appendBlob.DownloadText());
		}

		private void button3_Click(object sender, RoutedEventArgs e)
		{
			var constr = new Properties.Settings().StorageConnectionString;

			// Retrieve storage account from connection string.
			CloudStorageAccount storageAccount = CloudStorageAccount.Parse(constr);

			//Create service client for credentialed access to the Blob service.
			CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

			//Get a reference to a container.
			CloudBlobContainer container = blobClient.GetContainerReference("movies");

			//Create the container if it does not already exist. 
			container.CreateIfNotExists();

			//Get a reference to an append blob.
			CloudAppendBlob appendBlob = container.GetAppendBlobReference("20140510_154631.mp4");

			//Create the append blob. Note that if the blob already exists, the CreateOrReplace() method will overwrite it.
			//You can check whether the blob exists to avoid overwriting it by using CloudAppendBlob.Exists().
			appendBlob.CreateOrReplace();


			//			var bytes = File.ReadAllBytes(@"O:\20140510_154631.mp4");

			var l = System.Environment.TickCount;

			appendBlob.AppendFromFile(@"O:\20140510_154631.mp4", FileMode.OpenOrCreate);

			//20MBのアップロードで、5.5秒
			Console.WriteLine("処理時間："+ (System.Environment.TickCount - l ));

			//int numBlocks = 10;

			////Simulate a logging operation by writing text data and byte data to the end of the append blob.
			//for (int i = 0; i < numBlocks; i++)
			//{
			//	appendBlob.AppendText(String.Format("Timestamp: {0:u} \tLog Entry: {1}{2}",
			//		DateTime.UtcNow, bytes[i], Environment.NewLine));
			//}

			////Read the append blob to the console window.
			//Console.WriteLine(appendBlob.DownloadText());
		}
	}
}
