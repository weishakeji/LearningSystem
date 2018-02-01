<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    Codebehind="Statistics_Details.aspx.cs" Inherits="Song.Site.Manage.Site.Statistics_Details"
    Title="无标题页" %>

<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<%@ Register Assembly="WeiSha.WebEditor" Namespace="WeiSha.WebEditor" TagPrefix="WebEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    当前考试的平均分：<span class="examAvg"></span>
    <div class="rptExamItem">
        <asp:Repeater ID="rptExamItem" runat="server">
            <ItemTemplate>
                <div class="examItem">
                    <%# Eval("Sbj_Name", "{0}")%>
                    ：平均<span><%# GetAvg(Eval("Exam_ID", "{0}"))%></span>分</div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    
    <asp:GridView ID="gvList" runat="server" CssClass="GridView">
    </asp:GridView>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
    <cc1:CloseButton ID="CloseButton1" runat="server" />
</asp:Content>
