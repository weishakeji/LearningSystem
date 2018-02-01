<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    Codebehind="News_Preview.aspx.cs" Inherits="Song.Site.Manage.View.News_Preview"
    Title="无标题页" %>

<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <div id="title">
        <asp:Literal ID="ltTitle" runat="server"></asp:Literal></div>
    <div id="attrBar">
        <span>作者：<asp:Literal ID="ltAuthor" runat="server"></asp:Literal></span> 
        <span>来源：<asp:Literal ID="ltSource" runat="server"></asp:Literal></span>
        <span>时间：<asp:Literal ID="ltCrtTime" runat="server"></asp:Literal></span>
    </div>
    <div id="Details">
        <asp:Literal ID="ltDetails" runat="server"></asp:Literal>
    </div>
    <div>
    <asp:DataList ID="dlAcc" runat="server">
        <ItemTemplate>
        <%# Container.ItemIndex   + 1 %>、

            <asp:HyperLink ID="hl" runat="server" NavigateUrl='<%#Eval("As_FileName") %>' Target="_blank"><%#Eval("As_Name") %></asp:HyperLink>
            
        </ItemTemplate>
        <HeaderTemplate>
            附件：
        </HeaderTemplate>
    </asp:DataList>
    </div>
    <div id="artFoot" runat=server>
        <span>关键字：<asp:Literal ID="ltKeyword" runat="server"></asp:Literal></span>
    </div>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
    <cc1:CloseButton ID="CloseButton1" runat="server" />
</asp:Content>
