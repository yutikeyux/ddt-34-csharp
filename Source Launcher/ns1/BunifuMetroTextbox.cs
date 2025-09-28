using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
using Bunifu.Framework;

namespace ns1
{
	[ProvideProperty("BunifuFramework", typeof(Control))]
	[DefaultEvent("OnValueChanged")]
	public class BunifuMetroTextbox : UserControl
	{
		private int int_0 = 3;

		private Color color_0 = Color.FromArgb(64, 64, 64);

		private Color color_1 = Color.Blue;

		private Color color_2 = Color.Blue;

		private Color color_3 = Color.SeaGreen;

		private Color color_4 = Color.SeaGreen;

		private Graphics graphics_0;

		[CompilerGenerated]
		private EventHandler eventHandler_0;

		private bool bool_0;

		private IContainer icontainer_0;

		private TextBox textBox1;

		private System.Windows.Forms.Timer timer_0;

		internal static BunifuMetroTextbox BD74yXIGf6m40D5JHv5U;

		public int BorderThickness
		{
			get
			{
				return int_0;
			}
			set
			{
				if (value <= 0)
				{
					throw new Exception("Value shoud be grater than 0");
				}
				int_0 = value;
				Refresh();
			}
		}

		public Color BorderColorIdle
		{
			get
			{
				return color_0;
			}
			set
			{
				color_0 = value;
				if (base.DesignMode)
				{
					color_3 = value;
				}
				Refresh();
			}
		}

		public Color BorderColorFocused
		{
			get
			{
				return color_1;
			}
			set
			{
				color_1 = value;
				Refresh();
			}
		}

		public Color BorderColorMouseHover
		{
			get
			{
				return color_2;
			}
			set
			{
				color_2 = value;
				Refresh();
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Bindable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[Browsable(true)]
		public override string Text
		{
			get
			{
				return textBox1.Text;
			}
			set
			{
				textBox1.Text = value;
			}
		}

		public HorizontalAlignment TextAlign
		{
			get
			{
				return textBox1.TextAlign;
			}
			set
			{
				textBox1.TextAlign = value;
			}
		}

		public bool isPassword
		{
			get
			{
				return textBox1.UseSystemPasswordChar;
			}
			set
			{
				textBox1.UseSystemPasswordChar = value;
			}
		}

		public bool isOnFocused => textBox1 == base.ActiveControl;

		public event EventHandler OnValueChanged
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

		public BunifuMetroTextbox()
		{
			InitializeComponent();
		}

		private void BunifuMetroTextbox_Resize(object sender, EventArgs e)
		{
			if (base.Height < textBox1.Height + int_0 * 2)
			{
				base.Height = textBox1.Height + int_0 * 3;
			}
			Refresh();
		}

		public new void Refresh()
		{
			if (color_3 != color_4)
			{
				graphics_0 = CreateGraphics();
				graphics_0.Clear(BackColor);
				Pen pen = new Pen(color_3, int_0);
				Rectangle rect = new Rectangle(int_0, int_0, base.Width - int_0 * 2, base.Height - int_0 * 2);
				graphics_0.DrawRectangle(pen, rect);
				color_4 = color_3;
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			color_3 = BorderColorIdle;
			graphics_0 = CreateGraphics();
			graphics_0.Clear(BackColor);
			Pen pen = new Pen(color_3, int_0);
			Rectangle rect = new Rectangle(int_0, int_0, base.Width - int_0 * 2, base.Height - int_0 * 2);
			graphics_0.DrawRectangle(pen, rect);
			base.OnPaint(e);
		}

		private void textBox1_TextChanged(object sender, EventArgs e)
		{
			if (eventHandler_0 != null)
			{
				eventHandler_0(this, e);
			}
		}

		private void BunifuMetroTextbox_BackColorChanged(object sender, EventArgs e)
		{
			textBox1.BackColor = BackColor;
		}

		private void BunifuMetroTextbox_FontChanged(object sender, EventArgs e)
		{
			textBox1.Font = Font;
			textBox1.Left = int_0 + 5;
			textBox1.Width = base.Width - textBox1.Left * 2;
			textBox1.Top = base.Height / 2 - textBox1.Height / 2;
		}

		private void BunifuMetroTextbox_ForeColorChanged(object sender, EventArgs e)
		{
			textBox1.ForeColor = ForeColor;
		}

		private void textBox1_KeyDown(object sender, KeyEventArgs e)
		{
			OnKeyDown(e);
		}

		private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
		{
			OnKeyPress(e);
		}

		private void textBox1_KeyUp(object sender, KeyEventArgs e)
		{
			OnKeyUp(e);
		}

		private void textBox1_MouseEnter(object sender, EventArgs e)
		{
			OnMouseEnter(e);
		}

		private void method_0(object sender, EventArgs e)
		{
			OnMouseLeave(e);
		}

		private void BunifuMetroTextbox_Click(object sender, EventArgs e)
		{
			textBox1.Focus();
		}

		private void BunifuMetroTextbox_MouseEnter(object sender, EventArgs e)
		{
		}

		private void BunifuMetroTextbox_MouseLeave(object sender, EventArgs e)
		{
		}

		private void method_1(object sender, EventArgs e)
		{
			textBox1.Text = "";
			textBox1.Focus();
		}

		private void method_2(object sender, EventArgs e)
		{
			OnMouseEnter(e);
		}

		private void yPijfIsIk3(object sender, EventArgs e)
		{
		}

		private void BunifuMetroTextbox_Load(object sender, EventArgs e)
		{
			if (!base.DesignMode)
			{
				timer_0.Start();
			}
			if (base.DesignMode)
			{
				Bunifu.Framework.License.Check(this);
			}
		}

		private void timer_0_Tick(object sender, EventArgs e)
		{
			timer_0.Start();
			Point point = PointToScreen(Point.Empty);
			if (Control.MousePosition.X > point.X && Control.MousePosition.X < point.X + base.Width && Control.MousePosition.Y > point.Y && Control.MousePosition.Y < point.Y + base.Height)
			{
				if (!isOnFocused)
				{
					color_3 = BorderColorMouseHover;
					Refresh();
				}
				else
				{
					color_3 = BorderColorFocused;
					Refresh();
				}
			}
			else if (isOnFocused)
			{
				color_3 = BorderColorFocused;
				Refresh();
			}
			else
			{
				color_3 = BorderColorIdle;
				Refresh();
			}
			timer_0.Start();
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
			textBox1 = new System.Windows.Forms.TextBox();
			timer_0 = new System.Windows.Forms.Timer(icontainer_0);
			SuspendLayout();
			textBox1.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			textBox1.BackColor = System.Drawing.SystemColors.Control;
			textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
			textBox1.Font = new System.Drawing.Font("Century Gothic", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			textBox1.ForeColor = System.Drawing.Color.FromArgb(64, 64, 64);
			textBox1.Location = new System.Drawing.Point(8, 15);
			textBox1.Margin = new System.Windows.Forms.Padding(4);
			textBox1.Name = "textBox1";
			textBox1.Size = new System.Drawing.Size(353, 16);
			textBox1.TabIndex = 0;
			textBox1.TextChanged += new System.EventHandler(textBox1_TextChanged);
			textBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(textBox1_KeyDown);
			textBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(textBox1_KeyPress);
			textBox1.KeyUp += new System.Windows.Forms.KeyEventHandler(textBox1_KeyUp);
			textBox1.MouseEnter += new System.EventHandler(textBox1_MouseEnter);
			timer_0.Tick += new System.EventHandler(timer_0_Tick);
			base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 17f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Controls.Add(textBox1);
			Cursor = System.Windows.Forms.Cursors.IBeam;
			Font = new System.Drawing.Font("Century Gothic", 9.75f);
			ForeColor = System.Drawing.Color.FromArgb(64, 64, 64);
			base.Margin = new System.Windows.Forms.Padding(4);
			base.Name = "BunifuMetroTextbox";
			base.Size = new System.Drawing.Size(370, 44);
			base.Load += new System.EventHandler(BunifuMetroTextbox_Load);
			base.BackColorChanged += new System.EventHandler(BunifuMetroTextbox_BackColorChanged);
			base.FontChanged += new System.EventHandler(BunifuMetroTextbox_FontChanged);
			base.ForeColorChanged += new System.EventHandler(BunifuMetroTextbox_ForeColorChanged);
			base.Click += new System.EventHandler(BunifuMetroTextbox_Click);
			base.MouseEnter += new System.EventHandler(BunifuMetroTextbox_MouseEnter);
			base.MouseLeave += new System.EventHandler(BunifuMetroTextbox_MouseLeave);
			base.Resize += new System.EventHandler(BunifuMetroTextbox_Resize);
			base.ParentChanged += new System.EventHandler(yPijfIsIk3);
			ResumeLayout(false);
			PerformLayout();
		}

		internal static void clbT79IGmAqkhR19rKVh()
		{
		}

		internal static bool QqopbNIGUC3D63Q3oD8d()
		{
			return BD74yXIGf6m40D5JHv5U == null;
		}
	}
}
