<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HelpEditer.aspx.cs" Inherits="Song.Site.Manage.Panel.HelpEditer" %>
<%@ Register Assembly="WeiSha.WebEditor" Namespace="WeiSha.WebEditor" TagPrefix="WebEditor" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
<title>无标题页</title>
</head>
<body>
<form id="form1" runat="server">
  <table width="100%" border="0" cellspacing="0" cellpadding="0" style="margin-top:10px">
    <tr>
      <td>&nbsp; 编辑当前功能
        <asp:Label ID="lbName" runat="server" Text="Label" ForeColor="Red"></asp:Label>
        的帮助：</td>
      <td width="180" style="text-align: center;"><asp:HyperLink ID="linkHelp" runat="server" Target="_blank">预览帮助文档</asp:HyperLink></td>
    </tr>
  </table>
  <div>
    <WebEditor:Editor ID="tbHelpContext" runat="server" Height="400px" Width="98%"> </WebEditor:Editor>
  </div>
  <cc1:EnterButton verify="true" ID="btnEnter" runat="server" Text="确定" OnClick="btnEnter_Click" />
  <cc1:CloseButton ID="CloseButton1" runat="server" />
</form>
</body>
</html>
