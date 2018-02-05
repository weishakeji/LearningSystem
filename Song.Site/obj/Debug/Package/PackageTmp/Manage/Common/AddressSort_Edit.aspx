<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    Codebehind="AddressSort_Edit.aspx.cs" Inherits="Song.Site.Manage.Common.AddressSort_Edit"
    Title="无标题页" %>

<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">

    名称：<asp:TextBox ID="tbName" runat="server" nullable="false" Width="180px"
        MaxLength="20"></asp:TextBox>
    <br />
    状态：<asp:CheckBox ID="cbIsUse" runat="server" Checked="True" Text="是否启用" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
    <cc1:EnterButton ID="btnEnter" runat="server" Text="确定" verify="true" OnClick="btnEnter_Click"
        ValidationGroup="enter" />
    <cc1:CloseButton ID="CloseButton1" runat="server" />
</asp:Content>
