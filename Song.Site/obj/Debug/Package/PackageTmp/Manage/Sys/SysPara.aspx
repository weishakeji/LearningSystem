<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    Codebehind="SysPara.aspx.cs" Inherits="Song.Site.Manage.Sys.SysPara" Title="系统参数管理" %>

<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <div id="header">
        <uc1:toolsBar ID="ToolsBar1" runat="server" WinPath="SysPara_Edit.aspx" AddButtonOpen="true"
             WinWidth="600" WinHeight="400" 
            GvName="GridView1" OnDelete="DeleteEvent" DelShowMsg="注:\n删除系统参数有可能导致系统不稳定，请谨慎操作！" />
        <div class="searchBox">
            参数名：<asp:TextBox ID="tbKey" runat="server" Width="115" MaxLength="255"></asp:TextBox>&nbsp;说明：<asp:TextBox
                ID="tbIntro" runat="server" Width="115" MaxLength="255"></asp:TextBox>&nbsp;
            <asp:Button ID="btnSear" runat="server" Width="100" Text="查询" OnClick="BindData" />
        </div>
    </div>
    <cc1:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" SelectBoxKeyName="SelectBox"
        ShowSelectBox="True">
        <EmptyDataTemplate>
            还没有创建角色！
        </EmptyDataTemplate>
        <Columns>
            <asp:TemplateField HeaderText="序号">
                <itemstyle cssclass="center" width="40" />
                <itemtemplate>
<%# Container.DataItemIndex   + 1 %>
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="参数名（key）">
                <itemstyle CssClass="left" width="250" />
                <itemtemplate>
<%# Eval("Sys_Key")%>
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="参数值（value）">
                <itemstyle cssclass="center"  width="300" />
                <itemtemplate>
<div class="value"><%# Eval("Sys_Value")%></div>
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="单位">
                <itemstyle cssclass="center" width="60px" />
                <itemtemplate>
<%# Eval("Sys_Unit")%></itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="说明">
                <itemstyle cssclass="center" />
                <itemtemplate>
<%# Eval("Sys_ParaIntro")%></itemtemplate>
            </asp:TemplateField>
        </Columns>
    </cc1:GridView>
</asp:Content>
