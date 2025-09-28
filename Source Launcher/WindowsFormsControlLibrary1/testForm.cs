using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using ns1;

namespace WindowsFormsControlLibrary1
{
	public class testForm : Form
	{
		private bool bool_0;

		private int int_0 = 1;

		private Drag drag_0 = new Drag();

		private IContainer icontainer_0;

		private BunifuMetroTextbox bunifuMetroTextbox_0;

		private BunifuTrackbar bunifuTrackbar_0;

		private BunifuVTrackbar bunifuVTrackbar_0;

		private Label label1;

		private BunifuVTrackbar bunifuVTrackbar2;

		private BunifuGauge bunifuGauge1;

		private Button button1;

		private System.Windows.Forms.Timer timer_0;

		private BackgroundWorker backgroundWorker_0;

		private Panel panel1;

		private BunifuMaterialTextbox bunifuMaterialTextbox1;

		private BunifuMaterialTextbox bunifuMaterialTextbox2;

		private BunifuDropdown bunifuDropdown1;

		private BunifuThinButton2 bunifuThinButton21;

		private BunifuFormFadeTransition bunifuFormFadeTransition_0;

		internal static testForm XhIlqFIN9rDIrEMkU4ii;

		public testForm()
		{
			InitializeComponent();
		}

		private void yvTeufFhlv(object sender, EventArgs e)
		{
		}

		private void testForm_Paint(object sender, PaintEventArgs e)
		{
		}

		private void method_0(object sender, PaintEventArgs e)
		{
		}

		private void method_1(object sender, EventArgs e)
		{
			Close();
		}

		private void method_2(object sender, EventArgs e)
		{
		}

		private void method_3(object sender, EventArgs e)
		{
		}

		private void method_4(object sender, EventArgs e)
		{
		}

		private void method_5(object sender, EventArgs e)
		{
		}

		private void method_6(object sender, EventArgs e)
		{
		}

		private void method_7(object sender, EventArgs e)
		{
		}

		private void method_8(object sender, EventArgs e)
		{
		}

		private void method_9(object sender, EventArgs e)
		{
		}

		private void testForm_Load(object sender, EventArgs e)
		{
		}

		private void method_10(object sender, EventArgs e)
		{
		}

		private void method_11(object sender, EventArgs e)
		{
			MessageBox.Show("Hello World");
		}

		private void method_12(object sender, EventArgs e)
		{
		}

		private void method_13(object sender, EventArgs e)
		{
		}

		private void method_14(object sender, EventArgs e)
		{
			MessageBox.Show("Yeeeeah");
		}

		private void method_15(object sender, EventArgs e)
		{
		}

		private void method_16(object sender, MouseEventArgs e)
		{
			drag_0.Grab((Control)sender);
		}

		private void method_17(object sender, MouseEventArgs e)
		{
			drag_0.Release();
		}

		private void bunifuVTrackbar2_ValueChanged(object sender, EventArgs e)
		{
			label1.Text = bunifuVTrackbar2.Value.ToString();
		}

		private void bunifuGauge1_Load(object sender, EventArgs e)
		{
		}

		private void button1_Click(object sender, EventArgs e)
		{
			bunifuFormFadeTransition_0.ShowAsyc(this);
		}

		private void timer_0_Tick(object sender, EventArgs e)
		{
			panel1.Width = 250;
			panel1.Height = 250;
			try
			{
				backgroundWorker_0.RunWorkerAsync();
			}
			catch (Exception)
			{
				backgroundWorker_0.CancelAsync();
			}
		}

		private void backgroundWorker_0_DoWork(object sender, DoWorkEventArgs e)
		{
			int num = panel1.Width;
			while (panel1.Width > 10)
			{
				Thread.Sleep(1);
				backgroundWorker_0.ReportProgress(num);
				num--;
			}
		}

		private void backgroundWorker_0_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			panel1.Width = e.ProgressPercentage;
			panel1.Height = e.ProgressPercentage;
		}

		private void bunifuThinButton21_Click(object sender, EventArgs e)
		{
			MessageBox.Show("Clicked");
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WindowsFormsControlLibrary1.testForm));
			label1 = new System.Windows.Forms.Label();
			button1 = new System.Windows.Forms.Button();
			timer_0 = new System.Windows.Forms.Timer(icontainer_0);
			backgroundWorker_0 = new System.ComponentModel.BackgroundWorker();
			panel1 = new System.Windows.Forms.Panel();
			bunifuThinButton21 = new ns1.BunifuThinButton2();
			bunifuDropdown1 = new ns1.BunifuDropdown();
			bunifuMaterialTextbox2 = new ns1.BunifuMaterialTextbox();
			bunifuMaterialTextbox1 = new ns1.BunifuMaterialTextbox();
			bunifuGauge1 = new ns1.BunifuGauge();
			bunifuVTrackbar2 = new ns1.BunifuVTrackbar();
			bunifuFormFadeTransition_0 = new ns1.BunifuFormFadeTransition(icontainer_0);
			SuspendLayout();
			label1.AutoSize = true;
			label1.Location = new System.Drawing.Point(40, 539);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(35, 13);
			label1.TabIndex = 1;
			label1.Text = "label1";
			button1.Location = new System.Drawing.Point(199, 312);
			button1.Name = "button1";
			button1.Size = new System.Drawing.Size(75, 23);
			button1.TabIndex = 4;
			button1.Text = "button1";
			button1.UseVisualStyleBackColor = true;
			button1.Click += new System.EventHandler(button1_Click);
			timer_0.Tick += new System.EventHandler(timer_0_Tick);
			backgroundWorker_0.WorkerReportsProgress = true;
			backgroundWorker_0.WorkerSupportsCancellation = true;
			backgroundWorker_0.DoWork += new System.ComponentModel.DoWorkEventHandler(backgroundWorker_0_DoWork);
			backgroundWorker_0.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(backgroundWorker_0_ProgressChanged);
			panel1.BackColor = System.Drawing.Color.Indigo;
			panel1.Location = new System.Drawing.Point(669, 312);
			panel1.Name = "panel1";
			panel1.Size = new System.Drawing.Size(409, 255);
			panel1.TabIndex = 6;
			bunifuThinButton21.ActiveBorderThickness = 1;
			bunifuThinButton21.ActiveCornerRadius = 20;
			bunifuThinButton21.ActiveFillColor = System.Drawing.Color.SpringGreen;
			bunifuThinButton21.ActiveForecolor = System.Drawing.Color.White;
			bunifuThinButton21.ActiveLineColor = System.Drawing.Color.SkyBlue;
			bunifuThinButton21.BackColor = System.Drawing.Color.White;
			bunifuThinButton21.BackgroundImage = (System.Drawing.Image)resources.GetObject("bunifuThinButton21.BackgroundImage");
			bunifuThinButton21.ButtonText = "ThinButton";
			bunifuThinButton21.Cursor = System.Windows.Forms.Cursors.Hand;
			bunifuThinButton21.Font = new System.Drawing.Font("Century Gothic", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			bunifuThinButton21.ForeColor = System.Drawing.Color.SeaGreen;
			bunifuThinButton21.IdleBorderThickness = 1;
			bunifuThinButton21.IdleCornerRadius = 20;
			bunifuThinButton21.IdleFillColor = System.Drawing.Color.White;
			bunifuThinButton21.IdleForecolor = System.Drawing.Color.SlateBlue;
			bunifuThinButton21.IdleLineColor = System.Drawing.Color.SteelBlue;
			bunifuThinButton21.Location = new System.Drawing.Point(359, 347);
			bunifuThinButton21.Margin = new System.Windows.Forms.Padding(5);
			bunifuThinButton21.Name = "bunifuThinButton21";
			bunifuThinButton21.Size = new System.Drawing.Size(147, 46);
			bunifuThinButton21.TabIndex = 10;
			bunifuThinButton21.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			bunifuThinButton21.Click += new System.EventHandler(bunifuThinButton21_Click);
			bunifuDropdown1.BackColor = System.Drawing.Color.Transparent;
			bunifuDropdown1.BorderRadius = 5;
			bunifuDropdown1.ForeColor = System.Drawing.Color.White;
			bunifuDropdown1.Items = new string[3] { "All", "Male", "Female" };
			bunifuDropdown1.Location = new System.Drawing.Point(199, 68);
			bunifuDropdown1.Name = "bunifuDropdown1";
			bunifuDropdown1.NomalColor = System.Drawing.Color.Indigo;
			bunifuDropdown1.onHoverColor = System.Drawing.Color.Indigo;
			bunifuDropdown1.selectedIndex = 0;
			bunifuDropdown1.Size = new System.Drawing.Size(217, 35);
			bunifuDropdown1.TabIndex = 9;
			bunifuMaterialTextbox2.Cursor = System.Windows.Forms.Cursors.IBeam;
			bunifuMaterialTextbox2.Font = new System.Drawing.Font("Century Gothic", 9.75f);
			bunifuMaterialTextbox2.ForeColor = System.Drawing.Color.FromArgb(64, 64, 64);
			bunifuMaterialTextbox2.HintForeColor = System.Drawing.Color.Silver;
			bunifuMaterialTextbox2.HintText = "Password";
			bunifuMaterialTextbox2.isPassword = true;
			bunifuMaterialTextbox2.LineFocusedColor = System.Drawing.Color.Indigo;
			bunifuMaterialTextbox2.LineIdleColor = System.Drawing.Color.Gray;
			bunifuMaterialTextbox2.LineMouseHoverColor = System.Drawing.Color.Indigo;
			bunifuMaterialTextbox2.LineThickness = 3;
			bunifuMaterialTextbox2.Location = new System.Drawing.Point(617, 143);
			bunifuMaterialTextbox2.Margin = new System.Windows.Forms.Padding(4);
			bunifuMaterialTextbox2.Name = "bunifuMaterialTextbox2";
			bunifuMaterialTextbox2.Size = new System.Drawing.Size(370, 44);
			bunifuMaterialTextbox2.TabIndex = 8;
			bunifuMaterialTextbox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
			bunifuMaterialTextbox1.Cursor = System.Windows.Forms.Cursors.IBeam;
			bunifuMaterialTextbox1.Font = new System.Drawing.Font("Century Gothic", 9.75f);
			bunifuMaterialTextbox1.ForeColor = System.Drawing.Color.FromArgb(64, 64, 64);
			bunifuMaterialTextbox1.HintForeColor = System.Drawing.Color.Maroon;
			bunifuMaterialTextbox1.HintText = "Username or Email";
			bunifuMaterialTextbox1.isPassword = false;
			bunifuMaterialTextbox1.LineFocusedColor = System.Drawing.Color.Indigo;
			bunifuMaterialTextbox1.LineIdleColor = System.Drawing.Color.Gray;
			bunifuMaterialTextbox1.LineMouseHoverColor = System.Drawing.Color.Indigo;
			bunifuMaterialTextbox1.LineThickness = 3;
			bunifuMaterialTextbox1.Location = new System.Drawing.Point(617, 47);
			bunifuMaterialTextbox1.Margin = new System.Windows.Forms.Padding(4);
			bunifuMaterialTextbox1.Name = "bunifuMaterialTextbox1";
			bunifuMaterialTextbox1.Size = new System.Drawing.Size(370, 44);
			bunifuMaterialTextbox1.TabIndex = 7;
			bunifuMaterialTextbox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
			bunifuGauge1.BackgroundImage = (System.Drawing.Image)resources.GetObject("bunifuGauge1.BackgroundImage");
			bunifuGauge1.Font = new System.Drawing.Font("Century Gothic", 15.75f);
			bunifuGauge1.Location = new System.Drawing.Point(171, 143);
			bunifuGauge1.Margin = new System.Windows.Forms.Padding(6);
			bunifuGauge1.Name = "bunifuGauge1";
			bunifuGauge1.ProgressBgColor = System.Drawing.Color.Gray;
			bunifuGauge1.ProgressColor1 = System.Drawing.Color.SeaGreen;
			bunifuGauge1.ProgressColor2 = System.Drawing.Color.Tomato;
			bunifuGauge1.Size = new System.Drawing.Size(174, 117);
			bunifuGauge1.TabIndex = 3;
			bunifuGauge1.Thickness = 30;
			bunifuGauge1.Value = 0;
			bunifuGauge1.Load += new System.EventHandler(bunifuGauge1_Load);
			bunifuVTrackbar2.BackColor = System.Drawing.Color.Transparent;
			bunifuVTrackbar2.BackgroudColor = System.Drawing.Color.DarkGray;
			bunifuVTrackbar2.BorderRadius = 0;
			bunifuVTrackbar2.IndicatorColor = System.Drawing.Color.Indigo;
			bunifuVTrackbar2.Location = new System.Drawing.Point(47, 99);
			bunifuVTrackbar2.MaximumValue = 100;
			bunifuVTrackbar2.Name = "bunifuVTrackbar2";
			bunifuVTrackbar2.Size = new System.Drawing.Size(28, 415);
			bunifuVTrackbar2.SliderRadius = 5;
			bunifuVTrackbar2.TabIndex = 2;
			bunifuVTrackbar2.Value = 50;
			bunifuVTrackbar2.ValueChanged += new System.EventHandler(bunifuVTrackbar2_ValueChanged);
			bunifuFormFadeTransition_0.Delay = 1;
			BackColor = System.Drawing.Color.White;
			base.ClientSize = new System.Drawing.Size(1079, 579);
			base.Controls.Add(bunifuThinButton21);
			base.Controls.Add(bunifuDropdown1);
			base.Controls.Add(bunifuMaterialTextbox2);
			base.Controls.Add(bunifuMaterialTextbox1);
			base.Controls.Add(panel1);
			base.Controls.Add(button1);
			base.Controls.Add(bunifuGauge1);
			base.Controls.Add(bunifuVTrackbar2);
			base.Controls.Add(label1);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			base.Name = "testForm";
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			base.Load += new System.EventHandler(testForm_Load);
			base.Paint += new System.Windows.Forms.PaintEventHandler(testForm_Paint);
			ResumeLayout(false);
			PerformLayout();
		}

		internal static bool hXYvNkINZyuP88K4uOUE()
		{
			return XhIlqFIN9rDIrEMkU4ii == null;
		}
	}
}
