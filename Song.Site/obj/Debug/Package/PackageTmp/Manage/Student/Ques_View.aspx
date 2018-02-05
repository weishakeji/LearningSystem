<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    CodeBehind="Ques_View.aspx.cs" Inherits="Song.Site.Manage.Student.Ques_View"
    Title="无标题页" %>

<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<%@ Register Assembly="WeiSha.WebEditor" Namespace="WeiSha.WebEditor" TagPrefix="WebEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <div class="titleBox">
        【<%= typeStr[(int)mm.Qus_Type - 1]%>】 题干：</div>
    <div class="title">
        <asp:Literal ID="ltTitle" runat="server"></asp:Literal>
    </div>
    <div id="items" runat="server" class="itemBox">
        <asp:Repeater ID="rptItem" runat="server">
            <ItemTemplate>
                <div class="item">
                    <%# (char)(65+Container.ItemIndex)%>、<%# Eval("Ans_Context")%></div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    <div class="titleBox">
        正确答案：</div>
    <div class="ansContext">
        <%--答案，字符型的，针对单选、多选、判断、填空等较短的内容--%>
        <div class="ltAnswerWord" id="divAnswerWord" runat="server">
            <asp:Literal ID="ltAnswerWord" runat="server"></asp:Literal></div>
        <%--答案，文本型，针对简单题，内容较多--%>
        <div class="ltAnswerText" id="divAnswerText" runat="server">
            <asp:Literal ID="ltAnswerText" runat="server"></asp:Literal></div>
    </div>
    <div class="titleBox">
        知识点讲解：</div>
    <div class="explan">
        <asp:Literal ID="ltExplan" runat="server"></asp:Literal></div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
    <cc1:CloseButton ID="CloseButton1" runat="server" />
</asp:Content>
