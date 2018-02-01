<%@ Page Language="C#" AutoEventWireup="true" Codebehind="Statistics.aspx.cs" MasterPageFile="~/Manage/ManagePage.Master"
    Inherits="Song.Site.Manage.Exam.Statistics" %>

<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Src="../Utility/Pager.ascx" TagName="Pager" TagPrefix="uc2" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <div id="header">
        <div class="searchBox">
            名称：
            <asp:TextBox ID="tbSear" runat="server" Width="160" MaxLength="10"></asp:TextBox>
            &nbsp; 时间：<asp:DropDownList ID="ddlTime" runat="server">
                <asp:ListItem Value="-1">--所有时间--</asp:ListItem>
                <asp:ListItem Value="1">今天</asp:ListItem>
                <asp:ListItem Value="2">本周</asp:ListItem>
                <asp:ListItem Value="3">本月</asp:ListItem>
            </asp:DropDownList>
            <asp:Button ID="btnSear" runat="server" Width="100" Text="查询" OnClick="btnsear_Click" />
        </div>
    </div>
    <cc1:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" SelectBoxKeyName="SelectBox"
        ShowSelectBox="false">
        <EmptyDataTemplate>
            没有任何满足条件的考试！
        </EmptyDataTemplate>
        <Columns>
            <asp:TemplateField HeaderText="序号">
                <itemstyle cssclass="center" width="40px" />
                <itemtemplate>
<%# Container.DataItemIndex   +   1 %>
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="考试主题">
                <itemstyle cssclass="left" />
                <itemtemplate>
                <div class="Exam_Title" title="<%# Eval("Exam_Title")%>">
<%# Eval("Exam_Title")%></div>
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="考试时间">
                <itemstyle cssclass="center" width="120px" />
                <itemtemplate>
<%# Eval("Exam_Date", "{0:yyyy-MM-dd HH:mm}")%></itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="范围">
                <itemstyle cssclass="center" width="80px" />
                <itemtemplate>
<%# getGroupType(Eval("Exam_GroupType", "{0}"), Eval("Exam_UID", "{0}"))%></itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="及格率">
                <itemstyle cssclass="center" width="60px" />
                <itemtemplate>
                <%# getPassRate(Eval("Exam_UID", "{0}"))%> %</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="成绩详情">
                <itemstyle cssclass="center" width="80px" />
                <itemtemplate>
                <a href="#" onclick="OpenWin('Statistics_Details.aspx?id=<%# Eval("Exam_ID") %>','《<%# Eval("Exam_Title")%>》-成绩详情',800,600);return false;">查看 </a></itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="统计分析">
                <itemstyle cssclass="center" width="80px" />
                <itemtemplate>
                <a href="#" onclick="OpenWin('Statistics_Analysis.aspx?id=<%# Eval("Exam_ID") %>&type=<%# Eval("Exam_GroupType") %>','《<%# Eval("Exam_Title")%>》-分析',800,600);return false;">查看 </a></itemtemplate>
            </asp:TemplateField>
        </Columns>
    </cc1:GridView>
    <br />
    <uc2:Pager ID="Pager1" runat="server" Size="20" OnPageChanged="BindData" />
</asp:Content>
