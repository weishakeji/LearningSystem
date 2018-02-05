<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    Codebehind="Organization.aspx.cs" Inherits="Song.Site.Manage.Sys.Organization"
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
                <itemstyle cssclass="center" width="46px" />
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
               <%# Convert.ToBoolean(Eval("Org_IsRoot")) ? "[根]" : ""%> 
               <%# Convert.ToBoolean(Eval("Org_IsDefault")) ? "[默]" : ""%> 
                  <%# Eval("Org_Name")%>                         
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="平台名称">
                <itemstyle cssclass="center" />
                <itemtemplate>
               <%# Eval("Org_PlatformName")%>                    
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="域名">
                <itemstyle cssclass="center" />
                <itemtemplate>
               <a href="http://<%# Eval("Org_TwoDomain","{0}")=="" ? "www" : Eval("Org_TwoDomain")%>.<%=domain %>:<%=port %>" target="_blank"><%# Eval("Org_TwoDomain")%></a>              
</itemtemplate>
            </asp:TemplateField>
          <asp:TemplateField HeaderText="等级">
                <itemstyle cssclass="center" />
                <itemtemplate>
               <%# Eval("Olv_Name")%>                    
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="省市">
                <itemstyle cssclass="center" />
                <itemtemplate>
               <%# Eval("Org_Province")%>  <%# Eval("Org_City")%>                
</itemtemplate>
            </asp:TemplateField>
            
            <asp:TemplateField HeaderText="管理员">
                <itemstyle cssclass="center" width="60" />
                <itemtemplate>
              
<a href="#" class="adminState" orgname="<%# Eval("Org_Name")%>"><%# GetAdminState(Eval("Org_ID"))%></a>
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="启用 / 显示 / 审核">
                <itemstyle cssclass="center" width="160px" />
                <itemtemplate>
<cc1:StateButton id="sbUse" onclick="sbUse_Click" runat="server" TrueText="启用" FalseText="禁用" State='<%# Eval("Org_IsUse","{0}")=="True"%>'></cc1:StateButton> / 
<cc1:StateButton id="sbShow" onclick="sbShow_Click" ToolTip="在网站前端显示" runat="server" TrueText="显示" FalseText="隐藏" State='<%# Eval("Org_IsShow","{0}")=="True"%>'></cc1:StateButton> /
<cc1:StateButton id="sbPass" onclick="sbPass_Click" runat="server" TrueText="通过" FalseText="待审核" State='<%# Eval("Org_IsPass","{0}")=="True"%>'></cc1:StateButton> 
</itemtemplate>
            </asp:TemplateField>
          
            <asp:TemplateField HeaderText="默认">
                <itemstyle cssclass="center" width="60px" />
                <itemtemplate>
<cc1:StateButton id="sbDefualt" onclick="sbDefault_Click" runat="server" TrueText="普通" FalseText="默认机构" State='<%# Eval("Org_IsDefault","{0}")=="False"%>'></cc1:StateButton> 
</itemtemplate>
            </asp:TemplateField>
        </Columns>
    </cc1:GridView>
</asp:Content>
