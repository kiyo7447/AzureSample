using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureCommonLibrary
{
	public static class SettingsInfo
	{
		static SettingsInfo()
		{
			AzureStorageName = Environment.GetEnvironmentVariable("AURE_STORAGE_ACCOUNT_NAME"); ;
			AzureStorageKey = Environment.GetEnvironmentVariable("AURE_STORAGE_ACCOUNT_KEY"); ;
		}

		public static string AzureStorageName { get; }
		public static string AzureStorageKey { get; }

		public static string AzureStorageConnectionString
		{
			get
			{
				return $"DefaultEndpointsProtocol=https;AccountName={AzureStorageName};AccountKey={AzureStorageKey}";
			}
		}
	}
}
