using System;
using System.Drawing;
using System.Windows.Forms;
using MetroFramework.Animation;
using MetroFramework.Components;
using MetroFramework.Controls;
using MetroFramework.Drawing;
using MetroFramework.Interfaces;
using MetroFramework.Native;

namespace MetroFramework.Forms
{
	public sealed class MetroTaskWindow : MetroForm
	{
		private static MetroTaskWindow metroTaskWindow_0;

		private bool bool_4;

		private readonly int int_1;

		private int int_2;

		private int int_3;

		private DelayedCall delayedCall_0;

		private readonly MetroPanel metroPanel_0;

		private bool bool_5;

		private static MetroTaskWindow lppHIHhHArkpPmkogqb;

		public bool CancelTimer
		{
			get
			{
				return bool_4;
			}
			set
			{
				bool_4 = value;
			}
		}

		public static void ShowTaskWindow(IWin32Window parent, string title, Control userControl, int secToClose)
		{
			if (metroTaskWindow_0 != null)
			{
				metroTaskWindow_0.Close();
				metroTaskWindow_0.Dispose();
				metroTaskWindow_0 = null;
			}
			metroTaskWindow_0 = new MetroTaskWindow(secToClose, userControl);
			metroTaskWindow_0.Text = title;
			metroTaskWindow_0.Resizable = false;
			metroTaskWindow_0.Movable = true;
			metroTaskWindow_0.StartPosition = FormStartPosition.Manual;
			if (parent != null && parent is IMetroForm)
			{
				metroTaskWindow_0.Theme = ((IMetroForm)parent).Theme;
				metroTaskWindow_0.Style = ((IMetroForm)parent).Style;
				metroTaskWindow_0.StyleManager = ((IMetroForm)parent).StyleManager.Clone(metroTaskWindow_0) as MetroStyleManager;
			}
			metroTaskWindow_0.Show();
		}

		public static bool IsVisible()
		{
			if (metroTaskWindow_0 == null)
			{
				return false;
			}
			return metroTaskWindow_0.Visible;
		}

		public static void ShowTaskWindow(IWin32Window parent, string text, Control userControl)
		{
			ShowTaskWindow(parent, text, userControl, 0);
		}

		public static void ShowTaskWindow(string text, Control userControl, int secToClose)
		{
			ShowTaskWindow(null, text, userControl, secToClose);
		}

		public static void ShowTaskWindow(string text, Control userControl)
		{
			ShowTaskWindow(null, text, userControl);
		}

		public static void CancelAutoClose()
		{
			if (metroTaskWindow_0 != null)
			{
				metroTaskWindow_0.CancelTimer = true;
			}
		}

		public static void ForceClose()
		{
			if (metroTaskWindow_0 != null)
			{
				CancelAutoClose();
				metroTaskWindow_0.Close();
				metroTaskWindow_0.Dispose();
				metroTaskWindow_0 = null;
			}
		}

		public MetroTaskWindow()
		{
			metroPanel_0 = new MetroPanel();
			base.Controls.Add(metroPanel_0);
		}

		public MetroTaskWindow(int duration, Control userControl)
			: this()
		{
			metroPanel_0.Controls.Add(userControl);
			userControl.Dock = DockStyle.Fill;
			int_1 = duration * 500;
			if (int_1 > 0)
			{
				delayedCall_0 = DelayedCall.Start(method_9, 5);
			}
		}

		protected override void OnActivated(EventArgs e)
		{
			if (!bool_5)
			{
				metroPanel_0.Theme = base.Theme;
				metroPanel_0.Style = base.Style;
				metroPanel_0.StyleManager = base.StyleManager;
				base.MaximizeBox = false;
				base.MinimizeBox = false;
				base.Movable = true;
				base.TopMost = true;
				base.Size = new Size(400, 200);
				Taskbar taskbar = new Taskbar();
				switch (taskbar.Position)
				{
				default:
					base.Location = new Point(Screen.PrimaryScreen.Bounds.Width - base.Width - 5, Screen.PrimaryScreen.Bounds.Height - base.Height - 5);
					break;
				case TaskbarPosition.Left:
					base.Location = new Point(taskbar.Bounds.Width + 5, taskbar.Bounds.Height - base.Height - 5);
					break;
				case TaskbarPosition.Top:
					base.Location = new Point(taskbar.Bounds.Width - base.Width - 5, taskbar.Bounds.Height + 5);
					break;
				case TaskbarPosition.Right:
					base.Location = new Point(taskbar.Bounds.X - base.Width - 5, taskbar.Bounds.Height - base.Height - 5);
					break;
				case TaskbarPosition.Bottom:
					base.Location = new Point(taskbar.Bounds.Width - base.Width - 5, taskbar.Bounds.Y - base.Height - 5);
					break;
				}
				metroPanel_0.Location = new Point(0, 60);
				metroPanel_0.Size = new Size(base.Width - 40, base.Height - 80);
				metroPanel_0.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
				metroPanel_0.AutoScroll = false;
				metroPanel_0.HorizontalScrollbar = false;
				metroPanel_0.VerticalScrollbar = false;
				metroPanel_0.Refresh();
				if (base.StyleManager != null)
				{
					base.StyleManager.Update();
				}
				bool_5 = true;
				MoveAnimation moveAnimation = new MoveAnimation();
				moveAnimation.Start(metroPanel_0, new Point(20, 60), TransitionType.EaseInOutCubic, 15);
			}
			base.OnActivated(e);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			using SolidBrush brush = new SolidBrush(MetroPaint.BackColor.Form(base.Theme));
			e.Graphics.FillRectangle(brush, new Rectangle(base.Width - int_3, 0, int_3, 5));
		}

		private void method_9()
		{
			if (int_2 == int_1)
			{
				delayedCall_0.Dispose();
				delayedCall_0 = null;
				Close();
				return;
			}
			int_2 += 5;
			if (bool_4)
			{
				int_2 = 0;
			}
			double num = (double)int_2 / ((double)int_1 / 100.0);
			int_3 = (int)((double)base.Width * (num / 100.0));
			Invalidate(new Rectangle(0, 0, base.Width, 5));
			if (!bool_4)
			{
				delayedCall_0.Reset();
			}
		}

		internal static bool kOvw3fhl4rv6vNq7ahv()
		{
			return lppHIHhHArkpPmkogqb == null;
		}

		internal static void b6eLdlhJAhscRhhts7D()
		{
		}
	}
}
