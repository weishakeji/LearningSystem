<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    Codebehind="SiteSetup.aspx.cs" Inherits="Song.Site.Manage.Site.SiteSetup" Title="配置管理" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <br />
    <asp:CheckBox ID="cbIsStatic" runat="server" Text="是否采用静态网站" /><br />
    <asp:CheckBox ID="cbIsArticlePurview" runat="server" Text="文章管理 是否需要权限控制" /><br />
    <br />
    <asp:Button ID="btnEnter" runat="server" Text="确定" verify="true" OnClick="btnEnter_Click"
        ValidationGroup="enter" />
</asp:Content>
