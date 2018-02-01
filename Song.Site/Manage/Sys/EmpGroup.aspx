<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    Codebehind="EmpGroup.aspx.cs" Inherits="Song.Site.Manage.Sys.EmpGroup" Title="无标题页" %>

<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
  <div id="header">  <uc1:toolsBar ID="ToolsBar1" runat="server" WinPath="EmpGroup_Edit.aspx" AddButtonOpen="true"
         WinWidth="600" WinHeight="400" GvName="GridView1" OnDelete="DeleteEvent" /></div>
    <cc1:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
        SelectBoxKeyName="SelectBox" ShowSelectBox="True">
        <EmptyDataTemplate>
            还没有创建用户组！
        </EmptyDataTemplate>
        <Columns>
            <asp:TemplateField HeaderText="序号">
                <itemstyle cssclass="center" width="40" />
                <itemtemplate>
<%# Container.DataItemIndex   +  1 %>
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="操作">
                <itemtemplate>
<cc1:RowDelete id="btnDel" onclick="btnDel_Click" runat="server"></cc1:RowDelete> 
<cc1:RowEdit id="btnEdit" runat="server" ></cc1:RowEdit> 
</itemtemplate>
                <itemstyle cssclass="center" width="44px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="ID">
                <itemstyle cssclass="center" width="40" />
                <itemtemplate>
<%# Eval("EGrp_Id")%>
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="移动">
                <itemstyle cssclass="center" />
                <headerstyle width="100px" />
                <itemtemplate>
<asp:LinkButton id="lbUp" onclick="lbUp_Click" runat="server"  Enabled='<%# Container.DataItemIndex!=0 %>'>上移</asp:LinkButton> 
<asp:LinkButton id="lbDown" onclick="lbDown_Click" runat="server"  Enabled='<%# Container.DataItemIndex+1< ((Song.Entities.EmpGroup[])GridView1.DataSource).Length %>'>下移</asp:LinkButton>

</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="组名称">
                <itemstyle cssclass="center" />
                <itemtemplate>
<%# Eval("EGrp_Name")%> 
<asp:Label ID="Label1" runat="server" Text="*" ForeColor="Red" Visible='<%# Eval("EGrp_IsSystem","{0}")=="True"%>' ToolTip="系统组"></asp:Label>
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="显示">
                <itemstyle cssclass="center" width="60px" />
                <itemtemplate>
<cc1:StateButton id="sbShow" onclick="sbShow_Click" runat="server" TrueText="显示" FalseText="隐藏" State='<%# Eval("EGrp_IsUse","{0}")=="True"%>'></cc1:StateButton> 
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="组成员">
                <itemstyle cssclass="center" />
                <itemtemplate>
<a href="#" onclick="OpenWin('EmpGroup_Member.aspx?id=<%# Eval("EGrp_Id")%>','设置 (<%# Eval("EGrp_Name")%>) 组成员',800,600);return false;">组成员</a>
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="管理">
                <itemstyle cssclass="center" />
                <itemtemplate>
                <a href="#" onclick="OpenWin('purview.aspx?id=<%# Eval("EGrp_Id")%>&type=group','设置组权限',800,600);return false;">组权限</a>

</itemtemplate>
            </asp:TemplateField>
        </Columns>
    </cc1:GridView>
</asp:Content>
