// Decompiled with JetBrains decompiler
// Type: Tank.Request.gmtipallbyids
// Assembly: Tank.Request, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9A63E34D-BA53-472F-A1E3-A6542CA79FB7
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Bin\Tank.Request.dll

using Bussiness;
using log4net;
using Road.Flash;
using SqlDataProvider.Data;
using System;
using System.Reflection;
using System.Web;
using System.Xml.Linq;

namespace Tank.Request
{
    public class gmtipallbyids : IHttpHandler
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public void ProcessRequest(HttpContext context)
        {
            bool flag = false;
            string str1 = "Fail!";
            XElement node = new XElement((XName)"Result");
            try
            {
                string str2 = context.Request["ids"];
                string[] strArray = (string[])null;
                if (!string.IsNullOrEmpty(str2))
                    strArray = str2.Split(',');
                if (strArray == null)
                    return;
                using (ProduceBussiness produceBussiness = new ProduceBussiness())
                {
                    foreach (EdictumInfo info in produceBussiness.GetAllEdictum())
                    {
                        DateTime dateTime = info.EndDate;
                        DateTime date1 = dateTime.Date;
                        dateTime = DateTime.Now;
                        DateTime date2 = dateTime.Date;
                        if (date1 > date2)
                            node.Add((object)FlashUtils.CreateEdictum(info));
                    }
                    flag = true;
                    str1 = "Success!";
                }
            }
            catch (Exception ex)
            {
                str1 = ex.ToString();
            }
            finally
            {
                node.Add((object)new XAttribute((XName)"value", (object)flag));
                node.Add((object)new XAttribute((XName)"message", (object)str1));
                context.Response.ContentType = "image/jpeg";
                context.Response.Write(node.ToString(false));
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
