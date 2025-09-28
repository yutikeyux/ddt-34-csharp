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
	[ProvideProperty("BunifuFramework", typeof(Control))]
	[DebuggerStepThrough]
	[DefaultEvent("onValueChanged")]
	public class BunifuRating : UserControl
	{
		private int int_0;

		[CompilerGenerated]
		private EventHandler eventHandler_0;

		private IContainer icontainer_0;

		private PictureBox star1;

		private PictureBox star2;

		private PictureBox star3;

		private PictureBox star4;

		private PictureBox star5;

		private PictureBox off;

		private PictureBox on;

		private PictureBox offOrig;

		private PictureBox onOrig;

		private static BunifuRating cT3n5yIGH8tfrC1DmvC3;

		public int Value
		{
			get
			{
				return int_0;
			}
			set
			{
				if (value < 0 && value > 5)
				{
					throw new Exception("Invalid Value ( >=0 || <=5)");
				}
				int_0 = value;
				method_0();
				method_1();
			}
		}

		public event EventHandler onValueChanged
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

		public BunifuRating()
		{
			InitializeComponent();
			OnForeColorChanged(null);
			_ = LicenseManager.UsageMode;
			Bunifu.Framework.License.Check(this);
		}

		private void BunifuRating_Resize(object sender, EventArgs e)
		{
			PictureBox pictureBox = star1;
			PictureBox pictureBox2 = star2;
			PictureBox pictureBox3 = star3;
			PictureBox pictureBox4 = star4;
			int num2 = (star5.Height = base.Height);
			int num4 = (pictureBox4.Height = num2);
			int num6 = (pictureBox3.Height = num4);
			int num9 = (pictureBox.Height = (pictureBox2.Height = num6));
			PictureBox pictureBox5 = star1;
			PictureBox pictureBox6 = star2;
			PictureBox pictureBox7 = star3;
			PictureBox pictureBox8 = star4;
			num2 = (star5.Width = base.Height);
			num4 = (pictureBox8.Width = num2);
			num6 = (pictureBox7.Width = num4);
			num9 = (pictureBox5.Width = (pictureBox6.Width = num6));
			int num15 = (base.Width - base.Height * 5) / 4;
			star2.Left = star1.Right + num15;
			star3.Left = star2.Right + num15;
			star4.Left = star3.Right + num15;
			star5.Left = star4.Right + num15;
		}

		private void BunifuRating_ForeColorChanged(object sender, EventArgs e)
		{
			on.Image = onOrig.Image;
			off.Image = offOrig.Image;
			on.Image = Class7.smethod_1(on.Image, ForeColor);
			off.Image = Class7.smethod_1(off.Image, ForeColor);
			if (!(star1.Tag.ToString() == "on"))
			{
				star1.Image = off.Image;
			}
			else
			{
				star1.Image = on.Image;
			}
			if (!(star2.Tag.ToString() == "on"))
			{
				star2.Image = off.Image;
			}
			else
			{
				star2.Image = on.Image;
			}
			if (!(star3.Tag.ToString() == "on"))
			{
				star3.Image = off.Image;
			}
			else
			{
				star3.Image = on.Image;
			}
			if (!(star4.Tag.ToString() == "on"))
			{
				star4.Image = off.Image;
			}
			else
			{
				star4.Image = on.Image;
			}
			if (star5.Tag.ToString() == "on")
			{
				star5.Image = on.Image;
			}
			else
			{
				star5.Image = off.Image;
			}
		}

		private void method_0()
		{
			switch (int_0)
			{
			case 0:
				star1.Image = off.Image;
				star2.Image = off.Image;
				star3.Image = off.Image;
				star4.Image = off.Image;
				star5.Image = off.Image;
				break;
			case 1:
				star1.Image = on.Image;
				star2.Image = off.Image;
				star3.Image = off.Image;
				star4.Image = off.Image;
				star5.Image = off.Image;
				break;
			case 2:
				star1.Image = on.Image;
				star2.Image = on.Image;
				star3.Image = off.Image;
				star4.Image = off.Image;
				star5.Image = off.Image;
				break;
			case 3:
				star1.Image = on.Image;
				star2.Image = on.Image;
				star3.Image = on.Image;
				star4.Image = off.Image;
				star5.Image = off.Image;
				break;
			case 4:
				star1.Image = on.Image;
				star2.Image = on.Image;
				star3.Image = on.Image;
				star4.Image = on.Image;
				star5.Image = off.Image;
				break;
			case 5:
				star1.Image = on.Image;
				star2.Image = on.Image;
				star3.Image = on.Image;
				star4.Image = on.Image;
				star5.Image = on.Image;
				break;
			}
		}

		private void star5_Click(object sender, EventArgs e)
		{
			int num = int.Parse(((PictureBox)sender).Tag.ToString());
			if (((PictureBox)sender).Image == on.Image)
			{
				int_0 = num - 1;
				method_0();
				method_1();
			}
			else
			{
				int_0 = num;
				method_0();
				method_1();
			}
		}

		private void method_1()
		{
			if (eventHandler_0 != null)
			{
				eventHandler_0(this, null);
			}
		}

		private void BunifuRating_Load(object sender, EventArgs e)
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ns1.BunifuRating));
			star1 = new System.Windows.Forms.PictureBox();
			star2 = new System.Windows.Forms.PictureBox();
			star3 = new System.Windows.Forms.PictureBox();
			star4 = new System.Windows.Forms.PictureBox();
			star5 = new System.Windows.Forms.PictureBox();
			off = new System.Windows.Forms.PictureBox();
			on = new System.Windows.Forms.PictureBox();
			offOrig = new System.Windows.Forms.PictureBox();
			onOrig = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)star1).BeginInit();
			((System.ComponentModel.ISupportInitialize)star2).BeginInit();
			((System.ComponentModel.ISupportInitialize)star3).BeginInit();
			((System.ComponentModel.ISupportInitialize)star4).BeginInit();
			((System.ComponentModel.ISupportInitialize)star5).BeginInit();
			((System.ComponentModel.ISupportInitialize)off).BeginInit();
			((System.ComponentModel.ISupportInitialize)on).BeginInit();
			((System.ComponentModel.ISupportInitialize)offOrig).BeginInit();
			((System.ComponentModel.ISupportInitialize)onOrig).BeginInit();
			SuspendLayout();
			star1.BackColor = System.Drawing.Color.Transparent;
			star1.Cursor = System.Windows.Forms.Cursors.Hand;
			star1.Image = (System.Drawing.Image)resources.GetObject("star1.Image");
			star1.Location = new System.Drawing.Point(0, 1);
			star1.Name = "star1";
			star1.Size = new System.Drawing.Size(50, 46);
			star1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			star1.TabIndex = 0;
			star1.TabStop = false;
			star1.Tag = "1";
			star1.Click += new System.EventHandler(star5_Click);
			star2.BackColor = System.Drawing.Color.Transparent;
			star2.Cursor = System.Windows.Forms.Cursors.Hand;
			star2.Image = (System.Drawing.Image)resources.GetObject("star2.Image");
			star2.Location = new System.Drawing.Point(66, 1);
			star2.Name = "star2";
			star2.Size = new System.Drawing.Size(50, 46);
			star2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			star2.TabIndex = 1;
			star2.TabStop = false;
			star2.Tag = "2";
			star2.Click += new System.EventHandler(star5_Click);
			star3.BackColor = System.Drawing.Color.Transparent;
			star3.Cursor = System.Windows.Forms.Cursors.Hand;
			star3.Image = (System.Drawing.Image)resources.GetObject("star3.Image");
			star3.Location = new System.Drawing.Point(132, 1);
			star3.Name = "star3";
			star3.Size = new System.Drawing.Size(50, 46);
			star3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			star3.TabIndex = 2;
			star3.TabStop = false;
			star3.Tag = "3";
			star3.Click += new System.EventHandler(star5_Click);
			star4.BackColor = System.Drawing.Color.Transparent;
			star4.Cursor = System.Windows.Forms.Cursors.Hand;
			star4.Image = (System.Drawing.Image)resources.GetObject("star4.Image");
			star4.Location = new System.Drawing.Point(198, 1);
			star4.Name = "star4";
			star4.Size = new System.Drawing.Size(50, 46);
			star4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			star4.TabIndex = 3;
			star4.TabStop = false;
			star4.Tag = "4";
			star4.Click += new System.EventHandler(star5_Click);
			star5.BackColor = System.Drawing.Color.Transparent;
			star5.Cursor = System.Windows.Forms.Cursors.Hand;
			star5.Image = (System.Drawing.Image)resources.GetObject("star5.Image");
			star5.Location = new System.Drawing.Point(264, 1);
			star5.Name = "star5";
			star5.Size = new System.Drawing.Size(50, 46);
			star5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			star5.TabIndex = 4;
			star5.TabStop = false;
			star5.Tag = "5";
			star5.Click += new System.EventHandler(star5_Click);
			off.Cursor = System.Windows.Forms.Cursors.Hand;
			off.Image = (System.Drawing.Image)resources.GetObject("off.Image");
			off.Location = new System.Drawing.Point(44, 46);
			off.Name = "off";
			off.Size = new System.Drawing.Size(82, 36);
			off.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			off.TabIndex = 6;
			off.TabStop = false;
			off.Tag = "false";
			off.Visible = false;
			on.Cursor = System.Windows.Forms.Cursors.Hand;
			on.Image = (System.Drawing.Image)resources.GetObject("on.Image");
			on.Location = new System.Drawing.Point(-22, 46);
			on.Name = "on";
			on.Size = new System.Drawing.Size(82, 36);
			on.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			on.TabIndex = 5;
			on.TabStop = false;
			on.Tag = "false";
			on.Visible = false;
			offOrig.Cursor = System.Windows.Forms.Cursors.Hand;
			offOrig.Image = (System.Drawing.Image)resources.GetObject("offOrig.Image");
			offOrig.Location = new System.Drawing.Point(122, 51);
			offOrig.Name = "offOrig";
			offOrig.Size = new System.Drawing.Size(82, 36);
			offOrig.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			offOrig.TabIndex = 8;
			offOrig.TabStop = false;
			offOrig.Tag = "false";
			offOrig.Visible = false;
			onOrig.Cursor = System.Windows.Forms.Cursors.Hand;
			onOrig.Image = (System.Drawing.Image)resources.GetObject("onOrig.Image");
			onOrig.Location = new System.Drawing.Point(44, 58);
			onOrig.Name = "onOrig";
			onOrig.Size = new System.Drawing.Size(82, 36);
			onOrig.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			onOrig.TabIndex = 7;
			onOrig.TabStop = false;
			onOrig.Tag = "false";
			onOrig.Visible = false;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			BackColor = System.Drawing.Color.Transparent;
			base.Controls.Add(offOrig);
			base.Controls.Add(onOrig);
			base.Controls.Add(off);
			base.Controls.Add(on);
			base.Controls.Add(star5);
			base.Controls.Add(star4);
			base.Controls.Add(star3);
			base.Controls.Add(star2);
			base.Controls.Add(star1);
			ForeColor = System.Drawing.Color.SeaGreen;
			base.Name = "BunifuRating";
			base.Size = new System.Drawing.Size(316, 50);
			base.Load += new System.EventHandler(BunifuRating_Load);
			base.ForeColorChanged += new System.EventHandler(BunifuRating_ForeColorChanged);
			base.Resize += new System.EventHandler(BunifuRating_Resize);
			((System.ComponentModel.ISupportInitialize)star1).EndInit();
			((System.ComponentModel.ISupportInitialize)star2).EndInit();
			((System.ComponentModel.ISupportInitialize)star3).EndInit();
			((System.ComponentModel.ISupportInitialize)star4).EndInit();
			((System.ComponentModel.ISupportInitialize)star5).EndInit();
			((System.ComponentModel.ISupportInitialize)off).EndInit();
			((System.ComponentModel.ISupportInitialize)on).EndInit();
			((System.ComponentModel.ISupportInitialize)offOrig).EndInit();
			((System.ComponentModel.ISupportInitialize)onOrig).EndInit();
			ResumeLayout(false);
		}

		internal static void VK34BtIG6Ngpuaahf3HF()
		{
		}

		internal static bool A049BRIGlIHbK2PZftXs()
		{
			return cT3n5yIGH8tfrC1DmvC3 == null;
		}
	}
}
