using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using FontAwesome.Sharp;
using LauncherGHU.API;
using MetroFramework;
using MetroFramework.Controls;

namespace LauncherGHU.ControlsForm
{
	public class Account : UserControl
	{
		private IContainer icontainer_0 = null;

		private MetroTabControl tabControl;

		private TabPage changePhonePage;

		private TabPage changepasstwopage;

		private TabPage changepassPage;

		private Panel panelTop;

		private Label label1;

		private Panel panelBodyChangeMoney;

		private IconButton nhaplaiBtnExchange;

		private IconButton doingayBtn;

		private TextBox currentPasswordTxt;

		private Label label15;

		private TextBox newPasswordTxt;

		private Label label2;

		private TextBox repasswordnewTxt;

		private Label label3;

		private Panel panel1;

		private TextBox renewPasstwoTxt;

		private Label label4;

		private TextBox newPasstwoTxt;

		private Label label5;

		private IconButton nhaplaiPasstwoBtn;

		private IconButton doingayPassTwoBtn;

		private TextBox otppasstwoTxt;

		private Label label6;

		private Panel panel2;

		private Label label7;

		private Panel panel3;

		private TextBox newPhoneTxt;

		private Label label9;

		private IconButton nhaplaiPhoneBtn;

		private IconButton doiphoneBtn;

		private TextBox otpChangephoneTxt;

		private Label label10;

		private Panel panel4;

		private Label label11;

		internal static Account n6NpFOUjOelCJp3qt5f;

		public Account()
		{
			InitializeComponent();
			((TabControl)(object)tabControl).SelectedIndex = 0;
		}

		private void currentPasswordTxt_Enter(object sender, EventArgs e)
		{
			if (currentPasswordTxt.Text == "0000000000000000")
			{
				currentPasswordTxt.Text = "";
				currentPasswordTxt.ForeColor = Color.Maroon;
			}
		}

		private void currentPasswordTxt_Leave(object sender, EventArgs e)
		{
			if (currentPasswordTxt.Text == "")
			{
				currentPasswordTxt.Text = "0000000000000000";
				currentPasswordTxt.ForeColor = Color.DimGray;
			}
		}

		private void newPasswordTxt_Enter(object sender, EventArgs e)
		{
			if (newPasswordTxt.Text == "0000000000000000")
			{
				newPasswordTxt.Text = "";
				newPasswordTxt.ForeColor = Color.Maroon;
			}
		}

		private void newPasswordTxt_Leave(object sender, EventArgs e)
		{
			if (newPasswordTxt.Text == "")
			{
				newPasswordTxt.Text = "0000000000000000";
				newPasswordTxt.ForeColor = Color.DimGray;
			}
		}

		private void repasswordnewTxt_Enter(object sender, EventArgs e)
		{
			if (repasswordnewTxt.Text == "0000000000000000")
			{
				repasswordnewTxt.Text = "";
				repasswordnewTxt.ForeColor = Color.Maroon;
			}
		}

		private void repasswordnewTxt_Leave(object sender, EventArgs e)
		{
			if (repasswordnewTxt.Text == "")
			{
				repasswordnewTxt.Text = "0000000000000000";
				repasswordnewTxt.ForeColor = Color.DimGray;
			}
		}

		private void doingayBtn_Click(object sender, EventArgs e)
		{
			if (newPasswordTxt.Text != repasswordnewTxt.Text)
			{
				MessageBox.Show("Mật khẩu mới không khớp", "Lỗi");
				return;
			}
			try
			{
				string url = $"{ApplicationConfig.UrlApi}Account/changepassword.php";
				string text = AccountAPI.ChangePass(url, currentPasswordTxt.Text, newPasswordTxt.Text);
				MessageBox.Show(text, "Thông báo");
				if (text.IndexOf("thành công") > -1)
				{
					nhaplaiBtnExchange_Click(null, null);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message.ToString(), "Lỗi");
				ControlMgr.UpLogLauncher(ex.ToString() + "- (doingayBtn_Click)- Now Version : " + Assembly.GetEntryAssembly().GetName().Version);
			}
		}

		private void nhaplaiBtnExchange_Click(object sender, EventArgs e)
		{
			newPasswordTxt.Text = "";
			currentPasswordTxt.Text = "";
			repasswordnewTxt.Text = "";
			repasswordnewTxt_Leave(null, null);
			newPasswordTxt_Leave(null, null);
			currentPasswordTxt_Leave(null, null);
		}

		private void otppasstwoTxt_Enter(object sender, EventArgs e)
		{
			if (otppasstwoTxt.Text == "Soạn : ON OTPV gửi 8085 để lấy mã OTP")
			{
				otppasstwoTxt.Text = "";
				otppasstwoTxt.ForeColor = Color.Maroon;
			}
		}

		private void otppasstwoTxt_Leave(object sender, EventArgs e)
		{
			if (otppasstwoTxt.Text == "")
			{
				otppasstwoTxt.Text = "Soạn : ON OTPV gửi 8085 để lấy mã OTP";
				otppasstwoTxt.ForeColor = Color.DimGray;
			}
		}

		private void qdxMaRdkv0(object sender, EventArgs e)
		{
			if (newPasstwoTxt.Text == "0000000000000000")
			{
				newPasstwoTxt.Text = "";
				newPasstwoTxt.ForeColor = Color.Maroon;
			}
		}

		private void newPasstwoTxt_Leave(object sender, EventArgs e)
		{
			if (newPasstwoTxt.Text == "")
			{
				newPasstwoTxt.Text = "0000000000000000";
				newPasstwoTxt.ForeColor = Color.DimGray;
			}
		}

		private void renewPasstwoTxt_Enter(object sender, EventArgs e)
		{
			if (renewPasstwoTxt.Text == "0000000000000000")
			{
				renewPasstwoTxt.Text = "";
				renewPasstwoTxt.ForeColor = Color.Maroon;
			}
		}

		private void renewPasstwoTxt_Leave(object sender, EventArgs e)
		{
			if (renewPasstwoTxt.Text == "")
			{
				renewPasstwoTxt.Text = "0000000000000000";
				renewPasstwoTxt.ForeColor = Color.DimGray;
			}
		}

		private void doingayPassTwoBtn_Click(object sender, EventArgs e)
		{
			if (newPasstwoTxt.Text != renewPasstwoTxt.Text)
			{
				MessageBox.Show("Mật khẩu cấp 2 mới không khớp", "Lỗi");
				return;
			}
			try
			{
				string url = $"{ApplicationConfig.UrlApi}formlauncher/changepassword2.php";
				string text = AccountAPI.ChangePassTwo(url, newPasstwoTxt.Text, otppasstwoTxt.Text);
				MessageBox.Show(text, "Thông báo");
				if (text.IndexOf("thành công") > -1)
				{
					nhaplaiPasstwoBtn_Click(null, null);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message.ToString(), "Lỗi");
				ControlMgr.UpLogLauncher(ex.ToString() + "- (doingayPassTwoBtn_Click) Now Version  : " + Assembly.GetEntryAssembly().GetName().Version);
			}
		}

		private void nhaplaiPasstwoBtn_Click(object sender, EventArgs e)
		{
			otppasstwoTxt.Text = "";
			newPasstwoTxt.Text = "";
			renewPasstwoTxt.Text = "";
			otppasstwoTxt_Leave(null, null);
			newPasstwoTxt_Leave(null, null);
			renewPasstwoTxt_Leave(null, null);
		}

		private void otpChangephoneTxt_Enter(object sender, EventArgs e)
		{
			if (otpChangephoneTxt.Text == "Soạn : ON OTPV gửi 8085 để lấy mã OTP")
			{
				otpChangephoneTxt.Text = "";
				otpChangephoneTxt.ForeColor = Color.Maroon;
			}
		}

		private void otpChangephoneTxt_Leave(object sender, EventArgs e)
		{
			if (otpChangephoneTxt.Text == "")
			{
				otpChangephoneTxt.Text = "Soạn : ON OTPV gửi 8085 để lấy mã OTP";
				otpChangephoneTxt.ForeColor = Color.DimGray;
			}
		}

		private void newPhoneTxt_Enter(object sender, EventArgs e)
		{
			if (newPhoneTxt.Text == "0123456798")
			{
				newPhoneTxt.Text = "";
				newPhoneTxt.ForeColor = Color.Maroon;
			}
		}

		private void newPhoneTxt_Leave(object sender, EventArgs e)
		{
			if (newPhoneTxt.Text == "")
			{
				newPhoneTxt.Text = "0123456798";
				newPhoneTxt.ForeColor = Color.DimGray;
			}
		}

		private void nhaplaiPhoneBtn_Click(object sender, EventArgs e)
		{
			string text3 = (newPhoneTxt.Text = (otpChangephoneTxt.Text = ""));
			newPhoneTxt_Leave(null, null);
			otpChangephoneTxt_Leave(null, null);
		}

		private void doiphoneBtn_Click(object sender, EventArgs e)
		{
			try
			{
				string url = $"{ApplicationConfig.UrlApi}formlauncher/changephone.php";
				string text = AccountAPI.ChangePhoneNumber(url, newPhoneTxt.Text, otpChangephoneTxt.Text);
				MessageBox.Show(text, "Thông báo");
				if (text.IndexOf("thành công") > -1)
				{
					nhaplaiPhoneBtn_Click(null, null);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message.ToString(), "Lỗi");
				ControlMgr.UpLogLauncher(ex.ToString() + "-(doiphoneBtn_Click)- Now Version : " + Assembly.GetEntryAssembly().GetName().Version);
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
			tabControl = new MetroFramework.Controls.MetroTabControl();
			changePhonePage = new System.Windows.Forms.TabPage();
			changepasstwopage = new System.Windows.Forms.TabPage();
			changepassPage = new System.Windows.Forms.TabPage();
			panelTop = new System.Windows.Forms.Panel();
			label1 = new System.Windows.Forms.Label();
			panelBodyChangeMoney = new System.Windows.Forms.Panel();
			nhaplaiBtnExchange = new FontAwesome.Sharp.IconButton();
			doingayBtn = new FontAwesome.Sharp.IconButton();
			currentPasswordTxt = new System.Windows.Forms.TextBox();
			label15 = new System.Windows.Forms.Label();
			newPasswordTxt = new System.Windows.Forms.TextBox();
			label2 = new System.Windows.Forms.Label();
			repasswordnewTxt = new System.Windows.Forms.TextBox();
			label3 = new System.Windows.Forms.Label();
			panel1 = new System.Windows.Forms.Panel();
			renewPasstwoTxt = new System.Windows.Forms.TextBox();
			label4 = new System.Windows.Forms.Label();
			newPasstwoTxt = new System.Windows.Forms.TextBox();
			label5 = new System.Windows.Forms.Label();
			nhaplaiPasstwoBtn = new FontAwesome.Sharp.IconButton();
			doingayPassTwoBtn = new FontAwesome.Sharp.IconButton();
			otppasstwoTxt = new System.Windows.Forms.TextBox();
			label6 = new System.Windows.Forms.Label();
			panel2 = new System.Windows.Forms.Panel();
			label7 = new System.Windows.Forms.Label();
			panel3 = new System.Windows.Forms.Panel();
			newPhoneTxt = new System.Windows.Forms.TextBox();
			label9 = new System.Windows.Forms.Label();
			nhaplaiPhoneBtn = new FontAwesome.Sharp.IconButton();
			doiphoneBtn = new FontAwesome.Sharp.IconButton();
			otpChangephoneTxt = new System.Windows.Forms.TextBox();
			label10 = new System.Windows.Forms.Label();
			panel4 = new System.Windows.Forms.Panel();
			label11 = new System.Windows.Forms.Label();
			((System.Windows.Forms.Control)(object)tabControl).SuspendLayout();
			changePhonePage.SuspendLayout();
			changepasstwopage.SuspendLayout();
			changepassPage.SuspendLayout();
			panelTop.SuspendLayout();
			panelBodyChangeMoney.SuspendLayout();
			panel1.SuspendLayout();
			panel2.SuspendLayout();
			panel3.SuspendLayout();
			panel4.SuspendLayout();
			SuspendLayout();
			((System.Windows.Forms.Control)(object)tabControl).AllowDrop = true;
			((System.Windows.Forms.TabControl)(object)tabControl).Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
			((System.Windows.Forms.Control)(object)tabControl).Controls.Add(changepassPage);
			//((System.Windows.Forms.Control)(object)tabControl).Controls.Add(changepasstwopage);
			//((System.Windows.Forms.Control)(object)tabControl).Controls.Add(changePhonePage);
			((System.Windows.Forms.Control)(object)tabControl).Cursor = System.Windows.Forms.Cursors.Default;
			((System.Windows.Forms.Control)(object)tabControl).Dock = System.Windows.Forms.DockStyle.Fill;
			((System.Windows.Forms.TabControl)(object)tabControl).DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
			tabControl.FontWeight = MetroFramework.MetroTabControlWeight.Regular;
			((System.Windows.Forms.TabControl)(object)tabControl).HotTrack = true;
			((System.Windows.Forms.TabControl)(object)tabControl).ItemSize = new System.Drawing.Size(90, 40);
			((System.Windows.Forms.Control)(object)tabControl).Location = new System.Drawing.Point(0, 0);
			((System.Windows.Forms.Control)(object)tabControl).Margin = new System.Windows.Forms.Padding(0, 0, 3, 3);
			((System.Windows.Forms.TabControl)(object)tabControl).Multiline = true;
			((System.Windows.Forms.Control)(object)tabControl).Name = "tabControl";
			((System.Windows.Forms.TabControl)(object)tabControl).SelectedIndex = 2;
			((System.Windows.Forms.TabControl)(object)tabControl).ShowToolTips = true;
			((System.Windows.Forms.Control)(object)tabControl).Size = new System.Drawing.Size(1000, 625);
			((System.Windows.Forms.TabControl)(object)tabControl).SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
			tabControl.Style = MetroFramework.MetroColorStyle.Red;
			((System.Windows.Forms.Control)(object)tabControl).TabIndex = 1;
			tabControl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			tabControl.Theme = MetroFramework.MetroThemeStyle.Light;
			tabControl.UseCustomBackColor = true;
			tabControl.UseSelectable = true;
			changePhonePage.BackColor = System.Drawing.Color.White;
			changePhonePage.Controls.Add(panel3);
			changePhonePage.Controls.Add(panel4);
			changePhonePage.Location = new System.Drawing.Point(4, 44);
			changePhonePage.Name = "changePhonePage";
			changePhonePage.Size = new System.Drawing.Size(992, 577);
			changePhonePage.TabIndex = 1;
			changePhonePage.Text = "ĐỔI SỐ ĐIỆN THOẠI";
			changepasstwopage.Controls.Add(panel1);
			changepasstwopage.Controls.Add(panel2);
			changepasstwopage.Location = new System.Drawing.Point(4, 44);
			changepasstwopage.Name = "changepasstwopage";
			changepasstwopage.Size = new System.Drawing.Size(992, 577);
			changepasstwopage.TabIndex = 2;
			changepasstwopage.Text = "ĐỔI MẬT KHẨU CẤP 2";
			changepassPage.Controls.Add(panelBodyChangeMoney);
			changepassPage.Controls.Add(panelTop);
			changepassPage.Location = new System.Drawing.Point(4, 44);
			changepassPage.Name = "changepassPage";
			changepassPage.Size = new System.Drawing.Size(992, 577);
			changepassPage.TabIndex = 3;
			changepassPage.Text = "ĐỔI MẬT KHẨU";
			panelTop.BackColor = System.Drawing.SystemColors.ButtonHighlight;
			panelTop.Controls.Add(label1);
			panelTop.Dock = System.Windows.Forms.DockStyle.Top;
			panelTop.Location = new System.Drawing.Point(0, 0);
			panelTop.Name = "panelTop";
			panelTop.Size = new System.Drawing.Size(992, 108);
			panelTop.TabIndex = 1;
			label1.AutoSize = true;
			label1.Font = new System.Drawing.Font("Tahoma", 21.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			label1.ForeColor = System.Drawing.Color.Maroon;
			label1.Location = new System.Drawing.Point(374, 33);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(214, 35);
			label1.TabIndex = 3;
			label1.Text = "ĐỔI MẬT KHẨU";
			panelBodyChangeMoney.BackColor = System.Drawing.Color.White;
			panelBodyChangeMoney.Controls.Add(repasswordnewTxt);
			panelBodyChangeMoney.Controls.Add(label3);
			panelBodyChangeMoney.Controls.Add(newPasswordTxt);
			panelBodyChangeMoney.Controls.Add(label2);
			panelBodyChangeMoney.Controls.Add(nhaplaiBtnExchange);
			panelBodyChangeMoney.Controls.Add(doingayBtn);
			panelBodyChangeMoney.Controls.Add(currentPasswordTxt);
			panelBodyChangeMoney.Controls.Add(label15);
			panelBodyChangeMoney.Dock = System.Windows.Forms.DockStyle.Fill;
			panelBodyChangeMoney.Location = new System.Drawing.Point(0, 108);
			panelBodyChangeMoney.Name = "panelBodyChangeMoney";
			panelBodyChangeMoney.Size = new System.Drawing.Size(992, 469);
			panelBodyChangeMoney.TabIndex = 3;
			nhaplaiBtnExchange.BackColor = System.Drawing.Color.DarkOrange;
			nhaplaiBtnExchange.Cursor = System.Windows.Forms.Cursors.Hand;
			nhaplaiBtnExchange.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			nhaplaiBtnExchange.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
			nhaplaiBtnExchange.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			nhaplaiBtnExchange.ForeColor = System.Drawing.Color.Black;
			nhaplaiBtnExchange.IconChar = FontAwesome.Sharp.IconChar.Retweet;
			nhaplaiBtnExchange.IconColor = System.Drawing.Color.Maroon;
			nhaplaiBtnExchange.IconSize = 20;
			nhaplaiBtnExchange.Location = new System.Drawing.Point(490, 257);
			nhaplaiBtnExchange.Name = "nhaplaiBtnExchange";
			nhaplaiBtnExchange.Rotation = 0.0;
			nhaplaiBtnExchange.Size = new System.Drawing.Size(136, 36);
			nhaplaiBtnExchange.TabIndex = 12;
			nhaplaiBtnExchange.Text = "    NHẬP LẠI";
			nhaplaiBtnExchange.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			nhaplaiBtnExchange.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			nhaplaiBtnExchange.UseVisualStyleBackColor = false;
			nhaplaiBtnExchange.Click += new System.EventHandler(nhaplaiBtnExchange_Click);
			doingayBtn.BackColor = System.Drawing.Color.SpringGreen;
			doingayBtn.Cursor = System.Windows.Forms.Cursors.Hand;
			doingayBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			doingayBtn.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
			doingayBtn.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			doingayBtn.ForeColor = System.Drawing.Color.Maroon;
			doingayBtn.IconChar = FontAwesome.Sharp.IconChar.ExchangeAlt;
			doingayBtn.IconColor = System.Drawing.Color.Maroon;
			doingayBtn.IconSize = 20;
			doingayBtn.Location = new System.Drawing.Point(340, 257);
			doingayBtn.Name = "doingayBtn";
			doingayBtn.Rotation = 0.0;
			doingayBtn.Size = new System.Drawing.Size(136, 36);
			doingayBtn.TabIndex = 11;
			doingayBtn.Text = "    ĐỔI NGAY";
			doingayBtn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			doingayBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			doingayBtn.UseVisualStyleBackColor = false;
			doingayBtn.Click += new System.EventHandler(doingayBtn_Click);
			currentPasswordTxt.Font = new System.Drawing.Font("Tahoma", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			currentPasswordTxt.ForeColor = System.Drawing.Color.DimGray;
			currentPasswordTxt.Location = new System.Drawing.Point(241, 56);
			currentPasswordTxt.Name = "currentPasswordTxt";
			currentPasswordTxt.PasswordChar = '●';
			currentPasswordTxt.Size = new System.Drawing.Size(738, 30);
			currentPasswordTxt.TabIndex = 8;
			currentPasswordTxt.Text = "0000000000000000";
			currentPasswordTxt.Enter += new System.EventHandler(currentPasswordTxt_Enter);
			currentPasswordTxt.Leave += new System.EventHandler(currentPasswordTxt_Leave);
			label15.AutoSize = true;
			label15.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			label15.ForeColor = System.Drawing.Color.Black;
			label15.Location = new System.Drawing.Point(18, 62);
			label15.Name = "label15";
			label15.Size = new System.Drawing.Size(170, 19);
			label15.TabIndex = 0;
			label15.Text = "Mật Khẩu Hiện Tại :";
			newPasswordTxt.Font = new System.Drawing.Font("Tahoma", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			newPasswordTxt.ForeColor = System.Drawing.Color.DimGray;
			newPasswordTxt.Location = new System.Drawing.Point(241, 110);
			newPasswordTxt.Name = "newPasswordTxt";
			newPasswordTxt.PasswordChar = '●';
			newPasswordTxt.Size = new System.Drawing.Size(738, 30);
			newPasswordTxt.TabIndex = 9;
			newPasswordTxt.Text = "0000000000000000";
			newPasswordTxt.Enter += new System.EventHandler(newPasswordTxt_Enter);
			newPasswordTxt.Leave += new System.EventHandler(newPasswordTxt_Leave);
			label2.AutoSize = true;
			label2.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			label2.ForeColor = System.Drawing.Color.Black;
			label2.Location = new System.Drawing.Point(18, 116);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(131, 19);
			label2.TabIndex = 1;
			label2.Text = "Mật Khẩu Mới :";
			repasswordnewTxt.Font = new System.Drawing.Font("Tahoma", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			repasswordnewTxt.ForeColor = System.Drawing.Color.DimGray;
			repasswordnewTxt.Location = new System.Drawing.Point(241, 163);
			repasswordnewTxt.Name = "repasswordnewTxt";
			repasswordnewTxt.PasswordChar = '●';
			repasswordnewTxt.Size = new System.Drawing.Size(738, 30);
			repasswordnewTxt.TabIndex = 10;
			repasswordnewTxt.Text = "0000000000000000";
			repasswordnewTxt.Enter += new System.EventHandler(repasswordnewTxt_Enter);
			repasswordnewTxt.Leave += new System.EventHandler(repasswordnewTxt_Leave);
			label3.AutoSize = true;
			label3.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			label3.ForeColor = System.Drawing.Color.Black;
			label3.Location = new System.Drawing.Point(18, 169);
			label3.Name = "label3";
			label3.Size = new System.Drawing.Size(207, 19);
			label3.TabIndex = 2;
			label3.Text = "Nhập Lại Mật Khẩu Mới :";
			panel1.BackColor = System.Drawing.Color.White;
			panel1.Controls.Add(renewPasstwoTxt);
			panel1.Controls.Add(label4);
			panel1.Controls.Add(newPasstwoTxt);
			panel1.Controls.Add(label5);
			panel1.Controls.Add(nhaplaiPasstwoBtn);
			panel1.Controls.Add(doingayPassTwoBtn);
			panel1.Controls.Add(otppasstwoTxt);
			panel1.Controls.Add(label6);
			panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			panel1.Location = new System.Drawing.Point(0, 108);
			panel1.Name = "panel1";
			panel1.Size = new System.Drawing.Size(992, 469);
			panel1.TabIndex = 5;
			renewPasstwoTxt.Font = new System.Drawing.Font("Tahoma", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			renewPasstwoTxt.ForeColor = System.Drawing.Color.DimGray;
			renewPasstwoTxt.Location = new System.Drawing.Point(241, 163);
			renewPasstwoTxt.Name = "renewPasstwoTxt";
			renewPasstwoTxt.PasswordChar = '●';
			renewPasstwoTxt.Size = new System.Drawing.Size(738, 30);
			renewPasstwoTxt.TabIndex = 10;
			renewPasstwoTxt.Text = "0000000000000000";
			renewPasstwoTxt.Enter += new System.EventHandler(renewPasstwoTxt_Enter);
			renewPasstwoTxt.Leave += new System.EventHandler(renewPasstwoTxt_Leave);
			label4.AutoSize = true;
			label4.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			label4.ForeColor = System.Drawing.Color.Black;
			label4.Location = new System.Drawing.Point(18, 169);
			label4.Name = "label4";
			label4.Size = new System.Drawing.Size(207, 19);
			label4.TabIndex = 2;
			label4.Text = "Nhập Lại Mật Khẩu Mới :";
			newPasstwoTxt.Font = new System.Drawing.Font("Tahoma", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			newPasstwoTxt.ForeColor = System.Drawing.Color.DimGray;
			newPasstwoTxt.Location = new System.Drawing.Point(241, 110);
			newPasstwoTxt.Name = "newPasstwoTxt";
			newPasstwoTxt.PasswordChar = '●';
			newPasstwoTxt.Size = new System.Drawing.Size(738, 30);
			newPasstwoTxt.TabIndex = 9;
			newPasstwoTxt.Text = "0000000000000000";
			newPasstwoTxt.Enter += new System.EventHandler(qdxMaRdkv0);
			newPasstwoTxt.Leave += new System.EventHandler(newPasstwoTxt_Leave);
			label5.AutoSize = true;
			label5.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			label5.ForeColor = System.Drawing.Color.Black;
			label5.Location = new System.Drawing.Point(18, 116);
			label5.Name = "label5";
			label5.Size = new System.Drawing.Size(131, 19);
			label5.TabIndex = 1;
			label5.Text = "Mật Khẩu Mới :";
			nhaplaiPasstwoBtn.BackColor = System.Drawing.Color.DarkOrange;
			nhaplaiPasstwoBtn.Cursor = System.Windows.Forms.Cursors.Hand;
			nhaplaiPasstwoBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			nhaplaiPasstwoBtn.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
			nhaplaiPasstwoBtn.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			nhaplaiPasstwoBtn.ForeColor = System.Drawing.Color.Black;
			nhaplaiPasstwoBtn.IconChar = FontAwesome.Sharp.IconChar.Retweet;
			nhaplaiPasstwoBtn.IconColor = System.Drawing.Color.Maroon;
			nhaplaiPasstwoBtn.IconSize = 20;
			nhaplaiPasstwoBtn.Location = new System.Drawing.Point(490, 257);
			nhaplaiPasstwoBtn.Name = "nhaplaiPasstwoBtn";
			nhaplaiPasstwoBtn.Rotation = 0.0;
			nhaplaiPasstwoBtn.Size = new System.Drawing.Size(136, 36);
			nhaplaiPasstwoBtn.TabIndex = 12;
			nhaplaiPasstwoBtn.Text = "    NHẬP LẠI";
			nhaplaiPasstwoBtn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			nhaplaiPasstwoBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			nhaplaiPasstwoBtn.UseVisualStyleBackColor = false;
			nhaplaiPasstwoBtn.Click += new System.EventHandler(nhaplaiPasstwoBtn_Click);
			doingayPassTwoBtn.BackColor = System.Drawing.Color.SpringGreen;
			doingayPassTwoBtn.Cursor = System.Windows.Forms.Cursors.Hand;
			doingayPassTwoBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			doingayPassTwoBtn.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
			doingayPassTwoBtn.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			doingayPassTwoBtn.ForeColor = System.Drawing.Color.Maroon;
			doingayPassTwoBtn.IconChar = FontAwesome.Sharp.IconChar.ExchangeAlt;
			doingayPassTwoBtn.IconColor = System.Drawing.Color.Maroon;
			doingayPassTwoBtn.IconSize = 20;
			doingayPassTwoBtn.Location = new System.Drawing.Point(340, 257);
			doingayPassTwoBtn.Name = "doingayPassTwoBtn";
			doingayPassTwoBtn.Rotation = 0.0;
			doingayPassTwoBtn.Size = new System.Drawing.Size(136, 36);
			doingayPassTwoBtn.TabIndex = 11;
			doingayPassTwoBtn.Text = "    ĐỔI NGAY";
			doingayPassTwoBtn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			doingayPassTwoBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			doingayPassTwoBtn.UseVisualStyleBackColor = false;
			doingayPassTwoBtn.Click += new System.EventHandler(doingayPassTwoBtn_Click);
			otppasstwoTxt.Font = new System.Drawing.Font("Tahoma", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			otppasstwoTxt.ForeColor = System.Drawing.Color.DimGray;
			otppasstwoTxt.Location = new System.Drawing.Point(241, 56);
			otppasstwoTxt.Name = "otppasstwoTxt";
			otppasstwoTxt.Size = new System.Drawing.Size(738, 30);
			otppasstwoTxt.TabIndex = 8;
			otppasstwoTxt.Text = "Soạn : ON OTPV gửi 8085 để lấy mã OTP";
			otppasstwoTxt.Enter += new System.EventHandler(otppasstwoTxt_Enter);
			otppasstwoTxt.Leave += new System.EventHandler(otppasstwoTxt_Leave);
			label6.AutoSize = true;
			label6.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			label6.ForeColor = System.Drawing.Color.Black;
			label6.Location = new System.Drawing.Point(18, 62);
			label6.Name = "label6";
			label6.Size = new System.Drawing.Size(83, 19);
			label6.TabIndex = 0;
			label6.Text = "Mã OTP :";
			panel2.BackColor = System.Drawing.SystemColors.ButtonHighlight;
			panel2.Controls.Add(label7);
			panel2.Dock = System.Windows.Forms.DockStyle.Top;
			panel2.Location = new System.Drawing.Point(0, 0);
			panel2.Name = "panel2";
			panel2.Size = new System.Drawing.Size(992, 108);
			panel2.TabIndex = 4;
			label7.AutoSize = true;
			label7.Font = new System.Drawing.Font("Tahoma", 21.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			label7.ForeColor = System.Drawing.Color.Maroon;
			label7.Location = new System.Drawing.Point(333, 28);
			label7.Name = "label7";
			label7.Size = new System.Drawing.Size(298, 35);
			label7.TabIndex = 3;
			label7.Text = "ĐỔI MẬT KHẨU CẤP 2";
			panel3.BackColor = System.Drawing.Color.White;
			panel3.Controls.Add(newPhoneTxt);
			panel3.Controls.Add(label9);
			panel3.Controls.Add(nhaplaiPhoneBtn);
			panel3.Controls.Add(doiphoneBtn);
			panel3.Controls.Add(otpChangephoneTxt);
			panel3.Controls.Add(label10);
			panel3.Dock = System.Windows.Forms.DockStyle.Fill;
			panel3.Location = new System.Drawing.Point(0, 108);
			panel3.Name = "panel3";
			panel3.Size = new System.Drawing.Size(992, 469);
			panel3.TabIndex = 7;
			newPhoneTxt.Font = new System.Drawing.Font("Tahoma", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			newPhoneTxt.ForeColor = System.Drawing.Color.DimGray;
			newPhoneTxt.Location = new System.Drawing.Point(241, 137);
			newPhoneTxt.Name = "newPhoneTxt";
			newPhoneTxt.Size = new System.Drawing.Size(738, 30);
			newPhoneTxt.TabIndex = 9;
			newPhoneTxt.Text = "0123456798";
			newPhoneTxt.Enter += new System.EventHandler(newPhoneTxt_Enter);
			newPhoneTxt.Leave += new System.EventHandler(newPhoneTxt_Leave);
			label9.AutoSize = true;
			label9.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			label9.ForeColor = System.Drawing.Color.Black;
			label9.Location = new System.Drawing.Point(18, 143);
			label9.Name = "label9";
			label9.Size = new System.Drawing.Size(167, 19);
			label9.TabIndex = 1;
			label9.Text = "Số Điện Thoại Mới :";
			nhaplaiPhoneBtn.BackColor = System.Drawing.Color.DarkOrange;
			nhaplaiPhoneBtn.Cursor = System.Windows.Forms.Cursors.Hand;
			nhaplaiPhoneBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			nhaplaiPhoneBtn.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
			nhaplaiPhoneBtn.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			nhaplaiPhoneBtn.ForeColor = System.Drawing.Color.Black;
			nhaplaiPhoneBtn.IconChar = FontAwesome.Sharp.IconChar.Retweet;
			nhaplaiPhoneBtn.IconColor = System.Drawing.Color.Maroon;
			nhaplaiPhoneBtn.IconSize = 20;
			nhaplaiPhoneBtn.Location = new System.Drawing.Point(490, 241);
			nhaplaiPhoneBtn.Name = "nhaplaiPhoneBtn";
			nhaplaiPhoneBtn.Rotation = 0.0;
			nhaplaiPhoneBtn.Size = new System.Drawing.Size(136, 36);
			nhaplaiPhoneBtn.TabIndex = 12;
			nhaplaiPhoneBtn.Text = "    NHẬP LẠI";
			nhaplaiPhoneBtn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			nhaplaiPhoneBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			nhaplaiPhoneBtn.UseVisualStyleBackColor = false;
			nhaplaiPhoneBtn.Click += new System.EventHandler(nhaplaiPhoneBtn_Click);
			doiphoneBtn.BackColor = System.Drawing.Color.SpringGreen;
			doiphoneBtn.Cursor = System.Windows.Forms.Cursors.Hand;
			doiphoneBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			doiphoneBtn.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
			doiphoneBtn.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			doiphoneBtn.ForeColor = System.Drawing.Color.Maroon;
			doiphoneBtn.IconChar = FontAwesome.Sharp.IconChar.ExchangeAlt;
			doiphoneBtn.IconColor = System.Drawing.Color.Maroon;
			doiphoneBtn.IconSize = 20;
			doiphoneBtn.Location = new System.Drawing.Point(340, 241);
			doiphoneBtn.Name = "doiphoneBtn";
			doiphoneBtn.Rotation = 0.0;
			doiphoneBtn.Size = new System.Drawing.Size(136, 36);
			doiphoneBtn.TabIndex = 11;
			doiphoneBtn.Text = "    ĐỔI NGAY";
			doiphoneBtn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			doiphoneBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			doiphoneBtn.UseVisualStyleBackColor = false;
			doiphoneBtn.Click += new System.EventHandler(doiphoneBtn_Click);
			otpChangephoneTxt.Font = new System.Drawing.Font("Tahoma", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			otpChangephoneTxt.ForeColor = System.Drawing.Color.DimGray;
			otpChangephoneTxt.Location = new System.Drawing.Point(241, 83);
			otpChangephoneTxt.Name = "otpChangephoneTxt";
			otpChangephoneTxt.Size = new System.Drawing.Size(738, 30);
			otpChangephoneTxt.TabIndex = 8;
			otpChangephoneTxt.Text = "Soạn : ON OTPV gửi 8085 để lấy mã OTP";
			otpChangephoneTxt.Enter += new System.EventHandler(otpChangephoneTxt_Enter);
			otpChangephoneTxt.Leave += new System.EventHandler(otpChangephoneTxt_Leave);
			label10.AutoSize = true;
			label10.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			label10.ForeColor = System.Drawing.Color.Black;
			label10.Location = new System.Drawing.Point(18, 89);
			label10.Name = "label10";
			label10.Size = new System.Drawing.Size(83, 19);
			label10.TabIndex = 0;
			label10.Text = "Mã OTP :";
			panel4.BackColor = System.Drawing.SystemColors.ButtonHighlight;
			panel4.Controls.Add(label11);
			panel4.Dock = System.Windows.Forms.DockStyle.Top;
			panel4.Location = new System.Drawing.Point(0, 0);
			panel4.Name = "panel4";
			panel4.Size = new System.Drawing.Size(992, 108);
			panel4.TabIndex = 6;
			label11.AutoSize = true;
			label11.Font = new System.Drawing.Font("Tahoma", 21.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			label11.ForeColor = System.Drawing.Color.Maroon;
			label11.Location = new System.Drawing.Point(333, 28);
			label11.Name = "label11";
			label11.Size = new System.Drawing.Size(283, 35);
			label11.TabIndex = 3;
			label11.Text = "ĐỔI SỐ ĐIỆN THOẠI";
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Controls.Add((System.Windows.Forms.Control)(object)tabControl);
			base.Name = "Account";
			base.Size = new System.Drawing.Size(1000, 625);
			((System.Windows.Forms.Control)(object)tabControl).ResumeLayout(false);
			changePhonePage.ResumeLayout(false);
			changepasstwopage.ResumeLayout(false);
			changepassPage.ResumeLayout(false);
			panelTop.ResumeLayout(false);
			panelTop.PerformLayout();
			panelBodyChangeMoney.ResumeLayout(false);
			panelBodyChangeMoney.PerformLayout();
			panel1.ResumeLayout(false);
			panel1.PerformLayout();
			panel2.ResumeLayout(false);
			panel2.PerformLayout();
			panel3.ResumeLayout(false);
			panel3.PerformLayout();
			panel4.ResumeLayout(false);
			panel4.PerformLayout();
			ResumeLayout(false);
		}

		internal static void tyOgPXUz0T5bdskf7St()
		{
		}

		internal static bool GBCyGjUr5dM0uLpPkQB()
		{
			return n6NpFOUjOelCJp3qt5f == null;
		}
	}
}
