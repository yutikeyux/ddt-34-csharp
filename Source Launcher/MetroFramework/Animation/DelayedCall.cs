using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Timers;

namespace MetroFramework.Animation
{
	internal class DelayedCall : IDisposable
	{
		public delegate void Callback();

		protected static List<DelayedCall> dcList;

		protected System.Timers.Timer timer;

		protected object timerLock;

		private Callback callback_0;

		protected bool cancelled;

		protected SynchronizationContext context;

		private DelayedCall<object>.Callback callback_1;

		private object object_0;

		private static DelayedCall lZx3mONR9jhWsW10Eat;

		public static int RegisteredCount
		{
			get
			{
				lock (dcList)
				{
					return dcList.Count;
				}
			}
		}

		public static bool IsAnyWaiting
		{
			get
			{
				lock (dcList)
				{
					foreach (DelayedCall dc in dcList)
					{
						if (dc.IsWaiting)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		public bool IsWaiting
		{
			get
			{
				lock (timerLock)
				{
					return timer.Enabled && !cancelled;
				}
			}
		}

		public int Milliseconds
		{
			get
			{
				lock (timerLock)
				{
					return (int)timer.Interval;
				}
			}
			set
			{
				lock (timerLock)
				{
					if (value < 0)
					{
						throw new ArgumentOutOfRangeException("Milliseconds", "The new timeout must be 0 or greater.");
					}
					if (value != 0)
					{
						timer.Interval = value;
						return;
					}
					Cancel();
					FireNow();
					Unregister(this);
				}
			}
		}

		static DelayedCall()
		{
			dcList = new List<DelayedCall>();
		}

		protected DelayedCall()
		{
			timerLock = new object();
		}

		[Obsolete("Use the static method DelayedCall.Create instead.")]
		public DelayedCall(Callback cb)
			: this()
		{
			PrepareDCObject(this, 0, async: false);
			callback_0 = cb;
		}

		[Obsolete("Use the static method DelayedCall.Create instead.")]
		public DelayedCall(DelayedCall<object>.Callback cb, object data)
			: this()
		{
			PrepareDCObject(this, 0, async: false);
			callback_1 = cb;
			object_0 = data;
		}

		[Obsolete("Use the static method DelayedCall.Start instead.")]
		public DelayedCall(Callback cb, int milliseconds)
			: this()
		{
			PrepareDCObject(this, milliseconds, async: false);
			callback_0 = cb;
			if (milliseconds > 0)
			{
				Start();
			}
		}

		[Obsolete("Use the static method DelayedCall.Start instead.")]
		public DelayedCall(DelayedCall<object>.Callback cb, int milliseconds, object data)
			: this()
		{
			PrepareDCObject(this, milliseconds, async: false);
			callback_1 = cb;
			object_0 = data;
			if (milliseconds > 0)
			{
				Start();
			}
		}

		[Obsolete("Use the method Restart of the generic class instead.")]
		public void Reset(object data)
		{
			Cancel();
			object_0 = data;
			Start();
		}

		[Obsolete("Use the method Restart of the generic class instead.")]
		public void Reset(int milliseconds, object data)
		{
			Cancel();
			object_0 = data;
			Reset(milliseconds);
		}

		[Obsolete("Use the method Restart instead.")]
		public void SetTimeout(int milliseconds)
		{
			Reset(milliseconds);
		}

		public static DelayedCall Create(Callback cb, int milliseconds)
		{
			DelayedCall delayedCall = new DelayedCall();
			PrepareDCObject(delayedCall, milliseconds, async: false);
			delayedCall.callback_0 = cb;
			return delayedCall;
		}

		public static DelayedCall CreateAsync(Callback cb, int milliseconds)
		{
			DelayedCall delayedCall = new DelayedCall();
			PrepareDCObject(delayedCall, milliseconds, async: true);
			delayedCall.callback_0 = cb;
			return delayedCall;
		}

		public static DelayedCall Start(Callback cb, int milliseconds)
		{
			DelayedCall delayedCall = Create(cb, milliseconds);
			if (milliseconds > 0)
			{
				delayedCall.Start();
			}
			else if (milliseconds == 0)
			{
				delayedCall.FireNow();
			}
			return delayedCall;
		}

		public static DelayedCall StartAsync(Callback cb, int milliseconds)
		{
			DelayedCall delayedCall = CreateAsync(cb, milliseconds);
			if (milliseconds > 0)
			{
				delayedCall.Start();
			}
			else if (milliseconds == 0)
			{
				delayedCall.FireNow();
			}
			return delayedCall;
		}

		protected static void PrepareDCObject(DelayedCall dc, int milliseconds, bool async)
		{
			if (milliseconds < 0)
			{
				throw new ArgumentOutOfRangeException("milliseconds", "The new timeout must be 0 or greater.");
			}
			dc.context = null;
			if (!async)
			{
				dc.context = SynchronizationContext.Current;
				if (dc.context == null)
				{
					throw new InvalidOperationException("Cannot delay calls synchronously on a non-UI thread. Use the *Async methods instead.");
				}
			}
			if (dc.context == null)
			{
				dc.context = new SynchronizationContext();
			}
			dc.timer = new System.Timers.Timer();
			if (milliseconds > 0)
			{
				dc.timer.Interval = milliseconds;
			}
			dc.timer.AutoReset = false;
			dc.timer.Elapsed += dc.Timer_Elapsed;
			Register(dc);
		}

		protected static void Register(DelayedCall dc)
		{
			lock (dcList)
			{
				if (!dcList.Contains(dc))
				{
					dcList.Add(dc);
				}
			}
		}

		protected static void Unregister(DelayedCall dc)
		{
			lock (dcList)
			{
				dcList.Remove(dc);
			}
		}

		public static void CancelAll()
		{
			lock (dcList)
			{
				foreach (DelayedCall dc in dcList)
				{
					dc.Cancel();
				}
			}
		}

		public static void FireAll()
		{
			lock (dcList)
			{
				foreach (DelayedCall dc in dcList)
				{
					dc.Fire();
				}
			}
		}

		public static void DisposeAll()
		{
			lock (dcList)
			{
				while (dcList.Count > 0)
				{
					dcList[0].Dispose();
				}
			}
		}

		protected virtual void Timer_Elapsed(object sender, ElapsedEventArgs e)
		{
			FireNow();
			Unregister(this);
		}

		public void Dispose()
		{
			Unregister(this);
			timer.Dispose();
		}

		public void Start()
		{
			lock (timerLock)
			{
				cancelled = false;
				timer.Start();
				Register(this);
			}
		}

		public void Cancel()
		{
			lock (timerLock)
			{
				cancelled = true;
				Unregister(this);
				timer.Stop();
			}
		}

		public void Fire()
		{
			lock (timerLock)
			{
				if (!IsWaiting)
				{
					return;
				}
				timer.Stop();
			}
			FireNow();
		}

		public void FireNow()
		{
			OnFire();
			Unregister(this);
		}

		protected virtual void OnFire()
		{
			context.Post(delegate
			{
				lock (timerLock)
				{
					if (cancelled)
					{
						return;
					}
				}
				if (callback_0 != null)
				{
					callback_0();
				}
				if (callback_1 != null)
				{
					callback_1(object_0);
				}
			}, null);
		}

		public void Reset()
		{
			lock (timerLock)
			{
				Cancel();
				Start();
			}
		}

		public void Reset(int milliseconds)
		{
			lock (timerLock)
			{
				Cancel();
				Milliseconds = milliseconds;
				Start();
			}
		}

		[CompilerGenerated]
		private void method_0(object object_1)
		{
			lock (timerLock)
			{
				if (cancelled)
				{
					return;
				}
			}
			if (callback_0 != null)
			{
				callback_0();
			}
			if (callback_1 != null)
			{
				callback_1(object_0);
			}
		}

		internal static void L52yOVNjSnQoFw2pBSq()
		{
		}

		internal static bool n1XkadNAShBRUmvvSCU()
		{
			return lZx3mONR9jhWsW10Eat == null;
		}
	}
	internal class DelayedCall<T> : DelayedCall
	{
		public new delegate void Callback(T data);

		private Callback callback_2;

		private T gparam_0;

		private static object cmLh6QXOyIFf56tbZtC;

		public static DelayedCall<T> Create(Callback cb, T data, int milliseconds)
		{
			DelayedCall<T> delayedCall = new DelayedCall<T>();
			DelayedCall.PrepareDCObject(delayedCall, milliseconds, async: false);
			delayedCall.callback_2 = cb;
			delayedCall.gparam_0 = data;
			return delayedCall;
		}

		public static DelayedCall<T> CreateAsync(Callback cb, T data, int milliseconds)
		{
			DelayedCall<T> delayedCall = new DelayedCall<T>();
			DelayedCall.PrepareDCObject(delayedCall, milliseconds, async: true);
			delayedCall.callback_2 = cb;
			delayedCall.gparam_0 = data;
			return delayedCall;
		}

		public static DelayedCall<T> Start(Callback cb, T data, int milliseconds)
		{
			DelayedCall<T> delayedCall = Create(cb, data, milliseconds);
			delayedCall.Start();
			return delayedCall;
		}

		public static DelayedCall<T> StartAsync(Callback cb, T data, int milliseconds)
		{
			DelayedCall<T> delayedCall = CreateAsync(cb, data, milliseconds);
			delayedCall.Start();
			return delayedCall;
		}

		protected override void OnFire()
		{
			int num = 1;
			while (true)
			{
				context.Post(delegate
				{
					int num3 = 5;
					int num6 = default(int);
					while (true)
					{
						object obj;
						Monitor.Enter(obj = timerLock);
						int num4 = 4;
						if (voZmRPXUctVEJbLN5p7() != null)
						{
							goto IL_006a;
						}
						goto IL_008a;
						IL_008a:
						while (true)
						{
							switch (num4)
							{
							case 4:
								try
								{
									if (cancelled)
									{
										if (voZmRPXUctVEJbLN5p7() == null)
										{
											return;
										}
										switch (0)
										{
										default:
											return;
										case 1:
											break;
										}
									}
								}
								finally
								{
									Monitor.Exit(obj);
									int num5 = 0;
									if (voZmRPXUctVEJbLN5p7() != null)
									{
										num5 = num6;
									}
									switch (num5)
									{
									}
								}
								goto case 1;
							case 1:
								if (callback_2 == null)
								{
									goto IL_005d;
								}
								goto case 2;
							case 2:
								callback_2(gparam_0);
								num4 = 0;
								if (!kOgMyQXfi2ygJZIruff())
								{
									continue;
								}
								return;
							default:
								return;
							case 5:
								break;
							case 0:
							case 3:
								return;
							}
							break;
							IL_005d:
							num4 = 3;
							if (voZmRPXUctVEJbLN5p7() == null)
							{
								continue;
							}
							goto IL_006a;
						}
						continue;
						IL_006a:
						num4 = num3;
						goto IL_008a;
					}
				}, null);
				int num2 = 0;
				if (voZmRPXUctVEJbLN5p7() != null)
				{
					num2 = num;
				}
				switch (num2)
				{
				case 1:
					break;
				default:
					return;
				case 0:
					return;
				}
			}
		}

		public void Reset(T data, int milliseconds)
		{
			lock (timerLock)
			{
				Cancel();
				gparam_0 = data;
				base.Milliseconds = milliseconds;
				Start();
			}
		}

		[CompilerGenerated]
		private void method_1(object object_1)
		{
			int num = 5;
			int num4 = default(int);
			while (true)
			{
				object obj;
				Monitor.Enter(obj = timerLock);
				int num2 = 4;
				if (voZmRPXUctVEJbLN5p7() != null)
				{
					goto IL_006a;
				}
				goto IL_008a;
				IL_008a:
				while (true)
				{
					switch (num2)
					{
					case 4:
						try
						{
							if (cancelled)
							{
								if (voZmRPXUctVEJbLN5p7() == null)
								{
									return;
								}
								switch (0)
								{
								default:
									return;
								case 1:
									break;
								}
							}
						}
						finally
						{
							Monitor.Exit(obj);
							int num3 = 0;
							if (voZmRPXUctVEJbLN5p7() != null)
							{
								num3 = num4;
							}
							switch (num3)
							{
							}
						}
						goto case 1;
					case 1:
						if (callback_2 == null)
						{
							goto IL_005d;
						}
						goto case 2;
					case 2:
						callback_2(gparam_0);
						num2 = 0;
						if (!kOgMyQXfi2ygJZIruff())
						{
							continue;
						}
						return;
					default:
						return;
					case 5:
						break;
					case 0:
					case 3:
						return;
					}
					break;
					IL_005d:
					num2 = 3;
					if (voZmRPXUctVEJbLN5p7() == null)
					{
						continue;
					}
					goto IL_006a;
				}
				continue;
				IL_006a:
				num2 = num;
				goto IL_008a;
			}
		}

		internal static bool kOgMyQXfi2ygJZIruff()
		{
			return cmLh6QXOyIFf56tbZtC == null;
		}

		internal static object voZmRPXUctVEJbLN5p7()
		{
			return cmLh6QXOyIFf56tbZtC;
		}
	}
	internal class DelayedCall<T1, T2> : DelayedCall
	{
		public new delegate void Callback(T1 data1, T2 data2);

		private Callback callback_2;

		private T1 gparam_0;

		private T2 gparam_1;

		internal static object yO9DjpXGNh5jKQGb6gF;

		public static DelayedCall<T1, T2> Create(Callback cb, T1 data1, T2 data2, int milliseconds)
		{
			DelayedCall<T1, T2> delayedCall = new DelayedCall<T1, T2>();
			DelayedCall.PrepareDCObject(delayedCall, milliseconds, async: false);
			delayedCall.callback_2 = cb;
			delayedCall.gparam_0 = data1;
			delayedCall.gparam_1 = data2;
			return delayedCall;
		}

		public static DelayedCall<T1, T2> CreateAsync(Callback cb, T1 data1, T2 data2, int milliseconds)
		{
			DelayedCall<T1, T2> delayedCall = new DelayedCall<T1, T2>();
			DelayedCall.PrepareDCObject(delayedCall, milliseconds, async: true);
			delayedCall.callback_2 = cb;
			delayedCall.gparam_0 = data1;
			delayedCall.gparam_1 = data2;
			return delayedCall;
		}

		public static DelayedCall<T1, T2> Start(Callback cb, T1 data1, T2 data2, int milliseconds)
		{
			DelayedCall<T1, T2> delayedCall = Create(cb, data1, data2, milliseconds);
			delayedCall.Start();
			return delayedCall;
		}

		public static DelayedCall<T1, T2> StartAsync(Callback cb, T1 data1, T2 data2, int milliseconds)
		{
			DelayedCall<T1, T2> delayedCall = CreateAsync(cb, data1, data2, milliseconds);
			delayedCall.Start();
			return delayedCall;
		}

		protected override void OnFire()
		{
			int num = 1;
			while (true)
			{
				context.Post(delegate
				{
					int num3 = 3;
					int num5 = default(int);
					while (true)
					{
						IL_00b8:
						object obj;
						Monitor.Enter(obj = timerLock);
						while (true)
						{
							try
							{
								if (cancelled)
								{
									return;
								}
								int num4 = 2;
								if (!qQOfwHXmLAOQV4iycYQ())
								{
									num4 = num5;
								}
								switch (num4)
								{
								default:
									return;
								case 1:
								case 2:
									break;
								case 0:
									return;
								}
							}
							finally
							{
								Monitor.Exit(obj);
								if (yJBeeLXgHPS5rw64IhH() == null)
								{
									switch (0)
									{
									}
								}
							}
							while (true)
							{
								IL_00a8:
								if (callback_2 != null)
								{
									int num6 = 0;
									if (yJBeeLXgHPS5rw64IhH() != null)
									{
										num6 = num3;
									}
									while (true)
									{
										switch (num6)
										{
										case 2:
											break;
										default:
											callback_2(gparam_0, gparam_1);
											num6 = 1;
											if (qQOfwHXmLAOQV4iycYQ())
											{
												continue;
											}
											return;
										case 4:
											goto IL_00a8;
										case 3:
											goto IL_00b8;
										case 1:
											return;
										}
										break;
									}
									break;
								}
								return;
							}
						}
					}
				}, null);
				int num2 = 0;
				if (!qQOfwHXmLAOQV4iycYQ())
				{
					num2 = num;
				}
				switch (num2)
				{
				case 1:
					break;
				default:
					return;
				case 0:
					return;
				}
			}
		}

		public void Reset(T1 data1, T2 data2, int milliseconds)
		{
			lock (timerLock)
			{
				Cancel();
				gparam_0 = data1;
				gparam_1 = data2;
				base.Milliseconds = milliseconds;
				Start();
			}
		}

		[CompilerGenerated]
		private void method_1(object object_1)
		{
			int num = 3;
			int num3 = default(int);
			while (true)
			{
				IL_00b8:
				object obj;
				Monitor.Enter(obj = timerLock);
				while (true)
				{
					try
					{
						if (cancelled)
						{
							return;
						}
						int num2 = 2;
						if (!qQOfwHXmLAOQV4iycYQ())
						{
							num2 = num3;
						}
						switch (num2)
						{
						default:
							return;
						case 1:
						case 2:
							break;
						case 0:
							return;
						}
					}
					finally
					{
						Monitor.Exit(obj);
						if (yJBeeLXgHPS5rw64IhH() == null)
						{
							switch (0)
							{
							}
						}
					}
					while (true)
					{
						IL_00a8:
						if (callback_2 == null)
						{
							return;
						}
						int num4 = 0;
						if (yJBeeLXgHPS5rw64IhH() != null)
						{
							num4 = num;
						}
						while (true)
						{
							switch (num4)
							{
							case 2:
								break;
							default:
								callback_2(gparam_0, gparam_1);
								num4 = 1;
								if (qQOfwHXmLAOQV4iycYQ())
								{
									continue;
								}
								return;
							case 4:
								goto IL_00a8;
							case 3:
								goto IL_00b8;
							case 1:
								return;
							}
							break;
						}
						break;
					}
				}
			}
		}

		internal static bool qQOfwHXmLAOQV4iycYQ()
		{
			return yO9DjpXGNh5jKQGb6gF == null;
		}

		internal static object yJBeeLXgHPS5rw64IhH()
		{
			return yO9DjpXGNh5jKQGb6gF;
		}
	}
	internal class DelayedCall<T1, T2, T3> : DelayedCall
	{
		public new delegate void Callback(T1 data1, T2 data2, T3 data3);

		private Callback callback_2;

		private T1 gparam_0;

		private T2 gparam_1;

		private T3 gparam_2;

		internal static object imRoB9XqvdKnwO9gMMZ;

		public static DelayedCall<T1, T2, T3> Create(Callback cb, T1 data1, T2 data2, T3 data3, int milliseconds)
		{
			DelayedCall<T1, T2, T3> delayedCall = new DelayedCall<T1, T2, T3>();
			DelayedCall.PrepareDCObject(delayedCall, milliseconds, async: false);
			delayedCall.callback_2 = cb;
			delayedCall.gparam_0 = data1;
			delayedCall.gparam_1 = data2;
			delayedCall.gparam_2 = data3;
			return delayedCall;
		}

		public static DelayedCall<T1, T2, T3> CreateAsync(Callback cb, T1 data1, T2 data2, T3 data3, int milliseconds)
		{
			DelayedCall<T1, T2, T3> delayedCall = new DelayedCall<T1, T2, T3>();
			DelayedCall.PrepareDCObject(delayedCall, milliseconds, async: true);
			delayedCall.callback_2 = cb;
			delayedCall.gparam_0 = data1;
			delayedCall.gparam_1 = data2;
			delayedCall.gparam_2 = data3;
			return delayedCall;
		}

		public static DelayedCall<T1, T2, T3> Start(Callback cb, T1 data1, T2 data2, T3 data3, int milliseconds)
		{
			DelayedCall<T1, T2, T3> delayedCall = Create(cb, data1, data2, data3, milliseconds);
			delayedCall.Start();
			return delayedCall;
		}

		public static DelayedCall<T1, T2, T3> StartAsync(Callback cb, T1 data1, T2 data2, T3 data3, int milliseconds)
		{
			DelayedCall<T1, T2, T3> delayedCall = CreateAsync(cb, data1, data2, data3, milliseconds);
			delayedCall.Start();
			return delayedCall;
		}

		protected override void OnFire()
		{
			while (true)
			{
				context.Post(delegate
				{
					int num = 1;
					while (true)
					{
						object obj;
						Monitor.Enter(obj = timerLock);
						int num2 = 0;
						if (lOCaRpXsBCvLyEEjn7a() != null)
						{
							goto IL_0023;
						}
						goto IL_0070;
						IL_0070:
						while (true)
						{
							switch (num2)
							{
							case 2:
								if (callback_2 == null)
								{
									return;
								}
								goto IL_0016;
							default:
								try
								{
									if (cancelled)
									{
										if (qN5QcwXL7bnBjZn2jdm())
										{
											switch (1)
											{
											case 1:
												return;
											case 0:
												break;
											}
										}
									}
								}
								finally
								{
									Monitor.Exit(obj);
									if (qN5QcwXL7bnBjZn2jdm())
									{
										switch (0)
										{
										}
									}
								}
								goto case 2;
							case 1:
								break;
							case 4:
								callback_2(gparam_0, gparam_1, gparam_2);
								return;
							case 3:
								return;
							}
							break;
							IL_0016:
							num2 = 4;
							if (qN5QcwXL7bnBjZn2jdm())
							{
								continue;
							}
							goto IL_0023;
						}
						continue;
						IL_0023:
						num2 = num;
						goto IL_0070;
					}
				}, null);
				if (lOCaRpXsBCvLyEEjn7a() != null)
				{
					switch (0)
					{
					case 1:
						break;
					default:
						return;
					case 0:
						return;
					}
					continue;
				}
				break;
			}
		}

		public void Reset(T1 data1, T2 data2, T3 data3, int milliseconds)
		{
			lock (timerLock)
			{
				Cancel();
				gparam_0 = data1;
				gparam_1 = data2;
				gparam_2 = data3;
				base.Milliseconds = milliseconds;
				Start();
			}
		}

		[CompilerGenerated]
		private void method_1(object object_1)
		{
			int num = 1;
			while (true)
			{
				object obj;
				Monitor.Enter(obj = timerLock);
				int num2 = 0;
				if (lOCaRpXsBCvLyEEjn7a() != null)
				{
					goto IL_0023;
				}
				goto IL_0070;
				IL_0070:
				while (true)
				{
					switch (num2)
					{
					case 2:
						if (callback_2 == null)
						{
							return;
						}
						goto IL_0016;
					default:
						try
						{
							if (cancelled)
							{
								if (qN5QcwXL7bnBjZn2jdm())
								{
									switch (1)
									{
									case 1:
										return;
									case 0:
										break;
									}
								}
							}
						}
						finally
						{
							Monitor.Exit(obj);
							if (qN5QcwXL7bnBjZn2jdm())
							{
								switch (0)
								{
								}
							}
						}
						goto case 2;
					case 1:
						break;
					case 4:
						callback_2(gparam_0, gparam_1, gparam_2);
						return;
					case 3:
						return;
					}
					break;
					IL_0016:
					num2 = 4;
					if (qN5QcwXL7bnBjZn2jdm())
					{
						continue;
					}
					goto IL_0023;
				}
				continue;
				IL_0023:
				num2 = num;
				goto IL_0070;
			}
		}

		internal static bool qN5QcwXL7bnBjZn2jdm()
		{
			return imRoB9XqvdKnwO9gMMZ == null;
		}

		internal static object lOCaRpXsBCvLyEEjn7a()
		{
			return imRoB9XqvdKnwO9gMMZ;
		}
	}
}
