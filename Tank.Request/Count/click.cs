using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Ajax;
using Bussiness;

namespace Count
{
	// Token: 0x02000002 RID: 2
	public class click : Page
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		protected void Page_Load(object sender, EventArgs e)
		{
			Utility.RegisterTypeForAjax(typeof(click));
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002064 File Offset: 0x00000264
		[AjaxMethod]
		public string Logoff(string App_Id, string Direct_Url, string Referry_Url, string Begin_time, string ScreenW, string ScreenH, string Color, string Flash)
		{
			HttpContext current = HttpContext.Current;
			Dictionary<string, string> clientInfos = new Dictionary<string, string>();
			try
			{
				clientInfos.Add("Application_Id", App_Id);
				string ip = current.Request.UserHostAddress;
				string userAgent = (current.Request.UserAgent == null) ? "无" : current.Request.UserAgent;
				bool flag = current.Request.ServerVariables["HTTP_UA_CPU"] == null;
				if (flag)
				{
					clientInfos.Add("CPU", "未知");
				}
				else
				{
					clientInfos.Add("CPU", current.Request.ServerVariables["HTTP_UA_CPU"]);
				}
				clientInfos.Add("OperSystem", click.GetOSNameByUserAgent(userAgent));
				clientInfos.Add("IP", ip);
				clientInfos.Add("IPAddress", ip);
				bool flag2 = current.Request.Browser.ClrVersion == null;
				if (flag2)
				{
					clientInfos.Add(".NETCLR", "不支持");
				}
				else
				{
					clientInfos.Add("NETCLR", current.Request.Browser.ClrVersion.ToString());
				}
				clientInfos.Add("Browser", current.Request.Browser.Browser + current.Request.Browser.Version);
				clientInfos.Add("ActiveX", current.Request.Browser.ActiveXControls ? "True" : "False");
				clientInfos.Add("Cookies", current.Request.Browser.Cookies ? "True" : "False");
				clientInfos.Add("CSS", current.Request.Browser.SupportsCss ? "True" : "False");
				clientInfos.Add("Language", current.Request.UserLanguages[0]);
				string httpAccept = current.Request.ServerVariables["HTTP_ACCEPT"];
				bool flag3 = httpAccept == null;
				if (flag3)
				{
					clientInfos.Add("Computer", "False");
				}
				else
				{
					bool flag4 = httpAccept.IndexOf("wap") > -1;
					if (flag4)
					{
						clientInfos.Add("Computer", "False");
					}
					else
					{
						clientInfos.Add("Computer", "True");
					}
				}
				clientInfos.Add("Platform", current.Request.Browser.Platform);
				clientInfos.Add("Win16", current.Request.Browser.Win16 ? "True" : "False");
				clientInfos.Add("Win32", current.Request.Browser.Win32 ? "True" : "False");
				bool flag5 = current.Request.ServerVariables["HTTP_ACCEPT_ENCODING"] == null;
				if (flag5)
				{
					clientInfos.Add("AcceptEncoding", "无");
				}
				else
				{
					clientInfos.Add("AcceptEncoding", current.Request.ServerVariables["HTTP_ACCEPT_ENCODING"]);
				}
				clientInfos.Add("UserAgent", userAgent);
				clientInfos.Add("Referry", Referry_Url);
				clientInfos.Add("Redirect", Direct_Url);
				clientInfos.Add("TimeSpan", Begin_time.ToString());
				clientInfos.Add("ScreenWidth", ScreenW);
				clientInfos.Add("ScreenHeight", ScreenH);
				clientInfos.Add("Color", Color);
				clientInfos.Add("Flash", Flash);
				CountBussiness.InsertContentCount(clientInfos);
			}
			catch (Exception ex)
			{
				return ex.ToString();
			}
			return "ok";
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002430 File Offset: 0x00000630
		private static string GetOSNameByUserAgent(string userAgent)
		{
			string osVersion = "未知";
			bool flag = userAgent.Contains("NT 6.0");
			if (flag)
			{
				osVersion = "Windows Vista/Server 2008";
			}
			else
			{
				bool flag2 = userAgent.Contains("NT 5.2");
				if (flag2)
				{
					osVersion = "Windows Server 2003";
				}
				else
				{
					bool flag3 = userAgent.Contains("NT 5.1");
					if (flag3)
					{
						osVersion = "Windows XP";
					}
					else
					{
						bool flag4 = userAgent.Contains("NT 5");
						if (flag4)
						{
							osVersion = "Windows 2000";
						}
						else
						{
							bool flag5 = userAgent.Contains("NT 4");
							if (flag5)
							{
								osVersion = "Windows NT4";
							}
							else
							{
								bool flag6 = userAgent.Contains("Me");
								if (flag6)
								{
									osVersion = "Windows Me";
								}
								else
								{
									bool flag7 = userAgent.Contains("98");
									if (flag7)
									{
										osVersion = "Windows 98";
									}
									else
									{
										bool flag8 = userAgent.Contains("95");
										if (flag8)
										{
											osVersion = "Windows 95";
										}
										else
										{
											bool flag9 = userAgent.Contains("Mac");
											if (flag9)
											{
												osVersion = "Mac";
											}
											else
											{
												bool flag10 = userAgent.Contains("Unix");
												if (flag10)
												{
													osVersion = "UNIX";
												}
												else
												{
													bool flag11 = userAgent.Contains("Linux");
													if (flag11)
													{
														osVersion = "Linux";
													}
													else
													{
														bool flag12 = userAgent.Contains("SunOS");
														if (flag12)
														{
															osVersion = "SunOS";
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
			return osVersion;
		}

		// Token: 0x04000001 RID: 1
		protected HtmlForm form1;
	}
}
