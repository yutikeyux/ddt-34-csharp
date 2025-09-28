using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;

namespace ns1
{
	public class BunifuFormFadeTransition : Component
	{
		private Form form_0;

		private double double_0;

		private int int_0 = 1;

		[CompilerGenerated]
		private EventHandler eventHandler_0;

		private IContainer icontainer_0;

		private BackgroundWorker backgroundWorker_0;

		internal static BunifuFormFadeTransition p9v6mEIgso27JLTWv7H1;

		public int Delay
		{
			get
			{
				return int_0;
			}
			set
			{
				int_0 = value;
			}
		}

		public event EventHandler TransitionEnd
		{
			[CompilerGenerated]
			add
			{
				EventHandler eventHandler = eventHandler_0;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
					eventHandler = Interlocked.CompareExchange(ref eventHandler_0, value2, eventHandler2);
				}
				while ((object)eventHandler != eventHandler2);
			}
			[CompilerGenerated]
			remove
			{
				EventHandler eventHandler = eventHandler_0;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
					eventHandler = Interlocked.CompareExchange(ref eventHandler_0, value2, eventHandler2);
				}
				while ((object)eventHandler != eventHandler2);
			}
		}

		public BunifuFormFadeTransition()
		{
			method_0();
		}

		public BunifuFormFadeTransition(IContainer container)
		{
			container.Add(this);
			method_0();
		}

		public void ShowAsyc(Form frm)
		{
			try
			{
				form_0 = frm;
				backgroundWorker_0.RunWorkerAsync();
			}
			catch (Exception)
			{
			}
		}

		public void HideAsyc(Form frm, bool Close)
		{
			form_0 = frm;
		}

		private void backgroundWorker_0_DoWork(object sender, DoWorkEventArgs e)
		{
			for (double_0 = 0.0; double_0 < 1.0; double_0 += 0.001)
			{
				backgroundWorker_0.ReportProgress(0);
				Thread.Sleep(Delay);
			}
		}

		private void backgroundWorker_0_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			form_0.Opacity = double_0;
		}

		private void backgroundWorker_0_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			form_0.Opacity = 1.0;
			if (eventHandler_0 != null)
			{
				eventHandler_0(sender, e);
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && icontainer_0 != null)
			{
				icontainer_0.Dispose();
			}
			base.Dispose(disposing);
		}

		private void method_0()
		{
			backgroundWorker_0 = new BackgroundWorker();
			backgroundWorker_0.WorkerReportsProgress = true;
			backgroundWorker_0.DoWork += backgroundWorker_0_DoWork;
			backgroundWorker_0.ProgressChanged += backgroundWorker_0_ProgressChanged;
			backgroundWorker_0.RunWorkerCompleted += backgroundWorker_0_RunWorkerCompleted;
		}

		internal static void a2I3SHIga1JlNo0fug1i()
		{
		}

		internal static bool yiLxrgIgNGFkZrnSGBQL()
		{
			return p9v6mEIgso27JLTWv7H1 == null;
		}
	}
}
