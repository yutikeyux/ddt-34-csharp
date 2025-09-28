using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using FontAwesome.Sharp;
using LauncherGHU.API;
using LauncherGHU.Properties;
using MetroFramework;
using MetroFramework.Controls;

namespace LauncherGHU.ControlsForm
{
	public class Recharge : UserControl
	{
		private int int_0 = 0;

		private int int_1 = 0;

		private string CueDyPqJb6 = "";

		private string string_0 = "";

		private int int_2 = 0;

		private int serverid = 0;

		private int useridchange = 0;

		public static Dictionary<string, CharacterInfo> CharacterList;

		private IContainer icontainer_0 = null;

		private MetroTabControl tabControl;

		private TabPage rechargePage;

		private TabPage changemonePage;

		private TabPage logPage;

		private Panel panelTop;

		private Panel panelBotofTop;

		private Label label1;

		private Label label2;

		private Label coinNaptheTxt;

		private Label label4;

		private Label label3;

		private Panel panelBody;

		private GroupBox typeCardGroup;

		private Button viettelBtn;

		private Button vinaphoneBtn;

		private Button mobiphoneBtn;

		private GroupBox seriandpassGroup;

		private TextBox passcardTxt;

		private TextBox sericardTxt;

		private Label label6;

		private Label label5;

		private GroupBox moneycardGroup;

		private MetroRadioButton vnd20k;

		private MetroRadioButton vnd10k;

		private MetroRadioButton vnd30k;

		private MetroRadioButton vnd50k;

		private MetroRadioButton vnd100k;

		private MetroRadioButton vnd2000k;

		private MetroRadioButton vnd500k;

		private MetroRadioButton vnd1000k;

		private MetroRadioButton vnd200k;

		private MetroRadioButton vnd300k;

		private Label label7;

		private IconButton napngayBtn;

		private IconButton nhaplaiBtn;

		private Label thongbaoloaithelb;

		private Label thongbaomenhgialb;

		private Button gateBtn;

		private Button zingBtn;

		private Panel panel1;

		private Panel panel2;

		private Label label8;

		private Label label9;

		private Label label10;

		private Label label11;

		private Label label12;

		private Panel panelBodyChangeMoney;

		private MetroComboBox serverListCmBox;

		private Label label13;

		private Label label14;

		private MetroComboBox characterRewardmoneyCb;

		private TextBox passtwoChangemoneyTxt;

		private Label label15;

		private TextBox coinChangeTxt;

		private Label label16;

		private TextBox moneyRewardTxt;

		private Label label18;

		private Label label17;

		private IconButton nhaplaiBtnExchange;

		private IconButton doingayBtn;

		private MetroGrid datalogGrid;
        private Button vcoinBtn;
        private Button vnmbBtn;

		public Recharge()
		{
			InitializeComponent();
			method_1();
			method_4();
			method_0();
			((TabControl)(object)tabControl).SelectedIndex = 0;
		}

		private void method_0()
		{
			Dictionary<int, string> dictionary = new Dictionary<int, string>();
			dictionary.Add(0, "-- Vui lòng chọn máy chủ --");
			foreach (KeyValuePair<int, ServerInfo> server in ControlMgr.ServerList)
			{
				dictionary.Add(server.Key, server.Value.ServerName);
			}
			((ComboBox)(object)serverListCmBox).Items.Clear();
			((ComboBox)(object)serverListCmBox).DataSource = new BindingSource(dictionary, null);
			((ListControl)(object)serverListCmBox).DisplayMember = "Value";
			((ListControl)(object)serverListCmBox).ValueMember = "Key";
		}

		public bool IsNumeric(string value)
		{
			return value.All(char.IsNumber);
		}

		private void method_1()
		{
			string coin = ControlMgr.GetCoin();
			int_2 = int.Parse(coin.Replace("\r\n ", string.Empty).Replace(",", string.Empty));
			string text3 = (label10.Text = (coinNaptheTxt.Text = coin + " Coin"));
		}

		private void passcardTxt_Enter(object sender, EventArgs e)
		{
			if (passcardTxt.Text == "Vui lòng nhập mã thẻ")
			{
				passcardTxt.Text = "";
				passcardTxt.ForeColor = Color.Maroon;
			}
		}

		private void passcardTxt_Leave(object sender, EventArgs e)
		{
			if (passcardTxt.Text == "")
			{
				passcardTxt.Text = "Vui lòng nhập mã thẻ";
				passcardTxt.ForeColor = Color.DimGray;
			}
		}

		private void sericardTxt_Enter(object sender, EventArgs e)
		{
			if (sericardTxt.Text == "Vui lòng nhập Serial thẻ")
			{
				sericardTxt.Text = "";
				sericardTxt.ForeColor = Color.Maroon;
			}
		}

		private void sericardTxt_Leave(object sender, EventArgs e)
		{
			if (sericardTxt.Text == "")
			{
				sericardTxt.Text = "Vui lòng nhập Serial thẻ";
				sericardTxt.ForeColor = Color.DimGray;
			}
		}

		private void moneycardGroup_Enter(object sender, EventArgs e)
		{
		}
        private void vcoinBth_Click(object sender, EventArgs e)
        {
            int_0 = 9;
            viettelBtn.Enabled = true;
            vinaphoneBtn.Enabled = true;
            mobiphoneBtn.Enabled = true;
            gateBtn.Enabled = true;
            zingBtn.Enabled = true;
            vnmbBtn.Enabled = true;
            vcoinBtn.Enabled = false;
            method_2();
        }

        private void viettelBtn_Click(object sender, EventArgs e)
		{
			int_0 = 1;
			viettelBtn.Enabled = false;
			vinaphoneBtn.Enabled = true;
			mobiphoneBtn.Enabled = true;
			gateBtn.Enabled = true;
			zingBtn.Enabled = true;
			vnmbBtn.Enabled = true;
            vcoinBtn.Enabled = true;
            method_2();
		}

		private void method_2()
		{
			if (int_0 == 1)
			{
				thongbaoloaithelb.Text = "Bạn đã chọn loại thẻ : Viettel";
			}
			if (int_0 == 2)
			{
				thongbaoloaithelb.Text = "Bạn đã chọn loại thẻ : Mobiphone";
			}
			if (int_0 == 3)
			{
				thongbaoloaithelb.Text = "Bạn đã chọn loại thẻ : Vinaphone";
			}
			if (int_0 == 4)
			{
				thongbaoloaithelb.Text = "Bạn đã chọn loại thẻ : Gate";
			}
			if (int_0 == 7)
			{
				thongbaoloaithelb.Text = "Bạn đã chọn loại thẻ : Zing";
			}
			if (int_0 == 8)
			{
				thongbaoloaithelb.Text = "Bạn đã chọn loại thẻ : Vietnamobile";
			}
            if (int_0 == 9)
            {
                thongbaoloaithelb.Text = "Bạn đã chọn loại thẻ : Vcoin";
            }
        }

		private void vinaphoneBtn_Click(object sender, EventArgs e)
		{
			int_0 = 3;
			viettelBtn.Enabled = true;
			vinaphoneBtn.Enabled = false;
			mobiphoneBtn.Enabled = true;
			gateBtn.Enabled = true;
			zingBtn.Enabled = true;
			vnmbBtn.Enabled = true;
            vcoinBtn.Enabled = true;
            method_2();
		}

		private void mobiphoneBtn_Click(object sender, EventArgs e)
		{
			int_0 = 2;
			viettelBtn.Enabled = true;
			vinaphoneBtn.Enabled = true;
			mobiphoneBtn.Enabled = false;
			gateBtn.Enabled = true;
			zingBtn.Enabled = true;
			vnmbBtn.Enabled = true;
            vcoinBtn.Enabled = true;
            method_2();
		}

		private void vnd20k_CheckedChanged(object sender, EventArgs e)
		{
			if (((RadioButton)(object)vnd10k).Checked)
			{
				int_1 = 10000;
			}
			else if (!((RadioButton)(object)vnd20k).Checked)
			{
				if (((RadioButton)(object)vnd30k).Checked)
				{
					int_1 = 30000;
				}
				else if (!((RadioButton)(object)vnd50k).Checked)
				{
					if (((RadioButton)(object)vnd100k).Checked)
					{
						int_1 = 100000;
					}
					else if (!((RadioButton)(object)vnd200k).Checked)
					{
						if (((RadioButton)(object)vnd300k).Checked)
						{
							int_1 = 300000;
						}
						else if (!((RadioButton)(object)vnd500k).Checked)
						{
							if (!((RadioButton)(object)vnd1000k).Checked)
							{
								if (((RadioButton)(object)vnd2000k).Checked)
								{
									int_1 = 2000000;
								}
							}
							else
							{
								int_1 = 1000000;
							}
						}
						else
						{
							int_1 = 500000;
						}
					}
					else
					{
						int_1 = 200000;
					}
				}
				else
				{
					int_1 = 50000;
				}
			}
			else
			{
				int_1 = 20000;
			}
			thongbaomenhgialb.Text = $"Bạn đang chọn mệnh giá thẻ : {int_1:#,##.##} VNĐ";
		}

		private void napngayBtn_Click(object sender, EventArgs e)
		{
			try
			{
				if (int_0 != 0)
				{
					if (int_1 == 0)
					{
						MessageBox.Show("Vui lòng chọn mệnh giá thẻ", "Thông báo");
					}
					else if (!(sericardTxt.Text == ""))
					{
						if (passcardTxt.Text == "")
						{
							MessageBox.Show("Vui lòng nhập Mã thẻ", "Thông báo");
							return;
						}
						napngayBtn.Enabled = false;
						string url = $"{ApplicationConfig.UrlApi}Payment/Pay.php";
						string text = RechargeAPI.Recharge(url, LoginMgr.Username, LoginMgr.Password, int_0, sericardTxt.Text, passcardTxt.Text, int_1);
						MessageBox.Show(text, "Thông báo");
						if (text.IndexOf("thành công") > -1)
						{
							nhaplaiBtn_Click(null, null);
							method_1();
							method_4();
						}
						napngayBtn.Enabled = true;
					}
					else
					{
						MessageBox.Show("Vui lòng nhập Serial thẻ", "Thông báo");
					}
				}
				else
				{
					MessageBox.Show("Vui lòng chọn loại thẻ", "Thông báo");
				}
			}
			catch (Exception ex)
			{
				ControlMgr.UpLogLauncher(ex.ToString() + "-(napngayBtn_Click) Now Version : " + Assembly.GetEntryAssembly().GetName().Version);
				MessageBox.Show(ex.Message.ToString(), "Lỗi");
				napngayBtn.Enabled = true;
			}
		}

		private void nhaplaiBtn_Click(object sender, EventArgs e)
		{
			passcardTxt.Text = "";
			sericardTxt.Text = "";
			viettelBtn.Enabled = true;
			mobiphoneBtn.Enabled = true;
			vinaphoneBtn.Enabled = true;
			gateBtn.Enabled = true;
			zingBtn.Enabled = true;
			int_0 = 0;
			passcardTxt_Leave(null, null);
			sericardTxt_Leave(null, null);
		}

		private void gateBtn_Click(object sender, EventArgs e)
		{
			int_0 = 4;
			viettelBtn.Enabled = true;
			vinaphoneBtn.Enabled = true;
			mobiphoneBtn.Enabled = true;
			gateBtn.Enabled = false;
			zingBtn.Enabled = true;
			vnmbBtn.Enabled = true;
            vcoinBtn.Enabled = true;
            method_2();
		}

		private void zingBtn_Click(object sender, EventArgs e)
		{
			int_0 = 7;
			viettelBtn.Enabled = true;
			vinaphoneBtn.Enabled = true;
			mobiphoneBtn.Enabled = true;
			gateBtn.Enabled = true;
			zingBtn.Enabled = false;
			vnmbBtn.Enabled = true;
            vcoinBtn.Enabled = true;
            method_2();
		}

		private void serverListCmBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (((ListControl)(object)serverListCmBox).SelectedValue != null && IsNumeric(((ListControl)(object)serverListCmBox).SelectedValue.ToString()) && !(((ListControl)(object)serverListCmBox).SelectedValue.ToString() == "0"))
			{
				serverid = int.Parse(((ListControl)(object)serverListCmBox).SelectedValue.ToString());
				method_3();
			}
		}

		private void method_3()
		{
			CharacterList.Clear();
			string[] array = RechargeAPI.SetupCharater($"{ApplicationConfig.UrlApi}getlistcharacter.php", serverid, LoginMgr.Username).Split('|');
			if (array.Length != 0)
			{
				string[] array2 = array;
				foreach (string text in array2)
				{
					if (string.IsNullOrEmpty(text))
					{
						break;
					}
					string[] array3 = text.Split(',');
					if (array3.Length >= 3 && !CharacterList.ContainsKey(array3[0].Replace("\r\n ", "")))
					{
						CharacterInfo characterInfo = new CharacterInfo();
						characterInfo.UserID = array3[0].Replace("\r\n ", "");
						characterInfo.Nickname = array3[1].Replace("\r\n ", "");
						characterInfo.Grade = array3[2].Replace("\r\n ", "");
						CharacterList.Add(characterInfo.UserID, characterInfo);
					}
				}
			}
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("0", "-- Vui lòng chọn nhân vật --");
			foreach (KeyValuePair<string, CharacterInfo> character in CharacterList)
			{
				dictionary.Add(character.Key, character.Value.Nickname + " | Cấp : " + character.Value.Grade);
			}
			((ComboBox)(object)characterRewardmoneyCb).DataSource = null;
			((ComboBox)(object)characterRewardmoneyCb).Items.Clear();
			((ComboBox)(object)characterRewardmoneyCb).DataSource = new BindingSource(dictionary, null);
			((ListControl)(object)characterRewardmoneyCb).DisplayMember = "Value";
			((ListControl)(object)characterRewardmoneyCb).ValueMember = "Key";
		}

		private void characterRewardmoneyCb_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (((ListControl)(object)characterRewardmoneyCb).SelectedValue != null && IsNumeric(((ListControl)(object)characterRewardmoneyCb).SelectedValue.ToString()) && !(((ListControl)(object)characterRewardmoneyCb).SelectedValue.ToString() == "0"))
			{
				useridchange = int.Parse(((ListControl)(object)characterRewardmoneyCb).SelectedValue.ToString());
			}
		}

		private void passtwoChangemoneyTxt_Enter(object sender, EventArgs e)
		{
			if (passtwoChangemoneyTxt.Text == "Bỏ trống nếu không có")
			{
				passtwoChangemoneyTxt.Text = "";
				passtwoChangemoneyTxt.ForeColor = Color.Maroon;
			}
		}

		private void passtwoChangemoneyTxt_Leave(object sender, EventArgs e)
		{
			if (passtwoChangemoneyTxt.Text == "")
			{
				passtwoChangemoneyTxt.Text = "Bỏ trống nếu không có";
				passtwoChangemoneyTxt.ForeColor = Color.DimGray;
			}
		}

		private void coinChangeTxt_Enter(object sender, EventArgs e)
		{
			if (coinChangeTxt.Text == "Nhập số lượng Coin muốn đổi sang xu")
			{
				coinChangeTxt.Text = "";
				coinChangeTxt.ForeColor = Color.Maroon;
			}
		}

		private void coinChangeTxt_Leave(object sender, EventArgs e)
		{
			if (coinChangeTxt.Text == "")
			{
				coinChangeTxt.Text = "Nhập số lượng Coin muốn đổi sang xu";
				coinChangeTxt.ForeColor = Color.DimGray;
			}
		}

		private void coinChangeTxt_TextChanged(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(coinChangeTxt.Text))
			{
				if (IsNumeric(coinChangeTxt.Text))
				{
					int num = int.Parse(coinChangeTxt.Text);
					if (num > int_2)
					{
						coinChangeTxt.Text = int_2.ToString();
						return;
					}
					//int num2 = num / 10;
					moneyRewardTxt.Text = String.Format("{0:n0}", num);
				}
				else
				{
					moneyRewardTxt.Text = "Xu nhận được";
				}
			}
			else
			{
				moneyRewardTxt.Text = "Xu nhận được";
			}
		}

		private void nhaplaiBtnExchange_Click(object sender, EventArgs e)
		{
			((ListControl)(object)characterRewardmoneyCb).SelectedIndex = 0;
			((ListControl)(object)serverListCmBox).SelectedIndex = 0;
			passtwoChangemoneyTxt.Text = "";
			coinChangeTxt.Text = "";
			coinChangeTxt_Leave(null, null);
			passtwoChangemoneyTxt_Leave(null, null);
			coinChangeTxt_TextChanged(null, null);
		}

		private void doingayBtn_Click(object sender, EventArgs e)
		{
			doingayBtn.Enabled = false;
			string url = $"{ApplicationConfig.UrlApi}Payment/recharge.php";
			string text = RechargeAPI.ChangeMoney(url, LoginMgr.Username, LoginMgr.Password, coinChangeTxt.Text, serverid.ToString(), useridchange.ToString(), passtwoChangemoneyTxt.Text.Replace("Bỏ trống nếu không có", ""));
			MessageBox.Show(text, "Thông báo");
			if (text.IndexOf("thành công") > -1)
			{
				nhaplaiBtnExchange_Click(null, null);
				method_1();
			}
			doingayBtn.Enabled = true;
		}

		private void method_4()
		{
			((DataGridView)(object)datalogGrid).AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
			((DataGridView)(object)datalogGrid).AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
			((DataGridView)(object)datalogGrid).DataSource = LogCardInfo.ListLogCards;
			((DataGridView)(object)datalogGrid).Columns.Add("ID", "ID");
			((DataGridView)(object)datalogGrid).Columns.Add("CardType", "Loại thẻ");
			((DataGridView)(object)datalogGrid).Columns.Add("Serial", "Serial thẻ");
			((DataGridView)(object)datalogGrid).Columns.Add("Passcard", "Mã thẻ");
			((DataGridView)(object)datalogGrid).Columns.Add("Money", "Mệnh giá");
			((DataGridView)(object)datalogGrid).Columns.Add("Status", "Trạng thái");
			((DataGridView)(object)datalogGrid).Columns.Add("Time", "Thời gian");

			//((DataGridView)(object)datalogGrid).Columns[0].HeaderText = "ID";
			//((DataGridView)(object)datalogGrid).Columns[1].HeaderText = "Loại thẻ";
			//((DataGridView)(object)datalogGrid).Columns[2].HeaderText = "Serial thẻ";
			//((DataGridView)(object)datalogGrid).Columns[3].HeaderText = "Mã thẻ";
			//((DataGridView)(object)datalogGrid).Columns[4].HeaderText = "Mệnh giá";
			//((DataGridView)(object)datalogGrid).Columns[5].HeaderText = "Trạng thái";
			//((DataGridView)(object)datalogGrid).Columns[6].HeaderText = "Thời gian";
		}

		private void vnmbBth_Click(object sender, EventArgs e)
		{
			int_0 = 8;
			viettelBtn.Enabled = true;
			vinaphoneBtn.Enabled = true;
			mobiphoneBtn.Enabled = true;
			gateBtn.Enabled = true;
			zingBtn.Enabled = true;
			vnmbBtn.Enabled = false;
            vcoinBtn.Enabled = true;
            method_2();
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tabControl = new MetroFramework.Controls.MetroTabControl();
            this.rechargePage = new System.Windows.Forms.TabPage();
            this.panelBody = new System.Windows.Forms.Panel();
            this.nhaplaiBtn = new FontAwesome.Sharp.IconButton();
            this.napngayBtn = new FontAwesome.Sharp.IconButton();
            this.label7 = new System.Windows.Forms.Label();
            this.moneycardGroup = new System.Windows.Forms.GroupBox();
            this.thongbaomenhgialb = new System.Windows.Forms.Label();
            this.vnd2000k = new MetroFramework.Controls.MetroRadioButton();
            this.vnd500k = new MetroFramework.Controls.MetroRadioButton();
            this.vnd1000k = new MetroFramework.Controls.MetroRadioButton();
            this.vnd200k = new MetroFramework.Controls.MetroRadioButton();
            this.vnd300k = new MetroFramework.Controls.MetroRadioButton();
            this.vnd100k = new MetroFramework.Controls.MetroRadioButton();
            this.vnd30k = new MetroFramework.Controls.MetroRadioButton();
            this.vnd50k = new MetroFramework.Controls.MetroRadioButton();
            this.vnd10k = new MetroFramework.Controls.MetroRadioButton();
            this.vnd20k = new MetroFramework.Controls.MetroRadioButton();
            this.seriandpassGroup = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.sericardTxt = new System.Windows.Forms.TextBox();
            this.passcardTxt = new System.Windows.Forms.TextBox();
            this.typeCardGroup = new System.Windows.Forms.GroupBox();
            this.vcoinBtn = new System.Windows.Forms.Button();
            this.vnmbBtn = new System.Windows.Forms.Button();
            this.zingBtn = new System.Windows.Forms.Button();
            this.gateBtn = new System.Windows.Forms.Button();
            this.thongbaoloaithelb = new System.Windows.Forms.Label();
            this.mobiphoneBtn = new System.Windows.Forms.Button();
            this.vinaphoneBtn = new System.Windows.Forms.Button();
            this.viettelBtn = new System.Windows.Forms.Button();
            this.panelTop = new System.Windows.Forms.Panel();
            this.panelBotofTop = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.coinNaptheTxt = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.changemonePage = new System.Windows.Forms.TabPage();
            this.panelBodyChangeMoney = new System.Windows.Forms.Panel();
            this.nhaplaiBtnExchange = new FontAwesome.Sharp.IconButton();
            this.doingayBtn = new FontAwesome.Sharp.IconButton();
            this.moneyRewardTxt = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.coinChangeTxt = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.passtwoChangemoneyTxt = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.characterRewardmoneyCb = new MetroFramework.Controls.MetroComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.serverListCmBox = new MetroFramework.Controls.MetroComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.logPage = new System.Windows.Forms.TabPage();
            this.datalogGrid = new MetroFramework.Controls.MetroGrid();
            this.tabControl.SuspendLayout();
            this.rechargePage.SuspendLayout();
            this.panelBody.SuspendLayout();
            this.moneycardGroup.SuspendLayout();
            this.seriandpassGroup.SuspendLayout();
            this.typeCardGroup.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.panelBotofTop.SuspendLayout();
            this.changemonePage.SuspendLayout();
            this.panelBodyChangeMoney.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.logPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.datalogGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.AllowDrop = true;
            this.tabControl.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabControl.Controls.Add(this.rechargePage);
            this.tabControl.Controls.Add(this.changemonePage);
            this.tabControl.Controls.Add(this.logPage);
            this.tabControl.Cursor = System.Windows.Forms.Cursors.Default;
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.tabControl.FontWeight = MetroFramework.MetroTabControlWeight.Regular;
            this.tabControl.HotTrack = true;
            this.tabControl.ItemSize = new System.Drawing.Size(90, 40);
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Margin = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.tabControl.Multiline = true;
            this.tabControl.Name = "tabControl";
            this.tabControl.Padding = new System.Drawing.Point(6, 8);
            this.tabControl.SelectedIndex = 0;
            this.tabControl.ShowToolTips = true;
            this.tabControl.Size = new System.Drawing.Size(1000, 625);
            this.tabControl.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
            this.tabControl.Style = MetroFramework.MetroColorStyle.Red;
            this.tabControl.TabIndex = 0;
            this.tabControl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.tabControl.Theme = MetroFramework.MetroThemeStyle.Light;
            this.tabControl.UseCustomBackColor = true;
            this.tabControl.UseSelectable = true;
            // 
            // rechargePage
            // 
            this.rechargePage.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.rechargePage.Controls.Add(this.panelBody);
            this.rechargePage.Controls.Add(this.panelTop);
            this.rechargePage.Cursor = System.Windows.Forms.Cursors.Default;
            this.rechargePage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rechargePage.Location = new System.Drawing.Point(4, 44);
            this.rechargePage.Name = "rechargePage";
            this.rechargePage.Size = new System.Drawing.Size(992, 577);
            this.rechargePage.TabIndex = 0;
            this.rechargePage.Text = "NẠP THẺ";
            // 
            // panelBody
            // 
            this.panelBody.BackColor = System.Drawing.Color.White;
            this.panelBody.Controls.Add(this.nhaplaiBtn);
            this.panelBody.Controls.Add(this.napngayBtn);
            this.panelBody.Controls.Add(this.label7);
            this.panelBody.Controls.Add(this.moneycardGroup);
            this.panelBody.Controls.Add(this.seriandpassGroup);
            this.panelBody.Controls.Add(this.typeCardGroup);
            this.panelBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBody.Location = new System.Drawing.Point(0, 112);
            this.panelBody.Name = "panelBody";
            this.panelBody.Size = new System.Drawing.Size(992, 465);
            this.panelBody.TabIndex = 1;
            // 
            // nhaplaiBtn
            // 
            this.nhaplaiBtn.BackColor = System.Drawing.Color.DarkOrange;
            this.nhaplaiBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.nhaplaiBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.nhaplaiBtn.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nhaplaiBtn.ForeColor = System.Drawing.Color.Black;
            this.nhaplaiBtn.IconChar = FontAwesome.Sharp.IconChar.Retweet;
            this.nhaplaiBtn.IconColor = System.Drawing.Color.Maroon;
            this.nhaplaiBtn.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.nhaplaiBtn.IconSize = 20;
            this.nhaplaiBtn.Location = new System.Drawing.Point(476, 411);
            this.nhaplaiBtn.Name = "nhaplaiBtn";
            this.nhaplaiBtn.Size = new System.Drawing.Size(136, 36);
            this.nhaplaiBtn.TabIndex = 7;
            this.nhaplaiBtn.Text = "    NHẬP LẠI";
            this.nhaplaiBtn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.nhaplaiBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.nhaplaiBtn.UseVisualStyleBackColor = false;
            this.nhaplaiBtn.Click += new System.EventHandler(this.nhaplaiBtn_Click);
            // 
            // napngayBtn
            // 
            this.napngayBtn.BackColor = System.Drawing.Color.SpringGreen;
            this.napngayBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.napngayBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.napngayBtn.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.napngayBtn.ForeColor = System.Drawing.Color.Maroon;
            this.napngayBtn.IconChar = FontAwesome.Sharp.IconChar.EuroSign;
            this.napngayBtn.IconColor = System.Drawing.Color.Maroon;
            this.napngayBtn.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.napngayBtn.IconSize = 20;
            this.napngayBtn.Location = new System.Drawing.Point(326, 411);
            this.napngayBtn.Name = "napngayBtn";
            this.napngayBtn.Size = new System.Drawing.Size(136, 36);
            this.napngayBtn.TabIndex = 6;
            this.napngayBtn.Text = "    NẠP NGAY";
            this.napngayBtn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.napngayBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.napngayBtn.UseVisualStyleBackColor = false;
            this.napngayBtn.Click += new System.EventHandler(this.napngayBtn_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Red;
            this.label7.Location = new System.Drawing.Point(214, 387);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(623, 16);
            this.label7.TabIndex = 4;
            this.label7.Text = "Chú ý ! Chọn đúng mã thẻ, thẻ sai mệnh giá sẽ bị mất . Ban quản trị sẽ không chịu" +
    " trách nghiệm";
            // 
            // moneycardGroup
            // 
            this.moneycardGroup.Controls.Add(this.thongbaomenhgialb);
            this.moneycardGroup.Controls.Add(this.vnd2000k);
            this.moneycardGroup.Controls.Add(this.vnd500k);
            this.moneycardGroup.Controls.Add(this.vnd1000k);
            this.moneycardGroup.Controls.Add(this.vnd200k);
            this.moneycardGroup.Controls.Add(this.vnd300k);
            this.moneycardGroup.Controls.Add(this.vnd100k);
            this.moneycardGroup.Controls.Add(this.vnd30k);
            this.moneycardGroup.Controls.Add(this.vnd50k);
            this.moneycardGroup.Controls.Add(this.vnd10k);
            this.moneycardGroup.Controls.Add(this.vnd20k);
            this.moneycardGroup.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.moneycardGroup.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.moneycardGroup.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.moneycardGroup.Location = new System.Drawing.Point(20, 277);
            this.moneycardGroup.Name = "moneycardGroup";
            this.moneycardGroup.Size = new System.Drawing.Size(939, 100);
            this.moneycardGroup.TabIndex = 5;
            this.moneycardGroup.TabStop = false;
            this.moneycardGroup.Text = "Mệnh giá thẻ";
            this.moneycardGroup.Enter += new System.EventHandler(this.moneycardGroup_Enter);
            // 
            // thongbaomenhgialb
            // 
            this.thongbaomenhgialb.AutoSize = true;
            this.thongbaomenhgialb.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.thongbaomenhgialb.ForeColor = System.Drawing.Color.Blue;
            this.thongbaomenhgialb.Location = new System.Drawing.Point(610, 80);
            this.thongbaomenhgialb.Name = "thongbaomenhgialb";
            this.thongbaomenhgialb.Size = new System.Drawing.Size(164, 16);
            this.thongbaomenhgialb.TabIndex = 11;
            this.thongbaomenhgialb.Text = "Chưa chọn mệnh giá thẻ";
            // 
            // vnd2000k
            // 
            this.vnd2000k.AutoSize = true;
            this.vnd2000k.FontSize = MetroFramework.MetroCheckBoxSize.Medium;
            this.vnd2000k.Location = new System.Drawing.Point(790, 54);
            this.vnd2000k.Name = "vnd2000k";
            this.vnd2000k.Size = new System.Drawing.Size(120, 19);
            this.vnd2000k.TabIndex = 10;
            this.vnd2000k.Text = "2.000.000 VNĐ";
            this.vnd2000k.UseSelectable = true;
            this.vnd2000k.CheckedChanged += new System.EventHandler(this.vnd20k_CheckedChanged);
            // 
            // vnd500k
            // 
            this.vnd500k.AutoSize = true;
            this.vnd500k.FontSize = MetroFramework.MetroCheckBoxSize.Medium;
            this.vnd500k.Location = new System.Drawing.Point(409, 54);
            this.vnd500k.Name = "vnd500k";
            this.vnd500k.Size = new System.Drawing.Size(109, 19);
            this.vnd500k.TabIndex = 9;
            this.vnd500k.Text = "500.000 VNĐ";
            this.vnd500k.UseSelectable = true;
            this.vnd500k.CheckedChanged += new System.EventHandler(this.vnd20k_CheckedChanged);
            // 
            // vnd1000k
            // 
            this.vnd1000k.AutoSize = true;
            this.vnd1000k.FontSize = MetroFramework.MetroCheckBoxSize.Medium;
            this.vnd1000k.Location = new System.Drawing.Point(596, 54);
            this.vnd1000k.Name = "vnd1000k";
            this.vnd1000k.Size = new System.Drawing.Size(120, 19);
            this.vnd1000k.TabIndex = 8;
            this.vnd1000k.Text = "1.000.000 VNĐ";
            this.vnd1000k.UseSelectable = true;
            this.vnd1000k.CheckedChanged += new System.EventHandler(this.vnd20k_CheckedChanged);
            // 
            // vnd200k
            // 
            this.vnd200k.AutoSize = true;
            this.vnd200k.FontSize = MetroFramework.MetroCheckBoxSize.Medium;
            this.vnd200k.Location = new System.Drawing.Point(59, 54);
            this.vnd200k.Name = "vnd200k";
            this.vnd200k.Size = new System.Drawing.Size(109, 19);
            this.vnd200k.TabIndex = 7;
            this.vnd200k.Text = "200.000 VNĐ";
            this.vnd200k.UseSelectable = true;
            this.vnd200k.CheckedChanged += new System.EventHandler(this.vnd20k_CheckedChanged);
            // 
            // vnd300k
            // 
            this.vnd300k.AutoSize = true;
            this.vnd300k.FontSize = MetroFramework.MetroCheckBoxSize.Medium;
            this.vnd300k.Location = new System.Drawing.Point(231, 54);
            this.vnd300k.Name = "vnd300k";
            this.vnd300k.Size = new System.Drawing.Size(109, 19);
            this.vnd300k.TabIndex = 6;
            this.vnd300k.Text = "300.000 VNĐ";
            this.vnd300k.UseSelectable = true;
            this.vnd300k.CheckedChanged += new System.EventHandler(this.vnd20k_CheckedChanged);
            // 
            // vnd100k
            // 
            this.vnd100k.AutoSize = true;
            this.vnd100k.FontSize = MetroFramework.MetroCheckBoxSize.Medium;
            this.vnd100k.Location = new System.Drawing.Point(790, 25);
            this.vnd100k.Name = "vnd100k";
            this.vnd100k.Size = new System.Drawing.Size(109, 19);
            this.vnd100k.TabIndex = 5;
            this.vnd100k.Text = "100.000 VNĐ";
            this.vnd100k.UseSelectable = true;
            this.vnd100k.CheckedChanged += new System.EventHandler(this.vnd20k_CheckedChanged);
            // 
            // vnd30k
            // 
            this.vnd30k.AutoSize = true;
            this.vnd30k.FontSize = MetroFramework.MetroCheckBoxSize.Medium;
            this.vnd30k.Location = new System.Drawing.Point(409, 25);
            this.vnd30k.Name = "vnd30k";
            this.vnd30k.Size = new System.Drawing.Size(101, 19);
            this.vnd30k.TabIndex = 4;
            this.vnd30k.Text = "30.000 VNĐ";
            this.vnd30k.UseSelectable = true;
            this.vnd30k.CheckedChanged += new System.EventHandler(this.vnd20k_CheckedChanged);
            // 
            // vnd50k
            // 
            this.vnd50k.AutoSize = true;
            this.vnd50k.FontSize = MetroFramework.MetroCheckBoxSize.Medium;
            this.vnd50k.Location = new System.Drawing.Point(596, 25);
            this.vnd50k.Name = "vnd50k";
            this.vnd50k.Size = new System.Drawing.Size(101, 19);
            this.vnd50k.TabIndex = 3;
            this.vnd50k.Text = "50.000 VNĐ";
            this.vnd50k.UseSelectable = true;
            this.vnd50k.CheckedChanged += new System.EventHandler(this.vnd20k_CheckedChanged);
            // 
            // vnd10k
            // 
            this.vnd10k.AutoSize = true;
            this.vnd10k.FontSize = MetroFramework.MetroCheckBoxSize.Medium;
            this.vnd10k.Location = new System.Drawing.Point(59, 25);
            this.vnd10k.Name = "vnd10k";
            this.vnd10k.Size = new System.Drawing.Size(101, 19);
            this.vnd10k.TabIndex = 2;
            this.vnd10k.Text = "10.000 VNĐ";
            this.vnd10k.UseSelectable = true;
            this.vnd10k.CheckedChanged += new System.EventHandler(this.vnd20k_CheckedChanged);
            // 
            // vnd20k
            // 
            this.vnd20k.AutoSize = true;
            this.vnd20k.FontSize = MetroFramework.MetroCheckBoxSize.Medium;
            this.vnd20k.Location = new System.Drawing.Point(231, 25);
            this.vnd20k.Name = "vnd20k";
            this.vnd20k.Size = new System.Drawing.Size(101, 19);
            this.vnd20k.TabIndex = 1;
            this.vnd20k.Text = "20.000 VNĐ";
            this.vnd20k.UseSelectable = true;
            this.vnd20k.CheckedChanged += new System.EventHandler(this.vnd20k_CheckedChanged);
            // 
            // seriandpassGroup
            // 
            this.seriandpassGroup.Controls.Add(this.label6);
            this.seriandpassGroup.Controls.Add(this.label5);
            this.seriandpassGroup.Controls.Add(this.sericardTxt);
            this.seriandpassGroup.Controls.Add(this.passcardTxt);
            this.seriandpassGroup.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.seriandpassGroup.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.seriandpassGroup.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.seriandpassGroup.Location = new System.Drawing.Point(20, 154);
            this.seriandpassGroup.Name = "seriandpassGroup";
            this.seriandpassGroup.Size = new System.Drawing.Size(939, 120);
            this.seriandpassGroup.TabIndex = 3;
            this.seriandpassGroup.TabStop = false;
            this.seriandpassGroup.Text = "Serial và Mã thẻ";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Maroon;
            this.label6.Location = new System.Drawing.Point(30, 81);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(118, 19);
            this.label6.TabIndex = 4;
            this.label6.Text = "SERIAL THẺ :";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Maroon;
            this.label5.Location = new System.Drawing.Point(30, 33);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(84, 19);
            this.label5.TabIndex = 3;
            this.label5.Text = "MÃ THẺ :";
            // 
            // sericardTxt
            // 
            this.sericardTxt.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sericardTxt.ForeColor = System.Drawing.Color.DimGray;
            this.sericardTxt.Location = new System.Drawing.Point(164, 75);
            this.sericardTxt.Name = "sericardTxt";
            this.sericardTxt.Size = new System.Drawing.Size(710, 30);
            this.sericardTxt.TabIndex = 2;
            this.sericardTxt.Text = "Vui lòng nhập Serial thẻ";
            this.sericardTxt.Enter += new System.EventHandler(this.sericardTxt_Enter);
            this.sericardTxt.Leave += new System.EventHandler(this.sericardTxt_Leave);
            // 
            // passcardTxt
            // 
            this.passcardTxt.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.passcardTxt.ForeColor = System.Drawing.Color.DimGray;
            this.passcardTxt.Location = new System.Drawing.Point(164, 27);
            this.passcardTxt.Name = "passcardTxt";
            this.passcardTxt.Size = new System.Drawing.Size(710, 30);
            this.passcardTxt.TabIndex = 1;
            this.passcardTxt.Text = "Vui lòng nhập mã thẻ";
            this.passcardTxt.Enter += new System.EventHandler(this.passcardTxt_Enter);
            this.passcardTxt.Leave += new System.EventHandler(this.passcardTxt_Leave);
            // 
            // typeCardGroup
            // 
            this.typeCardGroup.Controls.Add(this.vcoinBtn);
            this.typeCardGroup.Controls.Add(this.vnmbBtn);
            this.typeCardGroup.Controls.Add(this.zingBtn);
            this.typeCardGroup.Controls.Add(this.gateBtn);
            this.typeCardGroup.Controls.Add(this.thongbaoloaithelb);
            this.typeCardGroup.Controls.Add(this.mobiphoneBtn);
            this.typeCardGroup.Controls.Add(this.vinaphoneBtn);
            this.typeCardGroup.Controls.Add(this.viettelBtn);
            this.typeCardGroup.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.typeCardGroup.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.typeCardGroup.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.typeCardGroup.Location = new System.Drawing.Point(20, 9);
            this.typeCardGroup.Name = "typeCardGroup";
            this.typeCardGroup.Size = new System.Drawing.Size(939, 146);
            this.typeCardGroup.TabIndex = 0;
            this.typeCardGroup.TabStop = false;
            this.typeCardGroup.Text = "Chọn loại thẻ cần nạp :";
            // 
            // vcoinBtn
            // 
            this.vcoinBtn.AutoEllipsis = true;
            this.vcoinBtn.BackColor = System.Drawing.Color.White;
            this.vcoinBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.vcoinBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.vcoinBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.vcoinBtn.Location = new System.Drawing.Point(458, 84);
            this.vcoinBtn.Name = "vcoinBtn";
            this.vcoinBtn.Size = new System.Drawing.Size(203, 51);
            this.vcoinBtn.TabIndex = 7;
            this.vcoinBtn.Text = "VCOIN";
            this.vcoinBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.vcoinBtn.UseVisualStyleBackColor = false;
            this.vcoinBtn.Click += new System.EventHandler(this.vcoinBth_Click);
            // 
            // vnmbBtn
            // 
            this.vnmbBtn.AutoEllipsis = true;
            this.vnmbBtn.BackColor = System.Drawing.Color.White;
            this.vnmbBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.vnmbBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.vnmbBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.vnmbBtn.Location = new System.Drawing.Point(242, 84);
            this.vnmbBtn.Name = "vnmbBtn";
            this.vnmbBtn.Size = new System.Drawing.Size(203, 51);
            this.vnmbBtn.TabIndex = 6;
            this.vnmbBtn.Text = "VNMOBILE";
            this.vnmbBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.vnmbBtn.UseVisualStyleBackColor = false;
            this.vnmbBtn.Click += new System.EventHandler(this.vnmbBth_Click);
            // 
            // zingBtn
            // 
            this.zingBtn.AutoEllipsis = true;
            this.zingBtn.BackColor = System.Drawing.Color.White;
            this.zingBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.zingBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.zingBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.zingBtn.Location = new System.Drawing.Point(24, 84);
            this.zingBtn.Name = "zingBtn";
            this.zingBtn.Size = new System.Drawing.Size(203, 51);
            this.zingBtn.TabIndex = 5;
            this.zingBtn.Text = "ZING";
            this.zingBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.zingBtn.UseVisualStyleBackColor = false;
            this.zingBtn.Click += new System.EventHandler(this.zingBtn_Click);
            // 
            // gateBtn
            // 
            this.gateBtn.AutoEllipsis = true;
            this.gateBtn.BackColor = System.Drawing.Color.White;
            this.gateBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.gateBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.gateBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.gateBtn.Location = new System.Drawing.Point(674, 25);
            this.gateBtn.Name = "gateBtn";
            this.gateBtn.Size = new System.Drawing.Size(203, 51);
            this.gateBtn.TabIndex = 4;
            this.gateBtn.Text = "GATE";
            this.gateBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.gateBtn.UseVisualStyleBackColor = false;
            this.gateBtn.Click += new System.EventHandler(this.gateBtn_Click);
            // 
            // thongbaoloaithelb
            // 
            this.thongbaoloaithelb.AutoSize = true;
            this.thongbaoloaithelb.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.thongbaoloaithelb.ForeColor = System.Drawing.Color.Blue;
            this.thongbaoloaithelb.Location = new System.Drawing.Point(708, 119);
            this.thongbaoloaithelb.Name = "thongbaoloaithelb";
            this.thongbaoloaithelb.Size = new System.Drawing.Size(128, 16);
            this.thongbaoloaithelb.TabIndex = 3;
            this.thongbaoloaithelb.Text = "Chưa chọn loại thẻ";
            // 
            // mobiphoneBtn
            // 
            this.mobiphoneBtn.AutoEllipsis = true;
            this.mobiphoneBtn.BackColor = System.Drawing.Color.White;
            this.mobiphoneBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.mobiphoneBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.mobiphoneBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.mobiphoneBtn.Location = new System.Drawing.Point(458, 25);
            this.mobiphoneBtn.Name = "mobiphoneBtn";
            this.mobiphoneBtn.Size = new System.Drawing.Size(203, 51);
            this.mobiphoneBtn.TabIndex = 2;
            this.mobiphoneBtn.Text = "MOBIPHONE";
            this.mobiphoneBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.mobiphoneBtn.UseVisualStyleBackColor = false;
            this.mobiphoneBtn.Click += new System.EventHandler(this.mobiphoneBtn_Click);
            // 
            // vinaphoneBtn
            // 
            this.vinaphoneBtn.AutoEllipsis = true;
            this.vinaphoneBtn.BackColor = System.Drawing.Color.White;
            this.vinaphoneBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.vinaphoneBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.vinaphoneBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.vinaphoneBtn.Location = new System.Drawing.Point(242, 25);
            this.vinaphoneBtn.Name = "vinaphoneBtn";
            this.vinaphoneBtn.Size = new System.Drawing.Size(203, 51);
            this.vinaphoneBtn.TabIndex = 1;
            this.vinaphoneBtn.Text = "VINAPHONE";
            this.vinaphoneBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.vinaphoneBtn.UseVisualStyleBackColor = false;
            this.vinaphoneBtn.Click += new System.EventHandler(this.vinaphoneBtn_Click);
            // 
            // viettelBtn
            // 
            this.viettelBtn.AutoEllipsis = true;
            this.viettelBtn.BackColor = System.Drawing.Color.White;
            this.viettelBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.viettelBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.viettelBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.viettelBtn.Location = new System.Drawing.Point(24, 25);
            this.viettelBtn.Name = "viettelBtn";
            this.viettelBtn.Size = new System.Drawing.Size(203, 51);
            this.viettelBtn.TabIndex = 0;
            this.viettelBtn.Text = "VIETTEL";
            this.viettelBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.viettelBtn.UseVisualStyleBackColor = false;
            this.viettelBtn.Click += new System.EventHandler(this.viettelBtn_Click);
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.panelTop.Controls.Add(this.panelBotofTop);
            this.panelTop.Controls.Add(this.label1);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(992, 112);
            this.panelTop.TabIndex = 0;
            // 
            // panelBotofTop
            // 
            this.panelBotofTop.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.panelBotofTop.Controls.Add(this.label4);
            this.panelBotofTop.Controls.Add(this.label3);
            this.panelBotofTop.Controls.Add(this.coinNaptheTxt);
            this.panelBotofTop.Controls.Add(this.label2);
            this.panelBotofTop.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBotofTop.Location = new System.Drawing.Point(0, 76);
            this.panelBotofTop.Name = "panelBotofTop";
            this.panelBotofTop.Size = new System.Drawing.Size(992, 36);
            this.panelBotofTop.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Orange;
            this.label4.Location = new System.Drawing.Point(813, 11);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(176, 16);
            this.label4.TabIndex = 3;
            this.label4.Text = "WebGame Gunny Lộc Phát";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.label3.Location = new System.Drawing.Point(315, 11);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(503, 16);
            this.label3.TabIndex = 2;
            this.label3.Text = "Nạp bằng phương thức : Chuyển khoản hoặc Momo vui lòng liên hệ Fanpage :";
            // 
            // coinNaptheTxt
            // 
            this.coinNaptheTxt.AutoSize = true;
            this.coinNaptheTxt.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.coinNaptheTxt.ForeColor = System.Drawing.Color.Red;
            this.coinNaptheTxt.Location = new System.Drawing.Point(127, 12);
            this.coinNaptheTxt.Name = "coinNaptheTxt";
            this.coinNaptheTxt.Size = new System.Drawing.Size(83, 16);
            this.coinNaptheTxt.TabIndex = 1;
            this.coinNaptheTxt.Text = "10.000 Coin";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(17, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 16);
            this.label2.TabIndex = 0;
            this.label2.Text = "Số dư hiện tại :";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Maroon;
            this.label1.Location = new System.Drawing.Point(344, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(303, 29);
            this.label1.TabIndex = 1;
            this.label1.Text = "NẠP COIN VÀO TÀI KHOẢN";
            // 
            // changemonePage
            // 
            this.changemonePage.BackColor = System.Drawing.Color.White;
            this.changemonePage.Controls.Add(this.panelBodyChangeMoney);
            this.changemonePage.Controls.Add(this.panel1);
            this.changemonePage.Location = new System.Drawing.Point(4, 44);
            this.changemonePage.Name = "changemonePage";
            this.changemonePage.Size = new System.Drawing.Size(992, 577);
            this.changemonePage.TabIndex = 1;
            this.changemonePage.Text = "ĐỔI XU";
            // 
            // panelBodyChangeMoney
            // 
            this.panelBodyChangeMoney.BackColor = System.Drawing.Color.White;
            this.panelBodyChangeMoney.Controls.Add(this.nhaplaiBtnExchange);
            this.panelBodyChangeMoney.Controls.Add(this.doingayBtn);
            this.panelBodyChangeMoney.Controls.Add(this.moneyRewardTxt);
            this.panelBodyChangeMoney.Controls.Add(this.label18);
            this.panelBodyChangeMoney.Controls.Add(this.label17);
            this.panelBodyChangeMoney.Controls.Add(this.coinChangeTxt);
            this.panelBodyChangeMoney.Controls.Add(this.label16);
            this.panelBodyChangeMoney.Controls.Add(this.passtwoChangemoneyTxt);
            this.panelBodyChangeMoney.Controls.Add(this.label15);
            this.panelBodyChangeMoney.Controls.Add(this.label14);
            this.panelBodyChangeMoney.Controls.Add(this.characterRewardmoneyCb);
            this.panelBodyChangeMoney.Controls.Add(this.label13);
            this.panelBodyChangeMoney.Controls.Add(this.serverListCmBox);
            this.panelBodyChangeMoney.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBodyChangeMoney.Location = new System.Drawing.Point(0, 125);
            this.panelBodyChangeMoney.Name = "panelBodyChangeMoney";
            this.panelBodyChangeMoney.Size = new System.Drawing.Size(992, 452);
            this.panelBodyChangeMoney.TabIndex = 2;
            // 
            // nhaplaiBtnExchange
            // 
            this.nhaplaiBtnExchange.BackColor = System.Drawing.Color.DarkOrange;
            this.nhaplaiBtnExchange.Cursor = System.Windows.Forms.Cursors.Hand;
            this.nhaplaiBtnExchange.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.nhaplaiBtnExchange.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nhaplaiBtnExchange.ForeColor = System.Drawing.Color.Black;
            this.nhaplaiBtnExchange.IconChar = FontAwesome.Sharp.IconChar.Retweet;
            this.nhaplaiBtnExchange.IconColor = System.Drawing.Color.Maroon;
            this.nhaplaiBtnExchange.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.nhaplaiBtnExchange.IconSize = 20;
            this.nhaplaiBtnExchange.Location = new System.Drawing.Point(509, 382);
            this.nhaplaiBtnExchange.Name = "nhaplaiBtnExchange";
            this.nhaplaiBtnExchange.Size = new System.Drawing.Size(136, 36);
            this.nhaplaiBtnExchange.TabIndex = 15;
            this.nhaplaiBtnExchange.Text = "    NHẬP LẠI";
            this.nhaplaiBtnExchange.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.nhaplaiBtnExchange.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.nhaplaiBtnExchange.UseVisualStyleBackColor = false;
            this.nhaplaiBtnExchange.Click += new System.EventHandler(this.nhaplaiBtnExchange_Click);
            // 
            // doingayBtn
            // 
            this.doingayBtn.BackColor = System.Drawing.Color.SpringGreen;
            this.doingayBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.doingayBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.doingayBtn.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.doingayBtn.ForeColor = System.Drawing.Color.Maroon;
            this.doingayBtn.IconChar = FontAwesome.Sharp.IconChar.ExchangeAlt;
            this.doingayBtn.IconColor = System.Drawing.Color.Maroon;
            this.doingayBtn.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.doingayBtn.IconSize = 20;
            this.doingayBtn.Location = new System.Drawing.Point(359, 382);
            this.doingayBtn.Name = "doingayBtn";
            this.doingayBtn.Size = new System.Drawing.Size(136, 36);
            this.doingayBtn.TabIndex = 14;
            this.doingayBtn.Text = "    ĐỔI NGAY";
            this.doingayBtn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.doingayBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.doingayBtn.UseVisualStyleBackColor = false;
            this.doingayBtn.Click += new System.EventHandler(this.doingayBtn_Click);
            // 
            // moneyRewardTxt
            // 
            this.moneyRewardTxt.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.moneyRewardTxt.ForeColor = System.Drawing.Color.OrangeRed;
            this.moneyRewardTxt.Location = new System.Drawing.Point(192, 315);
            this.moneyRewardTxt.Name = "moneyRewardTxt";
            this.moneyRewardTxt.ReadOnly = true;
            this.moneyRewardTxt.Size = new System.Drawing.Size(786, 30);
            this.moneyRewardTxt.TabIndex = 13;
            this.moneyRewardTxt.Text = "Xu nhận được";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.ForeColor = System.Drawing.Color.Black;
            this.label18.Location = new System.Drawing.Point(17, 321);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(134, 19);
            this.label18.TabIndex = 12;
            this.label18.Text = "Xu Nhận Được :";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.ForeColor = System.Drawing.Color.Blue;
            this.label17.Location = new System.Drawing.Point(79, 26);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(839, 16);
            this.label17.TabIndex = 11;
            this.label17.Text = "Chú ý : Chuyển đổi thành công , hệ thống sẽ tự chuyển đổi vào Game , xu khuyến mã" +
    "i tại Vòng Quay Măn Mắn được gửi vào thư !";
            // 
            // coinChangeTxt
            // 
            this.coinChangeTxt.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.coinChangeTxt.ForeColor = System.Drawing.Color.DimGray;
            this.coinChangeTxt.Location = new System.Drawing.Point(192, 257);
            this.coinChangeTxt.Name = "coinChangeTxt";
            this.coinChangeTxt.Size = new System.Drawing.Size(786, 30);
            this.coinChangeTxt.TabIndex = 10;
            this.coinChangeTxt.Text = "Nhập số lượng Coin muốn đổi sang xu";
            this.coinChangeTxt.TextChanged += new System.EventHandler(this.coinChangeTxt_TextChanged);
            this.coinChangeTxt.Enter += new System.EventHandler(this.coinChangeTxt_Enter);
            this.coinChangeTxt.Leave += new System.EventHandler(this.coinChangeTxt_Leave);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.ForeColor = System.Drawing.Color.Black;
            this.label16.Location = new System.Drawing.Point(17, 263);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(162, 19);
            this.label16.TabIndex = 9;
            this.label16.Text = "Số Coin Muốn Đổi :";
            // 
            // passtwoChangemoneyTxt
            // 
            this.passtwoChangemoneyTxt.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.passtwoChangemoneyTxt.ForeColor = System.Drawing.Color.DimGray;
            this.passtwoChangemoneyTxt.Location = new System.Drawing.Point(192, 196);
            this.passtwoChangemoneyTxt.Name = "passtwoChangemoneyTxt";
            this.passtwoChangemoneyTxt.Size = new System.Drawing.Size(786, 30);
            this.passtwoChangemoneyTxt.TabIndex = 8;
            this.passtwoChangemoneyTxt.Text = "Bỏ trống nếu không có";
            this.passtwoChangemoneyTxt.Enter += new System.EventHandler(this.passtwoChangemoneyTxt_Enter);
            this.passtwoChangemoneyTxt.Leave += new System.EventHandler(this.passtwoChangemoneyTxt_Leave);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.ForeColor = System.Drawing.Color.Black;
            this.label15.Location = new System.Drawing.Point(17, 202);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(148, 19);
            this.label15.TabIndex = 7;
            this.label15.Text = "Mật Khẩu Cấp 2 :";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ForeColor = System.Drawing.Color.Black;
            this.label14.Location = new System.Drawing.Point(18, 142);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(168, 19);
            this.label14.TabIndex = 6;
            this.label14.Text = "Nhân Vật Nhận Xu :";
            // 
            // characterRewardmoneyCb
            // 
            this.characterRewardmoneyCb.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.characterRewardmoneyCb.FormattingEnabled = true;
            this.characterRewardmoneyCb.ItemHeight = 23;
            this.characterRewardmoneyCb.Location = new System.Drawing.Point(192, 136);
            this.characterRewardmoneyCb.Name = "characterRewardmoneyCb";
            this.characterRewardmoneyCb.Size = new System.Drawing.Size(786, 29);
            this.characterRewardmoneyCb.TabIndex = 5;
            this.characterRewardmoneyCb.UseSelectable = true;
            this.characterRewardmoneyCb.SelectedIndexChanged += new System.EventHandler(this.characterRewardmoneyCb_SelectedIndexChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.Color.Black;
            this.label13.Location = new System.Drawing.Point(18, 82);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(135, 19);
            this.label13.TabIndex = 4;
            this.label13.Text = "Chọn Máy Chủ :";
            // 
            // serverListCmBox
            // 
            this.serverListCmBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.serverListCmBox.FormattingEnabled = true;
            this.serverListCmBox.ItemHeight = 23;
            this.serverListCmBox.Location = new System.Drawing.Point(192, 72);
            this.serverListCmBox.Name = "serverListCmBox";
            this.serverListCmBox.Size = new System.Drawing.Size(786, 29);
            this.serverListCmBox.TabIndex = 0;
            this.serverListCmBox.UseSelectable = true;
            this.serverListCmBox.SelectedIndexChanged += new System.EventHandler(this.serverListCmBox_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.label12);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(992, 125);
            this.panel1.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.label9);
            this.panel2.Controls.Add(this.label10);
            this.panel2.Controls.Add(this.label11);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 89);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(992, 36);
            this.panel2.TabIndex = 2;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.Orange;
            this.label8.Location = new System.Drawing.Point(818, 11);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(160, 16);
            this.label8.TabIndex = 3;
            this.label8.Text = "WebGame Gunny Hồi Ức";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.label9.Location = new System.Drawing.Point(315, 11);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(503, 16);
            this.label9.TabIndex = 2;
            this.label9.Text = "Nạp bằng phương thức : Chuyển khoản hoặc Momo vui lòng liên hệ Fanpage :";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.Red;
            this.label10.Location = new System.Drawing.Point(127, 12);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(83, 16);
            this.label10.TabIndex = 1;
            this.label10.Text = "10.000 Coin";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(17, 12);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(106, 16);
            this.label11.TabIndex = 0;
            this.label11.Text = "Số dư hiện tại :";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.Maroon;
            this.label12.Location = new System.Drawing.Point(325, 28);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(322, 29);
            this.label12.TabIndex = 1;
            this.label12.Text = "CHUYỂN ĐỔI COIN SANG XU";
            // 
            // logPage
            // 
            this.logPage.Controls.Add(this.datalogGrid);
            this.logPage.Location = new System.Drawing.Point(4, 44);
            this.logPage.Name = "logPage";
            this.logPage.Size = new System.Drawing.Size(992, 577);
            this.logPage.TabIndex = 2;
            this.logPage.Text = "LỊCH SỬ GIAO DỊCH            ";
            // 
            // datalogGrid
            // 
            this.datalogGrid.AllowDrop = true;
            this.datalogGrid.AllowUserToAddRows = false;
            this.datalogGrid.AllowUserToDeleteRows = false;
            this.datalogGrid.AllowUserToOrderColumns = true;
            this.datalogGrid.AllowUserToResizeColumns = false;
            this.datalogGrid.AllowUserToResizeRows = false;
            this.datalogGrid.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.datalogGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.datalogGrid.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.datalogGrid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.datalogGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.datalogGrid.ColumnHeadersHeight = 33;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.datalogGrid.DefaultCellStyle = dataGridViewCellStyle2;
            this.datalogGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.datalogGrid.EnableHeadersVisualStyles = false;
            this.datalogGrid.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.datalogGrid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.datalogGrid.Location = new System.Drawing.Point(0, 0);
            this.datalogGrid.Name = "datalogGrid";
            this.datalogGrid.ReadOnly = true;
            this.datalogGrid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.datalogGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.datalogGrid.RowHeadersVisible = false;
            this.datalogGrid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.datalogGrid.RowTemplate.Height = 30;
            this.datalogGrid.RowTemplate.ReadOnly = true;
            this.datalogGrid.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.datalogGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.datalogGrid.ShowCellErrors = false;
            this.datalogGrid.ShowCellToolTips = false;
            this.datalogGrid.ShowEditingIcon = false;
            this.datalogGrid.ShowRowErrors = false;
            this.datalogGrid.Size = new System.Drawing.Size(992, 577);
            this.datalogGrid.TabIndex = 0;
            // 
            // Recharge
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl);
            this.Name = "Recharge";
            this.Size = new System.Drawing.Size(1000, 625);
            this.tabControl.ResumeLayout(false);
            this.rechargePage.ResumeLayout(false);
            this.panelBody.ResumeLayout(false);
            this.panelBody.PerformLayout();
            this.moneycardGroup.ResumeLayout(false);
            this.moneycardGroup.PerformLayout();
            this.seriandpassGroup.ResumeLayout(false);
            this.seriandpassGroup.PerformLayout();
            this.typeCardGroup.ResumeLayout(false);
            this.typeCardGroup.PerformLayout();
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelBotofTop.ResumeLayout(false);
            this.panelBotofTop.PerformLayout();
            this.changemonePage.ResumeLayout(false);
            this.panelBodyChangeMoney.ResumeLayout(false);
            this.panelBodyChangeMoney.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.logPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.datalogGrid)).EndInit();
            this.ResumeLayout(false);

		}

		static Recharge()
		{
			CharacterList = new Dictionary<string, CharacterInfo>();
		}
    }
}
