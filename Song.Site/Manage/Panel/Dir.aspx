<%@ Page Language="C#" AutoEventWireup="true" %>

<%@ Import Namespace="System.IO" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>文件查看器</title>
</head>
<body>
    <%
        string page = this.Request.FilePath;
        //获取当前路径
        string path = this.Request.QueryString["path"];
        if (string.IsNullOrWhiteSpace(path)) path = "/";
        string hyroot = this.Server.MapPath("/");
        string hypath = this.Server.MapPath(this.Server.UrlDecode(path));   //物理路径
        if (!hypath.StartsWith(hyroot)) hypath = hyroot;
        Uri uri = new Uri(this.Request.Url, path);
        string[] arr = uri.Segments;
        //当前路径导航
        string paths = "";
        for (int i=0;i<arr.Length;i++)
        {
            string fullpath = string.Empty;
            for (int j = 0; j <=i; j++) fullpath += arr[j];
            paths += string.Format("<a href='{0}?path={1}'>{2}</a>", page, Server.UrlEncode(fullpath), arr[i]);
        }
        //当前路径下所有子目录
        string strDir = "<dl>";
        System.IO.DirectoryInfo currDir = new DirectoryInfo(hypath);
        foreach (System.IO.DirectoryInfo d in currDir.GetDirectories())
        {
            string dpath = d.FullName.Substring(hyroot.Length-1);
            dpath = dpath.Replace("\\","/");
            strDir += string.Format("<dd><a href='{0}?path={1}'>+{2}</a></dd>", page, Server.UrlEncode(dpath), d.Name);
        }
        strDir += "</dl>";
        //当前路径下所有文件
        string strFile = "<dl>";
        foreach (System.IO.FileInfo f in currDir.GetFiles())
        {
            string fpath = f.FullName.Substring(hyroot.Length);
            fpath = fpath.Replace("\\", "/");
            strFile += string.Format("<dd><a href='/{0}' target='_blank'>-{1}</a></dd>", Server.UrlEncode(fpath), f.Name);
        }
        strFile += "</dl>";
   
    %>
    <div>
        Current path:<span><%= paths%></span></div>
   <div>Directory:</div>
    <%= strDir%>
     <%= strFile%>
</body>
</html>
