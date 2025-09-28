using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
using Bunifu.Framework;
using ns0;

namespace ns1
{
	[DefaultEvent("OnValueChange")]
	[ProvideProperty("BunifuFramework", typeof(Control))]
	[DebuggerStepThrough]
	public class BunifuiOSSwitch : UserControl
	{
		[CompilerGenerated]
		private EventHandler eventHandler_0;

		private bool bool_0 = true;

		private Color color_0 = Color.FromArgb(71, 202, 94);

		private Color color_1 = Color.Gray;

		private Image image_0;

		private Image image_1;

		private IContainer icontainer_0;

		private PictureBox pictureBox1;

		private static BunifuiOSSwitch AJu34LIGAwBCOOunQmgq;

		public bool Value
		{
			get
			{
				return bool_0;
			}
			set
			{
				bool_0 = value;
				method_0();
			}
		}

		public Color OnColor
		{
			get
			{
				return color_0;
			}
			set
			{
				color_0 = value;
				if (BackgroundImage != null)
				{
					image_0 = Class7.smethod_1(BackgroundImage, OnColor);
				}
				method_0();
			}
		}

		public Color OffColor
		{
			get
			{
				return color_1;
			}
			set
			{
				color_1 = value;
				if (BackgroundImage != null)
				{
					image_1 = Class7.smethod_1(BackgroundImage, OffColor);
				}
				method_0();
			}
		}

		public event EventHandler OnValueChange
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

		public BunifuiOSSwitch()
		{
			InitializeComponent();
			image_0 = Class7.smethod_1(BackgroundImage, OnColor);
			image_1 = Class7.smethod_1(BackgroundImage, OffColor);
		}

		private void BunifuiOSSwitch_Resize(object sender, EventArgs e)
		{
			base.Size = new Size(43, 25);
		}

		private void BunifuiOSSwitch_Click(object sender, EventArgs e)
		{
			Value = !bool_0;
		}

		private void method_0()
		{
			if (!Value)
			{
				BackgroundImage = image_1;
				pictureBox1.Left = 0;
			}
			else
			{
				BackgroundImage = image_0;
				pictureBox1.Left = base.Width - pictureBox1.Width;
			}
			if (eventHandler_0 != null)
			{
				eventHandler_0(this, new EventArgs());
			}
		}

		private void BunifuiOSSwitch_Load(object sender, EventArgs e)
		{
			if (base.DesignMode)
			{
				Bunifu.Framework.License.Check(this);
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

		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ns1.BunifuiOSSwitch));
			pictureBox1 = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
			SuspendLayout();
			pictureBox1.Enabled = false;
			pictureBox1.Image = (System.Drawing.Image)resources.GetObject("pictureBox1.Image");
			pictureBox1.Location = new System.Drawing.Point(17, 0);
			pictureBox1.Name = "pictureBox1";
			pictureBox1.Size = new System.Drawing.Size(26, 25);
			pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			pictureBox1.TabIndex = 0;
			pictureBox1.TabStop = false;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			BackColor = System.Drawing.Color.Transparent;
			BackgroundImage = (System.Drawing.Image)resources.GetObject("$this.BackgroundImage");
			BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			base.Controls.Add(pictureBox1);
			Cursor = System.Windows.Forms.Cursors.Hand;
			DoubleBuffered = true;
			base.Name = "BunifuiOSSwitch";
			base.Size = new System.Drawing.Size(43, 25);
			base.Load += new System.EventHandler(BunifuiOSSwitch_Load);
			base.Click += new System.EventHandler(BunifuiOSSwitch_Click);
			base.Resize += new System.EventHandler(BunifuiOSSwitch_Resize);
			((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
			ResumeLayout(false);
		}

		internal static bool UMYWSxIGc7x6vvl8LKQI()
		{
			return AJu34LIGAwBCOOunQmgq == null;
		}
	}
}
