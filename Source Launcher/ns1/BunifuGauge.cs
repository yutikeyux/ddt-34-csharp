using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Windows.Forms;
using Bunifu.Framework;

namespace ns1
{
	public class BunifuGauge : UserControl
	{
		private Color color_0 = Color.Gray;

		private Color color_1 = Color.SeaGreen;

		private int int_0;

		private int int_1 = 30;

		private Color color_2 = Color.SeaGreen;

		private Color color_3 = Color.Tomato;

		private IContainer icontainer_0;

		private Label lblpass;

		private Label lblmin;

		private Label lblmax;

		private BunifuColorTransition bunifuColorTransition_0;

		private static BunifuGauge pKuSMXIUq4YkaJapWQc2;

		public Color ProgressColor1
		{
			get
			{
				return color_2;
			}
			set
			{
				color_2 = value;
				bunifuColorTransition_0.Color1 = color_2;
				method_0(int_0);
			}
		}

		public Color ProgressColor2
		{
			get
			{
				return color_3;
			}
			set
			{
				color_3 = value;
				bunifuColorTransition_0.Color2 = color_3;
				method_0(int_0);
			}
		}

		public Color ProgressBgColor
		{
			get
			{
				return color_0;
			}
			set
			{
				color_0 = value;
				method_0(int_0);
			}
		}

		public int Value
		{
			get
			{
				return int_0;
			}
			set
			{
				if (value <= 100)
				{
					int_0 = value;
					bunifuColorTransition_0.ProgessValue = int_0;
					method_0(int_0);
				}
			}
		}

		public int Thickness
		{
			get
			{
				return int_1;
			}
			set
			{
				int_1 = value;
				method_0(int_0);
			}
		}

		public BunifuGauge()
		{
			InitializeComponent();
			GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(this, true, null);
			BunifuGauge_Resize(this, new EventArgs());
			method_0(int_0);
		}

		private void method_0(int int_2)
		{
			Bitmap bitmap = new Bitmap(base.Size.Width, base.Size.Height);
			Graphics graphics = Graphics.FromImage(bitmap);
			graphics.SmoothingMode = SmoothingMode.HighQuality;
			graphics.Clear(Color.Transparent);
			Pen pen = new Pen(color_0, int_1);
			int num = base.Size.Width - int_1 * 2;
			Rectangle rect = new Rectangle(int_1, base.Size.Height / 4, num, num);
			Pen pen2 = new Pen(color_1, int_1);
			Graphics graphics2 = Graphics.FromImage(bitmap);
			graphics2.SmoothingMode = SmoothingMode.HighQuality;
			graphics2.DrawArc(pen, rect, 180f, 180f);
			lblpass.Text = int_2 + "%";
			graphics2.DrawArc(pen2, rect, 180f, method_1(int_2));
			BackgroundImage = bitmap;
		}

		private int method_1(int int_2)
		{
			return int.Parse(Math.Round((double)int_2 * 180.0 / 100.0, 0).ToString());
		}

		private void BunifuGauge_Resize(object sender, EventArgs e)
		{
			method_0(int_0);
			lblpass.Top = base.Height - lblpass.Height - 30;
			int num3 = (lblmin.Top = (lblmax.Top = base.Height - lblmax.Height - 10));
			lblmin.Left = 20;
			lblmax.Left = base.Size.Width - lblmax.Width - 20;
			lblpass.Left = base.Width / 2 - lblpass.Width / 2;
		}

		private void JkYrwWxpaj(object sender, EventArgs e)
		{
			Label label = lblpass;
			Label label2 = lblmin;
			Color color = (lblmax.ForeColor = ForeColor);
			Color color4 = (label.ForeColor = (label2.ForeColor = color));
			method_0(int_0);
		}

		private void BunifuGauge_FontChanged(object sender, EventArgs e)
		{
			lblpass.Font = Font;
			method_0(int_0);
		}

		private void bunifuColorTransition_0_OnValueChange(object sender, EventArgs e)
		{
			color_1 = bunifuColorTransition_0.Value;
		}

		private void BunifuGauge_Load(object sender, EventArgs e)
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
			icontainer_0 = new System.ComponentModel.Container();
			lblpass = new System.Windows.Forms.Label();
			lblmin = new System.Windows.Forms.Label();
			lblmax = new System.Windows.Forms.Label();
			bunifuColorTransition_0 = new ns1.BunifuColorTransition(icontainer_0);
			SuspendLayout();
			lblpass.AutoSize = true;
			lblpass.Font = new System.Drawing.Font("Century Gothic", 15.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			lblpass.Location = new System.Drawing.Point(83, 34);
			lblpass.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			lblpass.Name = "lblpass";
			lblpass.Size = new System.Drawing.Size(38, 24);
			lblpass.TabIndex = 1;
			lblpass.Text = "0%";
			lblmin.AutoSize = true;
			lblmin.Font = new System.Drawing.Font("Century Gothic", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			lblmin.Location = new System.Drawing.Point(26, 86);
			lblmin.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			lblmin.Name = "lblmin";
			lblmin.Size = new System.Drawing.Size(15, 17);
			lblmin.TabIndex = 2;
			lblmin.Text = "0";
			lblmax.AutoSize = true;
			lblmax.Font = new System.Drawing.Font("Century Gothic", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			lblmax.Location = new System.Drawing.Point(145, 86);
			lblmax.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			lblmax.Name = "lblmax";
			lblmax.Size = new System.Drawing.Size(29, 17);
			lblmax.TabIndex = 3;
			lblmax.Text = "100";
			bunifuColorTransition_0.Color1 = System.Drawing.Color.SeaGreen;
			bunifuColorTransition_0.Color2 = System.Drawing.Color.Tomato;
			bunifuColorTransition_0.ProgessValue = 0;
			bunifuColorTransition_0.OnValueChange += new System.EventHandler(bunifuColorTransition_0_OnValueChange);
			base.AutoScaleDimensions = new System.Drawing.SizeF(12f, 24f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Controls.Add(lblmax);
			base.Controls.Add(lblmin);
			base.Controls.Add(lblpass);
			Font = new System.Drawing.Font("Century Gothic", 15.75f);
			base.Margin = new System.Windows.Forms.Padding(6);
			base.Name = "BunifuGauge";
			base.Size = new System.Drawing.Size(174, 117);
			base.Load += new System.EventHandler(BunifuGauge_Load);
			base.FontChanged += new System.EventHandler(BunifuGauge_FontChanged);
			base.ForeColorChanged += new System.EventHandler(JkYrwWxpaj);
			base.Resize += new System.EventHandler(BunifuGauge_Resize);
			ResumeLayout(false);
			PerformLayout();
		}

		internal static void okaiW0IUN1Rnxi3BxkKJ()
		{
		}

		internal static bool v0O2gDIULUcnY3Z4jmCh()
		{
			return pKuSMXIUq4YkaJapWQc2 == null;
		}
	}
}
