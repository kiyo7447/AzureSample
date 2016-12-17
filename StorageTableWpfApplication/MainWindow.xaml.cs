using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
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

namespace AzureStorageTable
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
			//テーブル作成

			// Retrieve the storage account from the connection string.
			var constr = new Properties.Settings().StorageConnectionString;
			CloudStorageAccount storageAccount = CloudStorageAccount.Parse(constr);

			// Create the table client.
			CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

			// Create the table if it doesn't exist.
			CloudTable table = tableClient.GetTableReference("people");

			//同期処理へ変更
			//table.CreateIfNotExists();

			var l = System.Environment.TickCount;
			
			var task = table.CreateIfNotExistsAsync();

			task.Wait();

			//2015/11/27 遅い、、
			//処理時間=2625ms

			Debug.WriteLine("処理時間=" + (System.Environment.TickCount - l));

			//2016/12/17土 早くなっている？
			//処理時間=890ms

		}

		static int _n = 0;
		private void button1_Click(object sender, RoutedEventArgs e)
		{
			//エンティティをテーブルに１件追加
			var l = System.Environment.TickCount;

			var constr = new Properties.Settings().StorageConnectionString;
			CloudStorageAccount storageAccount = CloudStorageAccount.Parse(constr);

			// Create the table client.
			CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

			// Create the CloudTable object that represents the "people" table.
			CloudTable table = tableClient.GetTableReference("people");

			// Create a new customer entity.
			CustomerEntity customer1 = new CustomerEntity("Harp", "Walter" + _n);
			customer1.Email = "Walter@contoso.com";
			customer1.PhoneNumber = "425-555-0101";

			_n++;

			// Create the TableOperation object that inserts the customer entity.
			TableOperation insertOperation = TableOperation.Insert(customer1);

			// Execute the insert operation.
			table.Execute(insertOperation);

			Debug.WriteLine($"処理時間={(System.Environment.TickCount - l)}ms");
		}

		private void button2_Click(object sender, RoutedEventArgs e)
		{
			//パーティション内のすべてのエンティティをを取得する
			var l = System.Environment.TickCount;

			var constr = new Properties.Settings().StorageConnectionString;
			CloudStorageAccount storageAccount = CloudStorageAccount.Parse(constr);

			// Create the table client.
			CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

			// Create the CloudTable object that represents the "people" table.
			CloudTable table = tableClient.GetTableReference("people");

			// Construct the query operation for all customer entities where PartitionKey="Smith".
			TableQuery<CustomerEntity> query = new TableQuery<CustomerEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "kiyotaka"));

			// Print the fields for each customer.
			foreach (CustomerEntity entity in table.ExecuteQuery(query))
			{
				//Harp, Walter	Walter@contoso.com	425-555-0101
				Debug.WriteLine("{0}, {1}\t{2}\t{3}", entity.PartitionKey, entity.RowKey,
					entity.Email, entity.PhoneNumber);
			}

			//2016/12/17土
			//処理時間=421ms

			Debug.WriteLine($"処理時間={(System.Environment.TickCount - l)}ms");
		}

		private void button3_Click(object sender, RoutedEventArgs e)
		{
			//エンティティのバッチを挿入する

			var constr = new Properties.Settings().StorageConnectionString;
			CloudStorageAccount storageAccount = CloudStorageAccount.Parse(constr);

			// Create the table client.
			CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

			// Create the CloudTable object that represents the "people" table.
			CloudTable table = tableClient.GetTableReference("people");

			// Create the batch operation.
			TableBatchOperation batchOperation = new TableBatchOperation();

			var l = System.Environment.TickCount;

			for(var c = 0; c < 100; c++)
			{
				// Create a customer entity and add it to the table.
				CustomerEntity customer = new CustomerEntity("kiyotaka", "abe" + c);
				customer.Email = "Jeff@contoso.com" + c;
				customer.PhoneNumber = "425-555-0104" + c;

				batchOperation.Insert(customer);
			}

			// Execute the batch operation.
			table.ExecuteBatch(batchOperation);

			//2015/11/27
			//処理時間=1265ms

			//2016/12/17土
			//処理時間 = 313ms
			Debug.WriteLine($"処理時間={(System.Environment.TickCount - l)}ms");
		}

		private void button4_Click(object sender, RoutedEventArgs e)
		{
			var constr = new Properties.Settings().StorageConnectionString;
			CloudStorageAccount storageAccount = CloudStorageAccount.Parse(constr);

			// Create the table client.
			CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

			//テーブルストレージから、peopleの参照を取得する
			//tableClient.GetTableReference("people").CreateQuery<Wrok>();

			//var cache = new DataCache(a);





		}
	}

	public class Work
	{

	}

	public class CustomerEntity : TableEntity
	{
		public CustomerEntity(string lastName, string firstName)
		{
			//プライマリキー
			this.PartitionKey = lastName;
			this.RowKey = firstName;
		}

		public CustomerEntity() { }

		public string Email { get; set; }

		public string PhoneNumber { get; set; }
	}
}
