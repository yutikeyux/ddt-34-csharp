using System;
using System.Drawing;
using System.Media;
using System.Threading;
using System.Windows.Forms;

namespace MetroFramework
{
	public static class MetroMessageBox
	{
		private static object R5X8mxhRvuuYY8omfEL;

		public static DialogResult Show(IWin32Window owner, string message)
		{
			return Show(owner, message, "Notification", 211);
		}

		public static DialogResult Show(IWin32Window owner, string message, int height)
		{
			return Show(owner, message, "Notification", height);
		}

		public static DialogResult Show(IWin32Window owner, string message, string title)
		{
			return Show(owner, message, title, MessageBoxButtons.OK, 211);
		}

		public static DialogResult Show(IWin32Window owner, string message, string title, int height)
		{
			return Show(owner, message, title, MessageBoxButtons.OK, height);
		}

		public static DialogResult Show(IWin32Window owner, string message, string title, MessageBoxButtons buttons)
		{
			return Show(owner, message, title, buttons, MessageBoxIcon.None, 211);
		}

		public static DialogResult Show(IWin32Window owner, string message, string title, MessageBoxButtons buttons, int height)
		{
			return Show(owner, message, title, buttons, MessageBoxIcon.None, height);
		}

		public static DialogResult Show(IWin32Window owner, string message, string title, MessageBoxButtons buttons, MessageBoxIcon icon)
		{
			return Show(owner, message, title, buttons, icon, MessageBoxDefaultButton.Button1, 211);
		}

		public static DialogResult Show(IWin32Window owner, string message, string title, MessageBoxButtons buttons, MessageBoxIcon icon, int height)
		{
			return Show(owner, message, title, buttons, icon, MessageBoxDefaultButton.Button1, height);
		}

		public static DialogResult Show(IWin32Window owner, string message, string title, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultbutton)
		{
			return Show(owner, message, title, buttons, icon, defaultbutton, 211);
		}

		public static DialogResult Show(IWin32Window owner, string message, string title, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultbutton, int height)
		{
			DialogResult result = DialogResult.None;
			if (owner != null)
			{
				Form form = ((!(owner is Form)) ? ((UserControl)owner).ParentForm : ((Form)owner));
				switch (icon)
				{
				default:
					SystemSounds.Asterisk.Play();
					break;
				case MessageBoxIcon.Exclamation:
					SystemSounds.Exclamation.Play();
					break;
				case MessageBoxIcon.Question:
					SystemSounds.Beep.Play();
					break;
				case MessageBoxIcon.Hand:
					SystemSounds.Hand.Play();
					break;
				}
				MetroMessageBoxControl metroMessageBoxControl = new MetroMessageBoxControl();
				metroMessageBoxControl.BackColor = form.BackColor;
				metroMessageBoxControl.Properties.Buttons = buttons;
				metroMessageBoxControl.Properties.DefaultButton = defaultbutton;
				metroMessageBoxControl.Properties.Icon = icon;
				metroMessageBoxControl.Properties.Message = message;
				metroMessageBoxControl.Properties.Title = title;
				metroMessageBoxControl.Padding = new Padding(0, 0, 0, 0);
				metroMessageBoxControl.ControlBox = false;
				metroMessageBoxControl.ShowInTaskbar = false;
				metroMessageBoxControl.TopMost = true;
				metroMessageBoxControl.Size = new Size(form.Size.Width, height);
				metroMessageBoxControl.Location = new Point(form.Location.X, form.Location.Y + (form.Height - metroMessageBoxControl.Height) / 2);
				metroMessageBoxControl.ArrangeApperance();
				Convert.ToInt32(Math.Floor((double)metroMessageBoxControl.Size.Height * 0.28));
				metroMessageBoxControl.ShowDialog();
				metroMessageBoxControl.BringToFront();
				metroMessageBoxControl.SetDefaultButton();
				Action<MetroMessageBoxControl> action = smethod_0;
				IAsyncResult asyncResult = action.BeginInvoke(metroMessageBoxControl, null, action);
				bool flag = false;
				try
				{
					while (!asyncResult.IsCompleted)
					{
						Thread.Sleep(1);
						Application.DoEvents();
					}
				}
				catch
				{
					flag = true;
					if (!asyncResult.IsCompleted)
					{
						try
						{
							asyncResult = null;
						}
						catch
						{
						}
					}
					action = null;
				}
				if (!flag)
				{
					result = metroMessageBoxControl.Result;
					metroMessageBoxControl.Dispose();
					metroMessageBoxControl = null;
				}
			}
			return result;
		}

		private static void smethod_0(Control control_0)
		{
			while (control_0.Visible)
			{
			}
		}

		internal static bool eUTgclhAR1kqVrrQZGt()
		{
			return R5X8mxhRvuuYY8omfEL == null;
		}
	}
}
