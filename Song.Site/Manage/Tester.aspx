<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Tester.aspx.cs" Inherits="Song.Site.Manage.Tester" %>

<%@ Register Src="Utility/ExcelInput.ascx" TagName="ExcelInput" TagPrefix="uc1" %>
<%@ Register Src="Utility/Uploader.ascx" TagName="Uploader" TagPrefix="uc2" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<script type="text/javascript" src="CoreScripts/jquery.js"></script>
<head runat="server">
    <title>无标题页</title>
    <script type="text/javascript" src="/Utility/CoreScripts/jquery.js"></script>
    <script type="text/javascript" src="/Utility/CoreScripts/Verify.js"></script>
    <script type="text/javascript" src="/Utility/CoreScripts/Extend.js"></script>
</head>
<body>
<div>时区：<span id="Span1"><%= timename%></span></div>
<div>服务器时间：<span id="servertime"></span></div>
<div>浏览器时间：<span id="localtime"></span></div>
    <form id="form1" runat="server">
    <script language="javascript" type="text/javascript">
        var date = eval('new ' + eval('/Date(<%= stamp %>)/').source);
        //当前服务器端时间
        //        var ServerTime = new Date("<%= servertime %>");
        var ServerTime = date;
        //alert(date);
        $("#servertime").text(ServerTime.Format("yyyy-MM-dd hh:mm:ss"));
        //本地时间
        var Localtime = ServerTime.toLocal("<%= timediff%>");
        $("#localtime").text(Localtime.Format("yyyy-MM-dd hh:mm:ss"));

        
       // alert(date);
    </script>
    </form>
</body>
</html>
