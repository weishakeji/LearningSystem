<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true" CodeBehind="MenuBuilder.aspx.cs" Inherits="Song.Site.Manage.Sys.MenuBuilder" Title="无标题页" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <asp:Button ID="btnSysMenu" runat="server" Text="生成系统菜单" OnClick="btnSysMenu_Click" Visible=false />
    <asp:Button ID="btnPurTree" runat="server" Text="生成权限菜单" OnClick="btnPurTree_Click" />
  
</asp:Content>
