using AzureCommonLibrary;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
//access key  

//download
//https://abestorage2.blob.core.windows.net/movies/20140510_154631.mp4

namespace AzureStorageBlob
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

		private void buttonCreateBlobContainer_Click(object sender, RoutedEventArgs e)
		{
			//コンテナの作成
			using (var s = new Timer())
			{
				//Blobへの接続文字列を取得
				//efaultEndpointsProtocol=https;AccountName=hogehogestorageaccount;AccountKey=XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
				var constr = new Properties.Settings().StorageConnectionString;

				//ストレージアカウントを取得
				CloudStorageAccount storageAccount = CloudStorageAccount.Parse(constr);

				// Blobクライアントを作成します。
				CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

				// コンテナへの参照を取得します。
				CloudBlobContainer container = blobClient.GetContainerReference("movies");

				// コンテナがまだ存在しない場合は作成します。
				container.CreateIfNotExists();

				container.SetPermissions(
					new BlobContainerPermissions
					{
						//Blob, Continer（コンテナ）, Off(プライベート）
						PublicAccess = BlobContainerPublicAccessType.Blob
						//https://acom-feature-videos-twitter-card.azurewebsites.net/ja-jp/documentation/articles/storage-manage-access-to-resources/

					});
			}
			//2016/12/17
			//処理時間=781ms

			//2017/1/12			新規にコンテナをつくっても変わらない。
			//処理時間=797ms

		}


		private void buttonGetBlobList_Click(object sender, RoutedEventArgs e)
		{
			//Blobの一覧を取得する
			using (var s = new Timer())
			{
				//Blobへの接続文字列を取得
				//efaultEndpointsProtocol=https;AccountName=hogehogestorageaccount;AccountKey=XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
				var constr = new Properties.Settings().StorageConnectionString;

				// 接続文字列からストレージアカウントを取得します。
				CloudStorageAccount storageAccount = CloudStorageAccount.Parse(constr);

				// blobクライアントを作成します。
				CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

				// 以前に作成したコンテナへの参照を取得します。
				CloudBlobContainer container = blobClient.GetContainerReference("movies");

				// コンテナ内の項目をループし、長さとURIを出力します。
				foreach (IListBlobItem item in container.ListBlobs(null, false))
				{
					if (item.GetType() == typeof(CloudBlockBlob))
					{
						CloudBlockBlob blob = (CloudBlockBlob)item;

						Debug.WriteLine($"Block blob of length {blob.Properties.Length}: {blob.Uri}");

					}
					if (item.GetType() == typeof(CloudAppendBlob))
					{
						CloudAppendBlob blob = (CloudAppendBlob)item;

						Debug.WriteLine($"Block blob of length {blob.Properties.Length}: {blob.Uri}");

					}
					else if (item.GetType() == typeof(CloudPageBlob))
					{
						CloudPageBlob pageBlob = (CloudPageBlob)item;

						Debug.WriteLine($"Page blob of length {pageBlob.Properties.Length}: {pageBlob.Uri}");

					}
					else if (item.GetType() == typeof(CloudBlobDirectory))
					{
						CloudBlobDirectory directory = (CloudBlobDirectory)item;

						Debug.WriteLine($"Directory: {directory.Uri}");

						//対象のディレクトリのすべてのファイルを取得する。
						//CloudAppendBlob、CloudBlockBlobがすべて取得される
						var list = container.GetDirectoryReference("").ListBlobs(useFlatBlobListing: true);
						Debug.WriteLine($"root(sub)取得件数={list.Count()}");
						list.All(l =>
						{
							Debug.WriteLine($"取得={l.Uri}");
							return true;
						});

						//階層のサブ改造までは見ない
						//CloudBlobDirectory、CloudAppendBlob、CloudBlockBlobがすべて取得される
						list = container.GetDirectoryReference("").ListBlobs(useFlatBlobListing: false);
						Debug.WriteLine($"root(flat)取得件数={list.Count()}");
						list.All(l =>
						{
							Debug.WriteLine($"取得={l.Uri}");
							return true;
						});

						//対象のディレクトリのすべてのファイルを取得する。
						//CloudAppendBlob、CloudBlockBlobがすべて取得される
						list = container.GetDirectoryReference("abe").ListBlobs(useFlatBlobListing: true);
						Debug.WriteLine($"abe(sub)取得件数={list.Count()}");
						list.All(l =>
						{
							Debug.WriteLine($"取得={l.Uri}");
							return true;
						});

						//階層のサブ改造までは見ない
						//CloudBlobDirectory、CloudAppendBlob、CloudBlockBlobがすべて取得される
						list = container.GetDirectoryReference("abe").ListBlobs(useFlatBlobListing: false);
						Debug.WriteLine($"abe(Flat)取得件数={list.Count()}");
						list.All(l =>
						{
							Debug.WriteLine($"取得={l.Uri}");
							return true;
						});
					}
					else
					{
						//想定外のタイプ			追加Blobは？？
						var msg = $"想定外の型が指定された Error {item.GetType().FullName}";
						Debug.WriteLine(msg);
						throw new SystemException(msg);
					}
				}
			}
		}

		private void buttonAddAppendBlob_Click(object sender, RoutedEventArgs e)
		{
			//Blobの追加書き込み
			using (var timer = new Timer())
			{

				//Blobへの接続文字列を取得
				//efaultEndpointsProtocol=https;AccountName=hogehogestorageaccount;AccountKey=XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
				var constr = new Properties.Settings().StorageConnectionString;

				//接続文字列からストレージアカウントを取得します。
				CloudStorageAccount storageAccount = CloudStorageAccount.Parse(constr);

				//Blobサービスへの認証されたアクセスのためのサービスクライアントを作成します。
				CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

				//コンテナへの参照を取得します。
				CloudBlobContainer container = blobClient.GetContainerReference("movies");

				//コンテナがまだ存在しない場合は作成します。
				container.CreateIfNotExists();

				//追加BLOBへの参照を取得します。
				CloudAppendBlob appendBlob = container.GetAppendBlobReference("abe/kiyo/taka/test.log");


				//追加BLOBを作成します。ブロブがすでに存在する場合、CreateOrReplace（）メソッドはそれを上書きします。
				// CloudAppendBlob.Exists（）を使用してBLOBが上書きされないようにBLOBが存在するかどうかを確認できます。				
				appendBlob.CreateOrReplace();

				int numBlocks = 10;

				//乱数の配列を生成します。
				Random rnd = new Random();
				byte[] bytes = new byte[numBlocks];
				rnd.NextBytes(bytes);

				//テキストデータとバイトデータをappendブロブの末尾に書き込むことによって、ロギング操作をシミュレートします。
				for (int i = 0; i < numBlocks; i++)
				{
					appendBlob.AppendText(
						content: $"Timestamp: {DateTime.UtcNow:u} \tLog Entry: {bytes[i]}{Environment.NewLine}",
						encoding: Encoding.GetEncoding("UTF-8")
					);
				}

				//append blobをコンソールウィンドウに読み込みます。
				Debug.WriteLine(appendBlob.DownloadText());
			}
		}

		private void buttonAddBlobLargeFile_Click(object sender, RoutedEventArgs e)
		{
			//Blobへの追加書き込み９２ＭＢ
			using (var timer = new Timer())
			{
				//Blobへの接続文字列を取得
				//efaultEndpointsProtocol=https;AccountName=hogehogestorageaccount;AccountKey=XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
				var constr = new Properties.Settings().StorageConnectionString;

				//接続文字列からストレージアカウントを取得します。
				CloudStorageAccount storageAccount = CloudStorageAccount.Parse(constr);

				//Blobサービスへの認証されたアクセスのためのサービスクライアントを作成します。
				CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

				//コンテナへの参照を取得します。
				CloudBlobContainer container = blobClient.GetContainerReference("movies");

				//コンテナがまだ存在しない場合は作成します。
				container.CreateIfNotExists();

				//Get a reference to an append blob.
				CloudAppendBlob appendBlob = container.GetAppendBlobReference("abe/kiyo/taka/discoverypartIIjonathanmitchellmp4.mp4");

				//追加BLOBを作成します。ブロブがすでに存在する場合、CreateOrReplace（）メソッドはそれを上書きします。
				// CloudAppendBlob.Exists（）を使用してBLOBが上書きされないようにBLOBが存在するかどうかを確認できます。
				appendBlob.CreateOrReplace();

				//			var bytes = File.ReadAllBytes(@"O:\20140510_154631.mp4");

				var l = System.Environment.TickCount;

				AccessCondition condition = new AccessCondition();

				BlobRequestOptions options = new BlobRequestOptions();

				appendBlob.AppendFromFile(
					path: @".\Contents\discoverypartIIjonathanmitchellmp4.mp4"
				//mode: FileMode.OpenOrCreate
				//operationContext:  FileMode.OpenOrCreate
				);

				//20MBのアップロードで、5.5秒
				//92MBのアップロードで、16.8秒
				Debug.WriteLine("処理時間：" + (System.Environment.TickCount - l));

				//int numBlocks = 10;

				////Simulate a logging operation by writing text data and byte data to the end of the append blob.
				//for (int i = 0; i < numBlocks; i++)
				//{
				//	appendBlob.AppendText(String.Format("Timestamp: {0:u} \tLog Entry: {1}{2}",
				//		DateTime.UtcNow, bytes[i], Environment.NewLine));
				//}

				////Read the append blob to the console window.
				//Console.WriteLine(appendBlob.DownloadText());

				//92MBのアップロード時間
				//処理時間=17234ms
			}
		}

		private void buttonCreateSAS_Click(object sender, RoutedEventArgs e)
		{
			//共有アクセス署名（SAS）の作成
			using (var time = new Timer())
			{
				//Blobへの接続文字列を取得
				//efaultEndpointsProtocol=https;AccountName=hogehogestorageaccount;AccountKey=XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
				var constr = new Properties.Settings().StorageConnectionString;

				//接続文字列からストレージアカウントを取得します。
				CloudStorageAccount storageAccount = CloudStorageAccount.Parse(constr);

				//Blobサービスへの認証されたアクセスのためのサービスクライアントを作成します。
				CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

				//コンテナへの参照を取得します。（大文字は使えません）
				CloudBlobContainer container = blobClient.GetContainerReference("sheardaccesssignature");

				//コンテナがまだ存在しない場合は作成します。
				container.CreateIfNotExists();

				////共有アクセスポリシーとパブリックアクセス設定で構成されるBLOBコンテナのアクセス許可を作成します。
				BlobContainerPermissions blobPermissions = new BlobContainerPermissions();

				// 共有アクセスポリシーは、コンテナへの読み取り/書き込みアクセスを10時間提供します。
				blobPermissions.SharedAccessPolicies.Add("mypolicy", new SharedAccessBlobPolicy()
				{
					// SASがすぐに有効になるように、開始時刻を設定しないでください。
					//このようにして、小さなクロックの違いによる失敗を避けることができます。
					SharedAccessExpiryTime = DateTime.UtcNow.AddHours(10),
					Permissions = SharedAccessBlobPermissions.Write |
					SharedAccessBlobPermissions.Read
				});

				//パブリックアクセス設定では、コンテナが非公開であるため、匿名でアクセスすることはできません。
				blobPermissions.PublicAccess = BlobContainerPublicAccessType.Off;

				//コンテナにアクセス許可ポリシーを設定します。
				container.SetPermissions(blobPermissions);

				//ユーザーと共有する共有アクセス署名を取得します。
				string sasToken = container.GetSharedAccessSignature(new SharedAccessBlobPolicy(), "mypolicy");
			}

		}

		private void buttonAccessSAS_Click(object sender, RoutedEventArgs e)
		{
			using (var timer = new Timer())
			{
				Properties.Settings settings = new Properties.Settings();

				//共有アクセス署名（SAS）の使用
				//Uri blobUri = new Uri("https://hogehogestorageaccount.blob.core.windows.net/sheardaccesssignature/MyBlob.txt");
				Uri blobUri = new Uri(settings.SASUri);

				//var sasToken = "?sv=2015-12-11&si=mypolicy&sr=c&sig=XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
				var sasToken = settings.SASToken;

				// Create credentials with the SAS token. The SAS token was created in previous example.
				StorageCredentials credentials = new StorageCredentials(sasToken);

				// Create a new blob.
				CloudBlockBlob blob = new CloudBlockBlob(blobUri, credentials);

				// Upload the blob. 
				// If the blob does not yet exist, it will be created. 
				// If the blob does exist, its existing content will be overwritten.
				using (var fileStream = System.IO.File.OpenRead(@".\Contents\MyBlob.txt"))
				{
					blob.UploadFromStream(fileStream);
				}
			}
			//処理時間=703ms
		}
	}
}
