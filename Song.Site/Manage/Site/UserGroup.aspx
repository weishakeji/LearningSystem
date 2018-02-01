<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    Codebehind="UserGroup.aspx.cs" Inherits="Song.Site.Manage.Site.UserGroup" Title="无标题页" %>

<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <uc1:toolsBar ID="ToolsBar1" runat="server" WinPath="UserGroup_Edit.aspx" AddButtonOpen="true"
        GvName="GridView1" OnDelete="DeleteEvent" DelShowMsg="注:\n1、当组包含用户时，无法删除; \n2、默认组无法删除" />
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
            <asp:TemplateField HeaderText="移动">
                <itemstyle cssclass="center"/>
                <headerstyle width="60px" />
                <itemtemplate>
<asp:LinkButton id="lbUp" onclick="lbUp_Click" runat="server"  Enabled='<%# Container.DataItemIndex!=0 %>'>上移</asp:LinkButton> 
<asp:LinkButton id="lbDown" onclick="lbDown_Click" runat="server"  Enabled='<%# Container.DataItemIndex+1< ((Song.Entities.UserGroup[])GridView1.DataSource).Length %>'>下移</asp:LinkButton>

</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="组名称">
                <itemstyle cssclass="center" />
                <itemtemplate>
<%# Eval("UGrp_Name")%><%# Eval("UGrp_IsDefault","{0}")=="True" ? "[默认]" : ""%>
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="状态">
                <itemstyle cssclass="center" width="60px" />
                <itemtemplate>
<cc1:StateButton id="sbShow" onclick="sbShow_Click" runat="server" TrueText="使用" FalseText="禁用" State='<%# Eval("UGrp_IsUse","{0}")=="True"%>'></cc1:StateButton> 
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="组成员">
                <itemstyle cssclass="center" width="80px" />
                <itemtemplate>
<a href="#" onclick="OpenWin('EmpGroup_Member.aspx?id=<%# Eval("UGrp_Id")%>','设置 (<%# Eval("UGrp_Name")%>) 组成员',80,80);return false;">组成员</a>
</itemtemplate>
            </asp:TemplateField>           
        </Columns>
    </cc1:GridView>
</asp:Content>
