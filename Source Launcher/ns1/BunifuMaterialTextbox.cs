using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
using Bunifu.Framework;

namespace ns1
{
	[DefaultEvent("OnValueChanged")]
	[ProvideProperty("BunifuFramework", typeof(Control))]
	public class BunifuMaterialTextbox : UserControl
	{
		private Color color_0 = Color.Gray;

		private Color color_1 = Color.Blue;

		private Color color_2 = Color.Blue;

		private Color color_3;

		private string string_0 = "";

		[CompilerGenerated]
		private EventHandler eventHandler_0;

		private bool bool_0;

		private bool bool_1;

		private IContainer icontainer_0;

		private TextBox textBox1;

		private System.Windows.Forms.Timer timer_0;

		private Panel panel1;

		private PictureBox line;

		private static BunifuMaterialTextbox TmgKQaIUzXL9KijSoEdq;

		public int LineThickness
		{
			get
			{
				return line.Height;
			}
			set
			{
				if (value <= 0)
				{
					throw new Exception("Value shoud be grater than 0");
				}
				line.Height = value;
			}
		}

		public Color LineIdleColor
		{
			get
			{
				return color_0;
			}
			set
			{
				color_0 = value;
				Refresh();
			}
		}

		public Color LineMouseHoverColor
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

		public Color LineFocusedColor
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

		public Color HintForeColor
		{
			get
			{
				return color_3;
			}
			set
			{
				color_3 = value;
				if (textBox1.Text.Length > 0 && textBox1.Text != HintText)
				{
					textBox1.ForeColor = ForeColor;
					return;
				}
				textBox1.ForeColor = HintForeColor;
				textBox1.Text = HintText;
			}
		}

		public string HintText
		{
			get
			{
				return string_0;
			}
			set
			{
				string_0 = value;
				if (textBox1.Text.Length <= 0 || !(textBox1.Text != HintText))
				{
					textBox1.ForeColor = HintForeColor;
					textBox1.Text = HintText;
				}
				else
				{
					textBox1.ForeColor = ForeColor;
				}
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Bindable(true)]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public override string Text
		{
			get
			{
				if (textBox1.Text != HintText)
				{
					return textBox1.Text;
				}
				return "";
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
				bool_0 = textBox1.UseSystemPasswordChar;
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

		public BunifuMaterialTextbox()
		{
			InitializeComponent();
		}

		private void BunifuMaterialTextbox_Resize(object sender, EventArgs e)
		{
			if (base.Height < textBox1.Height + 5)
			{
				base.Height = textBox1.Height + 5;
			}
			Refresh();
		}

		public new void Refresh()
		{
			if (isOnFocused)
			{
				line.BackColor = color_1;
			}
			else
			{
				line.BackColor = LineIdleColor;
			}
		}

		private void textBox1_TextChanged(object sender, EventArgs e)
		{
			if (eventHandler_0 != null)
			{
				eventHandler_0(this, e);
			}
		}

		private void BunifuMaterialTextbox_BackColorChanged(object sender, EventArgs e)
		{
			textBox1.BackColor = BackColor;
		}

		private void BunifuMaterialTextbox_FontChanged(object sender, EventArgs e)
		{
			textBox1.Font = Font;
			textBox1.Left = 5;
			textBox1.Width = base.Width - textBox1.Left * 2;
			textBox1.Top = base.Height / 2 - textBox1.Height / 2;
		}

		private void BunifuMaterialTextbox_ForeColorChanged(object sender, EventArgs e)
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

		private void BunifuMaterialTextbox_Click(object sender, EventArgs e)
		{
			textBox1.Focus();
		}

		private void BunifuMaterialTextbox_MouseEnter(object sender, EventArgs e)
		{
		}

		private void BunifuMaterialTextbox_MouseLeave(object sender, EventArgs e)
		{
		}

		private void BunifuMaterialTextbox_ParentChanged(object sender, EventArgs e)
		{
		}

		private void method_1(object sender, EventArgs e)
		{
		}

		private void timer_0_Tick(object sender, EventArgs e)
		{
			timer_0.Start();
			Point point = PointToScreen(Point.Empty);
			if (Control.MousePosition.X > point.X && Control.MousePosition.X < point.X + base.Width && Control.MousePosition.Y > point.Y && Control.MousePosition.Y < point.Y + base.Height)
			{
				if (!isOnFocused)
				{
					line.BackColor = LineMouseHoverColor;
				}
				else
				{
					line.BackColor = LineFocusedColor;
					if (textBox1.Text == HintText)
					{
						textBox1.Text = "";
					}
				}
			}
			else if (!isOnFocused)
			{
				line.BackColor = LineIdleColor;
				if (textBox1.Text.Length > 0 && textBox1.Text != HintText)
				{
					textBox1.ForeColor = ForeColor;
					if (bool_0)
					{
						textBox1.UseSystemPasswordChar = true;
						bool_0 = false;
					}
				}
				else if (!textBox1.Focused)
				{
					textBox1.ForeColor = HintForeColor;
					textBox1.Text = HintText;
					if (isPassword)
					{
						textBox1.UseSystemPasswordChar = false;
						bool_0 = true;
					}
				}
			}
			else
			{
				line.BackColor = LineFocusedColor;
			}
			timer_0.Start();
		}

		private void BunifuMaterialTextbox_Load(object sender, EventArgs e)
		{
			if (!base.DesignMode)
			{
				timer_0.Start();
			}
			if (base.DesignMode)
			{
				Bunifu.Framework.License.Check(this);
			}
			if (textBox1.Text.Length <= 0 || !(textBox1.Text != HintText))
			{
				textBox1.ForeColor = HintForeColor;
				textBox1.Text = HintText;
			}
			else
			{
				textBox1.ForeColor = ForeColor;
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
			textBox1 = new System.Windows.Forms.TextBox();
			timer_0 = new System.Windows.Forms.Timer(icontainer_0);
			panel1 = new System.Windows.Forms.Panel();
			line = new System.Windows.Forms.PictureBox();
			panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)line).BeginInit();
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
			panel1.BackColor = System.Drawing.Color.Gray;
			panel1.Controls.Add(line);
			panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			panel1.Location = new System.Drawing.Point(0, 41);
			panel1.Name = "panel1";
			panel1.Size = new System.Drawing.Size(370, 3);
			panel1.TabIndex = 1;
			line.BackColor = System.Drawing.Color.Gray;
			line.Dock = System.Windows.Forms.DockStyle.Bottom;
			line.Enabled = false;
			line.Location = new System.Drawing.Point(0, 0);
			line.Name = "line";
			line.Size = new System.Drawing.Size(370, 3);
			line.TabIndex = 2;
			line.TabStop = false;
			base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 17f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Controls.Add(panel1);
			base.Controls.Add(textBox1);
			Cursor = System.Windows.Forms.Cursors.IBeam;
			Font = new System.Drawing.Font("Century Gothic", 9.75f);
			ForeColor = System.Drawing.Color.FromArgb(64, 64, 64);
			base.Margin = new System.Windows.Forms.Padding(4);
			base.Name = "BunifuMaterialTextbox";
			base.Size = new System.Drawing.Size(370, 44);
			base.Load += new System.EventHandler(BunifuMaterialTextbox_Load);
			base.BackColorChanged += new System.EventHandler(BunifuMaterialTextbox_BackColorChanged);
			base.FontChanged += new System.EventHandler(BunifuMaterialTextbox_FontChanged);
			base.ForeColorChanged += new System.EventHandler(BunifuMaterialTextbox_ForeColorChanged);
			base.Click += new System.EventHandler(BunifuMaterialTextbox_Click);
			base.MouseEnter += new System.EventHandler(BunifuMaterialTextbox_MouseEnter);
			base.MouseLeave += new System.EventHandler(BunifuMaterialTextbox_MouseLeave);
			base.Resize += new System.EventHandler(BunifuMaterialTextbox_Resize);
			base.ParentChanged += new System.EventHandler(BunifuMaterialTextbox_ParentChanged);
			panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)line).EndInit();
			ResumeLayout(false);
			PerformLayout();
		}

		internal static void VByljNIG1CPGMS1h3AMo()
		{
		}

		internal static bool xwA8XTIGPI0RiOAk6EU9()
		{
			return TmgKQaIUzXL9KijSoEdq == null;
		}
	}
}
