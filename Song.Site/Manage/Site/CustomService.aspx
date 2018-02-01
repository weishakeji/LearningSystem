<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    Codebehind="CustomService.aspx.cs" Inherits="Song.Site.Manage.Site.CustomService"
    Title="无标题页" %>

<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Src="../Utility/Pager.ascx" TagName="Pager" TagPrefix="uc2" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <uc1:toolsBar ID="ToolsBar1" runat="server" WinPath="CustomService_Edit.aspx" GvName="GridView1"
        WinWidth="600" WinHeight="400" IsWinOpen="true" OnDelete="DeleteEvent" />
    <cc1:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
        SelectBoxKeyName="SelectBox" ShowSelectBox="True">
        <EmptyDataTemplate>
            没有满足条件的信息！
        </EmptyDataTemplate>
        <Columns>
            <asp:TemplateField HeaderText="序号">
                <itemstyle cssclass="center" width="40" />
                <itemtemplate>
<%# Container.DataItemIndex   +  1 %>
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="员工姓名">
                <itemstyle cssclass="left" />
                <itemtemplate>
                <%# Eval("acc_Name")%>
                 <span class="mobileTel" title="员工移动电话"><%# Eval("acc_MobileTel")%></span>

</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="帐号">
                <itemstyle cssclass="center" width="120px" />
                <itemtemplate>
               
<%# Eval("acc_accName","{0}")%>
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="所在院系">
                <itemstyle cssclass="center" width="100px" />
                <itemtemplate>
               
<%# Eval("Dep_CnName", "{0}")%>
</itemtemplate>
            </asp:TemplateField>
        </Columns>
    </cc1:GridView>
</asp:Content>
