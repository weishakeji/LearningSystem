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
*{
    font-size: 18px !important;
	font-family: Arial, Helvetica, sans-serif;
}
</style>
</head>
    <body>
    <%       
        //直接打开，例如图片等
        string openfiles = "jpg,png,gif,flv,mp4";        
        //当前文件
        string file = this.Request.QueryString["file"];
        System.IO.FileInfo fi = new FileInfo(this.Server.MapPath(this.Server.UrlDecode(file)));
        string ext = fi.Extension.ToLower();
        if (ext.IndexOf(".") > -1) ext = ext.Substring(ext.IndexOf(".")+1);
        //如果不用打开，则跳转
        foreach (string s in openfiles.Split(','))
        {
            if(s.Equals(ext,StringComparison.CurrentCultureIgnoreCase)){
                this.Response.Redirect(this.Server.UrlDecode(file));
                this.Response.End();                
            }
        }
        //查看源码
        string text = string.Empty;
        string path = string.Format("<a href='{0}' target='_blank'>{1}</a>",file,file);
        using (System.IO.StreamReader sr = new StreamReader(fi.FullName))
        {
            text = sr.ReadToEnd();
            sr.Dispose();
        } 
    %>
    <div>文件：<%= path%></div>
    <div>源码：<br />
    <textarea name="textarea" id="textarea" style="width:100%;height:80%;" rows="30"><%= text%></textarea>
    </div>
</body>
</html>
