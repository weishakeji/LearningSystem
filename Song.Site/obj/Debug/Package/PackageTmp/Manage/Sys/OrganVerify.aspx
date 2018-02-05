<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    Codebehind="OrganVerify.aspx.cs" Inherits="Song.Site.Manage.Sys.OrganVerify"
    Title="无标题页" %>

<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Src="../Utility/Pager.ascx" TagName="Pager" TagPrefix="uc2" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <div id="header">
        <uc1:toolsBar ID="ToolsBar1" runat="server" WinPath="Organization_Edit.aspx" GvName="GridView1"
            WinWidth="800" WinHeight="600" IsWinOpen="true" OnDelete="DeleteEvent" />
        <div class="searchBox">等级：<asp:DropDownList ID="ddlLevel" runat="server" Width="160"></asp:DropDownList> 
            名称：<asp:TextBox ID="tbSear" runat="server" Width="115" MaxLength="10"></asp:TextBox>&nbsp;
            <asp:Button ID="btnSear" runat="server" Width="100" Text="查询" OnClick="btnsear_Click" />
        </div>
    </div>
    <cc1:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" SelectBoxKeyName="SelectBox"
        ShowSelectBox="True">
        <EmptyDataTemplate>
            没有满足条件的信息！
        </EmptyDataTemplate>
        <Columns>
            <asp:TemplateField HeaderText="操作">
                <itemtemplate>
<cc1:RowDelete id="btnDel" onclick="btnDel_Click" runat="server"></cc1:RowDelete> 
<cc1:RowEdit id="btnEdit" runat="server" ></cc1:RowEdit> 
</itemtemplate>
                <itemstyle cssclass="center" width="44px" />
            </asp:TemplateField>
            <%-- <asp:TemplateField HeaderText="ID">
                <itemstyle cssclass="center" width="40px" />
                <itemtemplate>
              
<%# Eval("Org_ID")%>
</itemtemplate>
            </asp:TemplateField>--%>
            <asp:TemplateField HeaderText="序号">
                <itemstyle cssclass="center" width="40" />
                <itemtemplate>
<%# Container.DataItemIndex   + 1 %>
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="机构名称">
                <itemstyle cssclass="left" />
                <itemtemplate>
               <%# Eval("Org_Name")%>   <%# Convert.ToBoolean(Eval("Org_IsRoot")) ? " - [根机构]" : ""%>                          
</itemtemplate>
            </asp:TemplateField>
          <asp:TemplateField HeaderText="等级">
                <itemstyle cssclass="center" />
                <itemtemplate>
               <%# Eval("Olv_Name")%>                    
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="审核">
                <itemstyle cssclass="center" width="60" />
                <itemtemplate>
              
<cc1:StateButton id="sbPass" onclick="sbPass_Click" runat="server" TrueText="通过" FalseText="未通过" State='<%# Eval("Org_IsPass","{0}")=="True"%>'></cc1:StateButton> 
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="使用">
                <itemstyle cssclass="center" width="60px" />
                <itemtemplate>
<cc1:StateButton id="sbUse" onclick="sbShow_Click" runat="server" TrueText="使用" FalseText="禁用" State='<%# Eval("Org_IsUse","{0}")=="True"%>'></cc1:StateButton> 
</itemtemplate>
            </asp:TemplateField>
        </Columns>
    </cc1:GridView>
</asp:Content>
