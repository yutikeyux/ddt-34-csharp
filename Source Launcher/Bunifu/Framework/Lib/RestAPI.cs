using System;
using System.IO;
using System.Net;
using System.Text;

namespace Bunifu.Framework.Lib
{
	public static class RestAPI
	{
		private static object yr2nIEIqmWEvdMYTLcpv;

		public static string smethod_0(string url)
		{
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
			try
			{
				using Stream stream = httpWebRequest.GetResponse().GetResponseStream();
				return new StreamReader(stream, Encoding.UTF8).ReadToEnd();
			}
			catch (WebException ex)
			{
				try
				{
					using (Stream stream2 = ex.Response.GetResponseStream())
					{
						new StreamReader(stream2, Encoding.GetEncoding("utf-8")).ReadToEnd();
					}
					throw;
				}
				catch (Exception ex2)
				{
					return "{\"error\": \"" + ex2.Message + "\"  }";
				}
			}
		}

		public static string POST(string url, string jsonContent)
		{
			try
			{
				string result = "";
				using (WebClient webClient = new WebClient())
				{
					webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
					result = webClient.UploadString(url, "POST", jsonContent);
				}
				return result;
			}
			catch (Exception ex)
			{
				return "{\"error\": \"" + ex.Message + "\"  }";
			}
		}

		public static string PATCH(string url, string jsonContent)
		{
			try
			{
				string result = "";
				using (WebClient webClient = new WebClient())
				{
					webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
					result = webClient.UploadString(url, "PATCH", jsonContent);
				}
				return result;
			}
			catch (Exception ex)
			{
				return "{\"error\": \"" + ex.Message + "\"  }";
			}
		}

		public static string DELETE(string url, string jsonContent)
		{
			try
			{
				string result = "";
				using (WebClient webClient = new WebClient())
				{
					webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
					result = webClient.UploadString(url, "DELETE", jsonContent);
				}
				return result;
			}
			catch (Exception ex)
			{
				return "{\"error\": \"" + ex.Message + "\"  }";
			}
		}

		internal static bool AlJLk6IqgiRnuXURrqeq()
		{
			return yr2nIEIqmWEvdMYTLcpv == null;
		}
	}
}
