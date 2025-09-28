using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using FontAwesome.Sharp;
using LauncherGHU.API;
using MetroFramework;
using MetroFramework.Controls;
using ns1;

namespace LauncherGHU
{
    public class RegisterForm : Form
    {
        private Form form_0;

        public const int WM_NCLBUTTONDOWN = 161;

        public const int HT_CAPTION = 2;

        private string string_0;

        private IContainer icontainer_0 = null;

        private BunifuGradientPanel menuRighttop;

        private Label menuLefttxt;

        private IconPictureBox iconPictureBox2;

        private MetroTextBox userTxt;

        private Label label1;

        private Label label2;

        private MetroTextBox passTxt;

        private Label label3;

        private MetroTextBox repassTxt;

        private MetroTextBox txtInvite;

        private Panel captchaPanel;

        private Label label6;

        private Label label7;

        private MetroTextBox captchaTxt;

        private MetroButton regBtn;

        private Label thongbaoTxt;
        private Label label5;
        private MetroComboBox metroComboBox1;
        private Label label8;
        internal static RegisterForm nHFhWTfdcHXDoCh2w10;

        public RegisterForm(Form form)
        {
            InitializeComponent();
            form_0 = form;
            thongbaoTxt.Text = "";
            base.AcceptButton = (IButtonControl)regBtn;
            this.metroComboBox1.Items.AddRange(new object[] { "Nam", "Nữ" });
        }

        private void method_0(object sender, EventArgs e)
        {
            Close();
        }

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private void method_1(MouseEventArgs mouseEventArgs_0)
        {
            if (mouseEventArgs_0.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(base.Handle, 161, 2, 0);
            }
        }

        private void RegisterForm_Load(object sender, EventArgs e)
        {
            method_3();
        }

        private void RegisterForm_MouseDown(object sender, MouseEventArgs e)
        {
            method_1(e);
        }

        private void iconPictureBox2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void iconPictureBox2_MouseEnter(object sender, EventArgs e)
        {
            iconPictureBox2.BackColor = Color.FromArgb(255, 211, 155);
        }

        private void iconPictureBox2_MouseLeave(object sender, EventArgs e)
        {
            iconPictureBox2.BackColor = Color.Transparent;
        }

        private void menuRighttop_MouseDown(object sender, MouseEventArgs e)
        {
            method_1(e);
        }

        public string randomString()
        {
            Random random = new Random();
            string text = md5(random.Next(1000, 9999).ToString());
            text = text.ToUpper();
            return text.Substring(0, 4);
        }

        public static byte[] encryptData(string data)
        {
            MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
            UTF8Encoding uTF8Encoding = new UTF8Encoding();
            return mD5CryptoServiceProvider.ComputeHash(uTF8Encoding.GetBytes(data));
        }

        public static string md5(string data)
        {
            return BitConverter.ToString(encryptData(data)).Replace("-", "").ToLower();
        }

        private Bitmap method_2(string string_1, int int_0, int int_1)
        {
            Bitmap bitmap = new Bitmap(int_0, int_1);
            Graphics graphics = Graphics.FromImage(bitmap);
            SolidBrush brush = new SolidBrush(Color.White);
            graphics.FillRectangle(brush, 0, 0, bitmap.Width, bitmap.Height);
            Font font = new Font("Tahoma", 30f);
            brush = new SolidBrush(Color.Blue);
            graphics.DrawString(string_1, font, brush, (float)(bitmap.Width / 2) - (float)(string_1.Length / 2) * font.Size, (float)(bitmap.Height / 2) - font.Size);
            int i = 0;
            Random random = new Random();
            for (; i < 1000; i++)
            {
                brush = new SolidBrush(Color.YellowGreen);
                graphics.FillEllipse(brush, random.Next(0, bitmap.Width), random.Next(0, bitmap.Height), 4, 2);
            }
            for (i = 0; i < 25; i++)
            {
                graphics.DrawLine(new Pen(Color.Pink), random.Next(0, bitmap.Width), random.Next(0, bitmap.Height), random.Next(0, bitmap.Width), random.Next(0, bitmap.Height));
            }
            return bitmap;
        }

        private void method_3()
        {
            string_0 = randomString();
            captchaPanel.BackgroundImage = method_2(string_0, captchaPanel.Width, captchaPanel.Height);
        }

        private void captchaPanel_Click(object sender, EventArgs e)
        {
            method_3();
        }

        private void regBtn_Click(object sender, EventArgs e)
        {
            string usernameText = ((Control)(object)userTxt).Text;
            string passwordText = ((Control)(object)passTxt).Text;
            string text3 = ((Control)(object)repassTxt).Text;
            //string phoneText = ((Control)(object)phoneTxt).Text;
            //string passwordTwoText = ((Control)(object)txtInvite).Text;
            //         string sexText = ((Control)(object)metroComboBox1).Text == "Nam" ? "true" : "false";
            if (((Control)(object)captchaTxt).Text != string_0)
            {
                thongbaoTxt.Text = "Sai mã Captcha, vui lòng thử lại !";
                method_3();
            }
            else if (passwordText != text3)
            {
                thongbaoTxt.Text = "Mật khẩu không trùng nhau";
            }
            //else if (!IsNumeric(phoneText))
            //{
            //	thongbaoTxt.Text = "Số điện thoại không hợp lệ";
            //}
            //else if (this.metroComboBox1.SelectedIndex == -1)
            //{
            //    thongbaoTxt.Text = "Vui lòng chọn giới tính hợp lệ";
            //}
            //else if(passwordTwoText.Length < 4 || passwordTwoText.Length > 50)
            //{
            //    thongbaoTxt.Text = "Mật khẩu cấp 2 phải từ 4 - 50 ký tự";
            //}
            else if (usernameText.Length > 4)
            {
                thongbaoTxt.Text = "Vui lòng đợt trong giây lát";
                string url = $"{ApplicationConfig.UrlApi}Account/register.php";
                string text5 = AccountAPI.Register(url, usernameText, passwordText/*, null/*, passwordTwoText, sexText*/);
                thongbaoTxt.Text = text5;
                if (text5.IndexOf("thành công") > -1 && MessageBox.Show("Đăng kí thành công, bạn muốn đăng nhập ngay?", "Thành công", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes && form_0 is formLogin)
                {
                    (form_0 as formLogin).LoginByForm(usernameText, passwordText, this);
                }
            }
            else
            {
                thongbaoTxt.Text = "Tên tài khoản phải từ 4 kí tự";
            }
        }

        public static bool IsNumeric(object Expression)
        {
            double result;
            return double.TryParse(Convert.ToString(Expression), NumberStyles.Any, NumberFormatInfo.InvariantInfo, out result);
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RegisterForm));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.captchaPanel = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.thongbaoTxt = new System.Windows.Forms.Label();
            this.regBtn = new MetroFramework.Controls.MetroButton();
            this.captchaTxt = new MetroFramework.Controls.MetroTextBox();
            this.txtInvite = new MetroFramework.Controls.MetroTextBox();
            this.repassTxt = new MetroFramework.Controls.MetroTextBox();
            this.passTxt = new MetroFramework.Controls.MetroTextBox();
            this.userTxt = new MetroFramework.Controls.MetroTextBox();
            this.menuRighttop = new ns1.BunifuGradientPanel();
            this.iconPictureBox2 = new FontAwesome.Sharp.IconPictureBox();
            this.menuLefttxt = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.metroComboBox1 = new MetroFramework.Controls.MetroComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.menuRighttop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(31, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 20);
            this.label1.TabIndex = 9;
            this.label1.Text = "Tên tài khoản :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(31, 84);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 20);
            this.label2.TabIndex = 11;
            this.label2.Text = "Mật khẩu :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(31, 119);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(137, 20);
            this.label3.TabIndex = 13;
            this.label3.Text = "Nhập lại mật khẩu :";
            // 
            // captchaPanel
            // 
            this.captchaPanel.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.captchaPanel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.captchaPanel.Location = new System.Drawing.Point(177, 232);
            this.captchaPanel.Name = "captchaPanel";
            this.captchaPanel.Size = new System.Drawing.Size(189, 48);
            this.captchaPanel.TabIndex = 18;
            this.captchaPanel.Click += new System.EventHandler(this.captchaPanel_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(31, 233);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(95, 20);
            this.label6.TabIndex = 19;
            this.label6.Text = "Mã Captcha :";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(31, 297);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(134, 20);
            this.label7.TabIndex = 21;
            this.label7.Text = "Xác nhận Captcha :";
            // 
            // thongbaoTxt
            // 
            this.thongbaoTxt.AutoSize = true;
            this.thongbaoTxt.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.thongbaoTxt.ForeColor = System.Drawing.Color.Maroon;
            this.thongbaoTxt.Location = new System.Drawing.Point(73, 386);
            this.thongbaoTxt.Name = "thongbaoTxt";
            this.thongbaoTxt.Size = new System.Drawing.Size(75, 17);
            this.thongbaoTxt.TabIndex = 23;
            this.thongbaoTxt.Text = "Thông báo";
            // 
            // regBtn
            // 
            this.regBtn.BackColor = System.Drawing.Color.MistyRose;
            this.regBtn.DisplayFocus = true;
            this.regBtn.Highlight = true;
            this.regBtn.Location = new System.Drawing.Point(84, 335);
            this.regBtn.Name = "regBtn";
            this.regBtn.Size = new System.Drawing.Size(233, 38);
            this.regBtn.TabIndex = 22;
            this.regBtn.Text = "Đăng kí tài khoản mới";
            this.regBtn.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.regBtn.UseCustomBackColor = true;
            this.regBtn.UseCustomForeColor = true;
            this.regBtn.UseSelectable = true;
            this.regBtn.UseStyleColors = true;
            this.regBtn.UseVisualStyleBackColor = false;
            this.regBtn.Click += new System.EventHandler(this.regBtn_Click);
            // 
            // captchaTxt
            // 
            // 
            // 
            // 
            this.captchaTxt.CustomButton.Image = null;
            this.captchaTxt.CustomButton.Location = new System.Drawing.Point(167, 1);
            this.captchaTxt.CustomButton.Name = "";
            this.captchaTxt.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.captchaTxt.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.captchaTxt.CustomButton.TabIndex = 1;
            this.captchaTxt.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.captchaTxt.CustomButton.UseSelectable = true;
            this.captchaTxt.CustomButton.Visible = false;
            this.captchaTxt.Lines = new string[] {
        "Nhập mã bên trên"};
            this.captchaTxt.Location = new System.Drawing.Point(177, 296);
            this.captchaTxt.MaxLength = 32767;
            this.captchaTxt.Name = "captchaTxt";
            this.captchaTxt.PasswordChar = '\0';
            this.captchaTxt.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.captchaTxt.SelectedText = "";
            this.captchaTxt.SelectionLength = 0;
            this.captchaTxt.SelectionStart = 0;
            this.captchaTxt.ShortcutsEnabled = true;
            this.captchaTxt.Size = new System.Drawing.Size(189, 23);
            this.captchaTxt.TabIndex = 20;
            this.captchaTxt.Text = "Nhập mã bên trên";
            this.captchaTxt.UseSelectable = true;
            this.captchaTxt.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.captchaTxt.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            this.captchaTxt.Enter += new System.EventHandler(this.captchaTxt_Enter);
            this.captchaTxt.Leave += new System.EventHandler(this.captchaTxt_Leave);
            // 
            // txtInvite
            // 
            // 
            // 
            // 
            this.txtInvite.CustomButton.Image = null;
            this.txtInvite.CustomButton.Location = new System.Drawing.Point(167, 1);
            this.txtInvite.CustomButton.Name = "";
            this.txtInvite.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.txtInvite.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtInvite.CustomButton.TabIndex = 1;
            this.txtInvite.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtInvite.CustomButton.UseSelectable = true;
            this.txtInvite.CustomButton.Visible = false;
            this.txtInvite.Lines = new string[0];
            this.txtInvite.Location = new System.Drawing.Point(177, 195);
            this.txtInvite.MaxLength = 32767;
            this.txtInvite.Name = "txtInvite";
            this.txtInvite.PasswordChar = '●';
            this.txtInvite.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtInvite.SelectedText = "";
            this.txtInvite.SelectionLength = 0;
            this.txtInvite.SelectionStart = 0;
            this.txtInvite.ShortcutsEnabled = true;
            this.txtInvite.Size = new System.Drawing.Size(189, 23);
            this.txtInvite.TabIndex = 16;
            this.txtInvite.UseSelectable = true;
            this.txtInvite.Visible = false;
            this.txtInvite.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtInvite.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // repassTxt
            // 
            // 
            // 
            // 
            this.repassTxt.CustomButton.Image = null;
            this.repassTxt.CustomButton.Location = new System.Drawing.Point(167, 1);
            this.repassTxt.CustomButton.Name = "";
            this.repassTxt.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.repassTxt.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.repassTxt.CustomButton.TabIndex = 1;
            this.repassTxt.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.repassTxt.CustomButton.UseSelectable = true;
            this.repassTxt.CustomButton.Visible = false;
            this.repassTxt.Lines = new string[0];
            this.repassTxt.Location = new System.Drawing.Point(177, 116);
            this.repassTxt.MaxLength = 32767;
            this.repassTxt.Name = "repassTxt";
            this.repassTxt.PasswordChar = '●';
            this.repassTxt.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.repassTxt.SelectedText = "";
            this.repassTxt.SelectionLength = 0;
            this.repassTxt.SelectionStart = 0;
            this.repassTxt.ShortcutsEnabled = true;
            this.repassTxt.Size = new System.Drawing.Size(189, 23);
            this.repassTxt.TabIndex = 12;
            this.repassTxt.UseSelectable = true;
            this.repassTxt.UseSystemPasswordChar = true;
            this.repassTxt.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.repassTxt.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // passTxt
            // 
            // 
            // 
            // 
            this.passTxt.CustomButton.Image = null;
            this.passTxt.CustomButton.Location = new System.Drawing.Point(167, 1);
            this.passTxt.CustomButton.Name = "";
            this.passTxt.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.passTxt.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.passTxt.CustomButton.TabIndex = 1;
            this.passTxt.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.passTxt.CustomButton.UseSelectable = true;
            this.passTxt.CustomButton.Visible = false;
            this.passTxt.Lines = new string[0];
            this.passTxt.Location = new System.Drawing.Point(177, 81);
            this.passTxt.MaxLength = 32767;
            this.passTxt.Name = "passTxt";
            this.passTxt.PasswordChar = '●';
            this.passTxt.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.passTxt.SelectedText = "";
            this.passTxt.SelectionLength = 0;
            this.passTxt.SelectionStart = 0;
            this.passTxt.ShortcutsEnabled = true;
            this.passTxt.Size = new System.Drawing.Size(189, 23);
            this.passTxt.TabIndex = 10;
            this.passTxt.UseSelectable = true;
            this.passTxt.UseSystemPasswordChar = true;
            this.passTxt.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.passTxt.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // userTxt
            // 
            // 
            // 
            // 
            this.userTxt.CustomButton.Image = null;
            this.userTxt.CustomButton.Location = new System.Drawing.Point(167, 1);
            this.userTxt.CustomButton.Name = "";
            this.userTxt.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.userTxt.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.userTxt.CustomButton.TabIndex = 1;
            this.userTxt.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.userTxt.CustomButton.UseSelectable = true;
            this.userTxt.CustomButton.Visible = false;
            this.userTxt.Lines = new string[0];
            this.userTxt.Location = new System.Drawing.Point(177, 46);
            this.userTxt.MaxLength = 32767;
            this.userTxt.Name = "userTxt";
            this.userTxt.PasswordChar = '\0';
            this.userTxt.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.userTxt.SelectedText = "";
            this.userTxt.SelectionLength = 0;
            this.userTxt.SelectionStart = 0;
            this.userTxt.ShortcutsEnabled = true;
            this.userTxt.Size = new System.Drawing.Size(189, 23);
            this.userTxt.TabIndex = 8;
            this.userTxt.UseSelectable = true;
            this.userTxt.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.userTxt.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // menuRighttop
            // 
            this.menuRighttop.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("menuRighttop.BackgroundImage")));
            this.menuRighttop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.menuRighttop.Controls.Add(this.iconPictureBox2);
            this.menuRighttop.Controls.Add(this.menuLefttxt);
            this.menuRighttop.Dock = System.Windows.Forms.DockStyle.Top;
            this.menuRighttop.GradientBottomLeft = System.Drawing.Color.DarkOrchid;
            this.menuRighttop.GradientBottomRight = System.Drawing.Color.SandyBrown;
            this.menuRighttop.GradientTopLeft = System.Drawing.Color.DarkGoldenrod;
            this.menuRighttop.GradientTopRight = System.Drawing.Color.Olive;
            this.menuRighttop.Location = new System.Drawing.Point(0, 0);
            this.menuRighttop.Margin = new System.Windows.Forms.Padding(4);
            this.menuRighttop.Name = "menuRighttop";
            this.menuRighttop.Quality = 10;
            this.menuRighttop.Size = new System.Drawing.Size(395, 35);
            this.menuRighttop.TabIndex = 7;
            this.menuRighttop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.menuRighttop_MouseDown);
            // 
            // iconPictureBox2
            // 
            this.iconPictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.iconPictureBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.iconPictureBox2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.iconPictureBox2.IconChar = FontAwesome.Sharp.IconChar.Times;
            this.iconPictureBox2.IconColor = System.Drawing.Color.White;
            this.iconPictureBox2.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconPictureBox2.IconSize = 22;
            this.iconPictureBox2.Location = new System.Drawing.Point(356, 4);
            this.iconPictureBox2.Margin = new System.Windows.Forms.Padding(4);
            this.iconPictureBox2.Name = "iconPictureBox2";
            this.iconPictureBox2.Size = new System.Drawing.Size(31, 22);
            this.iconPictureBox2.TabIndex = 15;
            this.iconPictureBox2.TabStop = false;
            this.iconPictureBox2.Click += new System.EventHandler(this.iconPictureBox2_Click);
            this.iconPictureBox2.MouseEnter += new System.EventHandler(this.iconPictureBox2_MouseEnter);
            this.iconPictureBox2.MouseLeave += new System.EventHandler(this.iconPictureBox2_MouseLeave);
            // 
            // menuLefttxt
            // 
            this.menuLefttxt.AutoSize = true;
            this.menuLefttxt.BackColor = System.Drawing.Color.Transparent;
            this.menuLefttxt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.menuLefttxt.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuLefttxt.ForeColor = System.Drawing.Color.White;
            this.menuLefttxt.Location = new System.Drawing.Point(109, 9);
            this.menuLefttxt.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.menuLefttxt.Name = "menuLefttxt";
            this.menuLefttxt.Size = new System.Drawing.Size(165, 19);
            this.menuLefttxt.TabIndex = 1;
            this.menuLefttxt.Text = "Đăng kí tài khoản mới";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(31, 197);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(117, 20);
            this.label5.TabIndex = 17;
            this.label5.Text = "Mật khẩu cấp 2 :";
            this.label5.Visible = false;
            // 
            // metroComboBox1
            // 
            this.metroComboBox1.FormattingEnabled = true;
            this.metroComboBox1.ItemHeight = 23;
            this.metroComboBox1.Location = new System.Drawing.Point(177, 153);
            this.metroComboBox1.Name = "metroComboBox1";
            this.metroComboBox1.Size = new System.Drawing.Size(189, 29);
            this.metroComboBox1.TabIndex = 24;
            this.metroComboBox1.UseSelectable = true;
            this.metroComboBox1.Visible = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.label8.Location = new System.Drawing.Point(31, 154);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(72, 20);
            this.label8.TabIndex = 25;
            this.label8.Text = "Giới tính :";
            this.label8.Visible = false;
            // 
            // RegisterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(395, 413);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.metroComboBox1);
            this.Controls.Add(this.thongbaoTxt);
            this.Controls.Add(this.regBtn);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.captchaTxt);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.captchaPanel);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.repassTxt);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.passTxt);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.userTxt);
            this.Controls.Add(this.menuRighttop);
            this.Controls.Add(this.txtInvite);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.Name = "RegisterForm";
            this.Opacity = 0.95D;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Đăng kí tài khoản";
            this.Load += new System.EventHandler(this.RegisterForm_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.RegisterForm_MouseDown);
            this.menuRighttop.ResumeLayout(false);
            this.menuRighttop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        internal static void WWxmwWf0OsUYGNbywgx()
        {
        }

        internal static bool coe5QWfiTyQD3SGfndQ()
        {
            return nHFhWTfdcHXDoCh2w10 == null;
        }

        private void captchaTxt_Enter(object sender, EventArgs e)
        {
            if (this.captchaTxt.Text == "Nhập mã bên trên")
            {
                this.captchaTxt.Text = "";
            }
        }

        private void captchaTxt_Leave(object sender, EventArgs e)
        {
            if (this.captchaTxt.Text == "")
            {
                this.captchaTxt.Text = "Nhập mã bên trên";
            }
        }
    }
}
