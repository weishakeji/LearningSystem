<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    Codebehind="Note_Edit.aspx.cs" Inherits="Song.Site.Manage.Content.Note_Edit"
    Title="无标题页" %>

<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebEditor" Namespace="WeiSha.WebEditor" TagPrefix="WebEditor" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    来源：<asp:Label ID="lbProvince" runat="server" Text=""></asp:Label><asp:Label ID="lbCity"
        runat="server" Text="Label"></asp:Label>IP：<asp:Label ID="lbIP" runat="server"
            Text="Label"></asp:Label>时间：<asp:Label ID="lbCrtTime" runat="server" Text="Label"></asp:Label><br />
    标题：<asp:TextBox ID="tbTitle" runat="server" Width="400px"></asp:TextBox> <br />内容：<br />
    <asp:TextBox ID="tbDetails" runat="server" Height="100px" TextMode="MultiLine" Width="80%"></asp:TextBox>
    <br />
    <asp:CheckBox ID="cbIsShow" runat="server" Text="是否显示" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
    <cc1:EnterButton verify="true" ID="btnEnter" runat="server" Text="确定" group="ent"
        OnClick="btnEnter_Click" ValidationGroup="enter" />
    <%--<Song:DeleteButton ID="DeleteButton1" runat="server" OnClick="btnDelete_Click" />--%>
    <cc1:CloseButton ID="CloseButton1" runat="server" />
</asp:Content>
