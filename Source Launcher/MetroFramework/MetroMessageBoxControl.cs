using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using MetroFramework.Controls;
using MetroFramework.Localization;

namespace MetroFramework
{
	public class MetroMessageBoxControl : Form
	{
		private MetroLocalize metroLocalize_0;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Color color_0 = Color.FromArgb(57, 179, 215);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Color color_1 = Color.FromArgb(210, 50, 45);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Color color_2 = Color.FromArgb(237, 156, 40);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Color color_3 = Color.FromArgb(71, 164, 71);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Color color_4 = Color.FromArgb(71, 164, 71);

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private MetroMessageBoxProperties metroMessageBoxProperties_0;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private DialogResult dialogResult_0;

		private IContainer icontainer_0;

		private Panel panelbody;

		private Label titleLabel;

		private Label messageLabel;

		private MetroButton metroButton1;

		private MetroButton metroButton2;

		private MetroButton metroButton3;

		private TableLayoutPanel tlpBody;

		private Panel pnlBottom;

		internal static MetroMessageBoxControl kpp6f2TfuHQ398JOaqB;

		public Panel Body => panelbody;

		public MetroMessageBoxProperties Properties => metroMessageBoxProperties_0;

		public DialogResult Result => dialogResult_0;

		public MetroMessageBoxControl()
		{
			InitializeComponent();
			metroMessageBoxProperties_0 = new MetroMessageBoxProperties(this);
			method_5(metroButton1);
			method_5(metroButton2);
			method_5(metroButton3);
			metroButton1.Click += metroButton3_Click;
			metroButton2.Click += metroButton3_Click;
			metroButton3.Click += metroButton3_Click;
			metroLocalize_0 = new MetroLocalize(this);
		}

		public void ArrangeApperance()
		{
			titleLabel.Text = metroMessageBoxProperties_0.Title;
			messageLabel.Text = metroMessageBoxProperties_0.Message;
			switch (metroMessageBoxProperties_0.Icon)
			{
			case MessageBoxIcon.Exclamation:
				panelbody.BackColor = color_2;
				break;
			case MessageBoxIcon.Hand:
				panelbody.BackColor = color_1;
				break;
			}
			switch (metroMessageBoxProperties_0.Buttons)
			{
			case MessageBoxButtons.OK:
				method_0(metroButton1);
				metroButton1.Text = metroLocalize_0.translate("Ok");
				metroButton1.Location = metroButton3.Location;
				metroButton1.Tag = System.Windows.Forms.DialogResult.OK;
				method_1(metroButton2, bool_0: false);
				method_1(metroButton3, bool_0: false);
				break;
			case MessageBoxButtons.OKCancel:
				method_0(metroButton1);
				metroButton1.Text = metroLocalize_0.translate("Ok");
				metroButton1.Location = metroButton2.Location;
				metroButton1.Tag = System.Windows.Forms.DialogResult.OK;
				method_0(metroButton2);
				metroButton2.Text = metroLocalize_0.translate("Cancel");
				metroButton2.Location = metroButton3.Location;
				metroButton2.Tag = System.Windows.Forms.DialogResult.Cancel;
				method_1(metroButton3, bool_0: false);
				break;
			case MessageBoxButtons.AbortRetryIgnore:
				method_0(metroButton1);
				metroButton1.Text = metroLocalize_0.translate("Abort");
				metroButton1.Tag = System.Windows.Forms.DialogResult.Abort;
				method_0(metroButton2);
				metroButton2.Text = metroLocalize_0.translate("Retry");
				metroButton2.Tag = System.Windows.Forms.DialogResult.Retry;
				method_0(metroButton3);
				metroButton3.Text = metroLocalize_0.translate("Ignore");
				metroButton3.Tag = System.Windows.Forms.DialogResult.Ignore;
				break;
			case MessageBoxButtons.YesNoCancel:
				method_0(metroButton1);
				metroButton1.Text = metroLocalize_0.translate("Yes");
				metroButton1.Tag = System.Windows.Forms.DialogResult.Yes;
				method_0(metroButton2);
				metroButton2.Text = metroLocalize_0.translate("No");
				metroButton2.Tag = System.Windows.Forms.DialogResult.No;
				method_0(metroButton3);
				metroButton3.Text = metroLocalize_0.translate("Cancel");
				metroButton3.Tag = System.Windows.Forms.DialogResult.Cancel;
				break;
			case MessageBoxButtons.YesNo:
				method_0(metroButton1);
				metroButton1.Text = metroLocalize_0.translate("Yes");
				metroButton1.Location = metroButton2.Location;
				metroButton1.Tag = System.Windows.Forms.DialogResult.Yes;
				method_0(metroButton2);
				metroButton2.Text = metroLocalize_0.translate("No");
				metroButton2.Location = metroButton3.Location;
				metroButton2.Tag = System.Windows.Forms.DialogResult.No;
				method_1(metroButton3, bool_0: false);
				break;
			case MessageBoxButtons.RetryCancel:
				method_0(metroButton1);
				metroButton1.Text = metroLocalize_0.translate("Retry");
				metroButton1.Location = metroButton2.Location;
				metroButton1.Tag = System.Windows.Forms.DialogResult.Retry;
				method_0(metroButton2);
				metroButton2.Text = metroLocalize_0.translate("Cancel");
				metroButton2.Location = metroButton3.Location;
				metroButton2.Tag = System.Windows.Forms.DialogResult.Cancel;
				method_1(metroButton3, bool_0: false);
				break;
			}
			switch (metroMessageBoxProperties_0.Icon)
			{
			case MessageBoxIcon.Question:
				panelbody.BackColor = color_4;
				break;
			case MessageBoxIcon.Hand:
				panelbody.BackColor = color_1;
				break;
			default:
				panelbody.BackColor = Color.DarkGray;
				break;
			case MessageBoxIcon.Asterisk:
				panelbody.BackColor = color_0;
				break;
			case MessageBoxIcon.Exclamation:
				panelbody.BackColor = color_2;
				break;
			}
		}

		private void method_0(MetroButton metroButton_0)
		{
			method_1(metroButton_0, bool_0: true);
		}

		private void method_1(MetroButton metroButton_0, bool bool_0)
		{
			metroButton_0.Enabled = bool_0;
			metroButton_0.Visible = bool_0;
		}

		public void SetDefaultButton()
		{
			switch (metroMessageBoxProperties_0.DefaultButton)
			{
			case MessageBoxDefaultButton.Button3:
				if (metroButton3 != null && metroButton3.Enabled)
				{
					metroButton3.Focus();
				}
				break;
			case MessageBoxDefaultButton.Button2:
				if (metroButton2 != null && metroButton2.Enabled)
				{
					metroButton2.Focus();
				}
				break;
			case MessageBoxDefaultButton.Button1:
				if (metroButton1 != null && metroButton1.Enabled)
				{
					metroButton1.Focus();
				}
				break;
			}
		}

		private void method_2(object sender, MouseEventArgs e)
		{
		}

		private void method_3(object sender, EventArgs e)
		{
			method_6((MetroButton)sender, bool_0: true);
		}

		private void method_4(object sender, EventArgs e)
		{
			method_5((MetroButton)sender);
		}

		private void method_5(MetroButton metroButton_0)
		{
			method_6(metroButton_0, bool_0: false);
		}

		private void method_6(MetroButton metroButton_0, bool bool_0)
		{
			metroButton_0.Cursor = Cursors.Hand;
			metroButton_0.MouseClick -= method_2;
			metroButton_0.MouseClick += method_2;
			metroButton_0.MouseEnter -= method_3;
			metroButton_0.MouseEnter += method_3;
			metroButton_0.MouseLeave -= method_4;
			metroButton_0.MouseLeave += method_4;
		}

		private void metroButton3_Click(object sender, EventArgs e)
		{
			MetroButton metroButton = (MetroButton)sender;
			if (metroButton.Enabled)
			{
				dialogResult_0 = (DialogResult)metroButton.Tag;
				Hide();
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
			panelbody = new System.Windows.Forms.Panel();
			tlpBody = new System.Windows.Forms.TableLayoutPanel();
			messageLabel = new System.Windows.Forms.Label();
			titleLabel = new System.Windows.Forms.Label();
			metroButton1 = new MetroFramework.Controls.MetroButton();
			metroButton3 = new MetroFramework.Controls.MetroButton();
			metroButton2 = new MetroFramework.Controls.MetroButton();
			pnlBottom = new System.Windows.Forms.Panel();
			panelbody.SuspendLayout();
			tlpBody.SuspendLayout();
			pnlBottom.SuspendLayout();
			SuspendLayout();
			panelbody.BackColor = System.Drawing.Color.DarkGray;
			panelbody.Controls.Add(tlpBody);
			panelbody.Dock = System.Windows.Forms.DockStyle.Fill;
			panelbody.Location = new System.Drawing.Point(0, 0);
			panelbody.Margin = new System.Windows.Forms.Padding(0);
			panelbody.Name = "panelbody";
			panelbody.Size = new System.Drawing.Size(804, 211);
			panelbody.TabIndex = 2;
			tlpBody.ColumnCount = 3;
			tlpBody.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10f));
			tlpBody.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80f));
			tlpBody.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10f));
			tlpBody.Controls.Add(messageLabel, 1, 2);
			tlpBody.Controls.Add(titleLabel, 1, 1);
			tlpBody.Controls.Add(pnlBottom, 1, 3);
			tlpBody.Dock = System.Windows.Forms.DockStyle.Fill;
			tlpBody.Location = new System.Drawing.Point(0, 0);
			tlpBody.Name = "tlpBody";
			tlpBody.RowCount = 4;
			tlpBody.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5f));
			tlpBody.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25f));
			tlpBody.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100f));
			tlpBody.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40f));
			tlpBody.Size = new System.Drawing.Size(804, 211);
			tlpBody.TabIndex = 6;
			messageLabel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			messageLabel.BackColor = System.Drawing.Color.Transparent;
			messageLabel.ForeColor = System.Drawing.Color.White;
			messageLabel.Location = new System.Drawing.Point(83, 30);
			messageLabel.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
			messageLabel.Name = "messageLabel";
			messageLabel.Size = new System.Drawing.Size(640, 141);
			messageLabel.TabIndex = 0;
			messageLabel.Text = "message here";
			titleLabel.AutoSize = true;
			titleLabel.BackColor = System.Drawing.Color.Transparent;
			titleLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			titleLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
			titleLabel.Location = new System.Drawing.Point(80, 5);
			titleLabel.Margin = new System.Windows.Forms.Padding(0);
			titleLabel.Name = "titleLabel";
			titleLabel.Size = new System.Drawing.Size(125, 25);
			titleLabel.TabIndex = 1;
			titleLabel.Text = "message title";
			metroButton1.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			metroButton1.BackColor = System.Drawing.Color.ForestGreen;
			metroButton1.FontWeight = MetroFramework.MetroButtonWeight.Regular;
			metroButton1.Location = new System.Drawing.Point(357, 1);
			metroButton1.Name = "metroButton1";
			metroButton1.Size = new System.Drawing.Size(90, 26);
			metroButton1.TabIndex = 3;
			metroButton1.Text = "button 1";
			metroButton1.UseSelectable = true;
			metroButton3.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			metroButton3.FontWeight = MetroFramework.MetroButtonWeight.Regular;
			metroButton3.Location = new System.Drawing.Point(553, 1);
			metroButton3.Name = "metroButton3";
			metroButton3.Size = new System.Drawing.Size(90, 26);
			metroButton3.TabIndex = 5;
			metroButton3.Text = "button 3";
			metroButton3.UseSelectable = true;
			metroButton2.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			metroButton2.FontWeight = MetroFramework.MetroButtonWeight.Regular;
			metroButton2.Location = new System.Drawing.Point(455, 1);
			metroButton2.Name = "metroButton2";
			metroButton2.Size = new System.Drawing.Size(90, 26);
			metroButton2.TabIndex = 4;
			metroButton2.Text = "button 2";
			metroButton2.UseSelectable = true;
			pnlBottom.BackColor = System.Drawing.Color.Transparent;
			pnlBottom.Controls.Add(metroButton2);
			pnlBottom.Controls.Add(metroButton1);
			pnlBottom.Controls.Add(metroButton3);
			pnlBottom.Dock = System.Windows.Forms.DockStyle.Fill;
			pnlBottom.Location = new System.Drawing.Point(80, 171);
			pnlBottom.Margin = new System.Windows.Forms.Padding(0);
			pnlBottom.Name = "pnlBottom";
			pnlBottom.Size = new System.Drawing.Size(643, 40);
			pnlBottom.TabIndex = 2;
			base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 21f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(804, 211);
			base.ControlBox = false;
			base.Controls.Add(panelbody);
			Font = new System.Drawing.Font("Segoe UI Light", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			base.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			base.Name = "MetroMessageBoxControl";
			base.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			panelbody.ResumeLayout(false);
			tlpBody.ResumeLayout(false);
			tlpBody.PerformLayout();
			pnlBottom.ResumeLayout(false);
			ResumeLayout(false);
		}

		internal static void iWrkKGTgtnaA0dNMaKx()
		{
		}

		internal static bool pxg30iTU3Rb2SIoEiDC()
		{
			return kpp6f2TfuHQ398JOaqB == null;
		}
	}
}
