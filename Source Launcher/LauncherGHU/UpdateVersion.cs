using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using LauncherGHU.Properties;
using MaterialSkin.Controls;

namespace LauncherGHU
{
	public class UpdateVersion : MaterialForm
	{
		public static XmlNodeList childNodes;

		public static string path;

		public static string version;

		public bool downloading;

		private static int int_1;

		private IContainer icontainer_0 = null;

		private Label txtTotalFile;

		private Label label2;

		private Label label1;

		private ProgressBar progressBarItem;

		private ProgressBar progressBarTotal;

		private Label lbRateItem;

		private Label lbRateTotal;

		private Timer timer_0;

		private Timer timer_1;

		private Label label3;

		internal static UpdateVersion U3kS2RUtD8JYsI0stJB;

		public UpdateVersion()
		{
			InitializeComponent();
		}

		private void UpdateVersion_Load(object sender, EventArgs e)
		{
			method_3();
			label3.Text = "Lưu ý : Nếu Folder Resource không cùng Folder Launcher , \nsau khi tải xong bạn phải copy hết File đã tải vào Thư mục Resource";
		}

		private void method_3()
		{
			WebClient webClient = new WebClient();
			byte[] bytes = webClient.DownloadData(ApplicationConfig.UrlApi + "versioninfo.xml");
			webClient.Dispose();
			string @string = Encoding.UTF8.GetString(bytes);
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(@string);
			childNodes = xmlDocument.DocumentElement.ChildNodes;
			version = xmlDocument.DocumentElement.Attributes.Item(2).InnerText;
			(xmlDocument as IDisposable)?.Dispose();
			progressBarTotal.Maximum = childNodes.Count;
			progressBarTotal.Value = 0;
			progressBarItem.Maximum = 100;
			progressBarItem.Value = 0;
			txtTotalFile.Text = progressBarTotal.Value + " / " + progressBarTotal.Maximum;
			txtTotalFile.Refresh();
			timer_1.Start();
		}

		private void method_4()
		{
			if (downloading)
			{
				return;
			}
			if (int_1 >= childNodes.Count)
			{
				timer_1.Stop();
				Settings.Default.versionres = version;
				Settings.Default.Save();
				Process.Start("rundll32.exe", "InetCpl.cpl,ClearMyTracksByProcess 8");
				Process.Start("rundll32.exe", "InetCpl.cpl,ClearMyTracksByProcess 16");
				MessageBox.Show("Đã cập nhật Resource thành công !.");
				PlayGameFrmFIB.IsUpdate = true;
				Close();
				return;
			}
			downloading = true;
			string innerText = childNodes.Item(int_1).Attributes.Item(0).InnerText;
			path = Path.Combine(childNodes.Item(int_1).Attributes.Item(1).InnerText);
			string directoryName = Path.GetDirectoryName(path);
			if (directoryName.Length > 0 && !Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			int_1++;
			progressBarItem.Value = 0;
			lbRateItem.Text = $"{progressBarItem.Value * 100 / progressBarItem.Maximum}%";
			label2.Text = "Đang tải File : " + path;
			WebClient webClient = new WebClient();
			webClient.DownloadFileCompleted += method_5;
			webClient.DownloadProgressChanged += method_6;
			webClient.DownloadFileAsync(new Uri(innerText), path);
		}

		private void method_5(object sender, AsyncCompletedEventArgs e)
		{
			if (e.Error == null)
			{
				progressBarTotal.Value++;
				txtTotalFile.Text = progressBarTotal.Value + " / " + progressBarTotal.Maximum;
				txtTotalFile.Refresh();
				lbRateTotal.Text = $"{progressBarTotal.Value * 100 / progressBarTotal.Maximum}%";
			}
			else
			{
				progressBarTotal.Maximum--;
				txtTotalFile.Text = progressBarTotal.Value + " / " + progressBarTotal.Maximum;
				txtTotalFile.Refresh();
				if (File.Exists(path))
				{
					File.Delete(path);
				}
			}
			downloading = false;
		}

		private void method_6(object sender, DownloadProgressChangedEventArgs e)
		{
			progressBarItem.Value = e.ProgressPercentage;
			lbRateItem.Text = $"{progressBarItem.Value * 100 / progressBarItem.Maximum}%";
		}

		private void timer_1_Tick(object sender, EventArgs e)
		{
			method_4();
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
			icontainer_0 = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LauncherGHU.UpdateVersion));
			txtTotalFile = new System.Windows.Forms.Label();
			label2 = new System.Windows.Forms.Label();
			label1 = new System.Windows.Forms.Label();
			progressBarItem = new System.Windows.Forms.ProgressBar();
			progressBarTotal = new System.Windows.Forms.ProgressBar();
			lbRateItem = new System.Windows.Forms.Label();
			lbRateTotal = new System.Windows.Forms.Label();
			timer_1 = new System.Windows.Forms.Timer(icontainer_0);
			label3 = new System.Windows.Forms.Label();
			SuspendLayout();
			txtTotalFile.AutoSize = true;
			txtTotalFile.BackColor = System.Drawing.Color.Transparent;
			txtTotalFile.Location = new System.Drawing.Point(172, 106);
			txtTotalFile.Name = "txtTotalFile";
			txtTotalFile.Size = new System.Drawing.Size(30, 13);
			txtTotalFile.TabIndex = 18;
			txtTotalFile.Text = "0 / 0";
			label2.AutoSize = true;
			label2.BackColor = System.Drawing.Color.Transparent;
			label2.Location = new System.Drawing.Point(27, 156);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(72, 13);
			label2.TabIndex = 17;
			label2.Text = "Đang tải file...";
			label1.AutoSize = true;
			label1.BackColor = System.Drawing.Color.Transparent;
			label1.Location = new System.Drawing.Point(27, 106);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(138, 13);
			label1.TabIndex = 16;
			label1.Text = "Số file đã tải / Tổng số file :";
			progressBarItem.Location = new System.Drawing.Point(13, 134);
			progressBarItem.Name = "progressBarItem";
			progressBarItem.Size = new System.Drawing.Size(370, 16);
			progressBarItem.TabIndex = 15;
			progressBarTotal.Location = new System.Drawing.Point(13, 80);
			progressBarTotal.Name = "progressBarTotal";
			progressBarTotal.Size = new System.Drawing.Size(370, 19);
			progressBarTotal.TabIndex = 14;
			lbRateItem.AutoSize = true;
			lbRateItem.BackColor = System.Drawing.Color.Transparent;
			lbRateItem.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			lbRateItem.ForeColor = System.Drawing.Color.Red;
			lbRateItem.Location = new System.Drawing.Point(386, 134);
			lbRateItem.Name = "lbRateItem";
			lbRateItem.Size = new System.Drawing.Size(23, 13);
			lbRateItem.TabIndex = 20;
			lbRateItem.Text = "0%";
			lbRateTotal.AutoSize = true;
			lbRateTotal.BackColor = System.Drawing.Color.Transparent;
			lbRateTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			lbRateTotal.ForeColor = System.Drawing.Color.Red;
			lbRateTotal.Location = new System.Drawing.Point(386, 82);
			lbRateTotal.Name = "lbRateTotal";
			lbRateTotal.Size = new System.Drawing.Size(23, 13);
			lbRateTotal.TabIndex = 19;
			lbRateTotal.Text = "0%";
			timer_1.Interval = 250;
			timer_1.Tick += new System.EventHandler(timer_1_Tick);
			label3.AutoSize = true;
			label3.BackColor = System.Drawing.Color.Transparent;
			label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8f, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
			label3.ForeColor = System.Drawing.Color.Red;
			label3.Location = new System.Drawing.Point(15, 183);
			label3.Name = "label3";
			label3.Size = new System.Drawing.Size(58, 13);
			label3.TabIndex = 21;
			label3.Text = "Lưu ý !!!!";
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			base.ClientSize = new System.Drawing.Size(430, 210);
			base.Controls.Add(label3);
			base.Controls.Add(txtTotalFile);
			base.Controls.Add(label2);
			base.Controls.Add(label1);
			base.Controls.Add(progressBarItem);
			base.Controls.Add(progressBarTotal);
			base.Controls.Add(lbRateItem);
			base.Controls.Add(lbRateTotal);
			base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
			base.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			base.MaximizeBox = false;
			base.Name = "UpdateVersion";
			Text = "Cập nhật Resource";
			base.Load += new System.EventHandler(UpdateVersion_Load);
			ResumeLayout(false);
			PerformLayout();
		}

		internal static bool LxO6JyUD7Mg4C1t1P6o()
		{
			return U3kS2RUtD8JYsI0stJB == null;
		}
	}
}
