namespace Game.Service.actions
{
	// Token: 0x02000008 RID: 8
	public partial class ServerManagementForm : global::System.Windows.Forms.Form
	{
		// Token: 0x0600008C RID: 140 RVA: 0x00009C3C File Offset: 0x00007E3C
		protected override void Dispose(bool disposing)
		{
			bool flag = disposing && this.components != null;
			bool flag2 = flag;
			if (flag2)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00009C78 File Offset: 0x00007E78
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			this.onlineTxt = new global::System.Windows.Forms.Label();
			this.UpdateUI = new global::System.Windows.Forms.Timer(this.components);
			this.checkBox3 = new global::System.Windows.Forms.CheckBox();
			this.groupBox3 = new global::System.Windows.Forms.GroupBox();
			this.textBox4 = new global::System.Windows.Forms.TextBox();
			this.textBox2 = new global::System.Windows.Forms.TextBox();
			this.textBox3 = new global::System.Windows.Forms.TextBox();
			this.textBox5 = new global::System.Windows.Forms.TextBox();
			this.button8 = new global::System.Windows.Forms.Button();
			this.label2 = new global::System.Windows.Forms.Label();
			this.label9 = new global::System.Windows.Forms.Label();
			this.textBox6 = new global::System.Windows.Forms.TextBox();
			this.label6 = new global::System.Windows.Forms.Label();
			this.label5 = new global::System.Windows.Forms.Label();
			this.label3 = new global::System.Windows.Forms.Label();
			this.lixtBox1 = new global::System.Windows.Forms.ListBox();
			this.button9 = new global::System.Windows.Forms.Button();
			this.button10 = new global::System.Windows.Forms.Button();
			this.button15 = new global::System.Windows.Forms.Button();
			this.textBox1 = new global::System.Windows.Forms.TextBox();
			this.button16 = new global::System.Windows.Forms.Button();
			this.groupBox6 = new global::System.Windows.Forms.GroupBox();
			this.label13 = new global::System.Windows.Forms.Label();
			this.label27 = new global::System.Windows.Forms.Label();
			this.label24 = new global::System.Windows.Forms.Label();
			this.nickName = new global::System.Windows.Forms.Label();
			this.label12 = new global::System.Windows.Forms.Label();
			this.label31 = new global::System.Windows.Forms.Label();
			this.label4 = new global::System.Windows.Forms.Label();
			this.label23 = new global::System.Windows.Forms.Label();
			this.label11 = new global::System.Windows.Forms.Label();
			this.label35 = new global::System.Windows.Forms.Label();
			this.label22 = new global::System.Windows.Forms.Label();
			this.label7 = new global::System.Windows.Forms.Label();
			this.label21 = new global::System.Windows.Forms.Label();
			this.label39 = new global::System.Windows.Forms.Label();
			this.label8 = new global::System.Windows.Forms.Label();
			this.label20 = new global::System.Windows.Forms.Label();
			this.label42 = new global::System.Windows.Forms.Label();
			this.label43 = new global::System.Windows.Forms.Label();
			this.label19 = new global::System.Windows.Forms.Label();
			this.label45 = new global::System.Windows.Forms.Label();
			this.button17 = new global::System.Windows.Forms.Button();
			this.textBox7 = new global::System.Windows.Forms.TextBox();
			this.textBox8 = new global::System.Windows.Forms.TextBox();
			this.button19 = new global::System.Windows.Forms.Button();
			this.groupBox7 = new global::System.Windows.Forms.GroupBox();
			this.label15 = new global::System.Windows.Forms.Label();
			this.label14 = new global::System.Windows.Forms.Label();
			this.label17 = new global::System.Windows.Forms.Label();
			this.label18 = new global::System.Windows.Forms.Label();
			this.label25 = new global::System.Windows.Forms.Label();
			this.label10 = new global::System.Windows.Forms.Label();
			this.groupBox5 = new global::System.Windows.Forms.GroupBox();
			this.groupBox4 = new global::System.Windows.Forms.GroupBox();
			this.textBox11 = new global::System.Windows.Forms.TextBox();
			this.button4 = new global::System.Windows.Forms.Button();
			this.comboBox1 = new global::System.Windows.Forms.ComboBox();
			this.groupBox8 = new global::System.Windows.Forms.GroupBox();
			this.label16 = new global::System.Windows.Forms.Label();
			this.textBox9 = new global::System.Windows.Forms.TextBox();
			this.label26 = new global::System.Windows.Forms.Label();
			this.button21 = new global::System.Windows.Forms.Button();
			this.textBox10 = new global::System.Windows.Forms.TextBox();
			this.groupBox9 = new global::System.Windows.Forms.GroupBox();
			this.textBox12 = new global::System.Windows.Forms.TextBox();
			this.button5 = new global::System.Windows.Forms.Button();
			this.comboBox2 = new global::System.Windows.Forms.ComboBox();
			this.groupBox1 = new global::System.Windows.Forms.GroupBox();
			this.button1 = new global::System.Windows.Forms.Button();
			this.comboBox3 = new global::System.Windows.Forms.ComboBox();
			this.dataGridView1 = new global::System.Windows.Forms.DataGridView();
			this.comboBox4 = new global::System.Windows.Forms.ComboBox();
			this.button2 = new global::System.Windows.Forms.Button();
			this.groupBox2 = new global::System.Windows.Forms.GroupBox();
			this.textBox13 = new global::System.Windows.Forms.TextBox();
			this.button3 = new global::System.Windows.Forms.Button();
			this.label1 = new global::System.Windows.Forms.Label();
			this.label28 = new global::System.Windows.Forms.Label();
			this.label29 = new global::System.Windows.Forms.Label();
			this.label30 = new global::System.Windows.Forms.Label();
			this.label32 = new global::System.Windows.Forms.Label();
			this.label33 = new global::System.Windows.Forms.Label();
			this.groupBox3.SuspendLayout();
			this.groupBox6.SuspendLayout();
			this.groupBox7.SuspendLayout();
			this.groupBox5.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.groupBox8.SuspendLayout();
			this.groupBox9.SuspendLayout();
			this.groupBox1.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.dataGridView1).BeginInit();
			this.groupBox2.SuspendLayout();
			base.SuspendLayout();
			this.onlineTxt.AutoSize = true;
			this.onlineTxt.BackColor = global::System.Drawing.Color.Black;
			this.onlineTxt.Font = new global::System.Drawing.Font("Consolas", 14f, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point, 0);
			this.onlineTxt.ForeColor = global::System.Drawing.Color.Lime;
			this.onlineTxt.Location = new global::System.Drawing.Point(186, 18);
			this.onlineTxt.Margin = new global::System.Windows.Forms.Padding(4, 0, 4, 0);
			this.onlineTxt.Name = "onlineTxt";
			this.onlineTxt.Size = new global::System.Drawing.Size(34, 25);
			this.onlineTxt.TabIndex = 0;
			this.onlineTxt.Text = "00";
			this.UpdateUI.Enabled = true;
			this.UpdateUI.Interval = 10000;
			this.UpdateUI.Tick += new global::System.EventHandler(this.UpdateUI_Tick);
			this.checkBox3.BackColor = global::System.Drawing.Color.FromArgb(20, 20, 20);
			this.checkBox3.ForeColor = global::System.Drawing.Color.Cyan;
			this.checkBox3.Location = new global::System.Drawing.Point(139, 247);
			this.checkBox3.Margin = new global::System.Windows.Forms.Padding(4, 4, 4, 4);
			this.checkBox3.Name = "checkBox3";
			this.checkBox3.Size = new global::System.Drawing.Size(139, 30);
			this.checkBox3.TabIndex = 0;
			this.groupBox3.Controls.Add(this.checkBox3);
			this.groupBox3.Font = new global::System.Drawing.Font("Consolas", 12f, global::System.Drawing.FontStyle.Bold);
			this.groupBox3.ForeColor = global::System.Drawing.Color.Cyan;
			this.groupBox3.Location = new global::System.Drawing.Point(1989, 20);
			this.groupBox3.Margin = new global::System.Windows.Forms.Padding(4, 4, 4, 4);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Padding = new global::System.Windows.Forms.Padding(4, 4, 4, 4);
			this.groupBox3.Size = new global::System.Drawing.Size(13, 12);
			this.groupBox3.TabIndex = 17;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Акции и дебаг";
			this.textBox4.BackColor = global::System.Drawing.Color.FromArgb(30, 30, 30);
			this.textBox4.ForeColor = global::System.Drawing.Color.Lime;
			this.textBox4.Location = new global::System.Drawing.Point(148, 113);
			this.textBox4.Margin = new global::System.Windows.Forms.Padding(4, 4, 4, 4);
			this.textBox4.Name = "textBox4";
			this.textBox4.Size = new global::System.Drawing.Size(142, 30);
			this.textBox4.TabIndex = 22;
			this.textBox2.BackColor = global::System.Drawing.Color.FromArgb(30, 30, 30);
			this.textBox2.ForeColor = global::System.Drawing.Color.Lime;
			this.textBox2.Location = new global::System.Drawing.Point(148, 74);
			this.textBox2.Margin = new global::System.Windows.Forms.Padding(4, 4, 4, 4);
			this.textBox2.Name = "textBox2";
			this.textBox2.Size = new global::System.Drawing.Size(142, 30);
			this.textBox2.TabIndex = 23;
			this.textBox3.BackColor = global::System.Drawing.Color.FromArgb(30, 30, 30);
			this.textBox3.ForeColor = global::System.Drawing.Color.Lime;
			this.textBox3.Location = new global::System.Drawing.Point(148, 34);
			this.textBox3.Margin = new global::System.Windows.Forms.Padding(4, 4, 4, 4);
			this.textBox3.Name = "textBox3";
			this.textBox3.Size = new global::System.Drawing.Size(142, 30);
			this.textBox3.TabIndex = 24;
			this.textBox5.BackColor = global::System.Drawing.Color.FromArgb(30, 30, 30);
			this.textBox5.ForeColor = global::System.Drawing.Color.Lime;
			this.textBox5.Location = new global::System.Drawing.Point(148, 151);
			this.textBox5.Margin = new global::System.Windows.Forms.Padding(4, 4, 4, 4);
			this.textBox5.Name = "textBox5";
			this.textBox5.Size = new global::System.Drawing.Size(142, 30);
			this.textBox5.TabIndex = 25;
			this.button8.BackColor = global::System.Drawing.Color.FromArgb(40, 40, 40);
			this.button8.FlatAppearance.BorderColor = global::System.Drawing.Color.Lime;
			this.button8.FlatStyle = global::System.Windows.Forms.FlatStyle.Flat;
			this.button8.Font = new global::System.Drawing.Font("Consolas", 12f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			this.button8.ForeColor = global::System.Drawing.Color.Cyan;
			this.button8.Location = new global::System.Drawing.Point(57, 230);
			this.button8.Margin = new global::System.Windows.Forms.Padding(4, 4, 4, 4);
			this.button8.Name = "button8";
			this.button8.Size = new global::System.Drawing.Size(191, 39);
			this.button8.TabIndex = 26;
			this.button8.Text = "Gönder";
			this.button8.TextImageRelation = global::System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.button8.UseVisualStyleBackColor = false;
			this.button8.Click += new global::System.EventHandler(this.button8_Click);
			this.label2.AutoSize = true;
			this.label2.ForeColor = global::System.Drawing.Color.Cyan;
			this.label2.Location = new global::System.Drawing.Point(32, 34);
			this.label2.Margin = new global::System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label2.Name = "label2";
			this.label2.Size = new global::System.Drawing.Size(93, 25);
			this.label2.TabIndex = 28;
			this.label2.Text = "İtem ID :";
			this.label9.AutoSize = true;
			this.label9.ForeColor = global::System.Drawing.Color.Cyan;
			this.label9.Location = new global::System.Drawing.Point(32, 192);
			this.label9.Margin = new global::System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label9.Name = "label9";
			this.label9.Size = new global::System.Drawing.Size(95, 25);
			this.label9.TabIndex = 35;
			this.label9.Text = "Bağlımı :";
			this.textBox6.BackColor = global::System.Drawing.Color.FromArgb(30, 30, 30);
			this.textBox6.ForeColor = global::System.Drawing.Color.Lime;
			this.textBox6.Location = new global::System.Drawing.Point(148, 192);
			this.textBox6.Margin = new global::System.Windows.Forms.Padding(4, 4, 4, 4);
			this.textBox6.Name = "textBox6";
			this.textBox6.Size = new global::System.Drawing.Size(142, 30);
			this.textBox6.TabIndex = 34;
			this.textBox6.Text = "True";
			this.label6.AutoSize = true;
			this.label6.ForeColor = global::System.Drawing.Color.Cyan;
			this.label6.Location = new global::System.Drawing.Point(32, 113);
			this.label6.Margin = new global::System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label6.Name = "label6";
			this.label6.Size = new global::System.Drawing.Size(77, 25);
			this.label6.TabIndex = 33;
			this.label6.Text = "İçerik :";
			this.label5.AutoSize = true;
			this.label5.ForeColor = global::System.Drawing.Color.Cyan;
			this.label5.Location = new global::System.Drawing.Point(32, 151);
			this.label5.Margin = new global::System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label5.Name = "label5";
			this.label5.Size = new global::System.Drawing.Size(70, 25);
			this.label5.TabIndex = 31;
			this.label5.Text = "Adet :";
			this.label3.AutoSize = true;
			this.label3.ForeColor = global::System.Drawing.Color.Cyan;
			this.label3.Location = new global::System.Drawing.Point(32, 75);
			this.label3.Margin = new global::System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label3.Name = "label3";
			this.label3.Size = new global::System.Drawing.Size(83, 25);
			this.label3.TabIndex = 29;
			this.label3.Text = "Başlık :";
			this.lixtBox1.BackColor = global::System.Drawing.Color.Black;
			this.lixtBox1.CausesValidation = false;
			this.lixtBox1.Font = new global::System.Drawing.Font("Consolas", 12f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			this.lixtBox1.ForeColor = global::System.Drawing.Color.Lime;
			this.lixtBox1.FormattingEnabled = true;
			this.lixtBox1.ItemHeight = 25;
			this.lixtBox1.Location = new global::System.Drawing.Point(16, 87);
			this.lixtBox1.Margin = new global::System.Windows.Forms.Padding(4, 4, 4, 4);
			this.lixtBox1.Name = "lixtBox1";
			this.lixtBox1.Size = new global::System.Drawing.Size(204, 129);
			this.lixtBox1.TabIndex = 29;
			this.lixtBox1.SelectedIndexChanged += new global::System.EventHandler(this.listBox1_SelectedIndexChanged);
			this.button9.BackColor = global::System.Drawing.Color.FromArgb(40, 40, 40);
			this.button9.FlatAppearance.BorderColor = global::System.Drawing.Color.Lime;
			this.button9.FlatStyle = global::System.Windows.Forms.FlatStyle.Flat;
			this.button9.Font = new global::System.Drawing.Font("Consolas", 12f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			this.button9.ForeColor = global::System.Drawing.Color.Cyan;
			this.button9.Location = new global::System.Drawing.Point(16, 224);
			this.button9.Margin = new global::System.Windows.Forms.Padding(4, 4, 4, 4);
			this.button9.Name = "button9";
			this.button9.Size = new global::System.Drawing.Size(204, 50);
			this.button9.TabIndex = 31;
			this.button9.Text = "Güncelle !";
			this.button9.UseVisualStyleBackColor = false;
			this.button9.Click += new global::System.EventHandler(this.button9_Click_1);
			this.button10.BackColor = global::System.Drawing.Color.FromArgb(50, 0, 0);
			this.button10.FlatAppearance.BorderColor = global::System.Drawing.Color.Red;
			this.button10.FlatStyle = global::System.Windows.Forms.FlatStyle.Flat;
			this.button10.Font = new global::System.Drawing.Font("Consolas", 12f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			this.button10.ForeColor = global::System.Drawing.Color.Red;
			this.button10.Location = new global::System.Drawing.Point(228, 342);
			this.button10.Margin = new global::System.Windows.Forms.Padding(4, 4, 4, 4);
			this.button10.Name = "button10";
			this.button10.Size = new global::System.Drawing.Size(219, 47);
			this.button10.TabIndex = 31;
			this.button10.Text = "Banla !";
			this.button10.UseVisualStyleBackColor = false;
			this.button10.Click += new global::System.EventHandler(this.button10_Click_1);
			this.button15.BackColor = global::System.Drawing.Color.FromArgb(0, 50, 0);
			this.button15.FlatAppearance.BorderColor = global::System.Drawing.Color.Lime;
			this.button15.FlatStyle = global::System.Windows.Forms.FlatStyle.Flat;
			this.button15.Font = new global::System.Drawing.Font("Consolas", 12f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			this.button15.ForeColor = global::System.Drawing.Color.Lime;
			this.button15.Location = new global::System.Drawing.Point(228, 395);
			this.button15.Margin = new global::System.Windows.Forms.Padding(4, 4, 4, 4);
			this.button15.Name = "button15";
			this.button15.Size = new global::System.Drawing.Size(219, 47);
			this.button15.TabIndex = 38;
			this.button15.Text = "Kickle !";
			this.button15.UseVisualStyleBackColor = false;
			this.button15.Click += new global::System.EventHandler(this.button15_Click);
			this.textBox1.AccessibleName = "";
			this.textBox1.BackColor = global::System.Drawing.Color.FromArgb(30, 30, 30);
			this.textBox1.ForeColor = global::System.Drawing.Color.Lime;
			this.textBox1.Location = new global::System.Drawing.Point(229, 522);
			this.textBox1.Margin = new global::System.Windows.Forms.Padding(4, 4, 4, 4);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new global::System.Drawing.Size(218, 22);
			this.textBox1.TabIndex = 33;
			this.button16.BackColor = global::System.Drawing.Color.FromArgb(0, 50, 0);
			this.button16.FlatAppearance.BorderColor = global::System.Drawing.Color.Lime;
			this.button16.FlatStyle = global::System.Windows.Forms.FlatStyle.Flat;
			this.button16.Font = new global::System.Drawing.Font("Consolas", 12f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			this.button16.ForeColor = global::System.Drawing.Color.Lime;
			this.button16.Location = new global::System.Drawing.Point(228, 454);
			this.button16.Margin = new global::System.Windows.Forms.Padding(4, 4, 4, 4);
			this.button16.Name = "button16";
			this.button16.Size = new global::System.Drawing.Size(221, 37);
			this.button16.TabIndex = 39;
			this.button16.Text = "Banı Aç !";
			this.button16.UseVisualStyleBackColor = false;
			this.button16.Click += new global::System.EventHandler(this.button16_Click);
			this.groupBox6.BackColor = global::System.Drawing.Color.FromArgb(20, 20, 20);
			this.groupBox6.Controls.Add(this.label13);
			this.groupBox6.Controls.Add(this.label27);
			this.groupBox6.Controls.Add(this.label24);
			this.groupBox6.Controls.Add(this.nickName);
			this.groupBox6.Controls.Add(this.label12);
			this.groupBox6.Controls.Add(this.label31);
			this.groupBox6.Controls.Add(this.label4);
			this.groupBox6.Controls.Add(this.label23);
			this.groupBox6.Controls.Add(this.label11);
			this.groupBox6.Controls.Add(this.label35);
			this.groupBox6.Controls.Add(this.label22);
			this.groupBox6.Controls.Add(this.label7);
			this.groupBox6.Controls.Add(this.label21);
			this.groupBox6.Controls.Add(this.label39);
			this.groupBox6.Controls.Add(this.label8);
			this.groupBox6.Controls.Add(this.label20);
			this.groupBox6.Controls.Add(this.label42);
			this.groupBox6.Controls.Add(this.label43);
			this.groupBox6.Controls.Add(this.label19);
			this.groupBox6.Controls.Add(this.label45);
			this.groupBox6.Font = new global::System.Drawing.Font("Consolas", 12f, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point, 0);
			this.groupBox6.ForeColor = global::System.Drawing.Color.Cyan;
			this.groupBox6.Location = new global::System.Drawing.Point(17, 340);
			this.groupBox6.Margin = new global::System.Windows.Forms.Padding(4, 4, 4, 4);
			this.groupBox6.Name = "groupBox6";
			this.groupBox6.Padding = new global::System.Windows.Forms.Padding(4, 4, 4, 4);
			this.groupBox6.Size = new global::System.Drawing.Size(203, 343);
			this.groupBox6.TabIndex = 61;
			this.groupBox6.TabStop = false;
			this.groupBox6.Text = "Oyuncu Bilgileri";
			this.label13.AutoSize = true;
			this.label13.Font = new global::System.Drawing.Font("Consolas", 9.75f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			this.label13.ForeColor = global::System.Drawing.Color.Cyan;
			this.label13.Location = new global::System.Drawing.Point(17, 41);
			this.label13.Margin = new global::System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label13.Name = "label13";
			this.label13.Size = new global::System.Drawing.Size(42, 20);
			this.label13.TabIndex = 44;
			this.label13.Text = "Nick";
			this.label27.AutoSize = true;
			this.label27.Font = new global::System.Drawing.Font("Consolas", 9.75f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			this.label27.ForeColor = global::System.Drawing.Color.Cyan;
			this.label27.Location = new global::System.Drawing.Point(19, 311);
			this.label27.Margin = new global::System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label27.Name = "label27";
			this.label27.Size = new global::System.Drawing.Size(100, 20);
			this.label27.TabIndex = 58;
			this.label27.Text = "Savaşma G.";
			this.label24.AutoSize = true;
			this.label24.Font = new global::System.Drawing.Font("Consolas", 9.75f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			this.label24.ForeColor = global::System.Drawing.Color.Lime;
			this.label24.Location = new global::System.Drawing.Point(111, 311);
			this.label24.Margin = new global::System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label24.Name = "label24";
			this.label24.Size = new global::System.Drawing.Size(100, 20);
			this.label24.TabIndex = 59;
			this.label24.Text = "Savaşma G.";
			this.nickName.AutoSize = true;
			this.nickName.Font = new global::System.Drawing.Font("Consolas", 9.75f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			this.nickName.ForeColor = global::System.Drawing.Color.Lime;
			this.nickName.Location = new global::System.Drawing.Point(111, 41);
			this.nickName.Margin = new global::System.Windows.Forms.Padding(4, 0, 4, 0);
			this.nickName.Name = "nickName";
			this.nickName.Size = new global::System.Drawing.Size(42, 20);
			this.nickName.TabIndex = 40;
			this.nickName.Text = "Nick";
			this.label12.AutoSize = true;
			this.label12.Font = new global::System.Drawing.Font("Consolas", 9.75f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			this.label12.ForeColor = global::System.Drawing.Color.Cyan;
			this.label12.Location = new global::System.Drawing.Point(19, 70);
			this.label12.Margin = new global::System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label12.Name = "label12";
			this.label12.Size = new global::System.Drawing.Size(48, 20);
			this.label12.TabIndex = 45;
			this.label12.Text = "K.Adı";
			this.label31.AutoSize = true;
			this.label31.Font = new global::System.Drawing.Font("Consolas", 9.75f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			this.label31.ForeColor = global::System.Drawing.Color.Cyan;
			this.label31.Location = new global::System.Drawing.Point(19, 279);
			this.label31.Margin = new global::System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label31.Name = "label31";
			this.label31.Size = new global::System.Drawing.Size(39, 20);
			this.label31.TabIndex = 57;
			this.label31.Text = "Can";
			this.label4.AutoSize = true;
			this.label4.Font = new global::System.Drawing.Font("Consolas", 9.75f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			this.label4.ForeColor = global::System.Drawing.Color.Lime;
			this.label4.Location = new global::System.Drawing.Point(111, 70);
			this.label4.Margin = new global::System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label4.Name = "label4";
			this.label4.Size = new global::System.Drawing.Size(48, 20);
			this.label4.TabIndex = 41;
			this.label4.Text = "K.Adı";
			this.label23.AutoSize = true;
			this.label23.Font = new global::System.Drawing.Font("Consolas", 9.75f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			this.label23.ForeColor = global::System.Drawing.Color.Lime;
			this.label23.Location = new global::System.Drawing.Point(111, 279);
			this.label23.Margin = new global::System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label23.Name = "label23";
			this.label23.Size = new global::System.Drawing.Size(39, 20);
			this.label23.TabIndex = 56;
			this.label23.Text = "Can";
			this.label11.AutoSize = true;
			this.label11.Font = new global::System.Drawing.Font("Consolas", 9.75f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			this.label11.ForeColor = global::System.Drawing.Color.Cyan;
			this.label11.Location = new global::System.Drawing.Point(19, 97);
			this.label11.Margin = new global::System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label11.Name = "label11";
			this.label11.Size = new global::System.Drawing.Size(49, 20);
			this.label11.TabIndex = 46;
			this.label11.Text = "Level";
			this.label35.AutoSize = true;
			this.label35.Font = new global::System.Drawing.Font("Consolas", 9.75f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			this.label35.ForeColor = global::System.Drawing.Color.Cyan;
			this.label35.Location = new global::System.Drawing.Point(19, 246);
			this.label35.Margin = new global::System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label35.Name = "label35";
			this.label35.Size = new global::System.Drawing.Size(47, 20);
			this.label35.TabIndex = 51;
			this.label35.Text = "Şans";
			this.label22.AutoSize = true;
			this.label22.Font = new global::System.Drawing.Font("Consolas", 9.75f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			this.label22.ForeColor = global::System.Drawing.Color.Lime;
			this.label22.Location = new global::System.Drawing.Point(111, 246);
			this.label22.Margin = new global::System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label22.Name = "label22";
			this.label22.Size = new global::System.Drawing.Size(47, 20);
			this.label22.TabIndex = 55;
			this.label22.Text = "Şans";
			this.label7.AutoSize = true;
			this.label7.Font = new global::System.Drawing.Font("Consolas", 9.75f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			this.label7.ForeColor = global::System.Drawing.Color.Lime;
			this.label7.Location = new global::System.Drawing.Point(110, 97);
			this.label7.Margin = new global::System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label7.Name = "label7";
			this.label7.Size = new global::System.Drawing.Size(49, 20);
			this.label7.TabIndex = 42;
			this.label7.Text = "Level";
			this.label21.AutoSize = true;
			this.label21.Font = new global::System.Drawing.Font("Consolas", 9.75f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			this.label21.ForeColor = global::System.Drawing.Color.Lime;
			this.label21.Location = new global::System.Drawing.Point(111, 213);
			this.label21.Margin = new global::System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label21.Name = "label21";
			this.label21.Size = new global::System.Drawing.Size(71, 20);
			this.label21.TabIndex = 54;
			this.label21.Text = "Çeviklik ";
			this.label39.AutoSize = true;
			this.label39.Font = new global::System.Drawing.Font("Consolas", 9.75f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			this.label39.ForeColor = global::System.Drawing.Color.Cyan;
			this.label39.Location = new global::System.Drawing.Point(19, 213);
			this.label39.Margin = new global::System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label39.Name = "label39";
			this.label39.Size = new global::System.Drawing.Size(71, 20);
			this.label39.TabIndex = 50;
			this.label39.Text = "Çeviklik ";
			this.label8.AutoSize = true;
			this.label8.Font = new global::System.Drawing.Font("Consolas", 9.75f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			this.label8.ForeColor = global::System.Drawing.Color.Lime;
			this.label8.Location = new global::System.Drawing.Point(111, 124);
			this.label8.Margin = new global::System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label8.Name = "label8";
			this.label8.Size = new global::System.Drawing.Size(56, 20);
			this.label8.TabIndex = 47;
			this.label8.Text = "Kupon";
			this.label20.AutoSize = true;
			this.label20.Font = new global::System.Drawing.Font("Consolas", 9.75f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			this.label20.ForeColor = global::System.Drawing.Color.Lime;
			this.label20.Location = new global::System.Drawing.Point(111, 182);
			this.label20.Margin = new global::System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label20.Name = "label20";
			this.label20.Size = new global::System.Drawing.Size(78, 20);
			this.label20.TabIndex = 53;
			this.label20.Text = "Savunma";
			this.label42.AutoSize = true;
			this.label42.Font = new global::System.Drawing.Font("Consolas", 9.75f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			this.label42.ForeColor = global::System.Drawing.Color.Cyan;
			this.label42.Location = new global::System.Drawing.Point(19, 124);
			this.label42.Margin = new global::System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label42.Name = "label42";
			this.label42.Size = new global::System.Drawing.Size(56, 20);
			this.label42.TabIndex = 43;
			this.label42.Text = "Kupon";
			this.label43.AutoSize = true;
			this.label43.Font = new global::System.Drawing.Font("Consolas", 9.75f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			this.label43.ForeColor = global::System.Drawing.Color.Cyan;
			this.label43.Location = new global::System.Drawing.Point(19, 182);
			this.label43.Margin = new global::System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label43.Name = "label43";
			this.label43.Size = new global::System.Drawing.Size(78, 20);
			this.label43.TabIndex = 49;
			this.label43.Text = "Savunma";
			this.label19.AutoSize = true;
			this.label19.Font = new global::System.Drawing.Font("Consolas", 9.75f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			this.label19.ForeColor = global::System.Drawing.Color.Lime;
			this.label19.Location = new global::System.Drawing.Point(111, 151);
			this.label19.Margin = new global::System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label19.Name = "label19";
			this.label19.Size = new global::System.Drawing.Size(56, 20);
			this.label19.TabIndex = 52;
			this.label19.Text = "Saldırı";
			this.label45.AutoSize = true;
			this.label45.Font = new global::System.Drawing.Font("Consolas", 9.75f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			this.label45.ForeColor = global::System.Drawing.Color.Cyan;
			this.label45.Location = new global::System.Drawing.Point(19, 151);
			this.label45.Margin = new global::System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label45.Name = "label45";
			this.label45.Size = new global::System.Drawing.Size(56, 20);
			this.label45.TabIndex = 48;
			this.label45.Text = "Saldırı";
			this.button17.BackColor = global::System.Drawing.Color.FromArgb(40, 40, 40);
			this.button17.FlatAppearance.BorderColor = global::System.Drawing.Color.Cyan;
			this.button17.FlatStyle = global::System.Windows.Forms.FlatStyle.Flat;
			this.button17.Font = new global::System.Drawing.Font("Consolas", 12f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			this.button17.ForeColor = global::System.Drawing.Color.Cyan;
			this.button17.Location = new global::System.Drawing.Point(17, 282);
			this.button17.Margin = new global::System.Windows.Forms.Padding(4, 4, 4, 4);
			this.button17.Name = "button17";
			this.button17.Size = new global::System.Drawing.Size(203, 50);
			this.button17.TabIndex = 62;
			this.button17.Text = "Ram'ı Temizle !";
			this.button17.UseVisualStyleBackColor = false;
			this.button17.Click += new global::System.EventHandler(this.button17_Click);
			this.textBox7.BackColor = global::System.Drawing.Color.FromArgb(30, 30, 30);
			this.textBox7.ForeColor = global::System.Drawing.Color.Lime;
			this.textBox7.Location = new global::System.Drawing.Point(13, 128);
			this.textBox7.Margin = new global::System.Windows.Forms.Padding(4, 4, 4, 4);
			this.textBox7.Name = "textBox7";
			this.textBox7.Size = new global::System.Drawing.Size(222, 30);
			this.textBox7.TabIndex = 64;
			this.textBox8.BackColor = global::System.Drawing.Color.FromArgb(30, 30, 30);
			this.textBox8.ForeColor = global::System.Drawing.Color.Lime;
			this.textBox8.Location = new global::System.Drawing.Point(13, 63);
			this.textBox8.Margin = new global::System.Windows.Forms.Padding(4, 4, 4, 4);
			this.textBox8.Name = "textBox8";
			this.textBox8.Size = new global::System.Drawing.Size(222, 30);
			this.textBox8.TabIndex = 65;
			this.button19.BackColor = global::System.Drawing.Color.FromArgb(40, 40, 40);
			this.button19.FlatAppearance.BorderColor = global::System.Drawing.Color.Cyan;
			this.button19.FlatStyle = global::System.Windows.Forms.FlatStyle.Flat;
			this.button19.Font = new global::System.Drawing.Font("Consolas", 12f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			this.button19.ForeColor = global::System.Drawing.Color.Cyan;
			this.button19.Location = new global::System.Drawing.Point(13, 169);
			this.button19.Margin = new global::System.Windows.Forms.Padding(4, 4, 4, 4);
			this.button19.Name = "button19";
			this.button19.Size = new global::System.Drawing.Size(222, 39);
			this.button19.TabIndex = 66;
			this.button19.Text = "Değiştir ";
			this.button19.UseVisualStyleBackColor = false;
			this.button19.Click += new global::System.EventHandler(this.button19_Click);
			this.groupBox7.Controls.Add(this.label15);
			this.groupBox7.Controls.Add(this.label14);
			this.groupBox7.Controls.Add(this.textBox7);
			this.groupBox7.Controls.Add(this.button19);
			this.groupBox7.Controls.Add(this.textBox8);
			this.groupBox7.Font = new global::System.Drawing.Font("Consolas", 12f, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point, 0);
			this.groupBox7.ForeColor = global::System.Drawing.Color.Cyan;
			this.groupBox7.Location = new global::System.Drawing.Point(773, 305);
			this.groupBox7.Margin = new global::System.Windows.Forms.Padding(4, 4, 4, 4);
			this.groupBox7.Name = "groupBox7";
			this.groupBox7.Padding = new global::System.Windows.Forms.Padding(4, 4, 4, 4);
			this.groupBox7.Size = new global::System.Drawing.Size(306, 225);
			this.groupBox7.TabIndex = 19;
			this.groupBox7.TabStop = false;
			this.groupBox7.Text = "Oyuncu Nick Değiştirme";
			this.label15.AutoSize = true;
			this.label15.ForeColor = global::System.Drawing.Color.Red;
			this.label15.Location = new global::System.Drawing.Point(8, 100);
			this.label15.Margin = new global::System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label15.Name = "label15";
			this.label15.Size = new global::System.Drawing.Size(116, 25);
			this.label15.TabIndex = 68;
			this.label15.Text = "Yeni Nick :";
			this.label14.AutoSize = true;
			this.label14.ForeColor = global::System.Drawing.Color.Red;
			this.label14.Location = new global::System.Drawing.Point(16, 38);
			this.label14.Margin = new global::System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label14.Name = "label14";
			this.label14.Size = new global::System.Drawing.Size(67, 25);
			this.label14.TabIndex = 67;
			this.label14.Text = "Nick :";
			this.label17.AutoSize = true;
			this.label17.Font = new global::System.Drawing.Font("Consolas", 12f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			this.label17.ForeColor = global::System.Drawing.Color.Cyan;
			this.label17.Location = new global::System.Drawing.Point(12, 18);
			this.label17.Margin = new global::System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label17.Name = "label17";
			this.label17.Size = new global::System.Drawing.Size(133, 25);
			this.label17.TabIndex = 71;
			this.label17.Text = "Online Sayısı:";
			this.label18.AutoSize = true;
			this.label18.Font = new global::System.Drawing.Font("Consolas", 12f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			this.label18.ForeColor = global::System.Drawing.Color.Cyan;
			this.label18.Location = new global::System.Drawing.Point(12, 49);
			this.label18.Margin = new global::System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label18.Name = "label18";
			this.label18.Size = new global::System.Drawing.Size(142, 25);
			this.label18.TabIndex = 72;
			this.label18.Text = "Ram Kullanımı:";
			this.label25.AutoSize = true;
			this.label25.BackColor = global::System.Drawing.Color.Black;
			this.label25.Font = new global::System.Drawing.Font("Consolas", 14f, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point, 0);
			this.label25.ForeColor = global::System.Drawing.Color.Lime;
			this.label25.Location = new global::System.Drawing.Point(186, 47);
			this.label25.Margin = new global::System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label25.Name = "label25";
			this.label25.Size = new global::System.Drawing.Size(34, 25);
			this.label25.TabIndex = 73;
			this.label25.Text = "00";
			this.label10.AutoSize = true;
			this.label10.Font = new global::System.Drawing.Font("Consolas", 12f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			this.label10.ForeColor = global::System.Drawing.Color.Cyan;
			this.label10.Location = new global::System.Drawing.Point(228, 495);
			this.label10.Margin = new global::System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label10.Name = "label10";
			this.label10.Size = new global::System.Drawing.Size(56, 25);
			this.label10.TabIndex = 74;
			this.label10.Text = "Nick:";
			this.groupBox5.Controls.Add(this.label9);
			this.groupBox5.Controls.Add(this.textBox6);
			this.groupBox5.Controls.Add(this.label6);
			this.groupBox5.Controls.Add(this.label5);
			this.groupBox5.Controls.Add(this.label3);
			this.groupBox5.Controls.Add(this.label2);
			this.groupBox5.Controls.Add(this.textBox3);
			this.groupBox5.Controls.Add(this.button8);
			this.groupBox5.Controls.Add(this.textBox2);
			this.groupBox5.Controls.Add(this.textBox4);
			this.groupBox5.Controls.Add(this.textBox5);
			this.groupBox5.Font = new global::System.Drawing.Font("Consolas", 12f, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point, 0);
			this.groupBox5.ForeColor = global::System.Drawing.Color.Cyan;
			this.groupBox5.Location = new global::System.Drawing.Point(773, 19);
			this.groupBox5.Margin = new global::System.Windows.Forms.Padding(4, 4, 4, 4);
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.Padding = new global::System.Windows.Forms.Padding(4, 4, 4, 4);
			this.groupBox5.Size = new global::System.Drawing.Size(306, 278);
			this.groupBox5.TabIndex = 28;
			this.groupBox5.TabStop = false;
			this.groupBox5.Text = "Online İtem Etkinliği";
			this.groupBox4.Controls.Add(this.textBox11);
			this.groupBox4.Controls.Add(this.button4);
			this.groupBox4.Controls.Add(this.comboBox1);
			this.groupBox4.Font = new global::System.Drawing.Font("Consolas", 12f, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point, 0);
			this.groupBox4.ForeColor = global::System.Drawing.Color.Cyan;
			this.groupBox4.Location = new global::System.Drawing.Point(233, 155);
			this.groupBox4.Margin = new global::System.Windows.Forms.Padding(4, 4, 4, 4);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Padding = new global::System.Windows.Forms.Padding(4, 4, 4, 4);
			this.groupBox4.Size = new global::System.Drawing.Size(216, 179);
			this.groupBox4.TabIndex = 27;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "Online Etkinliği";
			this.textBox11.AccessibleName = "";
			this.textBox11.BackColor = global::System.Drawing.Color.FromArgb(30, 30, 30);
			this.textBox11.ForeColor = global::System.Drawing.Color.Lime;
			this.textBox11.Location = new global::System.Drawing.Point(8, 76);
			this.textBox11.Margin = new global::System.Windows.Forms.Padding(4, 4, 4, 4);
			this.textBox11.Name = "textBox11";
			this.textBox11.Size = new global::System.Drawing.Size(161, 30);
			this.textBox11.TabIndex = 34;
			this.button4.BackColor = global::System.Drawing.Color.FromArgb(40, 40, 40);
			this.button4.FlatAppearance.BorderColor = global::System.Drawing.Color.Cyan;
			this.button4.FlatStyle = global::System.Windows.Forms.FlatStyle.Flat;
			this.button4.Font = new global::System.Drawing.Font("Consolas", 12f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			this.button4.ForeColor = global::System.Drawing.Color.Cyan;
			this.button4.Location = new global::System.Drawing.Point(8, 114);
			this.button4.Margin = new global::System.Windows.Forms.Padding(4, 4, 4, 4);
			this.button4.Name = "button4";
			this.button4.Size = new global::System.Drawing.Size(161, 39);
			this.button4.TabIndex = 27;
			this.button4.Text = "Gönder";
			this.button4.TextImageRelation = global::System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.button4.UseVisualStyleBackColor = false;
			this.button4.Click += new global::System.EventHandler(this.button4_Click_1);
			this.comboBox1.BackColor = global::System.Drawing.Color.FromArgb(30, 30, 30);
			this.comboBox1.ForeColor = global::System.Drawing.Color.Lime;
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Items.AddRange(new object[]
			{
				"Kupon",
				"Exp",
				"Onur",
				"Kart Ruhu",
				"Bağlı Kupon",
				"Mükafat"
			});
			this.comboBox1.Location = new global::System.Drawing.Point(8, 34);
			this.comboBox1.Margin = new global::System.Windows.Forms.Padding(4, 4, 4, 4);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new global::System.Drawing.Size(161, 33);
			this.comboBox1.TabIndex = 0;
			this.groupBox8.Controls.Add(this.label16);
			this.groupBox8.Controls.Add(this.textBox9);
			this.groupBox8.Controls.Add(this.label26);
			this.groupBox8.Controls.Add(this.button21);
			this.groupBox8.Controls.Add(this.textBox10);
			this.groupBox8.Font = new global::System.Drawing.Font("Consolas", 12f, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point, 0);
			this.groupBox8.ForeColor = global::System.Drawing.Color.Cyan;
			this.groupBox8.Location = new global::System.Drawing.Point(465, 538);
			this.groupBox8.Margin = new global::System.Windows.Forms.Padding(4, 4, 4, 4);
			this.groupBox8.Name = "groupBox8";
			this.groupBox8.Padding = new global::System.Windows.Forms.Padding(4, 4, 4, 4);
			this.groupBox8.Size = new global::System.Drawing.Size(300, 230);
			this.groupBox8.TabIndex = 75;
			this.groupBox8.TabStop = false;
			this.groupBox8.Text = "Oyuncu İP Adresi Bulma";
			this.label16.AutoSize = true;
			this.label16.ForeColor = global::System.Drawing.Color.Cyan;
			this.label16.Location = new global::System.Drawing.Point(16, 96);
			this.label16.Margin = new global::System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label16.Name = "label16";
			this.label16.Size = new global::System.Drawing.Size(113, 25);
			this.label16.TabIndex = 69;
			this.label16.Text = "İP Adresi :";
			this.textBox9.BackColor = global::System.Drawing.Color.FromArgb(30, 30, 30);
			this.textBox9.ForeColor = global::System.Drawing.Color.Lime;
			this.textBox9.Location = new global::System.Drawing.Point(21, 128);
			this.textBox9.Margin = new global::System.Windows.Forms.Padding(4, 4, 4, 4);
			this.textBox9.Name = "textBox9";
			this.textBox9.Size = new global::System.Drawing.Size(270, 30);
			this.textBox9.TabIndex = 68;
			this.label26.AutoSize = true;
			this.label26.ForeColor = global::System.Drawing.Color.Cyan;
			this.label26.Location = new global::System.Drawing.Point(16, 32);
			this.label26.Margin = new global::System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label26.Name = "label26";
			this.label26.Size = new global::System.Drawing.Size(67, 25);
			this.label26.TabIndex = 67;
			this.label26.Text = "Nick :";
			this.button21.BackColor = global::System.Drawing.Color.FromArgb(40, 40, 40);
			this.button21.FlatAppearance.BorderColor = global::System.Drawing.Color.Cyan;
			this.button21.FlatStyle = global::System.Windows.Forms.FlatStyle.Flat;
			this.button21.Font = new global::System.Drawing.Font("Consolas", 12f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			this.button21.ForeColor = global::System.Drawing.Color.Cyan;
			this.button21.Location = new global::System.Drawing.Point(21, 167);
			this.button21.Margin = new global::System.Windows.Forms.Padding(4, 4, 4, 4);
			this.button21.Name = "button21";
			this.button21.Size = new global::System.Drawing.Size(270, 39);
			this.button21.TabIndex = 66;
			this.button21.Text = "İP Adresini Göster";
			this.button21.UseVisualStyleBackColor = false;
			this.button21.Click += new global::System.EventHandler(this.button21_Click);
			this.textBox10.BackColor = global::System.Drawing.Color.FromArgb(30, 30, 30);
			this.textBox10.ForeColor = global::System.Drawing.Color.Lime;
			this.textBox10.Location = new global::System.Drawing.Point(21, 60);
			this.textBox10.Margin = new global::System.Windows.Forms.Padding(4, 4, 4, 4);
			this.textBox10.Name = "textBox10";
			this.textBox10.Size = new global::System.Drawing.Size(270, 30);
			this.textBox10.TabIndex = 65;
			this.groupBox9.Controls.Add(this.textBox12);
			this.groupBox9.Controls.Add(this.button5);
			this.groupBox9.Controls.Add(this.comboBox2);
			this.groupBox9.Font = new global::System.Drawing.Font("Consolas", 12f, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point, 0);
			this.groupBox9.ForeColor = global::System.Drawing.Color.Cyan;
			this.groupBox9.Location = new global::System.Drawing.Point(457, 18);
			this.groupBox9.Margin = new global::System.Windows.Forms.Padding(4, 4, 4, 4);
			this.groupBox9.Name = "groupBox9";
			this.groupBox9.Padding = new global::System.Windows.Forms.Padding(4, 4, 4, 4);
			this.groupBox9.Size = new global::System.Drawing.Size(308, 191);
			this.groupBox9.TabIndex = 76;
			this.groupBox9.TabStop = false;
			this.groupBox9.Text = "Yönetim Mesajı";
			this.textBox12.AccessibleName = "";
			this.textBox12.BackColor = global::System.Drawing.Color.FromArgb(30, 30, 30);
			this.textBox12.Font = new global::System.Drawing.Font("Consolas", 12f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			this.textBox12.ForeColor = global::System.Drawing.Color.Lime;
			this.textBox12.Location = new global::System.Drawing.Point(8, 76);
			this.textBox12.Margin = new global::System.Windows.Forms.Padding(4, 4, 4, 4);
			this.textBox12.Multiline = true;
			this.textBox12.Name = "textBox12";
			this.textBox12.Size = new global::System.Drawing.Size(291, 100);
			this.textBox12.TabIndex = 34;
			this.textBox12.Text = "Mesaj Yaz...";
			this.button5.BackColor = global::System.Drawing.Color.FromArgb(40, 40, 40);
			this.button5.FlatAppearance.BorderColor = global::System.Drawing.Color.Cyan;
			this.button5.FlatStyle = global::System.Windows.Forms.FlatStyle.Flat;
			this.button5.Font = new global::System.Drawing.Font("Consolas", 12f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			this.button5.ForeColor = global::System.Drawing.Color.Cyan;
			this.button5.Location = new global::System.Drawing.Point(179, 32);
			this.button5.Margin = new global::System.Windows.Forms.Padding(4, 4, 4, 4);
			this.button5.Name = "button5";
			this.button5.Size = new global::System.Drawing.Size(121, 39);
			this.button5.TabIndex = 27;
			this.button5.Text = "Gönder";
			this.button5.TextImageRelation = global::System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.button5.UseVisualStyleBackColor = false;
			this.button5.Click += new global::System.EventHandler(this.button5_Click);
			this.comboBox2.BackColor = global::System.Drawing.Color.FromArgb(30, 30, 30);
			this.comboBox2.ForeColor = global::System.Drawing.Color.Lime;
			this.comboBox2.FormattingEnabled = true;
			this.comboBox2.Items.AddRange(new object[]
			{
				"Büyük Hoparlör",
				"Küçük Hoparlör",
				"Sistem Mesajı",
				"Sarı Mesaj",
				"Mor Mesaj",
				"Kırmızı Mesaj",
				"Admin Mesajı"
			});
			this.comboBox2.Location = new global::System.Drawing.Point(8, 34);
			this.comboBox2.Margin = new global::System.Windows.Forms.Padding(4, 4, 4, 4);
			this.comboBox2.Name = "comboBox2";
			this.comboBox2.Size = new global::System.Drawing.Size(161, 33);
			this.comboBox2.TabIndex = 0;
			this.groupBox1.Controls.Add(this.button1);
			this.groupBox1.Controls.Add(this.comboBox3);
			this.groupBox1.Font = new global::System.Drawing.Font("Consolas", 12f, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point, 0);
			this.groupBox1.ForeColor = global::System.Drawing.Color.Cyan;
			this.groupBox1.Location = new global::System.Drawing.Point(233, 18);
			this.groupBox1.Margin = new global::System.Windows.Forms.Padding(4, 4, 4, 4);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Padding = new global::System.Windows.Forms.Padding(4, 4, 4, 4);
			this.groupBox1.Size = new global::System.Drawing.Size(216, 129);
			this.groupBox1.TabIndex = 77;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Verileri Güncelle";
			this.button1.BackColor = global::System.Drawing.Color.FromArgb(40, 40, 40);
			this.button1.FlatAppearance.BorderColor = global::System.Drawing.Color.Cyan;
			this.button1.FlatStyle = global::System.Windows.Forms.FlatStyle.Flat;
			this.button1.Font = new global::System.Drawing.Font("Consolas", 12f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			this.button1.ForeColor = global::System.Drawing.Color.Cyan;
			this.button1.Location = new global::System.Drawing.Point(8, 75);
			this.button1.Margin = new global::System.Windows.Forms.Padding(4, 4, 4, 4);
			this.button1.Name = "button1";
			this.button1.Size = new global::System.Drawing.Size(200, 39);
			this.button1.TabIndex = 27;
			this.button1.Text = "Güncelle";
			this.button1.TextImageRelation = global::System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.button1.UseVisualStyleBackColor = false;
			this.button1.Click += new global::System.EventHandler(this.button1_Click);
			this.comboBox3.BackColor = global::System.Drawing.Color.FromArgb(30, 30, 30);
			this.comboBox3.ForeColor = global::System.Drawing.Color.Lime;
			this.comboBox3.FormattingEnabled = true;
			this.comboBox3.Items.AddRange(new object[]
			{
				"Seçim Yapınız",
				"Tüm Veritabanını Güncelle",
				"Görev Güncelle",
				"Onur Listesi Güncelle",
				"Etkinlikleri Güncelle",
				"Pve_İnfo Güncelle",
				"Templatelist Güncelle",
				"Füzyon Güncelle",
				"Shop Güncelle",
				"Mission Güncelle",
				"Npc Güncelle",
				"Ball Güncelle",
				"Ball Config Güncelle",
				"Yazıları Güncelle",
				"Goldları Güncelle",
				"EventAward Güncelle",
				"Dropları Güncelle",
				"Mapları Güncelle"
			});
			this.comboBox3.Location = new global::System.Drawing.Point(8, 34);
			this.comboBox3.Margin = new global::System.Windows.Forms.Padding(4, 4, 4, 4);
			this.comboBox3.Name = "comboBox3";
			this.comboBox3.Size = new global::System.Drawing.Size(200, 33);
			this.comboBox3.TabIndex = 0;
			this.dataGridView1.BackgroundColor = global::System.Drawing.Color.Black;
			this.dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = global::System.Drawing.Color.FromArgb(40, 40, 40);
			this.dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = global::System.Drawing.Color.Cyan;
			this.dataGridView1.DefaultCellStyle.BackColor = global::System.Drawing.Color.FromArgb(30, 30, 30);
			this.dataGridView1.DefaultCellStyle.ForeColor = global::System.Drawing.Color.Lime;
			this.dataGridView1.GridColor = global::System.Drawing.Color.Cyan;
			this.dataGridView1.ColumnHeadersHeightSizeMode = global::System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView1.Location = new global::System.Drawing.Point(457, 254);
			this.dataGridView1.Margin = new global::System.Windows.Forms.Padding(4, 4, 4, 4);
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.RowHeadersWidth = 51;
			this.dataGridView1.Size = new global::System.Drawing.Size(308, 266);
			this.dataGridView1.TabIndex = 78;
			this.comboBox4.BackColor = global::System.Drawing.Color.FromArgb(30, 30, 30);
			this.comboBox4.ForeColor = global::System.Drawing.Color.Lime;
			this.comboBox4.Font = new global::System.Drawing.Font("Consolas", 10f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			this.comboBox4.FormattingEnabled = true;
			this.comboBox4.Items.AddRange(new object[]
			{
				"Banlı Hesaplar"
			});
			this.comboBox4.Location = new global::System.Drawing.Point(457, 217);
			this.comboBox4.Margin = new global::System.Windows.Forms.Padding(4, 4, 4, 4);
			this.comboBox4.Name = "comboBox4";
			this.comboBox4.Size = new global::System.Drawing.Size(155, 28);
			this.comboBox4.TabIndex = 79;
			this.comboBox4.Text = "Banlı Hesaplar";
			this.button2.BackColor = global::System.Drawing.Color.FromArgb(40, 40, 40);
			this.button2.FlatAppearance.BorderColor = global::System.Drawing.Color.Cyan;
			this.button2.FlatStyle = global::System.Windows.Forms.FlatStyle.Flat;
			this.button2.Font = new global::System.Drawing.Font("Consolas", 10f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			this.button2.ForeColor = global::System.Drawing.Color.Cyan;
			this.button2.Location = new global::System.Drawing.Point(620, 214);
			this.button2.Margin = new global::System.Windows.Forms.Padding(4, 4, 4, 4);
			this.button2.Name = "button2";
			this.button2.Size = new global::System.Drawing.Size(145, 31);
			this.button2.TabIndex = 80;
			this.button2.Text = "Göster";
			this.button2.TextImageRelation = global::System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.button2.UseVisualStyleBackColor = false;
			this.button2.Click += new global::System.EventHandler(this.button2_Click);
			this.groupBox2.Controls.Add(this.textBox13);
			this.groupBox2.Font = new global::System.Drawing.Font("Consolas", 12f, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point, 0);
			this.groupBox2.ForeColor = global::System.Drawing.Color.Cyan;
			this.groupBox2.Location = new global::System.Drawing.Point(229, 552);
			this.groupBox2.Margin = new global::System.Windows.Forms.Padding(4, 4, 4, 4);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Padding = new global::System.Windows.Forms.Padding(4, 4, 4, 4);
			this.groupBox2.Size = new global::System.Drawing.Size(220, 119);
			this.groupBox2.TabIndex = 81;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Şifre Değiştir";
			this.textBox13.AccessibleName = "";
			this.textBox13.BackColor = global::System.Drawing.Color.FromArgb(30, 30, 30);
			this.textBox13.ForeColor = global::System.Drawing.Color.Lime;
			this.textBox13.Location = new global::System.Drawing.Point(12, 36);
			this.textBox13.Margin = new global::System.Windows.Forms.Padding(4, 4, 4, 4);
			this.textBox13.Name = "textBox13";
			this.textBox13.Size = new global::System.Drawing.Size(200, 30);
			this.textBox13.TabIndex = 35;
			this.button3.BackColor = global::System.Drawing.Color.FromArgb(40, 40, 40);
			this.button3.FlatAppearance.BorderColor = global::System.Drawing.Color.Cyan;
			this.button3.FlatStyle = global::System.Windows.Forms.FlatStyle.Flat;
			this.button3.Font = new global::System.Drawing.Font("Consolas", 12f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			this.button3.ForeColor = global::System.Drawing.Color.Cyan;
			this.button3.Location = new global::System.Drawing.Point(241, 626);
			this.button3.Margin = new global::System.Windows.Forms.Padding(4, 4, 4, 4);
			this.button3.Name = "button3";
			this.button3.Size = new global::System.Drawing.Size(200, 39);
			this.button3.TabIndex = 27;
			this.button3.Text = "Değiştir";
			this.button3.TextImageRelation = global::System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.button3.UseVisualStyleBackColor = false;
			this.button3.Click += new global::System.EventHandler(this.button3_Click);
			this.label1.AutoSize = true;
			this.label1.Font = new global::System.Drawing.Font("Consolas", 12f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			this.label1.ForeColor = global::System.Drawing.Color.Cyan;
			this.label1.Location = new global::System.Drawing.Point(781, 552);
			this.label1.Margin = new global::System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label1.Name = "label1";
			this.label1.Size = new global::System.Drawing.Size(172, 25);
			this.label1.TabIndex = 82;
			this.label1.Text = "DDoS Durum:";
			this.label28.AutoSize = true;
			this.label28.Font = new global::System.Drawing.Font("Consolas", 12f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			this.label28.ForeColor = global::System.Drawing.Color.Cyan;
			this.label28.Location = new global::System.Drawing.Point(1063, 783);
			this.label28.Margin = new global::System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label28.Name = "label28";
			this.label28.Size = new global::System.Drawing.Size(0, 25);
			this.label28.TabIndex = 83;
			this.label29.AutoSize = true;
			this.label29.Font = new global::System.Drawing.Font("Consolas", 12f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			this.label29.ForeColor = global::System.Drawing.Color.Cyan;
			this.label29.Location = new global::System.Drawing.Point(781, 586);
			this.label29.Margin = new global::System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label29.Name = "label29";
			this.label29.Size = new global::System.Drawing.Size(146, 25);
			this.label29.TabIndex = 84;
			this.label29.Text = "Bağlantı Sayısı:";
			this.label30.AutoSize = true;
			this.label30.Font = new global::System.Drawing.Font("Consolas", 12f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			this.label30.ForeColor = global::System.Drawing.Color.Lime;
			this.label30.Location = new global::System.Drawing.Point(964, 586);
			this.label30.Margin = new global::System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label30.Name = "label30";
			this.label30.Size = new global::System.Drawing.Size(75, 25);
			this.label30.TabIndex = 85;
			this.label30.Text = "label30";
			this.label32.AutoSize = true;
			this.label32.Font = new global::System.Drawing.Font("Consolas", 12f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			this.label32.ForeColor = global::System.Drawing.Color.Cyan;
			this.label32.Location = new global::System.Drawing.Point(781, 626);
			this.label32.Margin = new global::System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label32.Name = "label32";
			this.label32.Size = new global::System.Drawing.Size(156, 25);
			this.label32.TabIndex = 86;
			this.label32.Text = "Bloklama Sayısı:";
			this.label33.AutoSize = true;
			this.label33.Font = new global::System.Drawing.Font("Consolas", 12f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			this.label33.ForeColor = global::System.Drawing.Color.Lime;
			this.label33.Location = new global::System.Drawing.Point(964, 633);
			this.label33.Margin = new global::System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label33.Name = "label33";
			this.label33.Size = new global::System.Drawing.Size(75, 25);
			this.label33.TabIndex = 87;
			this.label33.Text = "label33";
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(8f, 16f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = global::System.Drawing.Color.Black;
			base.ClientSize = new global::System.Drawing.Size(1089, 775);
			base.Controls.Add(this.label33);
			base.Controls.Add(this.button3);
			base.Controls.Add(this.label32);
			base.Controls.Add(this.label30);
			base.Controls.Add(this.label29);
			base.Controls.Add(this.label28);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.groupBox2);
			base.Controls.Add(this.button2);
			base.Controls.Add(this.comboBox4);
			base.Controls.Add(this.dataGridView1);
			base.Controls.Add(this.groupBox1);
			base.Controls.Add(this.groupBox9);
			base.Controls.Add(this.groupBox8);
			base.Controls.Add(this.label10);
			base.Controls.Add(this.label25);
			base.Controls.Add(this.label18);
			base.Controls.Add(this.label17);
			base.Controls.Add(this.groupBox7);
			base.Controls.Add(this.button17);
			base.Controls.Add(this.button9);
			base.Controls.Add(this.textBox1);
			base.Controls.Add(this.groupBox4);
			base.Controls.Add(this.button15);
			base.Controls.Add(this.button10);
			base.Controls.Add(this.button16);
			base.Controls.Add(this.groupBox6);
			base.Controls.Add(this.onlineTxt);
			base.Controls.Add(this.lixtBox1);
			base.Controls.Add(this.groupBox5);
			base.Controls.Add(this.groupBox3);
			this.ForeColor = global::System.Drawing.Color.Cyan;
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.Margin = new global::System.Windows.Forms.Padding(4, 4, 4, 4);
			base.MaximizeBox = false;
			base.Name = "ServerManagementForm";
			base.ShowIcon = false;
			this.Text = "BomBomRia Yönetim";
			base.Load += new global::System.EventHandler(this.ServerManagementForm_Load);
			this.groupBox3.ResumeLayout(false);
			this.groupBox6.ResumeLayout(false);
			this.groupBox6.PerformLayout();
			this.groupBox7.ResumeLayout(false);
			this.groupBox7.PerformLayout();
			this.groupBox5.ResumeLayout(false);
			this.groupBox5.PerformLayout();
			this.groupBox4.ResumeLayout(false);
			this.groupBox4.PerformLayout();
			this.groupBox8.ResumeLayout(false);
			this.groupBox8.PerformLayout();
			this.groupBox9.ResumeLayout(false);
			this.groupBox9.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			((global::System.ComponentModel.ISupportInitialize)this.dataGridView1).EndInit();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04000024 RID: 36
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04000025 RID: 37
		private global::System.Windows.Forms.Label onlineTxt;

		// Token: 0x04000026 RID: 38
		private global::System.Windows.Forms.Timer UpdateUI;

		// Token: 0x04000027 RID: 39
		private global::System.Windows.Forms.CheckBox checkBox3;

		// Token: 0x04000028 RID: 40
		private global::System.Windows.Forms.GroupBox groupBox3;

		// Token: 0x04000029 RID: 41
		private global::System.Windows.Forms.TextBox textBox4;

		// Token: 0x0400002A RID: 42
		private global::System.Windows.Forms.TextBox textBox2;

		// Token: 0x0400002B RID: 43
		private global::System.Windows.Forms.TextBox textBox3;

		// Token: 0x0400002C RID: 44
		private global::System.Windows.Forms.TextBox textBox5;

		// Token: 0x0400002D RID: 45
		private global::System.Windows.Forms.Button button8;

		// Token: 0x0400002E RID: 46
		internal global::System.Windows.Forms.Label label2;

		// Token: 0x0400002F RID: 47
		internal global::System.Windows.Forms.Label label5;

		// Token: 0x04000030 RID: 48
		internal global::System.Windows.Forms.Label label3;

		// Token: 0x04000031 RID: 49
		internal global::System.Windows.Forms.Label label6;

		// Token: 0x04000032 RID: 50
		private global::System.Windows.Forms.ListBox lixtBox1;

		// Token: 0x04000033 RID: 51
		private global::System.Windows.Forms.Button button9;

		// Token: 0x04000034 RID: 52
		private global::System.Windows.Forms.Button button10;

		// Token: 0x04000035 RID: 53
		private global::System.Windows.Forms.Button button15;

		// Token: 0x04000036 RID: 54
		private global::System.Windows.Forms.TextBox textBox1;

		// Token: 0x04000037 RID: 55
		private global::System.Windows.Forms.Button button16;

		// Token: 0x04000038 RID: 56
		internal global::System.Windows.Forms.Label label9;

		// Token: 0x04000039 RID: 57
		private global::System.Windows.Forms.TextBox textBox6;

		// Token: 0x0400003A RID: 58
		private global::System.Windows.Forms.GroupBox groupBox6;

		// Token: 0x0400003B RID: 59
		private global::System.Windows.Forms.Label label13;

		// Token: 0x0400003C RID: 60
		private global::System.Windows.Forms.Label label27;

		// Token: 0x0400003D RID: 61
		private global::System.Windows.Forms.Label label24;

		// Token: 0x0400003E RID: 62
		private global::System.Windows.Forms.Label nickName;

		// Token: 0x0400003F RID: 63
		private global::System.Windows.Forms.Label label12;

		// Token: 0x04000040 RID: 64
		private global::System.Windows.Forms.Label label31;

		// Token: 0x04000041 RID: 65
		private global::System.Windows.Forms.Label label4;

		// Token: 0x04000042 RID: 66
		private global::System.Windows.Forms.Label label23;

		// Token: 0x04000043 RID: 67
		private global::System.Windows.Forms.Label label11;

		// Token: 0x04000044 RID: 68
		private global::System.Windows.Forms.Label label35;

		// Token: 0x04000045 RID: 69
		private global::System.Windows.Forms.Label label22;

		// Token: 0x04000046 RID: 70
		private global::System.Windows.Forms.Label label7;

		// Token: 0x04000047 RID: 71
		private global::System.Windows.Forms.Label label21;

		// Token: 0x04000048 RID: 72
		private global::System.Windows.Forms.Label label39;

		// Token: 0x04000049 RID: 73
		private global::System.Windows.Forms.Label label8;

		// Token: 0x0400004A RID: 74
		private global::System.Windows.Forms.Label label20;

		// Token: 0x0400004B RID: 75
		private global::System.Windows.Forms.Label label42;

		// Token: 0x0400004C RID: 76
		private global::System.Windows.Forms.Label label43;

		// Token: 0x0400004D RID: 77
		private global::System.Windows.Forms.Label label19;

		// Token: 0x0400004E RID: 78
		private global::System.Windows.Forms.Label label45;

		// Token: 0x0400004F RID: 79
		private global::System.Windows.Forms.Button button17;

		// Token: 0x04000050 RID: 80
		private global::System.Windows.Forms.TextBox textBox7;

		// Token: 0x04000051 RID: 81
		private global::System.Windows.Forms.TextBox textBox8;

		// Token: 0x04000052 RID: 82
		private global::System.Windows.Forms.Button button19;

		// Token: 0x04000053 RID: 83
		private global::System.Windows.Forms.GroupBox groupBox7;

		// Token: 0x04000054 RID: 84
		internal global::System.Windows.Forms.Label label15;

		// Token: 0x04000055 RID: 85
		internal global::System.Windows.Forms.Label label14;

		// Token: 0x04000056 RID: 86
		internal global::System.Windows.Forms.Label label17;

		// Token: 0x04000057 RID: 87
		internal global::System.Windows.Forms.Label label18;

		// Token: 0x04000058 RID: 88
		private global::System.Windows.Forms.Label label25;

		// Token: 0x04000059 RID: 89
		internal global::System.Windows.Forms.Label label10;

		// Token: 0x0400005A RID: 90
		private global::System.Windows.Forms.GroupBox groupBox5;

		// Token: 0x0400005B RID: 91
		private global::System.Windows.Forms.GroupBox groupBox4;

		// Token: 0x0400005C RID: 92
		private global::System.Windows.Forms.GroupBox groupBox8;

		// Token: 0x0400005D RID: 93
		internal global::System.Windows.Forms.Label label26;

		// Token: 0x0400005E RID: 94
		private global::System.Windows.Forms.Button button21;

		// Token: 0x0400005F RID: 95
		private global::System.Windows.Forms.TextBox textBox10;

		// Token: 0x04000060 RID: 96
		private global::System.Windows.Forms.TextBox textBox9;

		// Token: 0x04000061 RID: 97
		internal global::System.Windows.Forms.Label label16;

		// Token: 0x04000062 RID: 98
		private global::System.Windows.Forms.ComboBox comboBox1;

		// Token: 0x04000063 RID: 99
		private global::System.Windows.Forms.Button button4;

		// Token: 0x04000064 RID: 100
		private global::System.Windows.Forms.TextBox textBox11;

		// Token: 0x04000065 RID: 101
		private global::System.Windows.Forms.GroupBox groupBox9;

		// Token: 0x04000066 RID: 102
		private global::System.Windows.Forms.TextBox textBox12;

		// Token: 0x04000067 RID: 103
		private global::System.Windows.Forms.Button button5;

		// Token: 0x04000068 RID: 104
		private global::System.Windows.Forms.ComboBox comboBox2;

		// Token: 0x04000069 RID: 105
		private global::System.Windows.Forms.GroupBox groupBox1;

		// Token: 0x0400006A RID: 106
		private global::System.Windows.Forms.Button button1;

		// Token: 0x0400006B RID: 107
		private global::System.Windows.Forms.ComboBox comboBox3;

		// Token: 0x0400006C RID: 108
		private global::System.Windows.Forms.DataGridView dataGridView1;

		// Token: 0x0400006D RID: 109
		private global::System.Windows.Forms.ComboBox comboBox4;

		// Token: 0x0400006E RID: 110
		private global::System.Windows.Forms.Button button2;

		// Token: 0x0400006F RID: 111
		private global::System.Windows.Forms.GroupBox groupBox2;

		// Token: 0x04000070 RID: 112
		private global::System.Windows.Forms.TextBox textBox13;

		// Token: 0x04000071 RID: 113
		private global::System.Windows.Forms.Label label1;

		// Token: 0x04000072 RID: 114
		private global::System.Windows.Forms.Label label28;

		// Token: 0x04000073 RID: 115
		private global::System.Windows.Forms.Label label29;

		// Token: 0x04000074 RID: 116
		private global::System.Windows.Forms.Label label30;

		// Token: 0x04000075 RID: 117
		private global::System.Windows.Forms.Label label32;

		// Token: 0x04000076 RID: 118
		private global::System.Windows.Forms.Label label33;

		// Token: 0x04000077 RID: 119
		private global::System.Windows.Forms.Button button3;
	}
}
