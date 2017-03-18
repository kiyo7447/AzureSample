using AzureCommonLibrary;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
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

namespace AzureDocumentDB
{
	/// <summary>
	/// MainWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class MainWindow : Window
	{
		DocumentClient _client;
		string _databaseName = "abedatabase";
		string _collectionName = "abecollection";

		public MainWindow()
		{
			InitializeComponent();
		}
		private DocumentClient GetDocumentClient()
		{
			try
			{
				Properties.Settings settings = new Properties.Settings();
				this._client = new DocumentClient(new Uri(settings.EndpointUri), settings.PrimaryKey);
				//await this._client.OpenAsync();
				return _client;
			}
			catch (DocumentClientException de)
			{
				Exception baseException = de.GetBaseException();
				MessageBox.Show(String.Format("{0} error occurred: {1}, Message: {2}", de.StatusCode, de.Message, baseException.Message));
				throw;
			}
			catch (Exception ex)
			{
				Exception baseException = ex.GetBaseException();
				MessageBox.Show(String.Format("Error: {0}, Message: {1}", ex.Message, baseException.Message));
				throw;
			}
			finally
			{
				Debug.WriteLine("コネクションの確立に成功しました。");
			}
		}

		private async Task CreateDatabaseIfNotExists(string databaseName)
		{
			try
			{
				await this._client.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(databaseName));
				Debug.WriteLine($"データベース {databaseName} は既に存在します。");
			}
			catch (DocumentClientException de)
			{
				// If the database does not exist, create a new database
				if (de.StatusCode == HttpStatusCode.NotFound)
				{
					await this._client.CreateDatabaseAsync(new Database { Id = databaseName });
					Debug.WriteLine($"データベース {databaseName} を作成しました。");
				}
				else
				{
					throw;
				}
			}
		}

		private async Task CreateDocumentCollectionIfNotExists(string databaseName, string collectionName)
		{
			try
			{
				await this._client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(databaseName, collectionName));
				Debug.WriteLine($"コレクション {collectionName} は既に存在します。");
			}
			catch (DocumentClientException de)
			{
				// If the document collection does not exist, create a new collection
				if (de.StatusCode == HttpStatusCode.NotFound)
				{
					DocumentCollection collectionInfo = new DocumentCollection();
					collectionInfo.Id = collectionName;

					// Configure collections for maximum query flexibility including string range queries.
					collectionInfo.IndexingPolicy = new IndexingPolicy(new RangeIndex(DataType.String) { Precision = -1 });

					// Here we create a collection with 400 RU/s.
					await this._client.CreateDocumentCollectionAsync(
						UriFactory.CreateDatabaseUri(databaseName),
						collectionInfo,
						new RequestOptions { OfferThroughput = 400 });

					Debug.WriteLine($"コレクション {collectionName} を作成しました。");
				}
				else
				{
					throw;
				}
			}
		}

		private async Task CreateFamilyDocumentIfNotExists(string databaseName, string collectionName, Family family)
		{
			try
			{
				await this._client.ReadDocumentAsync(UriFactory.CreateDocumentUri(databaseName, collectionName, family.Id));
				//this.WriteToConsoleAndPromptToContinue("Found {0}", family.Id);
				Debug.WriteLine($"Family.ID {family.Id} は既に存在します。");
			}
			catch (DocumentClientException de)
			{
				if (de.StatusCode == HttpStatusCode.NotFound)
				{
					await this._client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(databaseName, collectionName), family);
					//this.WriteToConsoleAndPromptToContinue("Created Family {0}", family.Id);
					Debug.WriteLine($"Family.ID {family.Id} を作成しました。");
				}
				else
				{
					//throw;
				}
			}
		}


		private void _buttonConnection_Click(object sender, RoutedEventArgs e)
		{
			using (var s = new Timer())
			{
				_client =  GetDocumentClient();
			}
			//処理時間 813ms

		}
		private async void _buttonCreateDB_Click(object sender, RoutedEventArgs e)
		{
			await CreateDatabaseIfNotExists("abedatabase");
		}

		private async void _buttonCreateCollection_Click(object sender, RoutedEventArgs e)
		{
			await CreateDocumentCollectionIfNotExists(databaseName: "abedatabase", collectionName: "abecollection");
		}

		private async void _buttonAddJson_Click(object sender, RoutedEventArgs e)
		{
			if (_client == null) _client = GetDocumentClient();

			Family andersenFamily = new Family
			{
				Id = "Andersen.1",
				LastName = "Andersen",
				Parents = new Parent[]
					{
				new Parent { FirstName = "Thomas" },
				new Parent { FirstName = "Mary Kay" }
					},
				Children = new Child[]
					{
				new Child
				{
						FirstName = "Henriette Thaulow",
						Gender = "female",
						Grade = 5,
						Pets = new Pet[]
						{
								new Pet { GivenName = "Fluffy" }
						}
				}
					},
				Address = new Address { State = "WA", County = "King", City = "Seattle" },
				IsRegistered = true
			};

			await this.CreateFamilyDocumentIfNotExists(_databaseName, _collectionName, andersenFamily);

			Family wakefieldFamily = new Family
			{
				Id = "Wakefield.7",
				LastName = "Wakefield",
				Parents = new Parent[]
					{
				new Parent { FamilyName = "Wakefield", FirstName = "Robin" },
				new Parent { FamilyName = "Miller", FirstName = "Ben" }
					},
				Children = new Child[]
					{
				new Child
				{
						FamilyName = "Merriam",
						FirstName = "Jesse",
						Gender = "female",
						Grade = 8,
						Pets = new Pet[]
						{
								new Pet { GivenName = "Goofy" },
								new Pet { GivenName = "Shadow" }
						}
				},
				new Child
				{
						FamilyName = "Miller",
						FirstName = "Lisa",
						Gender = "female",
						Grade = 1
				}
					},
				Address = new Address { State = "NY", County = "Manhattan", City = "NY" },
				IsRegistered = false
			};

			await this.CreateFamilyDocumentIfNotExists(_databaseName, _collectionName, wakefieldFamily);
		}


		private void WriteToConsoleAndPromptToContinue(string format, params object[] args)
		{
			Console.WriteLine(format, args);
			Console.WriteLine("Press any key to continue ...");
			Console.ReadKey();
		}

		public class Family
		{
			[JsonProperty(PropertyName = "id")]
			public string Id { get; set; }
			public string LastName { get; set; }
			public Parent[] Parents { get; set; }
			public Child[] Children { get; set; }
			public Address Address { get; set; }
			public bool IsRegistered { get; set; }
			public override string ToString()
			{
				return JsonConvert.SerializeObject(this);
			}
		}

		public class Parent
		{
			public string FamilyName { get; set; }
			public string FirstName { get; set; }
		}

		public class Child
		{
			public string FamilyName { get; set; }
			public string FirstName { get; set; }
			public string Gender { get; set; }
			public int Grade { get; set; }
			public Pet[] Pets { get; set; }
		}

		public class Pet
		{
			public string GivenName { get; set; }
		}

		public class Address
		{
			public string State { get; set; }
			public string County { get; set; }
			public string City { get; set; }
		}

		private void _buttonDocumentDBリソースをクエリする_Click(object sender, RoutedEventArgs e)
		{
			if (_client == null) _client = GetDocumentClient();

			// Set some common query options
			FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

			// Here we find the Andersen family via its LastName
			IQueryable<Family> familyQuery = this._client.CreateDocumentQuery<Family>(
					UriFactory.CreateDocumentCollectionUri(_databaseName, _collectionName), queryOptions)
					.Where(f => f.LastName == "Andersen");

			// The query is executed synchronously here, but can also be executed asynchronously via the IDocumentQuery<T> interface
			Debug.WriteLine("Running LINQ query...");
			foreach (Family family in familyQuery)
			{
				Debug.WriteLine("\tRead {0}", family);
			}

			// Now execute the same query via direct SQL
			IQueryable<Family> familyQueryInSql = this._client.CreateDocumentQuery<Family>(
					UriFactory.CreateDocumentCollectionUri(_databaseName, _collectionName),
					"SELECT * FROM Family WHERE Family.LastName = 'Andersen'",
					queryOptions);

			Debug.WriteLine("Running direct SQL query...");
			foreach (Family family in familyQueryInSql)
			{
				Debug.WriteLine("Running direct SQL query...");
				Debug.WriteLine("\tRead {0}", family);
			}

			Debug.WriteLine("Running direct SQL query...");


		}

		private async void _buttonAddJson100000_Click(object sender, RoutedEventArgs e)
		{
			if (_client == null) _client = GetDocumentClient();

			using (var timer = new Timer())
			{
				var maxCount = 10000;
				for (int idx = 5800; idx <= maxCount; idx++)
				{
					Family wakefieldFamily = new Family
					{
						Id = "Wakefield." + idx,
						LastName = "Wakefield",
						Parents = new Parent[]
							{
				new Parent { FamilyName = "Wakefield", FirstName = "Robin" },
				new Parent { FamilyName = "Miller", FirstName = "Ben" }
							},
						Children = new Child[]
							{
				new Child
				{
						FamilyName = "Merriam",
						FirstName = "Jesse",
						Gender = "female",
						Grade = 8,
						Pets = new Pet[]
						{
								new Pet { GivenName = "Goofy" },
								new Pet { GivenName = "Shadow" }
						}
				},
				new Child
				{
						FamilyName = "Miller",
						FirstName = "Lisa",
						Gender = "female",
						Grade = 1
				}
							},
						Address = new Address { State = "NY", County = "Manhattan", City = "NY" },
						IsRegistered = false
					};

					await this.CreateFamilyDocumentIfNotExists(_databaseName, _collectionName, wakefieldFamily);
					if ((idx % 1000) == 0)
					{
						Debug.WriteLine($"処理進捗：{idx}／{maxCount}、経過時間；{timer.GetTimePassed()}ms");
					}
				}
			}
			//処理時間
		}

		private void _buttonDocumentDBリソースをクエリする１件_10万件_Click(object sender, RoutedEventArgs e)
		{
			if (_client == null) _client = GetDocumentClient();

		}
	}
}
