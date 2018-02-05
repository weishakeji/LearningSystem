<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    Codebehind="LogsWork.aspx.cs" Inherits="Song.Site.Manage.Sys.LogsWork" Title="无标题页" %>

<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Src="../Utility/Pager.ascx" TagName="Pager" TagPrefix="uc2" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">

    <script language="javascript" src="../Utility/datepicker/WdatePicker.js" type="text/javascript"></script>

    <div id="header">
        <uc1:toolsBar ID="ToolsBar1" runat="server" GvName="GridView1" WinWidth="600" WinHeight="400"
            OnDelete="DeleteEvent" AddButtonEnable="false" ModifyButtonEnable="false" />
        <div class="searchBox">
            <asp:DropDownList ID="ddlEmp" runat="server" Width="80">
            </asp:DropDownList> 菜单项：<asp:TextBox ID="tbSear" runat="server" Width="115" MaxLength="10"></asp:TextBox>&nbsp;
            时间：<asp:TextBox ID="tbStart" runat="server" onfocus="WdatePicker()"  CssClass="Wdate" EnableTheming="false" Width="100"></asp:TextBox>至
            <asp:TextBox ID="tbEnd" runat="server" onfocus="WdatePicker()"  CssClass="Wdate" EnableTheming="false" Width="100"></asp:TextBox>
            <asp:Button ID="btnSear" runat="server" Width="100" Text="查询" OnClick="btnsear_Click" />
        </div>
    </div>
    <cc1:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" SelectBoxKeyName="SelectBox"
        ShowSelectBox="True">
        <EmptyDataTemplate>
            没有满足条件的日志信息！
        </EmptyDataTemplate>
        <Columns>
            <asp:TemplateField HeaderText="序号">
                <itemstyle cssclass="center" width="40" />
                <itemtemplate>
<%# Container.DataItemIndex   + Pager1.Size*(Pager1.Index-1) + 1 %>
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="名称">
                <itemstyle cssclass="center" width="100px" />
                <itemtemplate>
<%# Eval("acc_Name","{0}")%> 
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="操作时间">
                <itemstyle cssclass="center" />
                <itemtemplate>
                
<%# Eval("Log_Time","{0}")%> 
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="菜单项">
                <itemstyle cssclass="center" />
                <itemtemplate>
                
<%# Eval("Log_MenuName", "{0}")%> 
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="浏览器">
                <itemstyle cssclass="center" />
                <itemtemplate>
               
<%# Eval("Log_Browser","{0}")%>
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="操作系统">
                <itemstyle cssclass="center" width="120px" />
                <itemtemplate>
                <%# Eval("Log_OS","{0}")%>
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="来访IP">
                <itemstyle cssclass="center" width="120px" />
                <itemtemplate>
                <%# Eval("Log_IP","{0}")%>
</itemtemplate>
            </asp:TemplateField>
        </Columns>
    </cc1:GridView>
    <uc2:Pager ID="Pager1" runat="server" Size="15" OnPageChanged="BindData" />
</asp:Content>
