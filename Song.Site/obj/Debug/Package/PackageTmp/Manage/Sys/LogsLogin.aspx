<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    CodeBehind="LogsLogin.aspx.cs" Inherits="Song.Site.Manage.Sys.LogsLogin" Title="无标题页" %>

<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Src="../Utility/Pager.ascx" TagName="Pager" TagPrefix="uc2" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <div id="header">
        <script language="javascript" src="../Utility/datepicker/WdatePicker.js" type="text/javascript"></script>
        <uc1:toolsBar ID="ToolsBar1" runat="server" GvName="GridView1" WinWidth="600" WinHeight="400"
            OnDelete="DeleteEvent" AddButtonVisible="false" ModifyButtonEnable="false" />
        <div class="searchBox">
            <asp:DropDownList ID="ddlEmp" runat="server" Width="110">
            </asp:DropDownList>
            时间：<asp:TextBox ID="tbStart" runat="server" CssClass="Wdate" EnableTheming="false"
                onfocus="WdatePicker()" Width="100"></asp:TextBox> 至
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
                <ItemStyle CssClass="center" Width="40" />
                <ItemTemplate>
                    <%# Container.DataItemIndex   + Pager1.Size*(Pager1.Index-1) + 1 %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="名称">
                <ItemStyle CssClass="center" Width="100px" />
                <ItemTemplate>
                    <%# Eval("acc_Name","{0}")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="登录时间">
                <ItemStyle CssClass="center" />
                <ItemTemplate>
                    <%# Eval("Log_Time","{0}")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="浏览器">
                <ItemStyle CssClass="center" />
                <ItemTemplate>
                    <%# Eval("Log_Browser","{0}")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="操作系统">
                <ItemStyle CssClass="center" Width="120px" />
                <ItemTemplate>
                    <%# Eval("Log_OS","{0}")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="来访IP">
                <ItemStyle CssClass="center" Width="120px" />
                <ItemTemplate>
                    <%# Eval("Log_IP","{0}")%>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </cc1:GridView>
    <uc2:Pager ID="Pager1" runat="server" Size="15" OnPageChanged="BindData" />
</asp:Content>
