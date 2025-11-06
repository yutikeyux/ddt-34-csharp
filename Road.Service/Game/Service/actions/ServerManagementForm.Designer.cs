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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.onlineTxt = new System.Windows.Forms.Label();
            this.UpdateUI = new System.Windows.Forms.Timer(this.components);
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.button8 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lixtBox1 = new System.Windows.Forms.ListBox();
            this.button9 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.button15 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button16 = new System.Windows.Forms.Button();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.nickName = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label35 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label39 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label42 = new System.Windows.Forms.Label();
            this.label43 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label45 = new System.Windows.Forms.Label();
            this.button17 = new System.Windows.Forms.Button();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.textBox8 = new System.Windows.Forms.TextBox();
            this.button19 = new System.Windows.Forms.Button();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.textBox11 = new System.Windows.Forms.TextBox();
            this.button4 = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.label16 = new System.Windows.Forms.Label();
            this.textBox9 = new System.Windows.Forms.TextBox();
            this.label26 = new System.Windows.Forms.Label();
            this.button21 = new System.Windows.Forms.Button();
            this.textBox10 = new System.Windows.Forms.TextBox();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.textBox12 = new System.Windows.Forms.TextBox();
            this.button5 = new System.Windows.Forms.Button();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.comboBox4 = new System.Windows.Forms.ComboBox();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBox13 = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.label33 = new System.Windows.Forms.Label();
            this.groupBox3.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // onlineTxt
            // 
            this.onlineTxt.AutoSize = true;
            this.onlineTxt.BackColor = System.Drawing.Color.Black;
            this.onlineTxt.Font = new System.Drawing.Font("Consolas", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.onlineTxt.ForeColor = System.Drawing.Color.Lime;
            this.onlineTxt.Location = new System.Drawing.Point(140, 15);
            this.onlineTxt.Name = "onlineTxt";
            this.onlineTxt.Size = new System.Drawing.Size(30, 22);
            this.onlineTxt.TabIndex = 0;
            this.onlineTxt.Text = "00";
            // 
            // UpdateUI
            // 
            this.UpdateUI.Enabled = true;
            this.UpdateUI.Interval = 10000;
            this.UpdateUI.Tick += new System.EventHandler(this.UpdateUI_Tick);
            // 
            // checkBox3
            // 
            this.checkBox3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.checkBox3.ForeColor = System.Drawing.Color.Cyan;
            this.checkBox3.Location = new System.Drawing.Point(104, 201);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(104, 24);
            this.checkBox3.TabIndex = 0;
            this.checkBox3.UseVisualStyleBackColor = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.checkBox3);
            this.groupBox3.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold);
            this.groupBox3.ForeColor = System.Drawing.Color.Cyan;
            this.groupBox3.Location = new System.Drawing.Point(1492, 16);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(10, 10);
            this.groupBox3.TabIndex = 17;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Акции и дебаг";
            // 
            // textBox4
            // 
            this.textBox4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.textBox4.ForeColor = System.Drawing.Color.Lime;
            this.textBox4.Location = new System.Drawing.Point(111, 92);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(108, 26);
            this.textBox4.TabIndex = 22;
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.textBox2.ForeColor = System.Drawing.Color.Lime;
            this.textBox2.Location = new System.Drawing.Point(111, 60);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(108, 26);
            this.textBox2.TabIndex = 23;
            // 
            // textBox3
            // 
            this.textBox3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.textBox3.ForeColor = System.Drawing.Color.Lime;
            this.textBox3.Location = new System.Drawing.Point(111, 28);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(108, 26);
            this.textBox3.TabIndex = 24;
            // 
            // textBox5
            // 
            this.textBox5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.textBox5.ForeColor = System.Drawing.Color.Lime;
            this.textBox5.Location = new System.Drawing.Point(111, 123);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(108, 26);
            this.textBox5.TabIndex = 25;
            // 
            // button8
            // 
            this.button8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.button8.FlatAppearance.BorderColor = System.Drawing.Color.Lime;
            this.button8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button8.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button8.ForeColor = System.Drawing.Color.Cyan;
            this.button8.Location = new System.Drawing.Point(43, 187);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(143, 32);
            this.button8.TabIndex = 26;
            this.button8.Text = "Gönder";
            this.button8.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.button8.UseVisualStyleBackColor = false;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Cyan;
            this.label2.Location = new System.Drawing.Point(24, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 19);
            this.label2.TabIndex = 28;
            this.label2.Text = "İtem ID :";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.Cyan;
            this.label9.Location = new System.Drawing.Point(24, 156);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(90, 19);
            this.label9.TabIndex = 35;
            this.label9.Text = "Bağlımı :";
            // 
            // textBox6
            // 
            this.textBox6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.textBox6.ForeColor = System.Drawing.Color.Lime;
            this.textBox6.Location = new System.Drawing.Point(111, 156);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(108, 26);
            this.textBox6.TabIndex = 34;
            this.textBox6.Text = "True";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.Cyan;
            this.label6.Location = new System.Drawing.Point(24, 92);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(81, 19);
            this.label6.TabIndex = 33;
            this.label6.Text = "İçerik :";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.Cyan;
            this.label5.Location = new System.Drawing.Point(24, 123);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 19);
            this.label5.TabIndex = 31;
            this.label5.Text = "Adet :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Cyan;
            this.label3.Location = new System.Drawing.Point(24, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 19);
            this.label3.TabIndex = 29;
            this.label3.Text = "Başlık :";
            // 
            // lixtBox1
            // 
            this.lixtBox1.BackColor = System.Drawing.Color.Black;
            this.lixtBox1.CausesValidation = false;
            this.lixtBox1.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lixtBox1.ForeColor = System.Drawing.Color.Lime;
            this.lixtBox1.FormattingEnabled = true;
            this.lixtBox1.ItemHeight = 19;
            this.lixtBox1.Location = new System.Drawing.Point(12, 71);
            this.lixtBox1.Name = "lixtBox1";
            this.lixtBox1.Size = new System.Drawing.Size(154, 99);
            this.lixtBox1.TabIndex = 29;
            this.lixtBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // button9
            // 
            this.button9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.button9.FlatAppearance.BorderColor = System.Drawing.Color.Lime;
            this.button9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button9.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button9.ForeColor = System.Drawing.Color.Cyan;
            this.button9.Location = new System.Drawing.Point(12, 182);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(153, 32);
            this.button9.TabIndex = 31;
            this.button9.Text = "Güncelle !";
            this.button9.UseVisualStyleBackColor = false;
            this.button9.Click += new System.EventHandler(this.button9_Click_1);
            // 
            // button10
            // 
            this.button10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.button10.FlatAppearance.BorderColor = System.Drawing.Color.Red;
            this.button10.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button10.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button10.ForeColor = System.Drawing.Color.Red;
            this.button10.Location = new System.Drawing.Point(226, 277);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(164, 38);
            this.button10.TabIndex = 31;
            this.button10.Text = "Banla !";
            this.button10.UseVisualStyleBackColor = false;
            this.button10.Click += new System.EventHandler(this.button10_Click_1);
            // 
            // button15
            // 
            this.button15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(50)))), ((int)(((byte)(0)))));
            this.button15.FlatAppearance.BorderColor = System.Drawing.Color.Lime;
            this.button15.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button15.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button15.ForeColor = System.Drawing.Color.Lime;
            this.button15.Location = new System.Drawing.Point(226, 321);
            this.button15.Name = "button15";
            this.button15.Size = new System.Drawing.Size(164, 38);
            this.button15.TabIndex = 38;
            this.button15.Text = "Kickle !";
            this.button15.UseVisualStyleBackColor = false;
            this.button15.Click += new System.EventHandler(this.button15_Click);
            // 
            // textBox1
            // 
            this.textBox1.AccessibleName = "";
            this.textBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.textBox1.ForeColor = System.Drawing.Color.Lime;
            this.textBox1.Location = new System.Drawing.Point(227, 424);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(164, 20);
            this.textBox1.TabIndex = 33;
            // 
            // button16
            // 
            this.button16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(50)))), ((int)(((byte)(0)))));
            this.button16.FlatAppearance.BorderColor = System.Drawing.Color.Lime;
            this.button16.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button16.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button16.ForeColor = System.Drawing.Color.Lime;
            this.button16.Location = new System.Drawing.Point(226, 365);
            this.button16.Name = "button16";
            this.button16.Size = new System.Drawing.Size(164, 30);
            this.button16.TabIndex = 39;
            this.button16.Text = "Banı Aç !";
            this.button16.UseVisualStyleBackColor = false;
            this.button16.Click += new System.EventHandler(this.button16_Click);
            // 
            // groupBox6
            // 
            this.groupBox6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
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
            this.groupBox6.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox6.ForeColor = System.Drawing.Color.Cyan;
            this.groupBox6.Location = new System.Drawing.Point(13, 276);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(192, 279);
            this.groupBox6.TabIndex = 61;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Oyuncu Bilgileri";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.Color.Cyan;
            this.label13.Location = new System.Drawing.Point(13, 33);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(35, 15);
            this.label13.TabIndex = 44;
            this.label13.Text = "Nick";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label27.ForeColor = System.Drawing.Color.Cyan;
            this.label27.Location = new System.Drawing.Point(14, 253);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(77, 15);
            this.label27.TabIndex = 58;
            this.label27.Text = "Savaşma G.";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label24.ForeColor = System.Drawing.Color.Lime;
            this.label24.Location = new System.Drawing.Point(97, 252);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(77, 15);
            this.label24.TabIndex = 59;
            this.label24.Text = "Savaşma G.";
            // 
            // nickName
            // 
            this.nickName.AutoSize = true;
            this.nickName.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nickName.ForeColor = System.Drawing.Color.Lime;
            this.nickName.Location = new System.Drawing.Point(97, 33);
            this.nickName.Name = "nickName";
            this.nickName.Size = new System.Drawing.Size(35, 15);
            this.nickName.TabIndex = 40;
            this.nickName.Text = "Nick";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.Cyan;
            this.label12.Location = new System.Drawing.Point(14, 57);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(42, 15);
            this.label12.TabIndex = 45;
            this.label12.Text = "K.Adı";
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label31.ForeColor = System.Drawing.Color.Cyan;
            this.label31.Location = new System.Drawing.Point(14, 227);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(28, 15);
            this.label31.TabIndex = 57;
            this.label31.Text = "Can";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Lime;
            this.label4.Location = new System.Drawing.Point(97, 58);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(42, 15);
            this.label4.TabIndex = 41;
            this.label4.Text = "K.Adı";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label23.ForeColor = System.Drawing.Color.Lime;
            this.label23.Location = new System.Drawing.Point(97, 226);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(28, 15);
            this.label23.TabIndex = 56;
            this.label23.Text = "Can";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.Cyan;
            this.label11.Location = new System.Drawing.Point(14, 79);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(42, 15);
            this.label11.TabIndex = 46;
            this.label11.Text = "Level";
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label35.ForeColor = System.Drawing.Color.Cyan;
            this.label35.Location = new System.Drawing.Point(14, 200);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(35, 15);
            this.label35.TabIndex = 51;
            this.label35.Text = "Şans";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.ForeColor = System.Drawing.Color.Lime;
            this.label22.Location = new System.Drawing.Point(97, 200);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(35, 15);
            this.label22.TabIndex = 55;
            this.label22.Text = "Şans";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Lime;
            this.label7.Location = new System.Drawing.Point(97, 79);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(42, 15);
            this.label7.TabIndex = 42;
            this.label7.Text = "Level";
            this.label7.Click += new System.EventHandler(this.label7_Click);
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.ForeColor = System.Drawing.Color.Lime;
            this.label21.Location = new System.Drawing.Point(97, 175);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(70, 15);
            this.label21.TabIndex = 54;
            this.label21.Text = "Çeviklik ";
            // 
            // label39
            // 
            this.label39.AutoSize = true;
            this.label39.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label39.ForeColor = System.Drawing.Color.Cyan;
            this.label39.Location = new System.Drawing.Point(14, 173);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(70, 15);
            this.label39.TabIndex = 50;
            this.label39.Text = "Çeviklik ";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.Lime;
            this.label8.Location = new System.Drawing.Point(97, 101);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(42, 15);
            this.label8.TabIndex = 47;
            this.label8.Text = "Kupon";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.ForeColor = System.Drawing.Color.Lime;
            this.label20.Location = new System.Drawing.Point(97, 148);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(56, 15);
            this.label20.TabIndex = 53;
            this.label20.Text = "Savunma";
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label42.ForeColor = System.Drawing.Color.Cyan;
            this.label42.Location = new System.Drawing.Point(14, 101);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(42, 15);
            this.label42.TabIndex = 43;
            this.label42.Text = "Kupon";
            // 
            // label43
            // 
            this.label43.AutoSize = true;
            this.label43.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label43.ForeColor = System.Drawing.Color.Cyan;
            this.label43.Location = new System.Drawing.Point(14, 148);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(56, 15);
            this.label43.TabIndex = 49;
            this.label43.Text = "Savunma";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.ForeColor = System.Drawing.Color.Lime;
            this.label19.Location = new System.Drawing.Point(97, 123);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(56, 15);
            this.label19.TabIndex = 52;
            this.label19.Text = "Saldırı";
            // 
            // label45
            // 
            this.label45.AutoSize = true;
            this.label45.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label45.ForeColor = System.Drawing.Color.Cyan;
            this.label45.Location = new System.Drawing.Point(14, 123);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(56, 15);
            this.label45.TabIndex = 48;
            this.label45.Text = "Saldırı";
            // 
            // button17
            // 
            this.button17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.button17.FlatAppearance.BorderColor = System.Drawing.Color.Cyan;
            this.button17.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button17.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button17.ForeColor = System.Drawing.Color.Cyan;
            this.button17.Location = new System.Drawing.Point(13, 219);
            this.button17.Name = "button17";
            this.button17.Size = new System.Drawing.Size(152, 30);
            this.button17.TabIndex = 62;
            this.button17.Text = "Ram\'ı Temizle !";
            this.button17.UseVisualStyleBackColor = false;
            this.button17.Click += new System.EventHandler(this.button17_Click);
            // 
            // textBox7
            // 
            this.textBox7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.textBox7.ForeColor = System.Drawing.Color.Lime;
            this.textBox7.Location = new System.Drawing.Point(10, 104);
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new System.Drawing.Size(168, 26);
            this.textBox7.TabIndex = 64;
            // 
            // textBox8
            // 
            this.textBox8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.textBox8.ForeColor = System.Drawing.Color.Lime;
            this.textBox8.Location = new System.Drawing.Point(10, 51);
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new System.Drawing.Size(168, 26);
            this.textBox8.TabIndex = 65;
            // 
            // button19
            // 
            this.button19.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.button19.FlatAppearance.BorderColor = System.Drawing.Color.Cyan;
            this.button19.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button19.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button19.ForeColor = System.Drawing.Color.Cyan;
            this.button19.Location = new System.Drawing.Point(10, 137);
            this.button19.Name = "button19";
            this.button19.Size = new System.Drawing.Size(166, 32);
            this.button19.TabIndex = 66;
            this.button19.Text = "Değiştir ";
            this.button19.UseVisualStyleBackColor = false;
            this.button19.Click += new System.EventHandler(this.button19_Click);
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.label15);
            this.groupBox7.Controls.Add(this.label14);
            this.groupBox7.Controls.Add(this.textBox7);
            this.groupBox7.Controls.Add(this.button19);
            this.groupBox7.Controls.Add(this.textBox8);
            this.groupBox7.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox7.ForeColor = System.Drawing.Color.Cyan;
            this.groupBox7.Location = new System.Drawing.Point(652, 256);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(230, 183);
            this.groupBox7.TabIndex = 19;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Oyuncu Nick Değiştirme";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.ForeColor = System.Drawing.Color.Red;
            this.label15.Location = new System.Drawing.Point(6, 81);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(108, 19);
            this.label15.TabIndex = 68;
            this.label15.Text = "Yeni Nick :";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.ForeColor = System.Drawing.Color.Red;
            this.label14.Location = new System.Drawing.Point(12, 31);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(63, 19);
            this.label14.TabIndex = 67;
            this.label14.Text = "Nick :";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.ForeColor = System.Drawing.Color.Cyan;
            this.label17.Location = new System.Drawing.Point(9, 15);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(135, 19);
            this.label17.TabIndex = 71;
            this.label17.Text = "Online Sayısı:";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.ForeColor = System.Drawing.Color.Cyan;
            this.label18.Location = new System.Drawing.Point(9, 40);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(135, 19);
            this.label18.TabIndex = 72;
            this.label18.Text = "Ram Kullanımı:";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.BackColor = System.Drawing.Color.Black;
            this.label25.Font = new System.Drawing.Font("Consolas", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label25.ForeColor = System.Drawing.Color.Lime;
            this.label25.Location = new System.Drawing.Point(140, 38);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(30, 22);
            this.label25.TabIndex = 73;
            this.label25.Text = "00";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.Cyan;
            this.label10.Location = new System.Drawing.Point(231, 402);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(54, 19);
            this.label10.TabIndex = 74;
            this.label10.Text = "Nick:";
            // 
            // groupBox5
            // 
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
            this.groupBox5.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox5.ForeColor = System.Drawing.Color.Cyan;
            this.groupBox5.Location = new System.Drawing.Point(652, 25);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(230, 226);
            this.groupBox5.TabIndex = 28;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Online İtem Etkinliği";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.textBox11);
            this.groupBox4.Controls.Add(this.button4);
            this.groupBox4.Controls.Add(this.comboBox1);
            this.groupBox4.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.ForeColor = System.Drawing.Color.Cyan;
            this.groupBox4.Location = new System.Drawing.Point(175, 126);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(215, 144);
            this.groupBox4.TabIndex = 27;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Online Etkinliği";
            // 
            // textBox11
            // 
            this.textBox11.AccessibleName = "";
            this.textBox11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.textBox11.ForeColor = System.Drawing.Color.Lime;
            this.textBox11.Location = new System.Drawing.Point(6, 62);
            this.textBox11.Name = "textBox11";
            this.textBox11.Size = new System.Drawing.Size(203, 26);
            this.textBox11.TabIndex = 34;
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.button4.FlatAppearance.BorderColor = System.Drawing.Color.Cyan;
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button4.ForeColor = System.Drawing.Color.Cyan;
            this.button4.Location = new System.Drawing.Point(6, 93);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(203, 32);
            this.button4.TabIndex = 27;
            this.button4.Text = "Gönder";
            this.button4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.button4_Click_1);
            // 
            // comboBox1
            // 
            this.comboBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.comboBox1.ForeColor = System.Drawing.Color.Lime;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Kupon",
            "Exp",
            "Onur",
            "Kart Ruhu",
            "Bağlı Kupon",
            "Mükafat"});
            this.comboBox1.Location = new System.Drawing.Point(6, 28);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(203, 27);
            this.comboBox1.TabIndex = 0;
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.label16);
            this.groupBox8.Controls.Add(this.textBox9);
            this.groupBox8.Controls.Add(this.label26);
            this.groupBox8.Controls.Add(this.button21);
            this.groupBox8.Controls.Add(this.textBox10);
            this.groupBox8.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox8.ForeColor = System.Drawing.Color.Cyan;
            this.groupBox8.Location = new System.Drawing.Point(397, 424);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(225, 187);
            this.groupBox8.TabIndex = 75;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Oyuncu İP Adresi Bulma";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.ForeColor = System.Drawing.Color.Cyan;
            this.label16.Location = new System.Drawing.Point(12, 78);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(108, 19);
            this.label16.TabIndex = 69;
            this.label16.Text = "İP Adresi :";
            // 
            // textBox9
            // 
            this.textBox9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.textBox9.ForeColor = System.Drawing.Color.Lime;
            this.textBox9.Location = new System.Drawing.Point(16, 104);
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new System.Drawing.Size(204, 26);
            this.textBox9.TabIndex = 68;
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.ForeColor = System.Drawing.Color.Cyan;
            this.label26.Location = new System.Drawing.Point(12, 26);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(63, 19);
            this.label26.TabIndex = 67;
            this.label26.Text = "Nick :";
            // 
            // button21
            // 
            this.button21.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.button21.FlatAppearance.BorderColor = System.Drawing.Color.Cyan;
            this.button21.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button21.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button21.ForeColor = System.Drawing.Color.Cyan;
            this.button21.Location = new System.Drawing.Point(16, 136);
            this.button21.Name = "button21";
            this.button21.Size = new System.Drawing.Size(202, 32);
            this.button21.TabIndex = 66;
            this.button21.Text = "İP Adresini Göster";
            this.button21.UseVisualStyleBackColor = false;
            this.button21.Click += new System.EventHandler(this.button21_Click);
            // 
            // textBox10
            // 
            this.textBox10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.textBox10.ForeColor = System.Drawing.Color.Lime;
            this.textBox10.Location = new System.Drawing.Point(16, 49);
            this.textBox10.Name = "textBox10";
            this.textBox10.Size = new System.Drawing.Size(204, 26);
            this.textBox10.TabIndex = 65;
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.textBox12);
            this.groupBox9.Controls.Add(this.button5);
            this.groupBox9.Controls.Add(this.comboBox2);
            this.groupBox9.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox9.ForeColor = System.Drawing.Color.Cyan;
            this.groupBox9.Location = new System.Drawing.Point(400, 19);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(231, 155);
            this.groupBox9.TabIndex = 76;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "Yönetim Mesajı";
            // 
            // textBox12
            // 
            this.textBox12.AccessibleName = "";
            this.textBox12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.textBox12.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox12.ForeColor = System.Drawing.Color.Lime;
            this.textBox12.Location = new System.Drawing.Point(6, 62);
            this.textBox12.Multiline = true;
            this.textBox12.Name = "textBox12";
            this.textBox12.Size = new System.Drawing.Size(219, 82);
            this.textBox12.TabIndex = 34;
            this.textBox12.Text = "Mesaj Yaz...";
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.button5.FlatAppearance.BorderColor = System.Drawing.Color.Cyan;
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button5.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button5.ForeColor = System.Drawing.Color.Cyan;
            this.button5.Location = new System.Drawing.Point(134, 26);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(91, 32);
            this.button5.TabIndex = 27;
            this.button5.Text = "Gönder";
            this.button5.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.button5.UseVisualStyleBackColor = false;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // comboBox2
            // 
            this.comboBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.comboBox2.ForeColor = System.Drawing.Color.Lime;
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "Büyük Hoparlör",
            "Küçük Hoparlör",
            "Sistem Mesajı",
            "Sarı Mesaj",
            "Mor Mesaj",
            "Kırmızı Mesaj",
            "Admin Mesajı"});
            this.comboBox2.Location = new System.Drawing.Point(6, 28);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(122, 27);
            this.comboBox2.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.comboBox3);
            this.groupBox1.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.Cyan;
            this.groupBox1.Location = new System.Drawing.Point(175, 15);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(215, 105);
            this.groupBox1.TabIndex = 77;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Verileri Güncelle";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.button1.FlatAppearance.BorderColor = System.Drawing.Color.Cyan;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.Cyan;
            this.button1.Location = new System.Drawing.Point(6, 61);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(203, 32);
            this.button1.TabIndex = 27;
            this.button1.Text = "Güncelle";
            this.button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // comboBox3
            // 
            this.comboBox3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.comboBox3.ForeColor = System.Drawing.Color.Lime;
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Items.AddRange(new object[] {
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
            "Mapları Güncelle"});
            this.comboBox3.Location = new System.Drawing.Point(6, 28);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(203, 27);
            this.comboBox3.TabIndex = 0;
            // 
            // dataGridView1
            // 
            this.dataGridView1.BackgroundColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Cyan;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Lime;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridView1.GridColor = System.Drawing.Color.Cyan;
            this.dataGridView1.Location = new System.Drawing.Point(397, 228);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.Size = new System.Drawing.Size(249, 164);
            this.dataGridView1.TabIndex = 78;
            // 
            // comboBox4
            // 
            this.comboBox4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.comboBox4.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox4.ForeColor = System.Drawing.Color.Lime;
            this.comboBox4.FormattingEnabled = true;
            this.comboBox4.Items.AddRange(new object[] {
            "Banlı Hesaplar"});
            this.comboBox4.Location = new System.Drawing.Point(400, 193);
            this.comboBox4.Name = "comboBox4";
            this.comboBox4.Size = new System.Drawing.Size(117, 23);
            this.comboBox4.TabIndex = 79;
            this.comboBox4.Text = "Banlı Hesaplar";
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.button2.FlatAppearance.BorderColor = System.Drawing.Color.Cyan;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.ForeColor = System.Drawing.Color.Cyan;
            this.button2.Location = new System.Drawing.Point(523, 191);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(109, 25);
            this.button2.TabIndex = 80;
            this.button2.Text = "Göster";
            this.button2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBox13);
            this.groupBox2.Controls.Add(this.button3);
            this.groupBox2.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.ForeColor = System.Drawing.Color.Cyan;
            this.groupBox2.Location = new System.Drawing.Point(226, 458);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(165, 97);
            this.groupBox2.TabIndex = 81;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Şifre Değiştir";
            // 
            // textBox13
            // 
            this.textBox13.AccessibleName = "";
            this.textBox13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.textBox13.ForeColor = System.Drawing.Color.Lime;
            this.textBox13.Location = new System.Drawing.Point(9, 29);
            this.textBox13.Name = "textBox13";
            this.textBox13.Size = new System.Drawing.Size(151, 26);
            this.textBox13.TabIndex = 35;
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.button3.FlatAppearance.BorderColor = System.Drawing.Color.Cyan;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.ForeColor = System.Drawing.Color.Cyan;
            this.button3.Location = new System.Drawing.Point(10, 59);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(150, 32);
            this.button3.TabIndex = 27;
            this.button3.Text = "Değiştir";
            this.button3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Cyan;
            this.label1.Location = new System.Drawing.Point(648, 448);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 19);
            this.label1.TabIndex = 82;
            this.label1.Text = "DDoS Durum:";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label28.ForeColor = System.Drawing.Color.Cyan;
            this.label28.Location = new System.Drawing.Point(797, 636);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(0, 19);
            this.label28.TabIndex = 83;
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label29.ForeColor = System.Drawing.Color.Cyan;
            this.label29.Location = new System.Drawing.Point(648, 476);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(153, 19);
            this.label29.TabIndex = 84;
            this.label29.Text = "Bağlantı Sayısı:";
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label30.ForeColor = System.Drawing.Color.Lime;
            this.label30.Location = new System.Drawing.Point(791, 476);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(72, 19);
            this.label30.TabIndex = 85;
            this.label30.Text = "label30";
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label32.ForeColor = System.Drawing.Color.Cyan;
            this.label32.Location = new System.Drawing.Point(648, 503);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(153, 19);
            this.label32.TabIndex = 86;
            this.label32.Text = "Bloklama Sayısı:";
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label33.ForeColor = System.Drawing.Color.Lime;
            this.label33.Location = new System.Drawing.Point(791, 503);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(72, 19);
            this.label33.TabIndex = 87;
            this.label33.Text = "label33";
            // 
            // ServerManagementForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(897, 630);
            this.Controls.Add(this.label33);
            this.Controls.Add(this.label32);
            this.Controls.Add(this.label30);
            this.Controls.Add(this.label29);
            this.Controls.Add(this.label28);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.comboBox4);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox9);
            this.Controls.Add(this.groupBox8);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label25);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.button17);
            this.Controls.Add(this.button9);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.button15);
            this.Controls.Add(this.button10);
            this.Controls.Add(this.button16);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.onlineTxt);
            this.Controls.Add(this.lixtBox1);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox3);
            this.ForeColor = System.Drawing.Color.Cyan;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "ServerManagementForm";
            this.ShowIcon = false;
            this.Text = "BomBomRia Yönetim";
            this.Load += new System.EventHandler(this.ServerManagementForm_Load);
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
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

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
