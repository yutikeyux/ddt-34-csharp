using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Windows.Forms;
using Bussiness;
using Bussiness.Managers;
using Game.Base;
using Game.Base.Packets;
using Game.Logic;
using Game.Server;
using Game.Server.Managers;
using Game.Server.Rooms;
using SqlDataProvider.Data;

namespace Game.Service.actions
{
    // Token: 0x0200000B RID: 11
    public partial class ServerManagementForm : Form
    {
        // Token: 0x1700000E RID: 14
        // (get) Token: 0x0600003D RID: 61 RVA: 0x0000225D File Offset: 0x0000045D
        // (set) Token: 0x0600003E RID: 62 RVA: 0x00002265 File Offset: 0x00000465
        public string nickname { get; set; }

        // Token: 0x1700000F RID: 15
        // (get) Token: 0x0600003F RID: 63 RVA: 0x0000226E File Offset: 0x0000046E
        // (set) Token: 0x06000040 RID: 64 RVA: 0x00002276 File Offset: 0x00000476
        public GamePlayer player { get; set; }

        // Token: 0x17000010 RID: 16
        // (get) Token: 0x06000041 RID: 65 RVA: 0x0000227F File Offset: 0x0000047F
        // (set) Token: 0x06000042 RID: 66 RVA: 0x00002287 File Offset: 0x00000487
        public string NULL { get; set; }

        // Token: 0x06000043 RID: 67 RVA: 0x000043F8 File Offset: 0x000025F8
        public ServerManagementForm()
        {
            this.InitializeComponent();
        }

        // Token: 0x06000044 RID: 68 RVA: 0x00004450 File Offset: 0x00002650
        public void updateState()
        {
            try
            {
                GamePlayer[] allPlayers = WorldMgr.GetAllPlayers();
                GamePlayer[] array = allPlayers;
                List<BaseRoom> allUsingRoom = RoomMgr.GetAllUsingRoom();
                int num = (allPlayers != null) ? allPlayers.Length : 0;
                double num2 = (double)GC.GetTotalMemory(false);
                int num3 = 0;
                int num4 = 0;
                foreach (BaseRoom item in allUsingRoom)
                {
                    bool flag = !item.IsEmpty;
                    if (flag)
                    {
                        num3++;
                        bool isPlaying = item.IsPlaying;
                        if (isPlaying)
                        {
                            num4++;
                        }
                    }
                }
                this.onlineTxt.Text = num.ToString();
                this.label25.Text = Math.Round(num2 / 1024.0 / 1024.0).ToString() + "MB";
                this.lixtBox1.Items.Clear();
                for (int i = 0; i < array.Length; i++)
                {
                    GamePlayer gamePlayer = allPlayers[i];
                   // Console.WriteLine(string.Concat(new string[]
                  //  {
                  //  gamePlayer.PlayerCharacter.UserName,
                  //      "- [",
                  //     gamePlayer.PlayerCharacter.NickName,
                  //     "] seviye: ",
                 //      gamePlayer.PlayerCharacter.Grade.ToString()
               //     }));
                    this.lixtBox1.Items.Add(gamePlayer.PlayerCharacter.NickName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
            }
        }

        // Token: 0x06000045 RID: 69 RVA: 0x00004628 File Offset: 0x00002828
        private void UpdateUI_Tick(object sender, EventArgs e)
        {
            try
            {
                this.updateState();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
            }
        }

        // Token: 0x06000046 RID: 70 RVA: 0x00004664 File Offset: 0x00002864
        private void reloadBtn_Click(object sender, EventArgs e)
        {
            try
            {
                ServerManagementForm.AllReload();
                MessageBox.Show("Tüm Veritabanı Güncellenmiştir.");
            }
            catch (Exception)
            {
                MessageBox.Show("Tüm Veritabanı Güncellenmiştir.");
                Console.WriteLine("Tüm Veritabanı Güncellenmiştir.");
                Console.WriteLine("Ula böle sistem kimsede yok 1 tuşla hepsini güncelleme :)");
            }
        }

        // Token: 0x06000047 RID: 71 RVA: 0x00002290 File Offset: 0x00000490
        private static void AllReload()
        {
            throw new NotImplementedException();
        }

        // Token: 0x06000048 RID: 72 RVA: 0x00002298 File Offset: 0x00000498
        private void PlayerList_Enter(object sender, EventArgs e)
        {
            Console.WriteLine("Click");
        }

        // Token: 0x06000049 RID: 73 RVA: 0x000046C0 File Offset: 0x000028C0
        private void button8_Click(object sender, EventArgs e)
        {
            int validDate = 0;
            int gold = 0;
            int money = 0;
            int strengthenLevel = 0;
            int attackCompose = 0;
            int defendCompose = 0;
            int agilityCompose = 0;
            int luckCompose = 0;
            int templateID = int.Parse(this.textBox3.Text.ToString());
            string title = this.textBox2.Text.ToString();
            string content = this.textBox4.Text.ToString();
            this.textBox5.Text.ToString();
            int count = int.Parse(this.textBox5.Text.ToString());
            bool isBinds = bool.Parse(this.textBox6.Text.ToString());
            GamePlayer[] allPlayers = WorldMgr.GetAllPlayers();
            foreach (GamePlayer gamePlayer in allPlayers)
            {
                PlayerInfo playerInfo = new PlayerInfo();
                playerInfo = gamePlayer.PlayerCharacter;
                new PlayerBussiness().SendMailAndItem(title, content, playerInfo.ID, templateID, count, validDate, gold, money, strengthenLevel, attackCompose, defendCompose, agilityCompose, luckCompose, isBinds);
                string msg = "[ Online Oyuncu Etkinliği Sistemi ] Hediye Ödüller Gönderilmiştir.";
                gamePlayer.SendMessage(msg);
            }
        }

        // Token: 0x0600004A RID: 74 RVA: 0x000047D0 File Offset: 0x000029D0
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.player = WorldMgr.GetClientByPlayerNickName(this.lixtBox1.GetItemText(this.lixtBox1.SelectedItem));
            this.nickName.Text = this.player.PlayerCharacter.NickName;
            this.label4.Text = this.player.PlayerCharacter.UserName;
            this.label7.Text = this.player.PlayerCharacter.Grade.ToString();
            this.label8.Text = this.player.PlayerCharacter.Money.ToString();
            this.label19.Text = this.player.PlayerCharacter.Attack.ToString();
            this.label20.Text = this.player.PlayerCharacter.Defence.ToString();
            this.label21.Text = this.player.PlayerCharacter.Luck.ToString();
            this.label22.Text = this.player.PlayerCharacter.Agility.ToString();
            this.label23.Text = this.player.PlayerCharacter.hp.ToString();
            this.label24.Text = this.player.PlayerCharacter.FightPower.ToString();
        }

        // Token: 0x0600004B RID: 75 RVA: 0x00004958 File Offset: 0x00002B58
        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                GamePlayer[] allPlayers = WorldMgr.GetAllPlayers();
                GamePlayer[] array = allPlayers;
                List<BaseRoom> allUsingRoom = RoomMgr.GetAllUsingRoom();
                bool flag = allPlayers != null;
                if (flag)
                {
                    int num = allPlayers.Length;
                }
                GC.GetTotalMemory(false);
                int num2 = 0;
                int num3 = 0;
                foreach (BaseRoom item in allUsingRoom)
                {
                    bool flag2 = !item.IsEmpty;
                    if (flag2)
                    {
                        num2++;
                        bool isPlaying = item.IsPlaying;
                        if (isPlaying)
                        {
                            num3++;
                        }
                    }
                }
                this.lixtBox1.Items.Clear();
                for (int i = 0; i < array.Length; i++)
                {
                    GamePlayer gamePlayer = allPlayers[i];
                    Console.WriteLine(string.Concat(new string[]
                    {
gamePlayer.PlayerCharacter.UserName,
" - [",
gamePlayer.PlayerCharacter.NickName,
"] seviye: ",
gamePlayer.PlayerCharacter.Grade.ToString()
                    }));
                    this.lixtBox1.Items.Add(gamePlayer.PlayerCharacter.NickName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
            }
        }

        // Token: 0x0600004C RID: 76 RVA: 0x00004AE4 File Offset: 0x00002CE4
        private void button10_Click(object sender, EventArgs e)
        {
            DateTime date = new DateTime(2050, 7, 2);
            bool flag = this.player == null;
            if (!flag)
            {
                using (ManageBussiness manageBussiness = new ManageBussiness())
                {
                    manageBussiness.ForbidPlayerByNickName(this.player.PlayerCharacter.NickName, date, false);
                    MessageBox.Show(this.player.PlayerCharacter.NickName + " Oyuncu Banlandı !");
                    GamePlayer[] allPlayers = WorldMgr.GetAllPlayers();
                    foreach (GamePlayer gamePlayer in allPlayers)
                    {
                        string msg = "Oyuncumuz <" + this.player.PlayerCharacter.NickName + "> oyun kurallarına aykırı gelirken yakaladık ve BANLADIK. Sizde böyle olmak istemiyorsanız kurallara uyunuz.";
                        gamePlayer.SendMessage(msg);
                    }
                }
            }
        }

        // Token: 0x0600004D RID: 77 RVA: 0x000022A6 File Offset: 0x000004A6
        private void button11_Click(object sender, EventArgs e)
        {
            Process.Start("http://88.209.248.52/ddt-quest-s1/activelist.ashx");
            MessageBox.Show("(Zaman Sınırlı) Güncellendi [Başarılı]");
        }

        // Token: 0x0600004E RID: 78 RVA: 0x000022BF File Offset: 0x000004BF
        private void button12_Click(object sender, EventArgs e)
        {
            Process.Start("http://88.209.248.52/ddt-quest-s1/CelebList/CreateAllCeleb.ashx");
            MessageBox.Show("(Onur Listesi) Güncellendi [Başarılı]");
        }

        // Token: 0x0600004F RID: 79 RVA: 0x000022D8 File Offset: 0x000004D8
        private void button13_Click(object sender, EventArgs e)
        {
            Process.Start("http://88.209.248.52/ddt-quest-s1/LoadPVEItems.ashx");
            MessageBox.Show("(Droplar) Güncellendi [Başarılı]");
        }

        // Token: 0x06000050 RID: 80 RVA: 0x000022F1 File Offset: 0x000004F1
        private void button14_Click(object sender, EventArgs e)
        {
            Process.Start("http://88.209.248.52/ddt-quest-s1/questlist.ashx");
            MessageBox.Show("(Görevler) Güncellendi [Başarılı]");
        }

        // Token: 0x06000051 RID: 81 RVA: 0x00004BC0 File Offset: 0x00002DC0
        private void button16_Click(object sender, EventArgs e)
        {
            DateTime date = new DateTime(2050, 7, 2);
            using (ManageBussiness manageBussiness = new ManageBussiness())
            {
                manageBussiness.ForbidPlayerByNickName(this.textBox1.Text, date, true);
            }
            GamePlayer[] allPlayers = WorldMgr.GetAllPlayers();
            int num = 0;
            bool flag = num < allPlayers.Length;
            if (flag)
            {
                GamePlayer gamePlayer = allPlayers[num];
                MessageBox.Show("Oyuncumuz Banı Açılmıştır ");
                string msg = "Oyuncumuz <" + this.textBox1.Text + "> Banı Açılmıştır ";
                gamePlayer.SendMessage(msg);
            }
        }

        // Token: 0x06000052 RID: 82 RVA: 0x00004C64 File Offset: 0x00002E64
        private void button10_Click_1(object sender, EventArgs e)
        {
            DateTime date = new DateTime(2050, 7, 2);
            bool flag = this.player == null;
            if (!flag)
            {
                using (ManageBussiness manageBussiness = new ManageBussiness())
                {
                    manageBussiness.ForbidPlayerByNickName(this.nickName.Text, date, false);
                    MessageBox.Show(this.player.PlayerCharacter.NickName + " Oyuncu Banlandı !");
                    GamePlayer[] allPlayers = WorldMgr.GetAllPlayers();
                    foreach (GamePlayer gamePlayer in allPlayers)
                    {
                        string msg = "Oyuncumuz <" + this.player.PlayerCharacter.NickName + "> oyun kurallarına aykırı gelirken yakaladık ve BANLADIK. Sizde böyle olmak istemiyorsanız kurallara uyunuz.";
                        gamePlayer.SendMessage(msg);
                    }
                }
            }
        }

        // Token: 0x06000053 RID: 83 RVA: 0x00004D3C File Offset: 0x00002F3C
        private void button15_Click(object sender, EventArgs e)
        {
            bool flag = this.player == null;
            if (!flag)
            {
                using (ManageBussiness manageBussiness = new ManageBussiness())
                {
                    manageBussiness.KitoffUserByNickName(this.nickName.Text, " Kick yedi");
                    MessageBox.Show(this.player.PlayerCharacter.NickName + " kicklendi !");
                    GamePlayer[] allPlayers = WorldMgr.GetAllPlayers();
                    for (int i = 0; i < allPlayers.Length; i++)
                    {
                        allPlayers[i].SendMessage(string.Format("Oyuncu [{0}] Oyunculara rahatsızlığından dolayı kick yemiştir", Array.Empty<object>()));
                    }
                }
            }
        }

        // Token: 0x06000054 RID: 84 RVA: 0x00004628 File Offset: 0x00002828
        private void button9_Click_1(object sender, EventArgs e)
        {
            try
            {
                this.updateState();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
            }
        }

        // Token: 0x06000055 RID: 85 RVA: 0x0000230A File Offset: 0x0000050A
        private void button17_Click(object sender, EventArgs e)
        {
            GC.Collect();
            MessageBox.Show("Ram temizlendi !");
            this.updateState();
        }

        // Token: 0x06000056 RID: 86 RVA: 0x00002325 File Offset: 0x00000525
        private void button18_Click(object sender, EventArgs e)
        {
            Process.Start("http://88.209.248.52/ddt-quest-s1/NPCInfoList.ashx");
            MessageBox.Show("(NPC'Ler) Güncellendi [Başarılı]");
        }

        // Token: 0x06000057 RID: 87 RVA: 0x00004DE8 File Offset: 0x00002FE8
        private void button19_Click(object sender, EventArgs e)
        {
            bool keepRunning = GameServer.KeepRunning;
            if (keepRunning)
            {
                string text = this.textBox8.Text;
                string text2 = this.textBox7.Text;
                SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.AppSettings.Get("conString"));
                sqlConnection.Open();
                new SqlCommand(string.Concat(new string[]
                {
"update Sys_Users_Detail set NickName ='",
text2,
"' WHERE NickName=('",
text,
"')"
                }))
                {
                    Connection = sqlConnection
                }.ExecuteNonQuery();
                sqlConnection.Close();
                MessageBox.Show("Nick Değişme İşleminiz Gerçekleşmiştir !");
                this.textBox8.Text = null;
                this.textBox7.Text = null;
            }
            else
            {
                MessageBox.Show("Bilgileri Kontrol Ediniz ! ");
            }
        }

        // Token: 0x06000058 RID: 88 RVA: 0x00004EB8 File Offset: 0x000030B8
        private void button20_Click(object sender, EventArgs e)
        {
            bool flag = BallMgr.ReLoad();
            if (flag)
            {
                Console.WriteLine("Ball info Güncelleniyor !");
            }
            Console.WriteLine("Ball info Güncellendi !");
            bool flag2 = MapMgr.ReLoadMap();
            if (flag2)
            {
                Console.WriteLine("Map info Güncelleniyor !");
            }
            Console.WriteLine("Map info Güncellendi !");
            bool flag3 = MapMgr.ReLoadMapServer();
            if (flag3)
            {
                Console.WriteLine("mapserver info Güncelleniyor !");
            }
            Console.WriteLine("mapserver Güncellendi !");
            bool flag4 = PropItemMgr.Reload();
            if (flag4)
            {
                Console.WriteLine("prop info Güncelleniyor !");
            }
            Console.WriteLine("prop info Güncellendi !");
            bool flag5 = ItemMgr.ReLoad();
            if (flag5)
            {
                Console.WriteLine("item info Güncelleniyor !");
            }
            Console.WriteLine("item info Güncellendi !");
            bool flag6 = ShopMgr.ReLoad();
            if (flag6)
            {
                Console.WriteLine("shop info Güncelleniyor !");
            }
            Console.WriteLine("shop info Güncellendi !");
            bool flag7 = QuestMgr.ReLoad();
            if (flag7)
            {
                Console.WriteLine("quest info Güncelleniyor !");
            }
            Console.WriteLine("quest info Güncellendi !");
            bool flag8 = FusionMgr.ReLoad();
            if (flag8)
            {
                Console.WriteLine("fusion info Güncelleniyor !");
            }
            Console.WriteLine("fusion info Güncellendi !");
            bool flag9 = ConsortiaMgr.ReLoad();
            if (flag9)
            {
                Console.WriteLine("consortiaMgr info Güncelleniyor !");
            }
            Console.WriteLine("consortiaMgr info Güncellendi !");
            bool flag10 = RateMgr.ReLoad();
            if (flag10)
            {
                Console.WriteLine("Rate Rate Güncelleniyor !");
            }
            Console.WriteLine("Rate Rate Güncellendi !");
            bool flag11 = NPCInfoMgr.ReLoad();
            if (flag11)
            {
                Console.WriteLine("NPCInfo Güncelleniyor !");
            }
            Console.WriteLine("NPCInfo Güncellendi !");
            bool flag12 = FightRateMgr.ReLoad();
            if (flag12)
            {
                Console.WriteLine("FightRateMgr Güncelleniyor !");
            }
            Console.WriteLine("FightRateMgr Güncellendi !");
            bool flag13 = AwardMgr.ReLoad();
            if (flag13)
            {
                Console.WriteLine("dailyaward Güncelleniyor !");
            }
            Console.WriteLine("dailyaward Güncellendi !");
            bool flag14 = LanguageMgr.Reload("");
            if (flag14)
            {
                Console.WriteLine("language Güncelleniyor !");
            }
            Console.WriteLine("language Güncellendi !");
            Console.WriteLine("Tek bir tıkla tüm veritabanı'nı güncelledin daha ne istiyorsun :)");
            MessageBox.Show("Veritabanı Güncellenmiştir.");
        }

        // Token: 0x06000059 RID: 89 RVA: 0x000050C4 File Offset: 0x000032C4
        private void button21_Click(object sender, EventArgs e)
        {
            string text = this.textBox10.Text;
            SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.AppSettings.Get("conString"));
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = sqlConnection;
            sqlCommand.CommandText = "SELECT ActiveIP FROM Sys_Users_Detail WHERE NickName='" + text + "'";
            sqlConnection.Open();
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            while (sqlDataReader.Read())
            {
                this.textBox9.Text = this.NULL;
                this.textBox9.Text = sqlDataReader["ActiveIP"].ToString();
            }
            sqlConnection.Close();
        }

        // Token: 0x0600005A RID: 90 RVA: 0x0000516C File Offset: 0x0000336C
        private void button4_Click_1(object sender, EventArgs e)
        {
            bool flag = !string.IsNullOrEmpty(this.comboBox1.Text);
            if (flag)
            {
                GamePlayer[] allPlayers = WorldMgr.GetAllPlayers();
                foreach (GamePlayer gamePlayer in allPlayers)
                {
                    bool flag2 = this.comboBox1.Text == "Kupon";
                    if (flag2)
                    {
                        gamePlayer.AddMoney(int.Parse(this.textBox11.Text));
                    }
                    else
                    {
                        bool flag3 = this.comboBox1.Text == "Exp";
                        if (flag3)
                        {
                            gamePlayer.AddGP(int.Parse(this.textBox11.Text));
                        }
                        else
                        {
                            bool flag4 = this.comboBox1.Text == "Onur";
                            if (flag4)
                            {
                                gamePlayer.AddHonor(int.Parse(this.textBox11.Text));
                            }
                            else
                            {
                                bool flag5 = this.comboBox1.Text == "Kart Ruhu";
                                if (!flag5)
                                {
                                    bool flag6 = this.comboBox1.Text == "Bağlı Kupon";
                                    if (flag6)
                                    {
                                        gamePlayer.AddGiftToken(int.Parse(this.textBox11.Text));
                                    }
                                    else
                                    {
                                        bool flag7 = this.comboBox1.Text == "Mükafat";
                                        if (flag7)
                                        {
                                            gamePlayer.AddOffer(int.Parse(this.textBox11.Text));
                                        }
                                    }
                                }
                            }
                        }
                    }
                    gamePlayer.SendMessage(string.Concat(new string[]
                    {
"Tüm Çevrimiçi Oyunculara [",
this.textBox11.Text,
"] [",
this.comboBox1.Text,
"] Gönderilmiştir ^_^"
                    }));
                }
            }
            else
            {
                MessageBox.Show("Lütfen Seçim Yapınız.!", "Uyarı");
            }
        }

        // Token: 0x0600005B RID: 91 RVA: 0x00005350 File Offset: 0x00003550
        private void button5_Click(object sender, EventArgs e)
        {
            bool flag = !string.IsNullOrEmpty(this.comboBox2.Text);
            if (flag)
            {
                bool flag2 = this.comboBox2.Text == "Küçük Hoparlör";
                if (flag2)
                {
                    GSPacketIn gSPacketIn = new GSPacketIn(71);
                    gSPacketIn.WriteInt(0);
                    gSPacketIn.WriteString("Sistem");
                    gSPacketIn.WriteString(this.textBox12.Text);
                    GameServer.Instance.LoginServer.SendPacket(gSPacketIn);
                    ServerManagementForm.Herkes(gSPacketIn);
                }
                else
                {
                    bool flag3 = this.comboBox2.Text == "Büyük Hoparlör";
                    if (flag3)
                    {
                        GSPacketIn gSPacketIn2 = new GSPacketIn(72);
                        gSPacketIn2.WriteInt(0);
                        gSPacketIn2.WriteInt(0);
                        gSPacketIn2.WriteString("Sistem");
                        gSPacketIn2.WriteString(this.textBox12.Text);
                        GameServer.Instance.LoginServer.SendPacket(gSPacketIn2);
                        ServerManagementForm.Herkes(gSPacketIn2);
                    }
                    else
                    {
                        bool flag4 = this.comboBox2.Text == "Sarı Mesaj";
                        if (flag4)
                        {
                            ServerManagementForm.Herkes_2(eMessageType.ChatNormal, this.textBox12.Text);
                        }
                        else
                        {
                            bool flag5 = this.comboBox2.Text == "Sistem Mesajı";
                            if (flag5)
                            {
                                ServerManagementForm.Herkes_2(eMessageType.ChatERROR, this.textBox12.Text);
                            }
                            else
                            {
                                bool flag6 = this.comboBox2.Text == "Kırmızı Mesaj";
                                if (flag6)
                                {
                                    GSPacketIn gSPacketIn3 = new GSPacketIn(73, 0);
                                    gSPacketIn3.WriteInt(1);
                                    gSPacketIn3.WriteInt(0);
                                    gSPacketIn3.WriteString("Sistem");
                                    gSPacketIn3.WriteString(this.textBox12.Text);
                                    gSPacketIn3.WriteString("Yönetim");
                                    GameServer.Instance.LoginServer.SendPacket(gSPacketIn3);
                                    ServerManagementForm.Herkes(gSPacketIn3);
                                }
                                else
                                {
                                    bool flag7 = this.comboBox2.Text == "Mor Mesaj";
                                    if (flag7)
                                    {
                                        new ManageBussiness().SystemNotice(this.textBox12.Text);
                                    }
                                    else
                                    {
                                        bool flag8 = this.comboBox2.Text == "Admin Mesajı";
                                        if (flag8)
                                        {
                                            ServerManagementForm.Herkes_2(eMessageType.ALERT, this.textBox12.Text);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Lütfen Seçim Yapınız.!", "Uyarı");
            }
        }

        // Token: 0x0600005C RID: 92 RVA: 0x000055B8 File Offset: 0x000037B8
        public static void Herkes(GSPacketIn Paket)
        {
            GamePlayer[] allPlayers = WorldMgr.GetAllPlayers();
            for (int i = 0; i < allPlayers.Length; i++)
            {
                allPlayers[i].SendTCP(Paket);
            }
        }

        // Token: 0x0600005D RID: 93 RVA: 0x000055EC File Offset: 0x000037EC
        public static void Herkes_2(eMessageType Msj, string Msj2)
        {
            GamePlayer[] allPlayers = WorldMgr.GetAllPlayers();
            for (int i = 0; i < allPlayers.Length; i++)
            {
                allPlayers[i].Out.SendMessage(Msj, Msj2);
            }
        }

        // Token: 0x0600005E RID: 94 RVA: 0x00005624 File Offset: 0x00003824
        private void güncel1(string güncelle)
        {
            try
            {
                string address = ServerManagementForm.link + güncelle;
                new WebClient().DownloadString(address);
                Console.WriteLine("Güncelleme Başarılı: " + DateTime.Now.ToString());
            }
            catch
            {
                Console.WriteLine("Güncelleme Başarısız.!");
            }
        }

        // Token: 0x0600005F RID: 95 RVA: 0x0000568C File Offset: 0x0000388C
        private void button1_Click(object sender, EventArgs e)
        {
            bool flag = !string.IsNullOrEmpty(this.comboBox3.Text);
            if (flag)
            {
                bool flag2 = this.comboBox3.Text == "Tüm Veritabanını Güncelle";
                if (flag2)
                {
                    bool flag3 = BallMgr.ReLoad();
                    if (flag3)
                    {
                        Console.WriteLine("Ball info Güncelleniyor !");
                        this.güncel1("Balllist.ashx");
                    }
                    Console.WriteLine("Ball info Güncellendi !");
                    bool flag4 = MapMgr.ReLoadMap();
                    if (flag4)
                    {
                        Console.WriteLine("Map info Güncelleniyor !");
                        this.güncel1("MapServerList.ashx");
                    }
                    Console.WriteLine("Map info Güncellendi !");
                    bool flag5 = MapMgr.ReLoadMapServer();
                    if (flag5)
                    {
                        Console.WriteLine("mapserver info Güncelleniyor !");
                        this.güncel1("MapServerList.ashx");
                    }
                    Console.WriteLine("mapserver Güncellendi !");
                    bool flag6 = PropItemMgr.Reload();
                    if (flag6)
                    {
                        Console.WriteLine("prop info Güncelleniyor !");
                    }
                    Console.WriteLine("prop info Güncellendi !");
                    bool flag7 = ItemMgr.ReLoad();
                    if (flag7)
                    {
                        Console.WriteLine("item info Güncelleniyor !");
                    }
                    Console.WriteLine("item info Güncellendi !");
                    bool flag8 = ShopMgr.ReLoad();
                    if (flag8)
                    {
                        Console.WriteLine("shop info Güncelleniyor !");
                        this.güncel1("ShopItemList.ashx");
                    }
                    Console.WriteLine("shop info Güncellendi !");
                    bool flag9 = QuestMgr.ReLoad();
                    if (flag9)
                    {
                        Console.WriteLine("quest info Güncelleniyor !");
                        this.güncel1("QuestList.ashx");
                    }
                    Console.WriteLine("quest info Güncellendi !");
                    bool flag10 = FusionMgr.ReLoad();
                    if (flag10)
                    {
                        Console.WriteLine("fusion info Güncelleniyor !");
                    }
                    Console.WriteLine("fusion info Güncellendi !");
                    bool flag11 = ConsortiaMgr.ReLoad();
                    if (flag11)
                    {
                        Console.WriteLine("consortiaMgr info Güncelleniyor !");
                        this.güncel1("ConsortiaAllyList.ashx");
                    }
                    Console.WriteLine("consortiaMgr info Güncellendi !");
                    bool flag12 = RateMgr.ReLoad();
                    if (flag12)
                    {
                        Console.WriteLine("Rate Rate Güncelleniyor !");
                    }
                    Console.WriteLine("Rate Rate Güncellendi !");
                    bool flag13 = NPCInfoMgr.ReLoad();
                    if (flag13)
                    {
                        Console.WriteLine("NPCInfo Güncelleniyor !");
                        this.güncel1("NPCInfoList.ashx");
                    }
                    Console.WriteLine("NPCInfo Güncellendi !");
                    bool flag14 = FightRateMgr.ReLoad();
                    if (flag14)
                    {
                        Console.WriteLine("FightRateMgr Güncelleniyor !");
                    }
                    Console.WriteLine("FightRateMgr Güncellendi !");
                    bool flag15 = AwardMgr.ReLoad();
                    if (flag15)
                    {
                        Console.WriteLine("dailyaward Güncelleniyor !");
                    }
                    Console.WriteLine("dailyaward Güncellendi !");
                    bool flag16 = LanguageMgr.Reload("");
                    if (flag16)
                    {
                        Console.WriteLine("language Güncelleniyor !");
                    }
                    Console.WriteLine("language Güncellendi !");
                    Console.WriteLine("Veritabanı Başarıyla Güncellendi !");
                    MessageBox.Show("Veritabanı Güncellenmiştir.");
                }
                else
                {
                    bool flag17 = this.comboBox3.Text == "Görev Güncelle";
                    if (flag17)
                    {
                        this.güncel1("QuestList.ashx");
                        MessageBox.Show("(Görevler) Güncellendi [Başarılı]");
                    }
                    else
                    {
                        bool flag18 = this.comboBox3.Text == "Onur Listesi Güncelle";
                        if (flag18)
                        {
                            this.güncel1("CelebList/CreateAllCeleb.ashx");
                            MessageBox.Show("(Onur Listesi) Güncellendi [Başarılı]");
                        }
                        else
                        {
                            bool flag19 = this.comboBox3.Text == "Etkinlikleri Güncelle";
                            if (flag19)
                            {
                                this.güncel1("ActiveList.ashx");
                                MessageBox.Show("(Zaman Sınırlı) Güncellendi [Başarılı]");
                            }
                            else
                            {
                                bool flag20 = this.comboBox3.Text == "Pve_İnfo Güncelle";
                                if (flag20)
                                {
                                    this.güncel1("LoadPVEItems.ashx");
                                    MessageBox.Show("(Droplar) Güncellendi [Başarılı]");
                                }
                                else
                                {
                                    bool flag21 = this.comboBox3.Text == "Templatelist Güncelle";
                                    if (flag21)
                                    {
                                        this.güncel1("TemplateAlllist.ashx");
                                        MessageBox.Show("Templist Güncellendi. [Başarılı]");
                                    }
                                    else
                                    {
                                        bool flag22 = this.comboBox3.Text == "Füzyon Güncelle";
                                        if (flag22)
                                        {
                                            bool flag23 = FusionMgr.ReLoad();
                                            if (flag23)
                                            {
                                                Console.WriteLine("fusion Güncelleniyor...");
                                            }
                                            Console.WriteLine("fusion Güncellendi.!");
                                            MessageBox.Show("Füzyon Güncellendi.");
                                        }
                                        else
                                        {
                                            bool flag24 = this.comboBox3.Text == "Shop Güncelle";
                                            if (flag24)
                                            {
                                                bool flag25 = ShopMgr.ReLoad();
                                                if (flag25)
                                                {
                                                    this.güncel1("ShopItemList.ashx");
                                                    Console.WriteLine("Shop Güncelleniyor...");
                                                }
                                                Console.WriteLine("Shop Güncellendi.!");
                                                MessageBox.Show("Shop Güncellendi.");
                                            }
                                            else
                                            {
                                                bool flag26 = this.comboBox3.Text == "Mission Güncelle";
                                                if (flag26)
                                                {
                                                    bool flag27 = MissionInfoMgr.Reload();
                                                    if (flag27)
                                                    {
                                                        Console.WriteLine("Şartlar Güncelleniyor...");
                                                    }
                                                    Console.WriteLine("Şartlar Güncellendi.!");
                                                    MessageBox.Show("Şartlar Güncellendi.");
                                                }
                                                else
                                                {
                                                    bool flag28 = this.comboBox3.Text == "Npc Güncelle";
                                                    if (flag28)
                                                    {
                                                        bool flag29 = NPCInfoMgr.ReLoad();
                                                        if (flag29)
                                                        {
                                                            this.güncel1("NPCInfoList.ashx");
                                                            Console.WriteLine("Keşifler Güncelleniyor !");
                                                        }
                                                        Console.WriteLine("Keşifler Güncellendi !");
                                                        MessageBox.Show("Keşifler Güncellenmiştir.");
                                                    }
                                                    else
                                                    {
                                                        bool flag30 = this.comboBox3.Text == "Ball Güncelle";
                                                        if (flag30)
                                                        {
                                                            bool flag31 = BallMgr.ReLoad();
                                                            if (flag31)
                                                            {
                                                                this.güncel1("Balllist.ashx");
                                                                Console.WriteLine("Ball Güncelleniyor...");
                                                            }
                                                            Console.WriteLine("Ball Güncellendi.!");
                                                            MessageBox.Show("Ball Güncellendi.");
                                                        }
                                                        else
                                                        {
                                                            bool flag32 = this.comboBox3.Text == "Ball Config Güncelle";
                                                            if (flag32)
                                                            {
                                                                bool flag33 = BallConfigMgr.ReLoad();
                                                                if (flag33)
                                                                {
                                                                    this.güncel1("BombConfig.ashx");
                                                                    Console.WriteLine("Ball Config Güncelleniyor...");
                                                                }
                                                                Console.WriteLine("Ball Config Güncellendi.!");
                                                                MessageBox.Show("Ball Config Güncellendi.");
                                                            }
                                                            else
                                                            {
                                                                bool flag34 = this.comboBox3.Text == "Yazıları Güncelle";
                                                                if (flag34)
                                                                {
                                                                    bool flag35 = LanguageMgr.Reload("");
                                                                    if (flag35)
                                                                    {
                                                                        Console.WriteLine("language Güncelleniyor...");
                                                                    }
                                                                    Console.WriteLine("language Güncellendi.!");
                                                                    MessageBox.Show("Yazılar Güncellendi.");
                                                                }
                                                                else
                                                                {
                                                                    bool flag36 = this.comboBox3.Text == "Goldları Güncelle";
                                                                    if (flag36)
                                                                    {
                                                                        bool flag37 = GoldEquipMgr.ReLoad();
                                                                        if (flag37)
                                                                        {
                                                                            this.güncel1("GoldEquipTemplateLoad.ashx");
                                                                            Console.WriteLine("Goldlar Güncelleniyor...");
                                                                        }
                                                                        Console.WriteLine("Goldlar Güncellendi.!");
                                                                        MessageBox.Show("Goldlar Güncellendi.");
                                                                    }
                                                                    else
                                                                    {
                                                                        bool flag38 = this.comboBox3.Text == "Mapları Güncelle";
                                                                        if (flag38)
                                                                        {
                                                                            bool flag39 = MapMgr.ReLoadMap();
                                                                            if (flag39)
                                                                            {
                                                                                Console.WriteLine("Map info Güncelleniyor !");
                                                                            }
                                                                            bool flag40 = MapMgr.ReLoadMapServer();
                                                                            if (flag40)
                                                                            {
                                                                                Console.WriteLine("mapserver info Güncelleniyor !");
                                                                            }
                                                                            this.güncel1("MapServerList.ashx");
                                                                            Console.WriteLine("Maplar Güncellendi !");
                                                                            MessageBox.Show("Maplar Başarıyla Güncellendi !", "Bilgi");
                                                                        }
                                                                        else
                                                                        {
                                                                            bool flag41 = this.comboBox3.Text == "Dropları Güncelle";
                                                                            if (flag41)
                                                                            {
                                                                                bool flag42 = DropMgr.ReLoad();
                                                                                if (flag42)
                                                                                {
                                                                                    this.güncel1("LoadPVEItems.ashx");
                                                                                    Console.WriteLine("Droplar Güncelleniyor !");
                                                                                }
                                                                                Console.WriteLine("Droplar Güncellendi !");
                                                                                MessageBox.Show("Droplar Başarıyla Güncellendi !", "Bilgi");
                                                                            }
                                                                            else
                                                                            {
                                                                                bool flag43 = this.comboBox3.Text == "EventAward Güncelle";
                                                                                if (flag43)
                                                                                {
                                                                                    bool flag44 = EventAwardMgr.ReLoad();
                                                                                    if (flag44)
                                                                                    {
                                                                                        Console.WriteLine("EventAward Güncelleniyor !");
                                                                                    }
                                                                                    Console.WriteLine("EventAward Güncellendi !");
                                                                                    MessageBox.Show("EventAward Başarıyla Güncellendi !", "Bilgi");
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Lütfen Seçim Yapınız.!", "Uyarı");
            }
        }

        // Token: 0x06000060 RID: 96 RVA: 0x00005E48 File Offset: 0x00004048
        private void button2_Click(object sender, EventArgs e)
        {
            this.dataGridView1.ForeColor = Color.Black;
            bool flag = this.comboBox4.Text == "Banlı Hesaplar";
            if (flag)
            {
                this.Baglanti_Db.Open();
                DbDataAdapter dbDataAdapter = new SqlDataAdapter("Select NickName[Nick], ForbidReason[Ban Sebebi] From Sys_Users_Detail Where IsExist='" + 0.ToString() + "'", this.Baglanti_Db);
                DataTable dataTable = new DataTable();
                dbDataAdapter.Fill(dataTable);
                this.dataGridView1.DataSource = dataTable;
                this.Baglanti_Db.Close();
            }
        }

        // Token: 0x06000061 RID: 97 RVA: 0x00005EDC File Offset: 0x000040DC
        private void button3_Click(object sender, EventArgs e)
        {
            SqlCommand sqlCommand = new SqlCommand("select UserId From Mem_Users Where UserName ='" + this.textBox13.Text + "'", this.Baglanti_Membership);
            this.Baglanti_Membership.Open();
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            while (sqlDataReader.Read())
            {
                string text = sqlDataReader["UserId"].ToString();
                this.Baglanti_Membership_2.Open();
                new SqlCommand("Update Mem_UserInfo Set Password ='e10adc3949ba59abbe56e057f20f883e' Where UserId='" + text + "'", this.Baglanti_Membership_2).ExecuteNonQuery();
                this.Baglanti_Membership_2.Close();
            }
            this.Baglanti_Membership.Close();
            MessageBox.Show("Kişinin Şifresi Başarıyla Değiştirilmiştir, Yeni Şifre: 123456", "Bilgi");
        }

        // Token: 0x06000062 RID: 98 RVA: 0x00005F98 File Offset: 0x00004198
        private void ServerManagementForm_Load(object sender, EventArgs e)
        {
            bool isActive = HydroFilter.IsActive;
            if (isActive)
            {
                this.label28.Text = "Aktif";
                this.label28.ForeColor = Color.Green;
            }
            else
            {
                this.label28.Text = "Pasif";
                this.label28.ForeColor = Color.Red;
            }
            this.label30.Text = HydroFilter.ConnectionCount.ToString();
            this.label33.Text = HydroFilter.BlockedConnections.ToString();
        }

        // Token: 0x0400001D RID: 29
        private SqlConnection Baglanti_Db = new SqlConnection(ConfigurationManager.AppSettings.Get("conString"));

        // Token: 0x0400001E RID: 30
        private SqlConnection Baglanti_Membership = new SqlConnection("Data Source=element-yuti/SA;Initial Catalog=Db_Membership;Persist Security Info=True;User ID=sa;Password=ElementYuti2026.123@");

        // Token: 0x0400001F RID: 31
        private SqlConnection Baglanti_Membership_2 = new SqlConnection("Data Source=element-yuti/SA;Initial Catalog=Db_Membership;Persist Security Info=True;User ID=sa;Password=ElementYuti2026.123@");

        // Token: 0x04000020 RID: 32
        private static string link = "http://88.209.248.52/ddt-quest-s1/";

        private void label7_Click(object sender, EventArgs e)
        {

        }
    }
}