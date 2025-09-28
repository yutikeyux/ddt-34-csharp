using System;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading;

namespace NF
{
	public class TCPClient
	{
		private class Class15
		{
			internal static object aLbrTEgeYo2rI3AYne8;

			internal static void JFMlw3gVF3l5U4WGwKM()
			{
			}

			internal static bool cm7DCggMm2H6Nvu3LTL()
			{
				return aLbrTEgeYo2rI3AYne8 == null;
			}
		}

		public delegate void DelegateDataReceived(TCPClient client, byte[] bytes);

		public delegate void DelegateDataSend(TCPClient client, byte[] bytes);

		public delegate void DelegateDataReceivedComplete(TCPClient client, string message);

		public delegate void DelegateConnection(TCPClient client, string Info);

		public delegate void DelegateException(TCPClient client, Exception ex);

		public TcpClient Client;

		private NetworkStream networkStream_0;

		private byte[] byte_0;

		private string string_0;

		private int int_0;

		private bool bool_0 = false;

		private Timer timer_0;

		private int int_1 = 10;

		private Class15 class15_0 = new Class15();

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private DelegateDataReceived delegateDataReceived_0;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private DelegateDataSend delegateDataSend_0;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private DelegateConnection delegateConnection_0;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private DelegateConnection delegateConnection_1;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private DelegateException delegateException_0;

		private static TCPClient itodxtdFOopciKyypL;

		public int AutoConnectInterval
		{
			get
			{
				return int_1;
			}
			set
			{
				int_1 = value;
				if (value <= 0)
				{
					return;
				}
				try
				{
					if (timer_0 != null)
					{
						timer_0.Change(value * 1000, value * 1000);
					}
				}
				catch (Exception ex)
				{
					delegateException_0(this, ex);
				}
			}
		}

		public bool AutoConnect
		{
			get
			{
				return bool_0;
			}
			set
			{
				bool_0 = value;
				if (value)
				{
					method_0();
				}
			}
		}

		public bool IsRunning => timer_0 != null;

		public bool Connected
		{
			get
			{
				if (Client != null)
				{
					return Client.Connected;
				}
				return false;
			}
		}

		public event DelegateDataReceived DataReceived
		{
			[CompilerGenerated]
			add
			{
				DelegateDataReceived delegateDataReceived = delegateDataReceived_0;
				DelegateDataReceived delegateDataReceived2;
				do
				{
					delegateDataReceived2 = delegateDataReceived;
					DelegateDataReceived value2 = (DelegateDataReceived)Delegate.Combine(delegateDataReceived2, value);
					delegateDataReceived = Interlocked.CompareExchange(ref delegateDataReceived_0, value2, delegateDataReceived2);
				}
				while ((object)delegateDataReceived != delegateDataReceived2);
			}
			[CompilerGenerated]
			remove
			{
				DelegateDataReceived delegateDataReceived = delegateDataReceived_0;
				DelegateDataReceived delegateDataReceived2;
				do
				{
					delegateDataReceived2 = delegateDataReceived;
					DelegateDataReceived value2 = (DelegateDataReceived)Delegate.Remove(delegateDataReceived2, value);
					delegateDataReceived = Interlocked.CompareExchange(ref delegateDataReceived_0, value2, delegateDataReceived2);
				}
				while ((object)delegateDataReceived != delegateDataReceived2);
			}
		}

		public event DelegateDataSend DataSend
		{
			[CompilerGenerated]
			add
			{
				DelegateDataSend delegateDataSend = delegateDataSend_0;
				DelegateDataSend delegateDataSend2;
				do
				{
					delegateDataSend2 = delegateDataSend;
					DelegateDataSend value2 = (DelegateDataSend)Delegate.Combine(delegateDataSend2, value);
					delegateDataSend = Interlocked.CompareExchange(ref delegateDataSend_0, value2, delegateDataSend2);
				}
				while ((object)delegateDataSend != delegateDataSend2);
			}
			[CompilerGenerated]
			remove
			{
				DelegateDataSend delegateDataSend = delegateDataSend_0;
				DelegateDataSend delegateDataSend2;
				do
				{
					delegateDataSend2 = delegateDataSend;
					DelegateDataSend value2 = (DelegateDataSend)Delegate.Remove(delegateDataSend2, value);
					delegateDataSend = Interlocked.CompareExchange(ref delegateDataSend_0, value2, delegateDataSend2);
				}
				while ((object)delegateDataSend != delegateDataSend2);
			}
		}

		public event DelegateConnection ClientConnected
		{
			[CompilerGenerated]
			add
			{
				DelegateConnection delegateConnection = delegateConnection_0;
				DelegateConnection delegateConnection2;
				do
				{
					delegateConnection2 = delegateConnection;
					DelegateConnection value2 = (DelegateConnection)Delegate.Combine(delegateConnection2, value);
					delegateConnection = Interlocked.CompareExchange(ref delegateConnection_0, value2, delegateConnection2);
				}
				while ((object)delegateConnection != delegateConnection2);
			}
			[CompilerGenerated]
			remove
			{
				DelegateConnection delegateConnection = delegateConnection_0;
				DelegateConnection delegateConnection2;
				do
				{
					delegateConnection2 = delegateConnection;
					DelegateConnection value2 = (DelegateConnection)Delegate.Remove(delegateConnection2, value);
					delegateConnection = Interlocked.CompareExchange(ref delegateConnection_0, value2, delegateConnection2);
				}
				while ((object)delegateConnection != delegateConnection2);
			}
		}

		public event DelegateConnection ClientDisconnected
		{
			[CompilerGenerated]
			add
			{
				DelegateConnection delegateConnection = delegateConnection_1;
				DelegateConnection delegateConnection2;
				do
				{
					delegateConnection2 = delegateConnection;
					DelegateConnection value2 = (DelegateConnection)Delegate.Combine(delegateConnection2, value);
					delegateConnection = Interlocked.CompareExchange(ref delegateConnection_1, value2, delegateConnection2);
				}
				while ((object)delegateConnection != delegateConnection2);
			}
			[CompilerGenerated]
			remove
			{
				DelegateConnection delegateConnection = delegateConnection_1;
				DelegateConnection delegateConnection2;
				do
				{
					delegateConnection2 = delegateConnection;
					DelegateConnection value2 = (DelegateConnection)Delegate.Remove(delegateConnection2, value);
					delegateConnection = Interlocked.CompareExchange(ref delegateConnection_1, value2, delegateConnection2);
				}
				while ((object)delegateConnection != delegateConnection2);
			}
		}

		public event DelegateException ExceptionAppeared
		{
			[CompilerGenerated]
			add
			{
				DelegateException ex = delegateException_0;
				DelegateException ex2;
				do
				{
					ex2 = ex;
					DelegateException value2 = (DelegateException)Delegate.Combine(ex2, value);
					ex = Interlocked.CompareExchange(ref delegateException_0, value2, ex2);
				}
				while ((object)ex != ex2);
			}
			[CompilerGenerated]
			remove
			{
				DelegateException ex = delegateException_0;
				DelegateException ex2;
				do
				{
					ex2 = ex;
					DelegateException value2 = (DelegateException)Delegate.Remove(ex2, value);
					ex = Interlocked.CompareExchange(ref delegateException_0, value2, ex2);
				}
				while ((object)ex != ex2);
			}
		}

		public TCPClient(string server, int port)
		{
			string_0 = server;
			int_0 = port;
			ExceptionAppeared += method_6;
			ClientConnected += method_7;
			ClientDisconnected += method_8;
		}

		public override string ToString()
		{
			return $"{GetType()} {string_0}:{int_0}";
		}

		private void method_0()
		{
			if (bool_0 && timer_0 == null && int_1 > 0)
			{
				timer_0 = new Timer(method_5, null, int_1 * 1000, int_1 * 1000);
			}
		}

		public void Send(byte[] data)
		{
			try
			{
				networkStream_0.Write(data, 0, data.Length);
				if (delegateDataSend_0 != null)
				{
					delegateDataSend_0(this, data);
				}
			}
			catch (Exception ex)
			{
				delegateException_0(this, ex);
			}
		}

		private void method_1()
		{
			try
			{
				byte_0 = new byte[1024];
				networkStream_0.BeginRead(byte_0, 0, byte_0.Length, method_2, networkStream_0);
			}
			catch (Exception ex)
			{
				delegateException_0(this, ex);
			}
		}

		private void method_2(IAsyncResult iasyncResult_0)
		{
			try
			{
				NetworkStream networkStream = (NetworkStream)iasyncResult_0.AsyncState;
				if (!networkStream.CanRead)
				{
					return;
				}
				int num = networkStream.EndRead(iasyncResult_0);
				if (num <= 0)
				{
					if (delegateConnection_1 != null)
					{
						delegateConnection_1(this, "FIN");
					}
					if (bool_0)
					{
						method_3();
					}
					else
					{
						method_4();
					}
				}
				else
				{
					if (delegateDataReceived_0 != null)
					{
						byte[] array = new byte[num];
						Array.Copy(byte_0, 0, array, 0, num);
						delegateDataReceived_0(this, array);
					}
					networkStream.BeginRead(byte_0, 0, byte_0.Length, method_2, networkStream);
				}
			}
			catch (Exception ex)
			{
				delegateException_0(this, ex);
			}
		}

		public void ReConnect()
		{
			Disconnect();
			Connect();
		}

		public void Connect()
		{
			try
			{
				method_0();
				Client = new TcpClient(string_0, int_0);
				networkStream_0 = Client.GetStream();
				method_1();
				delegateConnection_0(this, $"server: {string_0} port: {int_0}");
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public void Ping()
		{
			Ping ping = new Ping();
			PingReply pingReply = ping.Send(string_0);
			if (pingReply.Status != 0)
			{
				throw new Exception($"Server {string_0} antwortet nicht auf Ping ");
			}
		}

		public void Ping(int waitTimeout)
		{
			Ping ping = new Ping();
			PingReply pingReply = ping.Send(string_0, waitTimeout);
			if (pingReply.Status != 0)
			{
				throw new Exception($"Server {string_0} antwortet nicht auf Ping ");
			}
		}

		public void Disconnect()
		{
			method_4();
			if (timer_0 != null)
			{
				timer_0.Dispose();
				timer_0 = null;
			}
			if (delegateConnection_1 != null)
			{
				delegateConnection_1(this, "Verbindung beendet");
			}
		}

		private void method_3()
		{
			method_4();
		}

		private void method_4()
		{
			if (Client != null)
			{
				Client.Close();
			}
			if (networkStream_0 != null)
			{
				networkStream_0.Close();
			}
		}

		private void method_5(object object_0)
		{
			try
			{
				lock (class15_0)
				{
					if (bool_0)
					{
						if (Client == null || !Client.Connected)
						{
							Client = new TcpClient(string_0, int_0);
							networkStream_0 = Client.GetStream();
							method_1();
							delegateConnection_0(this, $"server: {string_0} port: {int_0}");
						}
					}
					else if (timer_0 != null)
					{
						timer_0.Dispose();
						timer_0 = null;
					}
				}
			}
			catch (Exception ex)
			{
				delegateException_0(this, ex);
			}
		}

		private void method_6(TCPClient tcpclient_0, Exception exception_0)
		{
		}

		private void method_7(TCPClient tcpclient_0, string string_1)
		{
		}

		private void method_8(TCPClient tcpclient_0, string string_1)
		{
		}

		internal static void N1PqAJ0kf6bHo5xxQ3()
		{
		}

		internal static bool qV1S4ji7FtUvM5jJZj()
		{
			return itodxtdFOopciKyypL == null;
		}
	}
}
