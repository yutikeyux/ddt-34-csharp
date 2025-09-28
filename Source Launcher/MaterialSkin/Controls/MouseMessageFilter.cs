using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;

namespace MaterialSkin.Controls
{
	public class MouseMessageFilter : IMessageFilter
	{
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static MouseEventHandler mouseEventHandler_0;

		private static MouseMessageFilter htsFZbIVh9vfOkbofoMf;

		public static event MouseEventHandler MouseMove
		{
			[CompilerGenerated]
			add
			{
				MouseEventHandler mouseEventHandler = mouseEventHandler_0;
				MouseEventHandler mouseEventHandler2;
				do
				{
					mouseEventHandler2 = mouseEventHandler;
					MouseEventHandler value2 = (MouseEventHandler)Delegate.Combine(mouseEventHandler2, value);
					mouseEventHandler = Interlocked.CompareExchange(ref mouseEventHandler_0, value2, mouseEventHandler2);
				}
				while ((object)mouseEventHandler != mouseEventHandler2);
			}
			[CompilerGenerated]
			remove
			{
				MouseEventHandler mouseEventHandler = mouseEventHandler_0;
				MouseEventHandler mouseEventHandler2;
				do
				{
					mouseEventHandler2 = mouseEventHandler;
					MouseEventHandler value2 = (MouseEventHandler)Delegate.Remove(mouseEventHandler2, value);
					mouseEventHandler = Interlocked.CompareExchange(ref mouseEventHandler_0, value2, mouseEventHandler2);
				}
				while ((object)mouseEventHandler != mouseEventHandler2);
			}
		}

		public bool PreFilterMessage(ref Message m)
		{
			if (m.Msg == 512 && mouseEventHandler_0 != null)
			{
				int x = Control.MousePosition.X;
				int y = Control.MousePosition.Y;
				mouseEventHandler_0(null, new MouseEventArgs(MouseButtons.None, 0, x, y, 0));
			}
			return false;
		}

		internal static bool QM5rLeIVTOjfyUEkdnia()
		{
			return htsFZbIVh9vfOkbofoMf == null;
		}
	}
}
