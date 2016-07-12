using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace ServiceBusBrokeredMessageWpfApplication
{
	/// <summary>
	/// MainWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class MainWindow : Window
	{

		private static DataTable issues;
		private static List<BrokeredMessage> MessageList;
		public MainWindow()
		{
			InitializeComponent();
		}


		static DataTable ParseCSVFile()
		{
			DataTable tableIssues = new DataTable("Issues");
			string path = @"..\..\data.csv";
			try
			{
				using (StreamReader readFile = new StreamReader(path))
				{
					string line;
					string[] row;

					// create the columns
					line = readFile.ReadLine();
					foreach (string columnTitle in line.Split(','))
					{
						tableIssues.Columns.Add(columnTitle);
					}

					while ((line = readFile.ReadLine()) != null)
					{
						row = line.Split(',');
						tableIssues.Rows.Add(row);
					}
				}
			}
			catch (Exception e)
			{
				Console.WriteLine("Error:" + e.ToString());
			}

			return tableIssues;
		}

		static List<BrokeredMessage> GenerateMessages(DataTable issues)
		{
			// Instantiate the brokered list object
			List<BrokeredMessage> result = new List<BrokeredMessage>();

			// Iterate through the table and create a brokered message for each rowforeach (DataRow item in issues.Rows)
			{
				BrokeredMessage message = new BrokeredMessage();
				foreach (DataColumn property in issues.Columns)
				{
				//	message.Properties.Add(property.ColumnName, issues.Rows[property.]);
				}
				result.Add(message);
			}
			return result;
		}

		private void button_Click(object sender, RoutedEventArgs e)
		{
			issues = ParseCSVFile();
			MessageList = GenerateMessages(issues);
		}


	}
}
