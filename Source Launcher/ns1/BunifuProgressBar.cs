using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
using Bunifu.Framework;
using Bunifu.Framework.Lib;

namespace ns1
{
	[DefaultEvent("progressChanged")]
	[ProvideProperty("BunifuFramework", typeof(Control))]
	[DebuggerStepThrough]
	public class BunifuProgressBar : UserControl
	{
		[CompilerGenerated]
		private EventHandler eventHandler_0;

		public int _Value;

		public int Maximum_Value = 100;

		private int int_0 = 5;

		private IContainer icontainer_0;

		private Panel slider;

		internal static BunifuProgressBar dP6QRPIgbU6GC3tE6TY6;

		public int BorderRadius
		{
			get
			{
				return int_0;
			}
			set
			{
				int_0 = value;
				Elipse.Apply(slider, int_0);
				Elipse.Apply(this, int_0);
			}
		}

		public Color ProgressColor
		{
			get
			{
				return slider.BackColor;
			}
			set
			{
				slider.BackColor = value;
			}
		}

		public int Value
		{
			get
			{
				return _Value;
			}
			set
			{
				if (value > Maximum_Value)
				{
					throw new Exception("Maxium Value Rached");
				}
				_Value = value;
				slider.Width = base.Width * _Value / Maximum_Value;
				Elipse.Apply(slider, int_0);
				if (eventHandler_0 != null)
				{
					eventHandler_0(this, new EventArgs());
				}
			}
		}

		public int MaximumValue
		{
			get
			{
				return Maximum_Value;
			}
			set
			{
				Maximum_Value = value;
				try
				{
					slider.Width = base.Width * _Value / Maximum_Value;
					Elipse.Apply(slider, int_0);
				}
				catch (Exception)
				{
				}
			}
		}

		public event EventHandler progressChanged
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

		public BunifuProgressBar()
		{
			InitializeComponent();
		}

		private void BunifuProgressBar_Resize(object sender, EventArgs e)
		{
			slider.Width = base.Width * _Value / Maximum_Value;
			Elipse.Apply(slider, int_0);
			Elipse.Apply(this, int_0);
		}

		private void BunifuProgressBar_Load(object sender, EventArgs e)
		{
			Bunifu.Framework.License.Check(this);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && icontainer_0 != null)
			{
				icontainer_0.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			slider = new System.Windows.Forms.Panel();
			SuspendLayout();
			slider.BackColor = System.Drawing.Color.Teal;
			slider.Dock = System.Windows.Forms.DockStyle.Left;
			slider.Location = new System.Drawing.Point(0, 0);
			slider.Name = "slider";
			slider.Size = new System.Drawing.Size(0, 10);
			slider.TabIndex = 1;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			BackColor = System.Drawing.Color.Silver;
			base.Controls.Add(slider);
			base.Name = "BunifuProgressBar";
			base.Size = new System.Drawing.Size(410, 10);
			base.Load += new System.EventHandler(BunifuProgressBar_Load);
			base.Resize += new System.EventHandler(BunifuProgressBar_Resize);
			ResumeLayout(false);
		}

		internal static void CL24b7IgflLA3Z9wHUKg()
		{
		}

		internal static bool PCRgTaIgBbQI5leibHfo()
		{
			return dP6QRPIgbU6GC3tE6TY6 == null;
		}
	}
}
