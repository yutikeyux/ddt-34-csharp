using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using LauncherGHU.API;
using LauncherGHU.Properties;
using System.Threading;
using LoadLauncher;

namespace LauncherGHU
{
    public partial class formLogin : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

		private Thread thread;

		private RegisterForm registerForm;

		private ForgetForm forgetForm;

		public formLogin()
        {
            InitializeComponent();
			lplReport.Visible = false;
			this.KeyPreview = true;
		}

        private void formLogin_Load(object sender, EventArgs e)
        {

        }

        private void formLogin_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            registerForm = new RegisterForm(this);
			registerForm.ShowDialog();
        }

		private void openFormLoading()
		{
			System.Windows.Forms.Application.Run(new Form1());
		}

		public void OpenPlayGame()
		{
			if (base.InvokeRequired)
			{
				Invoke((MethodInvoker)delegate
				{
					OpenPlayGame();
				});
				return;
			}
			Close();
			thread = new Thread(openFormLoading);
			thread.SetApartmentState(ApartmentState.STA);
			thread.Start();
		}

		private void Login()
        {
			string username = ControlMgr.xoabug(txtUserName.Text).ToLower();
			string password = txtPassword.Text;
			if (username.Length >= 4)
			{
				if (password.Length >= 6 && password.Length <= 100)
				{
					string[] userinfo = AccountAPI.Login(username, password);
					if (userinfo[0] == "1")
					{
						LoginMgr.Login(userinfo[1], userinfo[2], 0);
						Settings.Default.ModeFlash = 0;
						Settings.Default.Save();
						OpenPlayGame();
					}
					else
					{
                        lplReport.Invoke((MethodInvoker)delegate
                        {
							lplReport.Visible = true;
							lplReport.Text = userinfo[1];
                        });
                    }
				}
				else
				{
					lplReport.Visible = true;
					lplReport.Text = "Vui lòng nhập mật khẩu hợp lệ!";
                }
			}
			else
			{
				lplReport.Visible = true;
				lplReport.Text = "Vui lòng nhập ta\u0300i khoa\u0309n hợp lệ!";
            }
		}

		private void pictureBox2_Click(object sender, EventArgs e)
        {
			Login();
		}

        private void formLogin_Enter(object sender, EventArgs e)
        {
			Login();
		}

        private void formLogin_KeyDown(object sender, KeyEventArgs e)
        {
			Login();
		}

		public void LoginByForm(string username, string password, Form frm)
		{
			if (frm is RegisterForm)
			{
				registerForm.Close();
			}
			if (frm is ForgetForm)
			{
				forgetForm.Close();
			}
			txtUserName.Text = username;
			txtPassword.Text = password;
		}

        private void btnForgetPass_Click(object sender, EventArgs e)
        {
			forgetForm = new ForgetForm(this);
			forgetForm.ShowDialog();
		}

        private void btnHome_Click(object sender, EventArgs e)
        {
			ControlMgr.OpenWebsite(ApplicationConfig.UrlHome);
		}

        private void btnPayment_Click(object sender, EventArgs e)
        {
			ControlMgr.OpenWebsite(ApplicationConfig.UrlRecharge);
		}

        private void btnFacebook_Click(object sender, EventArgs e)
        {
			ControlMgr.OpenWebsite(ApplicationConfig.FanpageLink);
		}

        private void pictureBox3_Click(object sender, EventArgs e)
        {
			base.WindowState = FormWindowState.Minimized;
		}

        private void pictureBox4_Click(object sender, EventArgs e)
        {
			if (MessageBox.Show("Bạn có chắc là muốn thoát Launcher?", "Thoát Launcher", MessageBoxButtons.YesNo, MessageBoxIcon.Hand) == DialogResult.Yes)
			{
				Application.Exit();
			}
		}
    }
}
