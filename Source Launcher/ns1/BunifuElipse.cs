using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
using Bunifu.Framework.Lib;

namespace ns1
{
	[DebuggerStepThrough]
	[ProvideProperty("BunifuFramework", typeof(Control))]
	public class BunifuElipse : Component
	{
		private ContainerControl containerControl_0;

		[CompilerGenerated]
		private EventHandler eventHandler_0;

		private Control control_0;

		private int int_0 = 5;

		private IContainer icontainer_0;

		private System.Windows.Forms.Timer timer_0;

		internal static BunifuElipse FTRe8bIfuh4kZFjKwO31;

		public override ISite Site
		{
			get
			{
				return base.Site;
			}
			set
			{
				base.Site = value;
				if (value == null)
				{
					return;
				}
				IDesignerHost designerHost = value.GetService(typeof(IDesignerHost)) as IDesignerHost;
				if (designerHost != null)
				{
					IComponent rootComponent = designerHost.RootComponent;
					if (rootComponent is ContainerControl)
					{
						iKrhcqcfaU(rootComponent as ContainerControl);
					}
				}
			}
		}

		public Control TargetControl
		{
			get
			{
				return control_0;
			}
			set
			{
				control_0 = value;
			}
		}

		public int ElipseRadius
		{
			get
			{
				return int_0;
			}
			set
			{
				int_0 = value;
				ApplyElipse();
			}
		}

		public event EventHandler TargetControlResized
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

		public BunifuElipse()
		{
			method_1();
			if (TargetControl == null)
			{
				TargetControl = method_0();
			}
		}

		public BunifuElipse(IContainer container)
		{
			container.Add(this);
			method_1();
		}

		[SpecialName]
		private ContainerControl method_0()
		{
			return containerControl_0;
		}

		[SpecialName]
		private void iKrhcqcfaU(ContainerControl value)
		{
			containerControl_0 = value;
			ApplyElipse();
		}

		private void control_0_Resize(object sender, EventArgs e)
		{
			Elipse.Apply(control_0, int_0);
			if (eventHandler_0 != null)
			{
				eventHandler_0(sender, e);
			}
		}

		public void ApplyElipse(int Radius)
		{
			if (control_0 != null)
			{
				Elipse.Apply(control_0, Radius);
			}
		}

		public void ApplyElipse()
		{
			try
			{
				if (control_0 != null)
				{
					Elipse.Apply(control_0, int_0);
				}
			}
			catch (Exception)
			{
			}
		}

		public void ApplyElipse(Control control, int Radius)
		{
			if (control != null)
			{
				Elipse.Apply(control, Radius);
			}
		}

		public void ApplyElipse(Control control)
		{
			if (control != null)
			{
				Elipse.Apply(control, int_0);
			}
		}

		private void timer_0_Tick(object sender, EventArgs e)
		{
			try
			{
				timer_0.Stop();
				if (control_0 != null)
				{
					control_0.Resize += control_0_Resize;
				}
				else
				{
					control_0 = method_0();
					control_0.Resize += control_0_Resize;
				}
				if (control_0.GetType() == typeof(Form))
				{
					((Form)control_0).FormBorderStyle = FormBorderStyle.None;
				}
				ApplyElipse();
			}
			catch (Exception)
			{
				timer_0.Start();
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

		private void method_1()
		{
			icontainer_0 = new Container();
			timer_0 = new System.Windows.Forms.Timer(icontainer_0);
			timer_0.Enabled = true;
			timer_0.Tick += timer_0_Tick;
		}

		internal static bool qNS5XDIfkuQmDl8uCl65()
		{
			return FTRe8bIfuh4kZFjKwO31 == null;
		}

		internal static void Yh3pVlIfEWXjT8K8APMi()
		{
		}
	}
}
