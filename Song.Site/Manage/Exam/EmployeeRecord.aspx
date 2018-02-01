<%@ Page Language="C#" AutoEventWireup="true" Codebehind="EmployeeRecord.aspx.cs"
    MasterPageFile="~/Manage/ManagePage.Master" Inherits="Song.Site.Manage.Exam.EmployeeRecord" %>

<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Src="../Utility/Pager.ascx" TagName="Pager" TagPrefix="uc2" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <div id="header">
        <uc1:toolsBar ID="ToolsBar1" runat="server" WinPath="EmployeeRecord_Edit.aspx" AddButtonOpen="true"
            GvName="GridView1" WinWidth="800" WinHeight="600" InputButtonVisible="false"
            AddButtonVisible="false" DelButtonVisible="false" />
        <div class="searchBox">
            <cc1:DropDownTree ID="ddlDepart" runat="server" Width="150" IdKeyName="dep_id" ParentIdKeyName="dep_PatId"
                TaxKeyName="dep_Tax" AutoPostBack="True" OnSelectedIndexChanged="ddlDepart_SelectedIndexChanged">
            </cc1:DropDownTree>
            &nbsp;&nbsp;班组：<asp:DropDownList ID="ddlTeam" runat="server" AutoPostBack="True"
                OnSelectedIndexChanged="ddlTeam_SelectedIndexChanged">
            </asp:DropDownList>
            考试：
            <asp:DropDownList ID="ddlExam" runat="server" Width="180px" AutoPostBack="True" OnSelectedIndexChanged="ddlExam_SelectedIndexChanged">
            </asp:DropDownList>
            场次：
            <asp:DropDownList ID="ddlExam2" runat="server">
            </asp:DropDownList>
            <asp:Button ID="btnSear" runat="server" Width="100" Text="查询" OnClick="btnsear_Click" />
        </div>
    </div>
    <cc1:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" SelectBoxKeyName="SelectBox"
        ShowSelectBox="false">
        <EmptyDataTemplate>
            没有任何满足条件的试题！
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
<cc1:RowEdit id="btnEdit" runat="server"  visible=false></cc1:RowEdit> 
</itemtemplate>
                <itemstyle cssclass="center" width="44px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="姓名">
               
                <itemtemplate>
               
<%# Eval("Acc_Name")%>
</itemtemplate>
 <headerstyle cssclass="center" width="60px"  />
 <itemstyle cssclass="center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="考试主题">
                <itemstyle cssclass="center"/>
                <itemtemplate>
<%# Eval("Exam_Title","{0}")%>
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="学科">
                <itemstyle cssclass="center" width="60px" />
                <itemtemplate>
<%# Eval("Sbj_Name","{0}")%>
</itemtemplate>
               
            </asp:TemplateField>
            <asp:TemplateField HeaderText="采用试卷">
                <itemstyle cssclass="center"/>
                <itemtemplate>
<%# getTestPager(Eval("Tp_ID","{0}"))%>
</itemtemplate>
               
            </asp:TemplateField>
            <asp:TemplateField HeaderText="应试时间">
                <itemstyle cssclass="center" width="180px" />
                <itemtemplate>
<%# Eval("Exr_CrtTime", "{0:yyyy月MM月dd日 hh:mm}")%>
</itemtemplate>
                
            </asp:TemplateField>
            <asp:TemplateField HeaderText="得分">
                <itemstyle cssclass="center" width="50px" />
                <itemtemplate>
                <%# Eval("Exr_ScoreFinal")%>
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="评分">
                <itemstyle cssclass="center" width="60px" />
                <itemtemplate>
                <a href="#" onclick="OpenWin('EmployeeRecord_Edit.aspx?id=<%# Eval("Exr_ID")%>','评分',800,600);return false;">评分</a>
</itemtemplate>
            </asp:TemplateField>
        </Columns>
    </cc1:GridView>
    <br />
    <uc2:Pager ID="Pager1" runat="server" Size="20" OnPageChanged="BindData" />
</asp:Content>
