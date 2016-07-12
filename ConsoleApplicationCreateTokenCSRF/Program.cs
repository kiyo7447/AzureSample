using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplicationCreateToken
{
	class Program
	{
		static void Main(string[] args)
		{

			var token = Csrf.GetCsrfToken();
			Console.Out.WriteLine($"toke={token}");

			Console.In.ReadLine();
		}
	}


	public class Csrf
	{
		//CSRF の安全なトークンの作成方法
		//https://www.websec-room.com/2013/03/05/451

		private static int TOKEN_LENGTH = 16; //16*2=32バイト

		//32バイトのCSRFトークンを作成
		public static string GetCsrfToken()
		{
			byte[] token = new byte[TOKEN_LENGTH];

			RNGCryptoServiceProvider gen = new RNGCryptoServiceProvider();
			gen.GetBytes(token);

			StringBuilder buf = new StringBuilder();

			for (int i = 0; i < token.Length; i++)
			{
				buf.AppendFormat("{0:x2}", token[i]);
			}

			return buf.ToString();
		}
	}
}
