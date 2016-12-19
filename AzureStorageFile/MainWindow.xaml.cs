using AzureCommonLibrary;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.File;
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

namespace AzureStorageFile
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
			//ファイル取得

			using (var timer = new Timer())
			{
				var constr = new Properties.Settings().StorageConnectionString;
				CloudStorageAccount storageAccount = CloudStorageAccount.Parse(constr);

				//var l = System.Environment.TickCount;

				// Create a CloudFileClient object for credentialed access to File storage.
				CloudFileClient fileClient = storageAccount.CreateCloudFileClient();

				// Get a reference to the file share we created previously.
				CloudFileShare share = fileClient.GetShareReference("logs");

				// Ensure that the share exists.
				if (share.Exists())
				{
					// Get a reference to the root directory for the share.
					CloudFileDirectory rootDir = share.GetRootDirectoryReference();

					// Get a reference to the directory we created previously.
					CloudFileDirectory sampleDir = rootDir.GetDirectoryReference("CustomLogs");

					// Ensure that the directory exists.
					if (sampleDir.Exists())
					{
						// Get a reference to the file we created previously.
						CloudFile file = sampleDir.GetFileReference("20140510_154631.mp4");

						// Ensure that the file exists.
						if (file.Exists())
						{
							// Write the contents of the file to the console window.
							//Console.WriteLine(file.DownloadTextAsync().Result);


							var task = file.DownloadToFileAsync(@"d:\a.mp4", System.IO.FileMode.CreateNew);
							task.Wait();


						}
					}
				}
				//2015/11頃
				//処理時間=5500    20MB

				//Debug.WriteLine("処理時間=" + (System.Environment.TickCount - l));
			}
		}
	}
}
