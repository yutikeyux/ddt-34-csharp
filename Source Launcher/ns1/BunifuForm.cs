using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Bunifu.Framework.Lib;

namespace ns1
{
	[DebuggerStepThrough]
	public class BunifuForm : Form
	{
		private int int_0;

		private IContainer icontainer_0;

		private static BunifuForm mOJXZVIUfr2fJOXxlm8k;

		public int BorderRadius
		{
			get
			{
				return int_0;
			}
			set
			{
				int_0 = value;
				Elipse.Apply(this, int_0);
			}
		}

		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams obj = base.CreateParams;
				obj.ClassStyle |= 131072;
				obj.ExStyle |= 33554432;
				return obj;
			}
		}

		public BunifuForm()
		{
			InitializeComponent();
			base.FormBorderStyle = FormBorderStyle.None;
		}

		protected override void OnResize(EventArgs e)
		{
			Elipse.Apply(this, int_0);
			base.OnResize(e);
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
			SuspendLayout();
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(284, 261);
			base.Name = "BunifuForm";
			Text = "BunifuForm";
			ResumeLayout(false);
		}

		internal static bool CmYmI1IUUc435txuobAR()
		{
			return mOJXZVIUfr2fJOXxlm8k == null;
		}
	}
}
