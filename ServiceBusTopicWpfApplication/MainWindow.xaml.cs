using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
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

namespace ServiceBusTopicWpfApplication
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

			if (!namespaceManager.TopicExists("TestTopic"))
			{
				namespaceManager.CreateTopic("TestTopic");
			}
		}

		private void button1_Click(object sender, RoutedEventArgs e)
		{
			// Configure Topic Settings.
			TopicDescription td = new TopicDescription("TestTopic2");
			td.MaxSizeInMegabytes = 5120;
			td.DefaultMessageTimeToLive = new TimeSpan(0, 1, 0);

			string connectionString = new Properties.Settings().ConnectionString;


			var namespaceManager =
				NamespaceManager.CreateFromConnectionString(connectionString);

			if (!namespaceManager.TopicExists("TestTopic2"))
			{
				namespaceManager.CreateTopic(td);
			}
		}
	}
}
