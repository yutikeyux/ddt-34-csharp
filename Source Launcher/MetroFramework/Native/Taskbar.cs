using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security;

namespace MetroFramework.Native
{
	internal class Taskbar
	{
		private Rectangle rectangle_0 = Rectangle.Empty;

		private TaskbarPosition taskbarPosition_0 = TaskbarPosition.Unknown;

		private bool bool_0;

		private bool bool_1;

		internal static Taskbar oJN8PUK47wEHcVeB83u;

		public Rectangle Bounds
		{
			get
			{
				return rectangle_0;
			}
			private set
			{
				rectangle_0 = value;
			}
		}

		public TaskbarPosition Position
		{
			get
			{
				return taskbarPosition_0;
			}
			private set
			{
				taskbarPosition_0 = value;
			}
		}

		public Point Location => Bounds.Location;

		public Size Size => Bounds.Size;

		public bool AlwaysOnTop
		{
			get
			{
				return bool_0;
			}
			private set
			{
				bool_0 = value;
			}
		}

		public bool AutoHide
		{
			get
			{
				return bool_1;
			}
			private set
			{
				bool_1 = value;
			}
		}

		[SecuritySafeCritical]
		public Taskbar()
		{
			IntPtr hWnd = WinApi.FindWindow("Shell_TrayWnd", null);
			WinApi.APPBARDATA pData = new WinApi.APPBARDATA
			{
				cbSize = (uint)Marshal.SizeOf(Type.GetTypeFromHandle((RuntimeTypeHandle)typeof(WinApi.APPBARDATA).TypeHandle)),
				hWnd = hWnd
			};
			IntPtr intPtr = WinApi.SHAppBarMessage(WinApi.ABM.GetTaskbarPos, ref pData);
			if (intPtr == IntPtr.Zero)
			{
				throw new InvalidOperationException();
			}
			Position = (TaskbarPosition)pData.uEdge;
			Bounds = Rectangle.FromLTRB(pData.rc.Left, pData.rc.Top, pData.rc.Right, pData.rc.Bottom);
			pData.cbSize = (uint)Marshal.SizeOf(Type.GetTypeFromHandle((RuntimeTypeHandle)typeof(WinApi.APPBARDATA).TypeHandle));
			int num = ((IntPtr)(nint)WinApi.SHAppBarMessage(WinApi.ABM.GetState, ref pData)).ToInt32();
			AlwaysOnTop = (num & 2) == 2;
			AutoHide = (num & 1) == 1;
		}

		internal static bool u5QHwKKH9oFW6jadFML()
		{
			return oJN8PUK47wEHcVeB83u == null;
		}

		internal static void PSn6x6KpAqJs2O85cMG()
		{
		}
	}
}
