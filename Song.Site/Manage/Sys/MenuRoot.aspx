<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    Codebehind="MenuRoot.aspx.cs" Inherits="Song.Site.Manage.Sys.MenuRoot" Title="菜单管理" %>

<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
   <div id="header"> <uc1:toolsBar ID="ToolsBar1" runat="server" WinPath="MenuRoot_Edit.aspx" AddButtonOpen="true"
        GvName="GridView1" OnDelete="DeleteEvent" DelShowMsg="注：删除菜单树，其下所有菜单项将全部删除！" /></div>
    <cc1:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
        SelectBoxKeyName="SelectBox" ShowSelectBox="True">
        <EmptyDataTemplate>
            还没有创建菜单树！
        </EmptyDataTemplate>
        <Columns>
            <asp:TemplateField HeaderText="序号">
                <itemstyle cssclass="center" width="40" />
                <itemtemplate>
<%# Container.DataItemIndex   +  1 %>
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="移动">
                <itemstyle cssclass="center" />
                <headerstyle width="100px" />
                <itemtemplate>
<asp:LinkButton id="lbUp" onclick="lbUp_Click" runat="server"  Enabled='<%# Container.DataItemIndex!=0 %>'>上移</asp:LinkButton> 
<asp:LinkButton id="lbDown" onclick="lbDown_Click" runat="server"  Enabled='<%# Container.DataItemIndex+1< ((Song.Entities.ManageMenu[])GridView1.DataSource).Length %>'>下移</asp:LinkButton>

</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="菜单树名称">
                <itemstyle cssclass="center" />
                <itemtemplate>
<%# Eval("MM_Name")%>
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="标识">
                <itemstyle cssclass="center" />
                <itemtemplate>
<%# Eval("MM_Marker")%>
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="显示">
                <itemstyle cssclass="center" width="60px" />
                <itemtemplate>
<cc1:StateButton id="sbShow" onclick="sbShow_Click" ToolTip="禁用后该菜单不再显示，包括超管" runat="server" TrueText="显示" FalseText="隐藏" State='<%# Eval("MM_IsShow","{0}")=="True"%>'></cc1:StateButton> 
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="使用">
                <itemstyle cssclass="center" width="60px" />
                <itemtemplate>
<cc1:StateButton id="sbUse" onclick="sbUse_Click" ToolTip="禁用后无法进行权限分配，但超管仍然可见该菜单" runat="server" TrueText="使用" FalseText="禁用" State='<%# Eval("MM_IsUse","{0}")=="True"%>'></cc1:StateButton> 
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="管理">
                <itemstyle cssclass="center" />
                <itemtemplate>
<a href="#" onclick="OpenWin('MenuTree.aspx?id=<%# Eval("MM_Id")%>','编辑菜单树',1000,80);return false;">管理树</a>
</itemtemplate>
            </asp:TemplateField>
        </Columns>
    </cc1:GridView>
</asp:Content>
