<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    Codebehind="General.aspx.cs" Inherits="Song.Site.Manage.Sys.General" Title="无标题页" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
<script type="text/javascript" src="../CoreScripts/iColorPicker.js"></script>

    系统名称：<asp:TextBox ID="tbSysName" runat="server" Width="50%" MaxLength="100"></asp:TextBox><br />
    <br />
    <asp:CheckBox ID="cbLogin" runat="server" Text="是否记录登录信息" />
    记录<asp:TextBox ID="tbLoginTimeSpan" runat="server" MaxLength="2" Width="30px"></asp:TextBox>天之内的登录信息<br />
    <asp:CheckBox ID="cbWork" runat="server" Text="是否记录操作信息" />
    记录<asp:TextBox ID="tbWorkTimeSpan" runat="server" MaxLength="2" Width="30px"></asp:TextBox>天之内的操作信息<br />
    <br />


     <br />
    <asp:Button ID="btnEnter" runat="server" verify="true" Text="确 定" OnClick="btnEnter_Click" />
</asp:Content>
