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

//https://azure.microsoft.com/ja-jp/documentation/articles/cache-dotnet-how-to-use-azure-redis-cache/
using StackExchange.Redis;

//256MBのキャッシュ作成について9:55かかった。約10分です。


namespace AzureRedisCache
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
			var l = System.Environment.TickCount;
			//ConnectionMultiplexer connection = ConnectionMultiplexer.Connect("contoso5.redis.cache.windows.net,abortConnect=false,ssl=true,password=...");
			// Connection refers to a property that returns a ConnectionMultiplexer
			// as shown in the previous example.
			IDatabase cache = Connection.GetDatabase();

			// Perform cache operations using the cache object...
			// Simple put of integral data types into the cache
			cache.StringSet("key1", "value");
			cache.StringSet("key2", 25);

			// Simple get of data types from the cache
			string key1 = cache.StringGet("key1");

			Console.WriteLine("key1:" + key1);

			int key2 = (int)cache.StringGet("key2");

			Console.WriteLine("key2:" + key2);

			//172ms		Azure上で実行すればもっと早いはず
			Console.WriteLine("time:" + (System.Environment.TickCount - l));
		}

		private void button1_Click(object sender, RoutedEventArgs e)
		{
			var l = System.Environment.TickCount;
			//ConnectionMultiplexer connection = ConnectionMultiplexer.Connect("contoso5.redis.cache.windows.net,abortConnect=false,ssl=true,password=...");
			// Connection refers to a property that returns a ConnectionMultiplexer
			// as shown in the previous example.
			IDatabase cache = Connection.GetDatabase();


			// Simple get of data types from the cache
			string key1 = cache.StringGet("key1");

			Console.WriteLine("key1:" + key1);

			int key2 = (int)cache.StringGet("key2");

			Console.WriteLine("key2:" + key2);

			//78ms
			Console.WriteLine("time:" + (System.Environment.TickCount - l));
		}


		private static Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
		{
			var connstr = new Properties.Settings().ConnectionString;

			return ConnectionMultiplexer.Connect(connstr);
		});

		public static ConnectionMultiplexer Connection
		{
			get
			{
				return lazyConnection.Value;
				/*
				System.ArgumentException はハンドルされませんでした。
				  HResult=-2147024809
				  Message=Keyword '' is not supported
				  Source=StackExchange.Redis
				  StackTrace:
					   場所 StackExchange.Redis.ConfigurationOptions.OptionKeys.Unknown(String key) 場所 c:\TeamCity\buildAgent\work\3ae0647004edff78\StackExchange.Redis\StackExchange\Redis\ConfigurationOptions.cs:行 68
					   場所 StackExchange.Redis.ConfigurationOptions.DoParse(String configuration, Boolean ignoreUnknown) 場所 c:\TeamCity\buildAgent\work\3ae0647004edff78\StackExchange.Redis\StackExchange\Redis\ConfigurationOptions.cs:行 595
					   場所 StackExchange.Redis.ConnectionMultiplexer.CreateMultiplexer(Object configuration) 場所 c:\TeamCity\buildAgent\work\3ae0647004edff78\StackExchange.Redis\StackExchange\Redis\ConnectionMultiplexer.cs:行 778
					   場所 StackExchange.Redis.ConnectionMultiplexer.<>c__DisplayClass27.<Connect>b__26() 場所 c:\TeamCity\buildAgent\work\3ae0647004edff78\StackExchange.Redis\StackExchange\Redis\ConnectionMultiplexer.cs:行 795
					   場所 StackExchange.Redis.ConnectionMultiplexer.ConnectImpl(Func`1 multiplexerFactory, TextWriter log) 場所 c:\TeamCity\buildAgent\work\3ae0647004edff78\StackExchange.Redis\StackExchange\Redis\ConnectionMultiplexer.cs:行 811
					   場所 StackExchange.Redis.ConnectionMultiplexer.Connect(String configuration, TextWriter log) 場所 c:\TeamCity\buildAgent\work\3ae0647004edff78\StackExchange.Redis\StackExchange\Redis\ConnectionMultiplexer.cs:行 795
					   場所 RedisCacheWpfApplication.MainWindow.<>c.<.cctor>b__12_0() 場所 D:\dev\sample\20151126_Azure_Redis_Cache\RedisCacheWpfApplication\RedisCacheWpfApplication\MainWindow.xaml.cs:行 83
					   場所 System.Lazy`1.CreateValue()
					   場所 System.Lazy`1.LazyInitValue()
					   場所 System.Lazy`1.get_Value()
					   場所 RedisCacheWpfApplication.MainWindow.get_Connection() 場所 D:\dev\sample\20151126_Azure_Redis_Cache\RedisCacheWpfApplication\RedisCacheWpfApplication\MainWindow.xaml.cs:行 90
					   場所 RedisCacheWpfApplication.MainWindow.button1_Click(Object sender, RoutedEventArgs e) 場所 D:\dev\sample\20151126_Azure_Redis_Cache\RedisCacheWpfApplication\RedisCacheWpfApplication\MainWindow.xaml.cs:行 65
					   場所 System.Windows.RoutedEventHandlerInfo.InvokeHandler(Object target, RoutedEventArgs routedEventArgs)
					   場所 System.Windows.EventRoute.InvokeHandlersImpl(Object source, RoutedEventArgs args, Boolean reRaised)
					   場所 System.Windows.UIElement.RaiseEventImpl(DependencyObject sender, RoutedEventArgs args)
					   場所 System.Windows.UIElement.RaiseEvent(RoutedEventArgs e)
					   場所 System.Windows.Controls.Primitives.ButtonBase.OnClick()
					   場所 System.Windows.Controls.Button.OnClick()
					   場所 System.Windows.Controls.Primitives.ButtonBase.OnMouseLeftButtonUp(MouseButtonEventArgs e)
					   場所 System.Windows.UIElement.OnMouseLeftButtonUpThunk(Object sender, MouseButtonEventArgs e)
					   場所 System.Windows.Input.MouseButtonEventArgs.InvokeEventHandler(Delegate genericHandler, Object genericTarget)
					   場所 System.Windows.RoutedEventArgs.InvokeHandler(Delegate handler, Object target)
					   場所 System.Windows.RoutedEventHandlerInfo.InvokeHandler(Object target, RoutedEventArgs routedEventArgs)
					   場所 System.Windows.EventRoute.InvokeHandlersImpl(Object source, RoutedEventArgs args, Boolean reRaised)
					   場所 System.Windows.UIElement.ReRaiseEventAs(DependencyObject sender, RoutedEventArgs args, RoutedEvent newEvent)
					   場所 System.Windows.UIElement.OnMouseUpThunk(Object sender, MouseButtonEventArgs e)
					   場所 System.Windows.Input.MouseButtonEventArgs.InvokeEventHandler(Delegate genericHandler, Object genericTarget)
					   場所 System.Windows.RoutedEventArgs.InvokeHandler(Delegate handler, Object target)
					   場所 System.Windows.RoutedEventHandlerInfo.InvokeHandler(Object target, RoutedEventArgs routedEventArgs)
					   場所 System.Windows.EventRoute.InvokeHandlersImpl(Object source, RoutedEventArgs args, Boolean reRaised)
					   場所 System.Windows.UIElement.RaiseEventImpl(DependencyObject sender, RoutedEventArgs args)
					   場所 System.Windows.UIElement.RaiseTrustedEvent(RoutedEventArgs args)
					   場所 System.Windows.Input.InputManager.ProcessStagingArea()
					   場所 System.Windows.Input.InputManager.ProcessInput(InputEventArgs input)
					   場所 System.Windows.Input.InputProviderSite.ReportInput(InputReport inputReport)
					   場所 System.Windows.Interop.HwndMouseInputProvider.ReportInput(IntPtr hwnd, InputMode mode, Int32 timestamp, RawMouseActions actions, Int32 x, Int32 y, Int32 wheel)
					   場所 System.Windows.Interop.HwndMouseInputProvider.FilterMessage(IntPtr hwnd, WindowMessage msg, IntPtr wParam, IntPtr lParam, Boolean& handled)
					   場所 System.Windows.Interop.HwndSource.InputFilterMessage(IntPtr hwnd, Int32 msg, IntPtr wParam, IntPtr lParam, Boolean& handled)
					   場所 MS.Win32.HwndWrapper.WndProc(IntPtr hwnd, Int32 msg, IntPtr wParam, IntPtr lParam, Boolean& handled)
					   場所 MS.Win32.HwndSubclass.DispatcherCallbackOperation(Object o)
					   場所 System.Windows.Threading.ExceptionWrapper.InternalRealCall(Delegate callback, Object args, Int32 numArgs)
					   場所 System.Windows.Threading.ExceptionWrapper.TryCatchWhen(Object source, Delegate callback, Object args, Int32 numArgs, Delegate catchHandler)
					   場所 System.Windows.Threading.Dispatcher.LegacyInvokeImpl(DispatcherPriority priority, TimeSpan timeout, Delegate method, Object args, Int32 numArgs)
					   場所 MS.Win32.HwndSubclass.SubclassWndProc(IntPtr hwnd, Int32 msg, IntPtr wParam, IntPtr lParam)
					   場所 MS.Win32.UnsafeNativeMethods.DispatchMessage(MSG& msg)
					   場所 System.Windows.Threading.Dispatcher.PushFrameImpl(DispatcherFrame frame)
					   場所 System.Windows.Threading.Dispatcher.PushFrame(DispatcherFrame frame)
					   場所 System.Windows.Application.RunDispatcher(Object ignore)
					   場所 System.Windows.Application.RunInternal(Window window)
					   場所 System.Windows.Application.Run(Window window)
					   場所 System.Windows.Application.Run()
					   場所 RedisCacheWpfApplication.App.Main() 場所 D:\dev\sample\20151126_Azure_Redis_Cache\RedisCacheWpfApplication\RedisCacheWpfApplication\obj\Debug\App.g.cs:行 0
					   場所 System.AppDomain._nExecuteAssembly(RuntimeAssembly assembly, String[] args)
					   場所 System.AppDomain.ExecuteAssembly(String assemblyFile, Evidence assemblySecurity, String[] args)
					   場所 Microsoft.VisualStudio.HostingProcess.HostProc.RunUsersAssembly()
					   場所 System.Threading.ThreadHelper.ThreadStart_Context(Object state)
					   場所 System.Threading.ExecutionContext.RunInternal(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
					   場所 System.Threading.ExecutionContext.Run(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
					   場所 System.Threading.ExecutionContext.Run(ExecutionContext executionContext, ContextCallback callback, Object state)
					   場所 System.Threading.ThreadHelper.ThreadStart()
				  InnerException: 

					*/
			}
		}
	}


}
