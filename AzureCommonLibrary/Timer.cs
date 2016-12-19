using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureCommonLibrary
{
    public class Timer : IDisposable
    {
		long _start;
		public Timer()
		{
			_start = Environment.TickCount;
		}

		//これはデストラクタで呼び出されるが、タイミングが不規則
		//~Timer()
		//{
		//	Debug.WriteLine($"処理時間={(System.Environment.TickCount - _start)}ms");
		//}

		public void Dispose()
		{
			Debug.WriteLine($"処理時間={(System.Environment.TickCount - _start)}ms");
		}
	}
}
