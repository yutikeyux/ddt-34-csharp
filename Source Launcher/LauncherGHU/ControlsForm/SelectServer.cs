using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;

namespace LauncherGHU.ControlsForm
{
	public class SelectServer : UserControl
	{
		private Form form_0;

		private string string_0 = "";

		private IContainer icontainer_0 = null;

		private Panel panelSelectsv;

		private Button btnServer;

        private Panel panel1;

        private Label lblsvrNumber;

        internal static SelectServer O1iiOomlE5gEhDvj6Gd;

		public SelectServer(Form frm)
		{
			InitializeComponent();
			form_0 = frm;
			method_0();
			if (GetWindowsScaling() >= 125)
			{
				btnServer.Font = new Font("Tahoma", 9.25f, System.Drawing.FontStyle.Bold, GraphicsUnit.Point, 0);
				lblsvrNumber.Font = new Font("Tahoma", 9.25f, System.Drawing.FontStyle.Bold, GraphicsUnit.Point, 0);
			}
		}

		public static int GetWindowsScaling()
		{
			return (int)((double)(100 * Screen.PrimaryScreen.Bounds.Width) / SystemParameters.PrimaryScreenWidth);
		}

		private void method_0()
		{
			foreach (KeyValuePair<int, ServerInfo> server in ControlMgr.ServerList)
			{
				if (server.Key == 1001)
				{
					btnServer.Visible = true;
					btnServer.Text = server.Value.ServerName;
					btnServer.Location = new System.Drawing.Point(server.Value.XButton, server.Value.YButton);
					lblsvrNumber.Text = "1";
					lblsvrNumber.Location = new System.Drawing.Point(btnServer.Location.X + 15, btnServer.Location.Y + 20);
				}
			}
			if (ControlMgr.ServerList.Count == 3)
			{
				panelSelectsv.Location = new System.Drawing.Point(panelSelectsv.Location.X, panelSelectsv.Location.Y);
			}
			if (ControlMgr.ServerList.Count == 2)
			{
				panelSelectsv.Location = new System.Drawing.Point(panelSelectsv.Location.X, panelSelectsv.Location.Y);
			}
			if (ControlMgr.ServerList.Count == 1)
			{
				panelSelectsv.Location = new System.Drawing.Point(panelSelectsv.Location.X, panelSelectsv.Location.Y);
			}
		}


		private void method_2()
		{
			if (form_0 is PlayGameFrmFIB)
			{
				(form_0 as PlayGameFrmFIB).InstallGameWebBrowse();
			}
			if (form_0 is PlayGameFrmFIB)
			{
				(form_0 as PlayGameFrmFIB).SetTitleForm(string_0);
			}
			if (form_0 is PlayGameFrm2FIB)
			{
				(form_0 as PlayGameFrm2FIB).InstallGameWebBrowse();
			}
			if (form_0 is PlayGameFrm2FIB)
			{
				(form_0 as PlayGameFrm2FIB).SetTitleForm(string_0);
			}
			if (form_0 is PlayGameFrmPRJ)
			{
				(form_0 as PlayGameFrmPRJ).InstallGameWebBrowse();
			}
			if (form_0 is PlayGameFrmPRJ)
			{
				(form_0 as PlayGameFrmPRJ).SetTitleForm(string_0);
			}
			if (form_0 is PlayGameFrm2PRJ)
			{
				(form_0 as PlayGameFrm2PRJ).InstallGameWebBrowse();
			}
			if (form_0 is PlayGameFrm2PRJ)
			{
				(form_0 as PlayGameFrm2PRJ).SetTitleForm(string_0);
			}
		}

		private void sv1001_MouseEnter(object sender, EventArgs e)
		{
			btnServer.ForeColor = Color.Black;
		}

		private void sv1001_MouseLeave(object sender, EventArgs e)
		{
			btnServer.ForeColor = Color.White;
		}


		private void sv1001_Click(object sender, EventArgs e)
		{
			if (ControlMgr.CheckServerIsOpen())
			{
				LoginMgr.ServerID = 1001;
				string_0 = btnServer.Text;
				method_2();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectServer));
            this.panel1 = new System.Windows.Forms.Panel();
            this.panelSelectsv = new System.Windows.Forms.Panel();
            this.lblsvrNumber = new System.Windows.Forms.Label();
            this.btnServer = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panelSelectsv.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = global::Properties.Resources.background_1;
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.Controls.Add(this.panelSelectsv);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1000, 625);
            this.panel1.TabIndex = 58;
            // 
            // panelSelectsv
            // 
            this.panelSelectsv.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelSelectsv.BackColor = System.Drawing.Color.Transparent;
            this.panelSelectsv.Controls.Add(this.lblsvrNumber);
            this.panelSelectsv.Controls.Add(this.btnServer);
            this.panelSelectsv.Location = new System.Drawing.Point(289, 132);
            this.panelSelectsv.Name = "panelSelectsv";
            this.panelSelectsv.Size = new System.Drawing.Size(376, 251);
            this.panelSelectsv.TabIndex = 57;
            // 
            // lblsvrNumber
            // 
            this.lblsvrNumber.AutoSize = true;
            this.lblsvrNumber.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(50)))), ((int)(((byte)(131)))));
            this.lblsvrNumber.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblsvrNumber.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(230)))), ((int)(((byte)(7)))));
            this.lblsvrNumber.Location = new System.Drawing.Point(50, 40);
            this.lblsvrNumber.Name = "lblsvrNumber";
            this.lblsvrNumber.Size = new System.Drawing.Size(15, 14);
            this.lblsvrNumber.TabIndex = 51;
            this.lblsvrNumber.Text = "1";
            // 
            // btnServer
            // 
            this.btnServer.BackColor = System.Drawing.Color.Transparent;
            this.btnServer.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnServer.BackgroundImage")));
            this.btnServer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnServer.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnServer.FlatAppearance.BorderSize = 0;
            this.btnServer.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnServer.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnServer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnServer.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnServer.ForeColor = System.Drawing.Color.White;
            this.btnServer.Location = new System.Drawing.Point(34, 19);
            this.btnServer.Name = "btnServer";
            this.btnServer.Size = new System.Drawing.Size(166, 55);
            this.btnServer.TabIndex = 50;
            this.btnServer.Text = "Chưa có";
            this.btnServer.UseVisualStyleBackColor = false;
            this.btnServer.Visible = false;
            this.btnServer.Click += new System.EventHandler(this.sv1001_Click);
            this.btnServer.MouseEnter += new System.EventHandler(this.sv1001_MouseEnter);
            this.btnServer.MouseLeave += new System.EventHandler(this.sv1001_MouseLeave);
            // 
            // SelectServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.panel1);
            this.Name = "SelectServer";
            this.Size = new System.Drawing.Size(1000, 625);
            this.panel1.ResumeLayout(false);
            this.panelSelectsv.ResumeLayout(false);
            this.panelSelectsv.PerformLayout();
            this.ResumeLayout(false);

		}

		internal static void S9aKsgmtn4qC2hd7fFt()
		{
		}

		internal static bool cqavbTmprxih6lZM7sd()
		{
			return O1iiOomlE5gEhDvj6Gd == null;
		}
	}
}
