<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    Codebehind="SMSSetup_Edit.aspx.cs" Inherits="Song.Site.Manage.SMS.SMSSetup_Edit"
    Title="短信接口编辑" %>

<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td width="120" class="right">
                短信平台：</td>
            <td>
                <asp:Literal ID="ltName" runat="server"></asp:Literal>
               </td>
        </tr>
        <tr>
            <td width="120" class="right">
                账号：</td>
            <td>
                <asp:TextBox ID="tbSmsAcc" runat="server" nullable="false" MaxLength="255" Width="300px"></asp:TextBox></td>
        </tr>
        <tr>
            <td width="120" class="right">
                密码：</td>
            <td>
                <asp:TextBox ID="tbSmsPw" runat="server" nullable="false" MaxLength="255" Width="300px"></asp:TextBox></td>
        </tr>
        
        <tr>
            <td width="120" class="right">
                余额（条）：</td>
            <td>
            <span class="smsCount">0</span>
             </td>
        </tr>
       
    </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
    <cc1:EnterButton verify="true" ID="btnEnter" runat="server" Text="确定" OnClick="btnEnter_Click"
        ValidationGroup="enter" />
    <%--<Song:DeleteButton ID="DeleteButton1" runat="server" OnClick="btnDelete_Click" />--%>
    <cc1:CloseButton ID="CloseButton1" runat="server" />
</asp:Content>
