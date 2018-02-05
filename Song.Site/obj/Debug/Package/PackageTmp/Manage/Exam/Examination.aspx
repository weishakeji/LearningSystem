<%@ Page Language="C#" AutoEventWireup="true" Codebehind="Examination.aspx.cs" MasterPageFile="~/Manage/ManagePage.Master"
    Inherits="Song.Site.Manage.Exam.Examination" Title="考试"  %>

<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Src="../Utility/Pager.ascx" TagName="Pager" TagPrefix="uc2" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <div id="header">
        <uc1:toolsBar ID="ToolsBar1" runat="server" WinPath="Examination_Edit.aspx" DelShowMsg="注：如果当前考试已经开展，则无法删除！"
            AddButtonOpen="true" GvName="GridView1" WinWidth="900" WinHeight="600" OnDelete="DeleteEvent" />
        <div class="searchBox">
            名称：
            <asp:TextBox ID="tbSear" runat="server" Width="160" MaxLength="10"></asp:TextBox>
            &nbsp;
            时间：<asp:DropDownList ID="ddlTime" runat="server">
                <asp:ListItem Value="-1">--所有时间--</asp:ListItem>
                <asp:ListItem Value="1">今天</asp:ListItem>
                <asp:ListItem Value="2">本周</asp:ListItem>
                <asp:ListItem Value="3">本月</asp:ListItem>
            </asp:DropDownList>
            <asp:Button ID="btnSear" runat="server" Width="100" Text="查询" OnClick="btnsear_Click" />
        </div>
    </div>
    <cc1:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" SelectBoxKeyName="SelectBox"
        ShowSelectBox="true">
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
            <asp:TemplateField HeaderText="操作">
                <itemtemplate>
<cc1:RowDelete id="btnDel" onclick="btnDel_Click" runat="server"></cc1:RowDelete> 
<cc1:RowEdit id="btnEdit" runat="server" ></cc1:RowEdit> 
</itemtemplate>
                <itemstyle cssclass="center" width="44px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="考试主题">
                <itemstyle cssclass="left" />
                <itemtemplate>
                <div class="Exam_Title" title="<%# Eval("Exam_Title")%>">
<%# Eval("Exam_Title")%></div>
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="时间设定">
                <itemstyle cssclass="center" width="80px" />
                <itemtemplate>
<%# Eval("Exam_DateType", "{0}") == "1" ? "定时开始" : "时间区间"%></itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="开始时间">
                <itemstyle cssclass="center" width="120px" />
                <itemtemplate>
<%# Eval("Exam_Date", "{0:yyyy-MM-dd HH:mm}")%></itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="参考人员">
                <itemstyle cssclass="center"/>
                <itemtemplate>
<%# getGroupType(Eval("Exam_GroupType", "{0}"), Eval("Exam_UID", "{0}"))%></itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="使用">
                <itemstyle cssclass="center" width="60px" />
                <itemtemplate>
<cc1:StateButton id="sbUse" onclick="sbUse_Click" runat="server" TrueText="使用" FalseText="禁用" State='<%# Eval("Exam_IsUse","{0}")=="True"%>'></cc1:StateButton> 
</itemtemplate>
            </asp:TemplateField>
        </Columns>
    </cc1:GridView>
    <br />
    <uc2:Pager ID="Pager1" runat="server" Size="20" OnPageChanged="BindData" />
</asp:Content>
