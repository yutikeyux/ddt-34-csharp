using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using zlib;

namespace Game.Logic.Protocol
{
    public class FingerConfig
    {
        private static readonly byte[] byte_0;

        private static string string_0;

        protected static int m_version;

        protected static string m_nameClient;

        protected static string m_hex;

        protected static string url_check;

        protected static bool m_checkTime;

        protected static bool m_checkHw;

        protected static DateTime m_endDate;

        private static List<string> DisibleEventList;

        public static bool CheckDisibleEvent(string name)
        {
            return DisibleEventList.Contains(name);
        }

        public static bool Init()
        {
            string text = smethod_1();
            if (text == null)
            {
                return true;
            }
            smethod_14(string.Format("Hi {0}! {1}.", (m_nameClient == null) ? "Guy" : m_nameClient, text));
            Environment.Exit(0);
            return false;
        }

        private static string smethod_0()
        {
            if (string.IsNullOrEmpty(string_0))
            {
                string_0 = smethod_5(smethod_9() + smethod_10() + smethod_12() + smethod_13());
            }
            return string_0;
        }

        private static string smethod_1()
        {
            try
            {
                if (PushValues(File.ReadAllBytes("ddt.cer")) != null)
                {
                    string text = "Your license is not valid or expired";
                    if (m_checkHw)
                    {
                        string text2 = smethod_0();
                        if (text2 == null || text2.Length <= 0 || m_hex == null || m_hex.Length <= 0 || !text2.Contains(m_hex))
                        {
                            return "Server can't receive your HWID or HWID is not vaild with your license!";
                        }
                        text = null;
                    }
                    if (m_checkTime)
                    {
                        DateTime dateTime = smethod_4();
                        if (!(m_endDate > DateTime.MinValue) || !(dateTime != DateTime.MinValue) || !(m_endDate > dateTime))
                        {
                            return string.Format("Your license is expired in {0}. Current time is: ", m_endDate.ToString(), dateTime);
                        }
                        text = null;
                    }
                    if (!AllowFromServer())
                    {
                        return $"Check your internet or license. Contact seller for help.";
                    }
                    return null;
                }
            }
            catch (Exception value)
            {
                Console.WriteLine(value);
            }
            return "Server is bug or error in check key. Please contact the seller.";
        }

        private static bool smethod_2(string string_1)
        {
            if (Uri.TryCreate(string_1, UriKind.Absolute, out Uri result))
            {
                return result.Scheme == Uri.UriSchemeHttp;
            }
            return false;
        }

        private static bool smethod_3(string string_1)
        {
            if (string_1.IndexOf("https://") != -1 && string_1.IndexOf("../") == -1 && smethod_2(string_1))
            {
                try
                {
                    using (HttpWebResponse httpWebResponse = (HttpWebResponse)((HttpWebRequest)WebRequest.Create(string_1)).GetResponse())
                    {
                        if (httpWebResponse.StatusCode == HttpStatusCode.OK)
                        {
                            return true;
                        }
                    }
                }
                catch (WebException)
                {
                    return false;
                }
                return false;
            }
            return false;
        }

        private static DateTime smethod_4()
        {
            try
            {
                string text = "https://www.google.com";
                if (!smethod_3(text))
                {
                    text = "https://www.google.com.vn";
                }
                return DateTime.ParseExact(((HttpWebRequest)WebRequest.Create(text)).GetResponse().Headers["date"], "ddd, dd MMM yyyy HH:mm:ss 'GMT'", CultureInfo.InvariantCulture.DateTimeFormat, DateTimeStyles.AssumeUniversal);
            }
            catch (Exception)
            {
                return DateTime.MinValue;
            }
        }

        protected static string GetResultURL(string url)
        {
            string result = "";
            try
            {
                using (new WebClient())
                {
                    result = new WebClient().DownloadString(url).Trim();
                    return result;
                }
            }
            catch (Exception)
            {
                return result;
            }
        }

        protected static bool AllowFromServer()
        {
            string hostName = Dns.GetHostName();
            string text = md5(m_nameClient + smethod_0() + hostName + "snapevn34gsrg24dherh");
            if (GetResultURL(string.Format("{0}?name={4}&hwid={1}&ip={2}&key={3}", "http://127.0.0.1/lic/check.php", smethod_0(), hostName, text, m_nameClient)).Trim() == text)
            {
                return true;
            }
            return false;
        }

        protected static byte[] PushValues(byte[] bit)
        {
            try
            {
                List<byte> list = new List<byte>();
                int num = 0;
                foreach (byte b in bit)
                {
                    if (num >= byte_0.Length)
                    {
                        num = 0;
                    }
                    list.Add((byte)(b ^ byte_0[num]));
                    num++;
                }
                if (((list[0] << 24) | (list[1] << 16) | (list[2] << 8) | list[3]) == 36582154 && (double)(int)list[4] < 213.296259615)
                {
                    List<byte> list2 = new List<byte>();
                    int num2 = byte_0.Length - 1;
                    for (int j = 5; j < list.Count; j++)
                    {
                        if (num2 < 0)
                        {
                            num2 = byte_0.Length - 1;
                        }
                        list2.Add((byte)(list[j] ^ byte_0[num2]));
                        num2--;
                    }
                    MemoryStream memoryStream = new MemoryStream();
                    ZOutputStream val = new ZOutputStream((Stream)memoryStream);
                    ((Stream)val).Write(list2.ToArray(), 0, list2.Count);
                    ((Stream)val).Close();
                    list2 = memoryStream.ToArray().ToList();
                    if (((list2[0] << 24) | (list2[1] << 16) | (list2[2] << 8) | list2[3]) == list2.Count && Encoding.UTF8.GetString(list2.ToArray(), 6, (list2[4] << 8) | list2[5]).Contains("snapetrminhpc_canpass"))
                    {
                        int num3 = 6 + ((list2[4] << 8) | list2[5]);
                        m_nameClient = Encoding.UTF8.GetString(list2.ToArray(), num3 + 2, (list2[num3] << 8) | list2[num3 + 1]);
                        num3 += 2 + ((list2[num3] << 8) | list2[num3 + 1]);
                        m_version = ((list2[num3] << 24) | (list2[num3 + 1] << 16) | (list2[num3 + 2] << 8) | list2[num3 + 3]);
                        m_checkTime = ((list2[num3 + 4] != 0) ? true : false);
                        m_checkHw = ((list2[num3 + 5] != 0) ? true : false);
                        m_endDate = new DateTime((list2[num3 + 6] << 8) | list2[num3 + 7], list2[num3 + 8], list2[num3 + 9], list2[num3 + 10], list2[num3 + 11], list2[num3 + 12]);
                        m_hex = Encoding.UTF8.GetString(list2.ToArray(), num3 + 15, (list2[num3 + 13] << 8) | list2[num3 + 14]);
                        num3 += 15 + ((list2[num3 + 13] << 8) | list2[num3 + 14]);
                        url_check = Encoding.UTF8.GetString(list2.ToArray(), num3 + 2, (list2[num3] << 8) | list2[num3 + 1]);
                        num3 += 2 + ((list2[num3] << 8) | list2[num3 + 1]);
                        int num4 = (list2[num3] << 24) | (list2[num3 + 1] << 16) | (list2[num3 + 2] << 8) | list2[num3 + 3];
                        num3 += 4;
                        for (int k = 0; k < num4; k++)
                        {
                            DisibleEventList.Add(Encoding.UTF8.GetString(list2.ToArray(), num3 + 2, (list2[num3] << 8) | list2[num3 + 1]));
                            num3 += 2 + ((list2[num3] << 8) | list2[num3 + 1]);
                        }
                        return list2.ToArray();
                    }
                }
            }
            catch (Exception value)
            {
                Console.WriteLine(value);
            }
            return null;
        }

        protected static byte[] encryptData(string data)
        {
            MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
            UTF8Encoding uTF8Encoding = new UTF8Encoding();
            return mD5CryptoServiceProvider.ComputeHash(uTF8Encoding.GetBytes(data));
        }

        protected static string md5(string data)
        {
            return BitConverter.ToString(encryptData(data)).Replace("-", "").ToLower();
        }

        private static string smethod_5(string string_1)
        {
            MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
            byte[] bytes = new ASCIIEncoding().GetBytes(string_1);
            return smethod_6(mD5CryptoServiceProvider.ComputeHash(bytes));
        }

        private static string smethod_6(object object_0)
        {
            string text = string.Empty;
            for (int i = 0; i < ((Array)object_0).Length; i++)
            {
                byte num = ((byte[])object_0)[i];
                int num2 = num & 0xF;
                int num3 = (num >> 4) & 0xF;
                text = ((num3 <= 9) ? (text + num3.ToString()) : (text + ((char)(ushort)(num3 - 10 + 65)).ToString()));
                text = ((num2 <= 9) ? (text + num2.ToString()) : (text + ((char)(ushort)(num2 - 10 + 65)).ToString()));
                if (i + 1 != ((Array)object_0).Length && (i + 1) % 2 == 0)
                {
                    text += "-";
                }
            }
            return text;
        }

        private static string smethod_7(string string_1, string string_2, string string_3)
        {
            string text = "";
            foreach (ManagementObject instance in new ManagementClass(string_1).GetInstances())
            {
                if (instance[string_3].ToString() == "True" && text == "")
                {
                    try
                    {
                        text = instance[string_2].ToString();
                        return text;
                    }
                    catch
                    {
                    }
                }
            }
            return text;
        }

        private static string smethod_8(string string_1, string string_2)
        {
            string text = "";
            foreach (ManagementObject instance in new ManagementClass(string_1).GetInstances())
            {
                if (text == "")
                {
                    try
                    {
                        text = instance[string_2].ToString();
                        return text;
                    }
                    catch
                    {
                    }
                }
            }
            return text;
        }

        private static string smethod_9()
        {
            string text = smethod_8("Win32_Processor", "UniqueId");
            if (text == "")
            {
                text = smethod_8("Win32_Processor", "ProcessorId");
                if (text == "")
                {
                    text = smethod_8("Win32_Processor", "Name");
                    if (text == "")
                    {
                        text = smethod_8("Win32_Processor", "Manufacturer");
                    }
                    text += smethod_8("Win32_Processor", "MaxClockSpeed");
                }
            }
            return text;
        }

        private static string smethod_10()
        {
            return smethod_8("Win32_BIOS", "Manufacturer") + smethod_8("Win32_BIOS", "SMBIOSBIOSVersion") + smethod_8("Win32_BIOS", "IdentificationCode") + smethod_8("Win32_BIOS", "SerialNumber") + smethod_8("Win32_BIOS", "ReleaseDate") + smethod_8("Win32_BIOS", "Version");
        }

        private static string smethod_11()
        {
            return smethod_8("Win32_DiskDrive", "Model") + smethod_8("Win32_DiskDrive", "Manufacturer") + smethod_8("Win32_DiskDrive", "Signature") + smethod_8("Win32_DiskDrive", "TotalHeads");
        }

        private static string smethod_12()
        {
            return smethod_8("Win32_BaseBoard", "Model") + smethod_8("Win32_BaseBoard", "Manufacturer") + smethod_8("Win32_BaseBoard", "Name") + smethod_8("Win32_BaseBoard", "SerialNumber");
        }

        private static string smethod_13()
        {
            return smethod_7("Win32_NetworkAdapterConfiguration", "MACAddress", "IPEnabled");
        }

        private static void smethod_14(string string_1)
        {
            MessageBox.Show(string_1, "License Notice", MessageBoxButtons.OK);
        }

        public FingerConfig()
        {


        }

        static FingerConfig()
        {

            byte_0 = new byte[9]
            {
                13,
                46,
                22,
                64,
                95,
                33,
                74,
                18,
                88
            };
            string_0 = string.Empty;
            m_version = 0;
            m_nameClient = null;
            m_hex = null;
            url_check = "";
            m_checkTime = true;
            m_checkHw = true;
            m_endDate = DateTime.MinValue;
            DisibleEventList = new List<string>();
        }
    }
}
