<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OuterLink.aspx.cs" Inherits="Song.Site.Manage.Utility.UploadPath.OuterLink" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>外部链接</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
</head>
<body>
    <form id="form1" runat="server">
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td width="80" class="right">
                视频链接：
            </td>
            <td>
                 <asp:TextBox ID="tbUrl" runat="server" Width="98%" nullable="false" MaxLength="3000"></asp:TextBox>
            </td>
        </tr>
         <tr>
            <td class="right">
                宽：
            </td>
            <td >
                <asp:TextBox ID="tbHeight" runat="server" Width="60" MaxLength="10"></asp:TextBox> px（像素）
            </td>
        </tr>
         <tr>
            <td class="right">
                高：
            </td>
            <td >
                <asp:TextBox ID="tbWidth" runat="server" Width="60" MaxLength="10"></asp:TextBox> px（像素）
            </td>
        </tr>
         <tr>
            <td class="right">
                时长：
            </td>
            <td >
                <asp:TextBox ID="tbSpan" runat="server" Width="80" MaxLength="20"></asp:TextBox> 分钟
            </td>
        </tr>
           <tr>
            <td class="right">
               
            </td>
            <td >
               &nbsp;<asp:Panel ID="panleShow" runat="server" Visible=false>
               <span style="color:red">该视频附件不是外部链接，不允许编辑；<br />
               如果需要添加，请删除原有视频；</span>
                </asp:Panel>
            </td>
        </tr>
           <tr>
            <td class="right">
            
            </td>
            <td >
                <cc1:Button ID="btnEnter" runat="server" Text="保存链接" verify="true" />
            </td>
        </tr>
        </table>
        
    </form>
</body>
</html>
