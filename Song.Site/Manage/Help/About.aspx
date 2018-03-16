<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="Song.Site.Manage.Help.About" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
</head>
<body>
    <div id="context">
    <p>
        <%= copyright["intro"]%></p>
    <hr />
    <p>
        <%= copyright["compay"]%> 感谢您的支持！</p>
    <!--  <p>创作人员：宋雷鸣（10522779@QQ.com）</p>-->
    <p>
        <a href="<%= copyright["url"]%>" target="_blank"><%= copyright["domain"]%></a> &nbsp;
        Tel: <%= copyright["tel"]%></p>
</div>
</body>
</html>
