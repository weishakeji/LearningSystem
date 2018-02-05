<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    Codebehind="SiteBackup.aspx.cs" Inherits="Song.Site.Manage.Sys.SiteBackup" Title="无标题页" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    文件名：<asp:TextBox ID="tbName" runat="server" MaxLength="100" nullable="false" Width="200px"></asp:TextBox><asp:Label
        ID="lbDate" runat="server" Text=""></asp:Label><asp:Label
        ID="lbTime" runat="server" Text=""></asp:Label>.zip<br />
    <asp:CheckBox ID="cbDate" runat="server" AutoPostBack="True" OnCheckedChanged="CheckedChanged"
        Text="是否加日期后缀" />
    <asp:CheckBox ID="cbTime" runat="server" AutoPostBack="True" OnCheckedChanged="CheckedChanged"
        Text="是否加时间后缀" /><br />
    <br />
    <asp:Button ID="btnEnter" runat="server" Text="全站打包下载" verify="true"  OnClick="btnEnter_Click" />
</asp:Content>
