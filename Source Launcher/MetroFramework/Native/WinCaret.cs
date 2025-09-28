using System;
using System.Runtime.InteropServices;
using System.Security;

namespace MetroFramework.Native
{
	[SuppressUnmanagedCodeSecurity]
	internal sealed class WinCaret
	{
		private IntPtr intptr_0;

		internal static WinCaret gCCgbbvF8CAEMgadG9r;

		[DllImport("user32.dll")]
		private static extern bool CreateCaret(IntPtr intptr_1, int int_0, int int_1, int int_2);

		[DllImport("user32.dll")]
		private static extern bool SetCaretPos(int int_0, int int_1);

		[DllImport("user32.dll")]
		private static extern bool DestroyCaret();

		[DllImport("user32.dll")]
		private static extern bool ShowCaret(IntPtr intptr_1);

		[DllImport("user32.dll")]
		public static extern bool HideCaret(IntPtr hWnd);

		public WinCaret(IntPtr ownerHandle)
		{
			intptr_0 = ownerHandle;
		}

		public bool Create(int width, int height)
		{
			return CreateCaret(intptr_0, 0, width, height);
		}

		public void Hide()
		{
			HideCaret(intptr_0);
		}

		public void Show()
		{
			ShowCaret(intptr_0);
		}

		public bool SetPosition(int x, int y)
		{
			return SetCaretPos(x, y);
		}

		public void Destroy()
		{
			DestroyCaret();
		}

		internal static void ScabWkvlAQ69ckqKAI9()
		{
		}

		internal static bool AIF76sv4Kq4GbPk4F7L()
		{
			return gCCgbbvF8CAEMgadG9r == null;
		}
	}
}
