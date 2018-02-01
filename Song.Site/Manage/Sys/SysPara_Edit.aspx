<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    Codebehind="SysPara_Edit.aspx.cs" Inherits="Song.Site.Manage.Sys.SysPara_Edit"
    Title="系统参数项编辑" %>

<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td width="120">
                参数名（key）：</td>
            <td>
                <asp:TextBox nullable="false" ID="tbKey" runat="server" MaxLength="255" Width="300px"></asp:TextBox>（请慎重更改）</td>
        </tr>
        <tr>
            <td width="120">
                参数值（value）：</td>
            <td>
                <asp:TextBox ID="tbValue" runat="server" MaxLength="255" Width="300px"></asp:TextBox></td>
        </tr>
        <tr>
            <td width="120">
                默认值：</td>
            <td>
                <asp:TextBox ID="tbDefault" runat="server" MaxLength="255" Width="300px"></asp:TextBox></td>
        </tr>
        <tr>
            <td width="120">
                供选择的单位：</td>
            <td>
                <asp:TextBox ID="tbSecUnit" runat="server" MaxLength="100" Width="300px"></asp:TextBox>（用,分隔单位项）</td>
        </tr>
        <tr>
            <td width="120">
                实际单位：</td>
            <td>
                <asp:TextBox ID="tbUnit" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td colspan="2">
                 说明：
                <br />
                <asp:TextBox ID="tbIntro" runat="server" Height="120px" MaxLength="255" TextMode="MultiLine"
                    Width="98%"></asp:TextBox></td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
    <cc1:EnterButton verify="true" ID="btnEnter" runat="server" Text="确定" OnClick="btnEnter_Click"
        ValidationGroup="enter" />
    <%--<Song:DeleteButton ID="DeleteButton1" runat="server" OnClick="btnDelete_Click" />--%>
    <cc1:CloseButton ID="CloseButton1" runat="server" />
</asp:Content>
