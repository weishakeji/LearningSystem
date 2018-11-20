<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    Codebehind="Outline_Export.aspx.cs" Inherits="Song.Site.Manage.Course.Outline_Export"
    Title="无标题页" %>

<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<%@ Register Src="~/Manage/Utility/ExcelInput.ascx" TagName="ExcelInput" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <div class="curname">当前课程：<asp:Label ID="lbCourName" runat="server" Text="无"></asp:Label></div>
    <br />
    <cc1:Button ID="btnEnter" runat="server" Text="导出当前课程的章节" verify="true" OnClick="btnEnter_Click" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
    <cc1:CloseButton ID="CloseButton1" runat="server" />
</asp:Content>