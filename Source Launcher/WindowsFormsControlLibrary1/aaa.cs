using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsControlLibrary1
{
	public class aaa : UserControl
	{
		private IContainer icontainer_0;

		private Button button1;

		internal static aaa qZKWf5INYUrEiEZepOX4;

		public aaa()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			new testForm().ShowDialog();
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
			button1 = new System.Windows.Forms.Button();
			SuspendLayout();
			button1.Location = new System.Drawing.Point(49, 57);
			button1.Name = "button1";
			button1.Size = new System.Drawing.Size(75, 23);
			button1.TabIndex = 0;
			button1.Text = "button1";
			button1.UseVisualStyleBackColor = true;
			button1.Click += new System.EventHandler(button1_Click);
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Controls.Add(button1);
			base.Name = "aaa";
			ResumeLayout(false);
		}

		internal static void ltbS6NINHLu1crCRyjhm()
		{
		}

		internal static bool HL3wZqINFkTUthfbOYjB()
		{
			return qZKWf5INYUrEiEZepOX4 == null;
		}
	}
}
