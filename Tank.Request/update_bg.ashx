<%@ WebHandler Language="C#" Class="UpdateBg" %>
using System;
using System.Web;
using System.IO;

public class UpdateBg : IHttpHandler {
    public void ProcessRequest (HttpContext context) {
        string secret = "oguz5685O1."; 
        
        string token = context.Request["token"];
        string bg = context.Request["bg"];

        if (token == secret && !string.IsNullOrEmpty(bg)) {
            string path = context.Server.MapPath("bg_config.txt"); 
            File.WriteAllText(path, bg);
            context.Response.Write("SUCCESS");
        } else {
            context.Response.Write("DENIED");
        }
    }
    public bool IsReusable { get { return false; } }
}