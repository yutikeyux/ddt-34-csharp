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
	[DebuggerStepThrough]
	[DefaultEvent("OnTextChange")]
	[ProvideProperty("BunifuFramework", typeof(Control))]
	public class BunifuTextbox : UserControl
	{
		[CompilerGenerated]
		private EventHandler eventHandler_0;

		[CompilerGenerated]
		private EventHandler eventHandler_1;

		[CompilerGenerated]
		private EventHandler eventHandler_2;

		[CompilerGenerated]
		private EventHandler eventHandler_3;

		private IContainer icontainer_0;

		private PictureBox _Picture;

		public TextBox _TextBox;

		private static BunifuTextbox wEOxwXIG0m6Sa1tdAd7K;

		public Image Icon
		{
			get
			{
				return _Picture.Image;
			}
			set
			{
				_Picture.Image = value;
				_Picture.Image = Class7.smethod_1(_Picture.Image, ForeColor);
			}
		}

		public string text
		{
			get
			{
				return _TextBox.Text;
			}
			set
			{
				_TextBox.Text = value;
			}
		}

		public event EventHandler OnTextChange
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

		public new event EventHandler KeyDown
		{
			[CompilerGenerated]
			add
			{
				EventHandler eventHandler = eventHandler_1;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
					eventHandler = Interlocked.CompareExchange(ref eventHandler_1, value2, eventHandler2);
				}
				while ((object)eventHandler != eventHandler2);
			}
			[CompilerGenerated]
			remove
			{
				EventHandler eventHandler = eventHandler_1;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
					eventHandler = Interlocked.CompareExchange(ref eventHandler_1, value2, eventHandler2);
				}
				while ((object)eventHandler != eventHandler2);
			}
		}

		public new event EventHandler KeyPress
		{
			[CompilerGenerated]
			add
			{
				EventHandler eventHandler = eventHandler_2;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
					eventHandler = Interlocked.CompareExchange(ref eventHandler_2, value2, eventHandler2);
				}
				while ((object)eventHandler != eventHandler2);
			}
			[CompilerGenerated]
			remove
			{
				EventHandler eventHandler = eventHandler_2;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
					eventHandler = Interlocked.CompareExchange(ref eventHandler_2, value2, eventHandler2);
				}
				while ((object)eventHandler != eventHandler2);
			}
		}

		public new event EventHandler KeyUp
		{
			[CompilerGenerated]
			add
			{
				EventHandler eventHandler = eventHandler_3;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
					eventHandler = Interlocked.CompareExchange(ref eventHandler_3, value2, eventHandler2);
				}
				while ((object)eventHandler != eventHandler2);
			}
			[CompilerGenerated]
			remove
			{
				EventHandler eventHandler = eventHandler_3;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
					eventHandler = Interlocked.CompareExchange(ref eventHandler_3, value2, eventHandler2);
				}
				while ((object)eventHandler != eventHandler2);
			}
		}

		public BunifuTextbox()
		{
			InitializeComponent();
			_Picture.Top = base.Height / 2 - _Picture.Height / 2;
			_TextBox.Top = base.Height / 2 - _TextBox.Height / 2;
			_TextBox.Width = base.Width - _TextBox.Left - 10;
			BackgroundImage = Class7.smethod_1(BackgroundImage, ForeColor);
			_Picture.Image = Class7.smethod_1(_Picture.Image, ForeColor);
			Bunifu.Framework.License.Check(this);
		}

		private void BunifuTextbox_ForeColorChanged(object sender, EventArgs e)
		{
			_TextBox.ForeColor = ForeColor;
			BackgroundImage = Class7.smethod_1(BackgroundImage, ForeColor);
			_Picture.Image = Class7.smethod_1(_Picture.Image, ForeColor);
		}

		private void BunifuTextbox_Resize(object sender, EventArgs e)
		{
			_Picture.Top = base.Height / 2 - _Picture.Height / 2;
			_TextBox.Top = base.Height / 2 - _TextBox.Height / 2;
			_TextBox.Width = base.Width - _TextBox.Left - 10;
		}

		private void BunifuTextbox_BackColorChanged(object sender, EventArgs e)
		{
			_TextBox.BackColor = BackColor;
		}

		private void _TextBox_TextChanged(object sender, EventArgs e)
		{
			if (eventHandler_0 != null)
			{
				eventHandler_0(this, e);
			}
		}

		private void BunifuTextbox_Load(object sender, EventArgs e)
		{
			if (base.DesignMode)
			{
				Bunifu.Framework.License.Check(this);
			}
		}

		private void _TextBox_KeyDown(object sender, KeyEventArgs e)
		{
			if (eventHandler_1 != null)
			{
				eventHandler_1(this, e);
			}
		}

		private void _TextBox_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (eventHandler_2 != null)
			{
				eventHandler_2(this, e);
			}
		}

		private void _TextBox_KeyUp(object sender, KeyEventArgs e)
		{
			if (eventHandler_3 != null)
			{
				eventHandler_3(this, e);
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ns1.BunifuTextbox));
			_TextBox = new System.Windows.Forms.TextBox();
			_Picture = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)_Picture).BeginInit();
			SuspendLayout();
			_TextBox.BackColor = System.Drawing.Color.Silver;
			_TextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			_TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 11f);
			_TextBox.ForeColor = System.Drawing.Color.SeaGreen;
			_TextBox.Location = new System.Drawing.Point(40, 13);
			_TextBox.Multiline = true;
			_TextBox.Name = "_TextBox";
			_TextBox.Size = new System.Drawing.Size(195, 20);
			_TextBox.TabIndex = 0;
			_TextBox.Text = "Bunifu TextBox";
			_TextBox.TextChanged += new System.EventHandler(_TextBox_TextChanged);
			_TextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(_TextBox_KeyDown);
			_TextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(_TextBox_KeyPress);
			_TextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(_TextBox_KeyUp);
			_Picture.BackColor = System.Drawing.Color.Transparent;
			_Picture.Image = (System.Drawing.Image)resources.GetObject("_Picture.Image");
			_Picture.Location = new System.Drawing.Point(9, 7);
			_Picture.Name = "_Picture";
			_Picture.Size = new System.Drawing.Size(23, 25);
			_Picture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			_Picture.TabIndex = 1;
			_Picture.TabStop = false;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			BackColor = System.Drawing.Color.Silver;
			BackgroundImage = (System.Drawing.Image)resources.GetObject("$this.BackgroundImage");
			BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			base.Controls.Add(_Picture);
			base.Controls.Add(_TextBox);
			DoubleBuffered = true;
			ForeColor = System.Drawing.Color.SeaGreen;
			base.Name = "BunifuTextbox";
			base.Size = new System.Drawing.Size(250, 42);
			base.Load += new System.EventHandler(BunifuTextbox_Load);
			base.BackColorChanged += new System.EventHandler(BunifuTextbox_BackColorChanged);
			base.ForeColorChanged += new System.EventHandler(BunifuTextbox_ForeColorChanged);
			base.Resize += new System.EventHandler(BunifuTextbox_Resize);
			((System.ComponentModel.ISupportInitialize)_Picture).EndInit();
			ResumeLayout(false);
			PerformLayout();
		}

		internal static void K01bvSIGEi5H507lq7cm()
		{
		}

		internal static bool ED8pPSIGuI02WJXC3dMr()
		{
			return wEOxwXIG0m6Sa1tdAd7K == null;
		}
	}
}
