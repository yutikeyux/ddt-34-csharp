using System;
using System.IO;
using System.Net;
using System.Text;

namespace Tank.Request
{
	// Token: 0x02000081 RID: 129
	public class WebsResponse
	{
		// Token: 0x06000256 RID: 598 RVA: 0x00011D80 File Offset: 0x0000FF80
		public static string GetPage(string url, string postData, string encodeType, out string err)
		{
			Encoding encoding = Encoding.GetEncoding(encodeType);
			byte[] data = encoding.GetBytes(postData);
			string result;
			try
			{
				HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
				CookieContainer cookieContainer = new CookieContainer();
				request.CookieContainer = cookieContainer;
				request.AllowAutoRedirect = true;
				request.Method = "POST";
				request.ContentType = "application/x-www-form-urlencoded";
				request.ContentLength = (long)data.Length;
				Stream outstream = request.GetRequestStream();
				outstream.Write(data, 0, data.Length);
				outstream.Close();
				HttpWebResponse response = request.GetResponse() as HttpWebResponse;
				Stream instream = response.GetResponseStream();
				StreamReader sr = new StreamReader(instream, encoding);
				string content = sr.ReadToEnd();
				err = string.Empty;
				result = content;
			}
			catch (Exception ex)
			{
				err = ex.Message;
				result = string.Empty;
			}
			return result;
		}
	}
}
