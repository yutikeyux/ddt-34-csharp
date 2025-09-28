using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
using Bunifu.Framework;
using Bunifu.Framework.Lib;
using ns0;

namespace ns1
{
	[ProvideProperty("BunifuFramework", typeof(Control))]
	[DefaultEvent("OnChange")]
	[DebuggerStepThrough]
	public class BunifuCheckbox : UserControl
	{
		[CompilerGenerated]
		private EventHandler eventHandler_0;

		private bool bool_0 = true;

		private Color color_0 = Color.FromArgb(51, 205, 117);

		private Color color_1 = Color.FromArgb(132, 135, 140);

		private IContainer icontainer_0;

		private PictureBox check;

		internal static BunifuCheckbox A8bKNoIOg7MFWTbZKMVM;

		public Color CheckedOnColor
		{
			get
			{
				return color_0;
			}
			set
			{
				color_0 = value;
				BackColor = color_0;
			}
		}

		public Color ChechedOffColor
		{
			get
			{
				return color_1;
			}
			set
			{
				color_1 = value;
			}
		}

		public bool Checked
		{
			get
			{
				return bool_0;
			}
			set
			{
				bool_0 = value;
				check.Visible = bool_0;
				if (bool_0)
				{
					BackColor = color_0;
				}
				else
				{
					BackColor = color_1;
				}
			}
		}

		public event EventHandler OnChange
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

		public BunifuCheckbox()
		{
			InitializeComponent();
			Elipse.Apply(this, 5);
		}

		private void BunifuCheckbox_Click(object sender, EventArgs e)
		{
			check.Visible = !check.Visible;
			Checked = check.Visible;
			if (eventHandler_0 != null)
			{
				eventHandler_0(this, null);
			}
		}

		private void BunifuCheckbox_Resize(object sender, EventArgs e)
		{
			base.Size = new Size(20, 20);
		}

		private void BunifuCheckbox_ForeColorChanged(object sender, EventArgs e)
		{
			check.Image = Class7.smethod_1(check.Image, ForeColor);
		}

		private void BunifuCheckbox_Load(object sender, EventArgs e)
		{
			if (bool_0)
			{
				BackColor = color_0;
			}
			else
			{
				BackColor = color_1;
			}
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ns1.BunifuCheckbox));
			check = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)check).BeginInit();
			SuspendLayout();
			check.BackColor = System.Drawing.Color.Transparent;
			check.Dock = System.Windows.Forms.DockStyle.Fill;
			check.Image = (System.Drawing.Image)resources.GetObject("check.Image");
			check.Location = new System.Drawing.Point(0, 0);
			check.Name = "check";
			check.Size = new System.Drawing.Size(20, 20);
			check.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			check.TabIndex = 0;
			check.TabStop = false;
			check.Click += new System.EventHandler(BunifuCheckbox_Click);
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			BackColor = System.Drawing.Color.SeaGreen;
			base.Controls.Add(check);
			ForeColor = System.Drawing.Color.White;
			base.Name = "BunifuCheckbox";
			base.Size = new System.Drawing.Size(20, 20);
			base.Load += new System.EventHandler(BunifuCheckbox_Load);
			base.ForeColorChanged += new System.EventHandler(BunifuCheckbox_ForeColorChanged);
			base.Click += new System.EventHandler(BunifuCheckbox_Click);
			base.Resize += new System.EventHandler(BunifuCheckbox_Resize);
			((System.ComponentModel.ISupportInitialize)check).EndInit();
			ResumeLayout(false);
		}

		internal static void e4hx8VIOs8P2c2Kfe4Kl()
		{
		}

		internal static bool J8lGV0IOqtURA3s0r5aB()
		{
			return A8bKNoIOg7MFWTbZKMVM == null;
		}
	}
}
