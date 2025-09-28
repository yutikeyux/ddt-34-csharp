using System;
using System.Runtime.CompilerServices;
using System.Security;
using System.Windows.Forms;

namespace MetroFramework.Native
{
	[SuppressUnmanagedCodeSecurity]
	internal class SubClass : NativeWindow
	{
		public delegate int SubClassWndProcEventHandler(ref Message m);

		private SubClassWndProcEventHandler subClassWndProcEventHandler_0;

		private bool bool_0;

		private static SubClass SQcBltKMbcYTMFjRiUa;

		public bool SubClassed
		{
			get
			{
				return bool_0;
			}
			set
			{
				bool_0 = value;
			}
		}

		public event SubClassWndProcEventHandler SubClassedWndProc
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				subClassWndProcEventHandler_0 = (SubClassWndProcEventHandler)Delegate.Combine(subClassWndProcEventHandler_0, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				subClassWndProcEventHandler_0 = (SubClassWndProcEventHandler)Delegate.Remove(subClassWndProcEventHandler_0, value);
			}
		}

		public SubClass(IntPtr Handle, bool _SubClass)
		{
			AssignHandle(Handle);
			bool_0 = _SubClass;
		}

		protected override void WndProc(ref Message m)
		{
			if (!bool_0 || method_1(ref m) == 0)
			{
				base.WndProc(ref m);
			}
		}

		public void CallDefaultWndProc(ref Message m)
		{
			base.WndProc(ref m);
		}

		public int HiWord(int Number)
		{
			return (Number >> 16) & 0xFFFF;
		}

		public int LoWord(int Number)
		{
			return Number & 0xFFFF;
		}

		public int MakeLong(int LoWord, int HiWord)
		{
			return (HiWord << 16) | (LoWord & 0xFFFF);
		}

		public IntPtr method_0(int LoWord, int HiWord)
		{
			return (IntPtr)((HiWord << 16) | (LoWord & 0xFFFF));
		}

		private int method_1(ref Message message_0)
		{
			if (subClassWndProcEventHandler_0 == null)
			{
				return 0;
			}
			return subClassWndProcEventHandler_0(ref message_0);
		}

		internal static bool x9KE3GKodJYxW3nn5PS()
		{
			return SQcBltKMbcYTMFjRiUa == null;
		}

		internal static void hva4fiKnODPgKCQH89C()
		{
		}
	}
}
