<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    Codebehind="DataBaseBackup_Edit.aspx.cs" Inherits="Song.Site.Manage.Sys.DataBaseBackup_Edit"
    Title="修改数据库备份" %>

<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    文件名称：<asp:TextBox nullable="false" ID="tbName" runat="server" MaxLength="250" Width="450"></asp:TextBox><br />
    <br />
    文件大小：<asp:Label ID="lbSize" runat="server" Text="Label"></asp:Label><br />
    创建时间：<asp:Label ID="lbTime" runat="server" Text="Label"></asp:Label><br />
    &nbsp;
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
    <cc1:EnterButton verify=true  ID="btnEnter" runat="server" Text="确定" OnClick="btnEnter_Click" />
    <%--<Song:DeleteButton ID="DeleteButton1" runat="server" OnClick="btnDelete_Click" />--%>
    <cc1:CloseButton ID="CloseButton1" runat="server" />
</asp:Content>
