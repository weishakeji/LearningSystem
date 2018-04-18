<%@ Page Language="C#" AutoEventWireup="true" %>
<%@ Import Namespace="System.IO" %>
<!doctype html>
<html>
<head runat="server">
<title>文件查看器</title>
<style type="text/css">
body {
	margin: 20px !important;
}
.currpath {
	font-size: 18px;
	font-family: Arial, Helvetica, sans-serif;
}
.currpath a {
	margin-left: 5px;
}
.dirtitle {
	font-size: 18px;
	font-family: Arial, Helvetica, sans-serif;
}
dl dd, dl dd a {
	font-size: 16px;
	font-family: Verdana, Geneva, sans-serif;
}
.dir dd a {
	font-weight: bold;
	color: #333;
}
.files dd a {
	color: #666;
}
</style>
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
        for (int i = 0; i < arr.Length; i++)
        {
            string fullpath = string.Empty;
            for (int j = 0; j <= i; j++) fullpath += arr[j];
            paths += string.Format("<a href='{0}?path={1}'>{2}</a>", page, Server.UrlEncode(fullpath), arr[i]);
        }
        //当前路径下所有子目录
        string strDir = "<dl class='dir'>";
        System.IO.DirectoryInfo currDir = new DirectoryInfo(hypath);
        foreach (System.IO.DirectoryInfo d in currDir.GetDirectories())
        {
            string dpath = d.FullName.Substring(hyroot.Length - 1);
            dpath = dpath.Replace("\\", "/");
            strDir += string.Format("<dd>+ <a href='{0}?path={1}'>{2}</a></dd>", page, Server.UrlEncode(dpath), d.Name);
        }
        strDir += "</dl>";
        //当前路径下所有文件
        string strFile = "<dl class='files'>";
        foreach (System.IO.FileInfo f in currDir.GetFiles())
        {
            string fpath = f.FullName.Substring(hyroot.Length);
            fpath = fpath.Replace("\\", "/");
            strFile += string.Format("<dd>- <a href='file.aspx?file=/{0}' target='_blank'>{1}</a></dd>", Server.UrlEncode(fpath), f.Name);
        }
        strFile += "</dl>";
   
    %>
    <div class="currpath"> Current path:<%= paths%></div>
    <div class="dirtitle"> Directory:</div>
    <%= strDir%> <%= strFile%>
</body>
</html>
