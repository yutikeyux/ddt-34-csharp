using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace hoiuclib
{
	public class AxCode : IDisposable
	{
		public delegate void OnLoadExternalResourceByFullPathEventHandler(object sender, string URL, Stream Stream, ref bool Handled);

		public delegate void OnSendDataHandler(object sender, byte[] buffer);

		private IntPtr intptr_0 = IntPtr.Zero;

		private cedrus__wrapper.LoadExternalResourceHandler loadExternalResourceHandler_0;

		private cedrus__wrapper.GetBindInfoHandler getBindInfoHandler_0;

		public static int MaxSoundVolume;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private OnLoadExternalResourceByFullPathEventHandler onLoadExternalResourceByFullPathEventHandler_0;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private OnSendDataHandler onSendDataHandler_0;

		private static AxCode OJINxl3eJushOam8Pd;

		public IntPtr Handle => intptr_0;

		public int SoundVolume
		{
			get
			{
				return cedrus__wrapper.FPC_GetSoundVolume(Handle);
			}
			set
			{
				cedrus__wrapper.FPC_SetSoundVolume(Handle, value);
			}
		}

		public bool SoundEnabled
		{
			get
			{
				return cedrus__wrapper.FPC_IsSoundEnabled(Handle) != 0;
			}
			set
			{
				cedrus__wrapper.FPC_EnableSound(Handle, value ? 1 : 0);
			}
		}

		public Version Version
		{
			get
			{
				cedrus__wrapper.FPC_GetVersionEx(Handle, out var version);
				return new Version(version.v3, version.v2, version.v1, version.v0);
			}
		}

		public event OnLoadExternalResourceByFullPathEventHandler OnLoadExternalResourceByFullPath
		{
			[CompilerGenerated]
			add
			{
				OnLoadExternalResourceByFullPathEventHandler onLoadExternalResourceByFullPathEventHandler = onLoadExternalResourceByFullPathEventHandler_0;
				OnLoadExternalResourceByFullPathEventHandler onLoadExternalResourceByFullPathEventHandler2;
				do
				{
					onLoadExternalResourceByFullPathEventHandler2 = onLoadExternalResourceByFullPathEventHandler;
					OnLoadExternalResourceByFullPathEventHandler value2 = (OnLoadExternalResourceByFullPathEventHandler)Delegate.Combine(onLoadExternalResourceByFullPathEventHandler2, value);
					onLoadExternalResourceByFullPathEventHandler = Interlocked.CompareExchange(ref onLoadExternalResourceByFullPathEventHandler_0, value2, onLoadExternalResourceByFullPathEventHandler2);
				}
				while ((object)onLoadExternalResourceByFullPathEventHandler != onLoadExternalResourceByFullPathEventHandler2);
			}
			[CompilerGenerated]
			remove
			{
				OnLoadExternalResourceByFullPathEventHandler onLoadExternalResourceByFullPathEventHandler = onLoadExternalResourceByFullPathEventHandler_0;
				OnLoadExternalResourceByFullPathEventHandler onLoadExternalResourceByFullPathEventHandler2;
				do
				{
					onLoadExternalResourceByFullPathEventHandler2 = onLoadExternalResourceByFullPathEventHandler;
					OnLoadExternalResourceByFullPathEventHandler value2 = (OnLoadExternalResourceByFullPathEventHandler)Delegate.Remove(onLoadExternalResourceByFullPathEventHandler2, value);
					onLoadExternalResourceByFullPathEventHandler = Interlocked.CompareExchange(ref onLoadExternalResourceByFullPathEventHandler_0, value2, onLoadExternalResourceByFullPathEventHandler2);
				}
				while ((object)onLoadExternalResourceByFullPathEventHandler != onLoadExternalResourceByFullPathEventHandler2);
			}
		}

		public event OnSendDataHandler OnSendData
		{
			[CompilerGenerated]
			add
			{
				OnSendDataHandler onSendDataHandler = onSendDataHandler_0;
				OnSendDataHandler onSendDataHandler2;
				do
				{
					onSendDataHandler2 = onSendDataHandler;
					OnSendDataHandler value2 = (OnSendDataHandler)Delegate.Combine(onSendDataHandler2, value);
					onSendDataHandler = Interlocked.CompareExchange(ref onSendDataHandler_0, value2, onSendDataHandler2);
				}
				while ((object)onSendDataHandler != onSendDataHandler2);
			}
			[CompilerGenerated]
			remove
			{
				OnSendDataHandler onSendDataHandler = onSendDataHandler_0;
				OnSendDataHandler onSendDataHandler2;
				do
				{
					onSendDataHandler2 = onSendDataHandler;
					OnSendDataHandler value2 = (OnSendDataHandler)Delegate.Remove(onSendDataHandler2, value);
					onSendDataHandler = Interlocked.CompareExchange(ref onSendDataHandler_0, value2, onSendDataHandler2);
				}
				while ((object)onSendDataHandler != onSendDataHandler2);
			}
		}

		private void method_0()
		{
			loadExternalResourceHandler_0 = method_1;
			cedrus__wrapper.FPC_AddOnLoadExternalResourceHandlerW(intptr_0, loadExternalResourceHandler_0, IntPtr.Zero);
			getBindInfoHandler_0 = method_2;
			cedrus__wrapper.FPC_AddGetBindInfoHandler(intptr_0, getBindInfoHandler_0, IntPtr.Zero);
		}

		public AxCode()
		{
			intptr_0 = cedrus__wrapper.FPC_LoadRegisteredOCX();
			if (IntPtr.Zero != intptr_0)
			{
				method_0();
			}
		}

		public AxCode(Stream StreamWithOCXCode)
		{
			MemoryStream memoryStream = new MemoryStream();
			byte[] buffer = new byte[65536];
			int count;
			while ((count = StreamWithOCXCode.Read(buffer, 0, 65536)) > 0)
			{
				memoryStream.Write(buffer, 0, count);
			}
			uint num = (uint)memoryStream.Length;
			IntPtr intPtr = Marshal.AllocHGlobal(new IntPtr(memoryStream.Length));
			byte[] array = new byte[num];
			memoryStream.Position = 0L;
			memoryStream.Read(array, 0, (int)memoryStream.Length);
			Marshal.Copy(array, 0, intPtr, (int)num);
			intptr_0 = cedrus__wrapper.FPC_LoadOCXCodeFromMemory(intPtr, num);
			Marshal.FreeHGlobal(intPtr);
			if (IntPtr.Zero != intptr_0)
			{
				method_0();
			}
		}

		~AxCode()
		{
			if (IntPtr.Zero != intptr_0)
			{
				cedrus__wrapper.FPC_UnloadCode(intptr_0);
				intptr_0 = IntPtr.Zero;
			}
		}

		public void Dispose()
		{
			if (IntPtr.Zero != intptr_0)
			{
				cedrus__wrapper.FPC_UnloadCode(intptr_0);
				intptr_0 = IntPtr.Zero;
			}
		}

		private int method_1(IntPtr intptr_1, ref IntPtr intptr_2, IntPtr intptr_3, IntPtr intptr_4)
		{
			bool Handled = false;
			if (onLoadExternalResourceByFullPathEventHandler_0 != null)
			{
				Stream stream = new Stream0(intptr_2, bAddRef: true);
				onLoadExternalResourceByFullPathEventHandler_0(this, Marshal.PtrToStringUni(intptr_1), stream, ref Handled);
				if (!Handled)
				{
					stream.Close();
				}
			}
			return (!Handled) ? (-1) : 0;
		}

		[DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall)]
		private static extern IntPtr GlobalLock(IntPtr intptr_1);

		[DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall)]
		private static extern int GlobalUnlock(IntPtr intptr_1);

		[DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall)]
		private static extern int GlobalSize(IntPtr intptr_1);

		private int method_2(IntPtr intptr_1, ref int int_0, ref cedrus__wrapper.BINDINFO bindinfo_0, IntPtr intptr_2)
		{
			if (onSendDataHandler_0 != null && IntPtr.Zero != bindinfo_0.stgmedData.hGlobal)
			{
				IntPtr intPtr = GlobalLock(bindinfo_0.stgmedData.hGlobal);
				int num = GlobalSize(bindinfo_0.stgmedData.hGlobal);
				if (IntPtr.Zero != intPtr && num > 0)
				{
					byte[] array = new byte[num];
					Marshal.Copy(intPtr, array, 0, num);
					onSendDataHandler_0(this, array);
				}
				GlobalUnlock(bindinfo_0.stgmedData.hGlobal);
			}
			return 1;
		}

		public void StopMinimizeMemoryTimer()
		{
			cedrus__wrapper.FPC_StopFPCMinimizeMemoryTimer(intptr_0);
		}

		static AxCode()
		{
			MaxSoundVolume = 65535;
		}

		internal static bool LNF6Cj24UDNt2NHQkU()
		{
			return OJINxl3eJushOam8Pd == null;
		}

		internal static void j4ZpqGjKeRnitvqEfo()
		{
		}
	}
}
