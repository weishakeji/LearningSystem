<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    Codebehind="List_Edit.aspx.cs" Inherits="Song.Site.Manage.Questions.List_Edit"
    Title="无标题页" %>

<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<%@ Register Assembly="WeiSha.WebEditor" Namespace="WeiSha.WebEditor" TagPrefix="WebEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <asp:Repeater ID="rptTypes" runat="server">
    <HeaderTemplate><div class="typeTit">请选择试题类型：</div><hr /></HeaderTemplate>
    <ItemTemplate>
    <a class="quesType" href="Questions_Edit<%# Container.ItemIndex+1%>.aspx?type=<%# Container.ItemIndex+1%>&<%= query %>">
    <%# Container.ItemIndex+1%>.<%# GetDataItem() %></a></ItemTemplate>
    </asp:Repeater>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
    <cc1:CloseButton ID="CloseButton1" runat="server" />
</asp:Content>
