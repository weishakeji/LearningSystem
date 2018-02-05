<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    Codebehind="Task.aspx.cs" Inherits="Song.Site.Manage.Common.Task" Title="无标题页" %>

<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Src="../Utility/Pager.ascx" TagName="Pager" TagPrefix="uc2" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <div id="header">
        <uc1:toolsBar ID="ToolsBar1" runat="server" WinPath="Task_Edit.aspx" GvName="GridView1"
            WinWidth="70" WinHeight="70" OnDelete="DeleteEvent" />

        <script language="javascript" src="../Utility/datepicker/WdatePicker.js" type="text/javascript"></script>

        <div class="searchBox">
            状态：<asp:DropDownList ID="ddlState" runat="server" Width="80">
                <asp:ListItem Value="1">完成</asp:ListItem>
                <asp:ListItem Value="2" Selected="true">未完成</asp:ListItem>
                <asp:ListItem Value="3">逾期未完成</asp:ListItem>
                <asp:ListItem Value="4">正在处理</asp:ListItem>
                <asp:ListItem Value="5">关闭</asp:ListItem>
            </asp:DropDownList>
            <asp:DropDownList ID="ddlLevel" runat="server" Width="80">
                <asp:ListItem Selected="true" Value="-1"> -优先级- </asp:ListItem>
                <asp:ListItem Value="5">5</asp:ListItem>
                <asp:ListItem Value="4">4</asp:ListItem>
                <asp:ListItem Value="3">3</asp:ListItem>
                <asp:ListItem Value="2">2</asp:ListItem>
                <asp:ListItem Value="1">1</asp:ListItem>
            </asp:DropDownList>
            标题：<asp:TextBox ID="tbSear" runat="server" Width="115" MaxLength="10"></asp:TextBox>&nbsp;
            结束时间：<asp:TextBox ID="tbStart" runat="server" onfocus="WdatePicker()" Width="80"></asp:TextBox>至
            <asp:TextBox ID="tbEnd" runat="server" onfocus="WdatePicker()" Width="80"></asp:TextBox>
            <asp:Button ID="btnSear" runat="server" Width="100" Text="查询" OnClick="btnsear_Click" />
        </div>
    </div>
    <cc1:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" SelectBoxKeyName="SelectBox"
        ShowSelectBox="True">
        <EmptyDataTemplate>
            没有满足条件的 信息！
        </EmptyDataTemplate>
        <Columns>
            <asp:TemplateField HeaderText="序号">
                <itemstyle cssclass="center" width="40" />
                <itemtemplate>
<%# Container.DataItemIndex   + Pager1.Size*(Pager1.Index-1) + 1 %>
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="优先级">
                <itemstyle cssclass="center" width="60px" />
                <itemtemplate>
                   <span type="level" style="font-weight: bold;">
<%# Eval("Task_Level","{0}")%> </span>
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="指派人/任务主题/完成度">
                <itemstyle cssclass="left" />
                <itemtemplate>
                <div class="compBar" complete="<%# Eval("Task_CompletePer","{0}")%>" tag="<%# Eval("Task_Id","{0}")%>"> &nbsp; </div>
                            <div class="taskTitle" tag="<%# Eval("Task_Id","{0}")%>" level="<%# Eval("Task_Level","{0}")%> "> 
                              [<%# Eval("Task_WorkerName", "{0}")%>]  
<%# Eval("Task_Name","{0}")%></div>
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="完成">
                <itemstyle cssclass="center" width="60px" />
                <itemtemplate>
                   <span class="CompletePer"><span>
<%# Eval("Task_CompletePer", "{0}")%> </span>%</span>
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="开始时间">
                <itemstyle cssclass="center" width="120px" />
                <itemtemplate>
                
<%# Eval("Task_StartTime", "{0:yyyy-MM-dd}")%> 
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="计划完成时间">
                <itemstyle cssclass="center" width="120px" />
                <itemtemplate>
             <%# Eval("Task_EndTime", "{0:yyyy-MM-dd}")%> 
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="剩余时间">
                <itemstyle cssclass="center" width="120px" />
                <itemtemplate><span type="RemainingTime" ><span><%# getRemainingTime(Eval("Task_EndTime", "{0}"))%></span> 小时 </span>
             
</itemtemplate>
            </asp:TemplateField>
        </Columns>
    </cc1:GridView>
    <uc2:Pager ID="Pager1" runat="server" Size="15" OnPageChanged="BindData" />
</asp:Content>
