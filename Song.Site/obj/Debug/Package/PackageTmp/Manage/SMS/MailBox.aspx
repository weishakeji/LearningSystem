<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    Codebehind="MailBox.aspx.cs" Inherits="Song.Site.Manage.SMS.MailBox" Title="无标题页" %>

<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Src="../Utility/Pager.ascx" TagName="Pager" TagPrefix="uc2" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
     <div id="header"> <uc1:toolsBar ID="ToolsBar1" runat="server" WinPath="SMSSend.aspx" GvName="GridView1"
        WinWidth="640" WinHeight="480" IsWinOpen="true" OnDelete="DeleteEvent" />
    <div class="searchBox">
        内容：<asp:TextBox ID="tbSear" runat="server" Width="115" MaxLength="10"></asp:TextBox>&nbsp;&nbsp;<a
            href="#" type="ClearBtn">清空</a>
        <asp:Button ID="btnSear" runat="server" Width="100" Text="查询" OnClick="BindData" />
    </div></div>
    <cc1:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
        SelectBoxKeyName="SelectBox" ShowSelectBox="True">
        <EmptyDataTemplate>
            没有满足条件的信息！
        </EmptyDataTemplate>
        <Columns>
            <asp:TemplateField HeaderText="序号">
                <itemstyle cssclass="center" width="40" />
                <itemtemplate>
<%# Container.DataItemIndex   + Pager1.Size*(Pager1.Index-1) + 1 %>
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="内容">
                <itemstyle cssclass="center"/>
                <itemtemplate>
<%# Eval("Sms_Context", "{0}")%>
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="发送目标">
                <itemstyle cssclass="center"  width="80" />
                <itemtemplate>
<%# getType(Eval("Sms_Type", "{0}"))%>
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="发送对象">
                <itemstyle cssclass="center"  width="100" />
                <itemtemplate>
<%# Eval("Sms_SendName", "{0}")%>
</itemtemplate>
            </asp:TemplateField>
        </Columns>
    </cc1:GridView>
    <br />
    <uc2:Pager ID="Pager1" runat="server" Size="15" OnPageChanged="BindData" />
</asp:Content>
