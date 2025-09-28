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
	[ProvideProperty("BunifuFramework", typeof(Control))]
	[DefaultEvent("onItemSelected")]
	public class BunifuDropdown : UserControl
	{
		public int _BorderRadius = 3;

		[CompilerGenerated]
		private EventHandler eventHandler_0;

		[CompilerGenerated]
		private EventHandler eventHandler_1;

		[CompilerGenerated]
		private EventHandler eventHandler_2;

		private string[] string_0 = new string[0];

		private IContainer icontainer_0;

		public BunifuFlatButton Style;

		private ComboBox Collections;

		internal static BunifuDropdown JS4wwWIfNxQtMiytWJks;

		public int BorderRadius
		{
			get
			{
				return _BorderRadius;
			}
			set
			{
				_BorderRadius = value;
				Style.BorderRadius = _BorderRadius;
			}
		}

		public int selectedIndex
		{
			get
			{
				return Collections.SelectedIndex;
			}
			set
			{
				if (Collections.Items.Count > value && value >= 0)
				{
					Collections.SelectedIndex = value;
					Style.ButtonText = "    " + Collections.Items[value].ToString();
				}
				else if (value != -1)
				{
					throw new Exception("Out of index");
				}
			}
		}

		public string selectedValue => Collections.Items[selectedIndex].ToString().Trim();

		public string[] Items
		{
			get
			{
				return string_0;
			}
			set
			{
				string_0 = value;
				Collections.Items.Clear();
				for (int i = 0; i < string_0.Length; i++)
				{
					Collections.Items.Add(string_0[i]);
				}
			}
		}

		public Color onHoverColor
		{
			get
			{
				return Style.OnHovercolor;
			}
			set
			{
				Style.OnHovercolor = value;
			}
		}

		public Color NomalColor
		{
			get
			{
				return Style.Normalcolor;
			}
			set
			{
				Style.Normalcolor = value;
				Style.Activecolor = value;
			}
		}

		public event EventHandler onItemSelected
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

		public event EventHandler onItemAdded
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

		public event EventHandler onItemRemoved
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

		public BunifuDropdown()
		{
			InitializeComponent();
			if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
			{
				Collections.Visible = false;
			}
			else
			{
				Collections.Visible = true;
			}
			if (Collections.Items.Count > 0)
			{
				Collections.SelectedIndex = 0;
			}
			Style.ButtonText = Collections.Text;
			OnResize(null);
			Bunifu.Framework.License.Check(this);
		}

		public void AddItem(string Item)
		{
			Collections.Items.Add(Item);
		}

		public void RemoveItem(string Item)
		{
			Collections.Items.Remove(Item);
			if (eventHandler_2 != null)
			{
				eventHandler_2(this, null);
			}
		}

		public void RemoveAt(int index)
		{
			Collections.Items.RemoveAt(index);
			if (selectedIndex == index)
			{
				Style.Text = "";
			}
			if (eventHandler_2 != null)
			{
				eventHandler_2(this, null);
			}
		}

		public void Clear()
		{
			Collections.Items.Clear();
			string_0 = new string[0];
			Style.Text = "";
		}

		private void BunifuDropdown_Resize(object sender, EventArgs e)
		{
		}

		private void Style_Click(object sender, EventArgs e)
		{
			Collections.Select();
			SendKeys.Send("%{DOWN}");
		}

		private void BunifuDropdown_FontChanged(object sender, EventArgs e)
		{
			Style.Font = Font;
		}

		private void BunifuDropdown_ForeColorChanged(object sender, EventArgs e)
		{
			Style.Textcolor = ForeColor;
			Style.OnHoverTextColor = ForeColor;
			if (Style.Iconimage_right != null)
			{
				Style.Iconimage_right = Class7.smethod_1(Style.Iconimage_right, ForeColor);
			}
			if (Style.Iconimage != null)
			{
				Style.Iconimage = Class7.smethod_1(Style.Iconimage, ForeColor);
			}
		}

		private void cwuhEltLnR(object sender, EventArgs e)
		{
			Style.ButtonText = "   " + Collections.Text;
			if (eventHandler_0 != null)
			{
				eventHandler_0(this, null);
			}
		}

		private void Collections_SelectionChangeCommitted(object sender, EventArgs e)
		{
		}

		private void BunifuDropdown_Load(object sender, EventArgs e)
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ns1.BunifuDropdown));
			Collections = new System.Windows.Forms.ComboBox();
			Style = new ns1.BunifuFlatButton();
			SuspendLayout();
			Collections.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			Collections.FormattingEnabled = true;
			Collections.Location = new System.Drawing.Point(7, 12);
			Collections.Name = "Collections";
			Collections.Size = new System.Drawing.Size(201, 21);
			Collections.TabIndex = 1;
			Collections.SelectedIndexChanged += new System.EventHandler(cwuhEltLnR);
			Collections.SelectionChangeCommitted += new System.EventHandler(Collections_SelectionChangeCommitted);
			Style.Activecolor = System.Drawing.Color.FromArgb(46, 139, 87);
			Style.BackColor = System.Drawing.Color.FromArgb(46, 139, 87);
			Style.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			Style.BorderRadius = 0;
			Style.ButtonText = "     DropDown";
			Style.Cursor = System.Windows.Forms.Cursors.Hand;
			Style.DisabledColor = System.Drawing.Color.Gray;
			Style.Dock = System.Windows.Forms.DockStyle.Fill;
			Style.Iconcolor = System.Drawing.Color.Transparent;
			Style.Iconimage = null;
			Style.Iconimage_right = (System.Drawing.Image)resources.GetObject("Style.Iconimage_right");
			Style.Iconimage_right_Selected = null;
			Style.Iconimage_Selected = null;
			Style.IconRightVisible = true;
			Style.IconRightZoom = 0.0;
			Style.IconVisible = true;
			Style.IconZoom = 90.0;
			Style.IsTab = false;
			Style.Location = new System.Drawing.Point(0, 0);
			Style.Name = "Style";
			Style.Normalcolor = System.Drawing.Color.FromArgb(46, 139, 87);
			Style.OnHovercolor = System.Drawing.Color.FromArgb(36, 129, 77);
			Style.OnHoverTextColor = System.Drawing.Color.White;
			Style.selected = false;
			Style.Size = new System.Drawing.Size(217, 35);
			Style.TabIndex = 2;
			Style.Text = "     DropDown";
			Style.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			Style.Textcolor = System.Drawing.Color.White;
			Style.TextFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			Style.Click += new System.EventHandler(Style_Click);
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			BackColor = System.Drawing.Color.Transparent;
			base.Controls.Add(Style);
			base.Controls.Add(Collections);
			ForeColor = System.Drawing.Color.White;
			base.Name = "BunifuDropdown";
			base.Size = new System.Drawing.Size(217, 35);
			base.Load += new System.EventHandler(BunifuDropdown_Load);
			base.FontChanged += new System.EventHandler(BunifuDropdown_FontChanged);
			base.ForeColorChanged += new System.EventHandler(BunifuDropdown_ForeColorChanged);
			base.Resize += new System.EventHandler(BunifuDropdown_Resize);
			ResumeLayout(false);
		}

		internal static void rqJPwoIfMIHiOFxXBIyi()
		{
		}

		internal static bool IMfK6XIfXJiXxH4kytqv()
		{
			return JS4wwWIfNxQtMiytWJks == null;
		}
	}
}
