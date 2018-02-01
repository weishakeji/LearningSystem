<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    Codebehind="Position.aspx.cs" Inherits="Song.Site.Manage.Sys.Position" Title="无标题页" %>

<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
   <div id="header"> <uc1:toolsBar ID="ToolsBar1" runat="server" WinPath="Position_Edit.aspx" AddButtonOpen="true"
         WinWidth="600" WinHeight="400" GvName="GridView1" OnDelete="DeleteEvent" DelShowMsg="注:\n超管角色无法删除！" />
         </div>
    <cc1:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
        SelectBoxKeyName="SelectBox" ShowSelectBox="True">
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
            <asp:TemplateField HeaderText="操作">
                <itemtemplate>
<cc1:RowDelete id="btnDel" onclick="btnDel_Click" runat="server"></cc1:RowDelete> 
<cc1:RowEdit id="btnEdit" runat="server" ></cc1:RowEdit> 
</itemtemplate>
                <itemstyle cssclass="center" width="44px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="移动">
                <itemstyle cssclass="center"/>
                <headerstyle width="100px" />
                <itemtemplate>
<asp:LinkButton id="lbUp" onclick="lbUp_Click" runat="server"  Enabled='<%# Container.DataItemIndex!=0 %>'>上移</asp:LinkButton> 
<asp:LinkButton id="lbDown" onclick="lbDown_Click" runat="server"  Enabled='<%# Container.DataItemIndex+1< ((Song.Entities.Position[])GridView1.DataSource).Length %>'>下移</asp:LinkButton>

</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="岗位名称">
                <itemstyle cssclass="center" />
                <itemtemplate>
<%# Eval("Posi_Name")%><%# Eval("Posi_IsAdmin","{0}")=="True" ? "[管理]" : ""%>
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="使用">
                <itemstyle cssclass="center" width="60px" />
                <itemtemplate>
<cc1:StateButton id="sbShow" onclick="sbShow_Click" runat="server" TrueText="使用" FalseText="禁用" State='<%# Eval("Posi_IsUse","{0}")=="True"%>' Visible='<%# Eval("Posi_IsAdmin","{0}")!="True"%>'></cc1:StateButton> 
</itemtemplate>
            </asp:TemplateField>
              <asp:TemplateField HeaderText="成员">
                <itemstyle cssclass="center" />
                <itemtemplate>
<a href="#" style='display:<%# Eval("Posi_IsAdmin","{0}")=="True" ? "none" : "block" %>' onclick="OpenWin('Position_Member.aspx?id=<%# Eval("Posi_Id")%>','设置隶属于岗位(<%# Eval("Posi_Name")%>)的员工',800,600);return false;">设置</a>
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="管理">
                <itemstyle cssclass="center" />
                <itemtemplate>
<a href="#" style='display:<%# Eval("Posi_IsAdmin","{0}")=="True" ? "none" : "block" %>' onclick="OpenWin('purview.aspx?id=<%# Eval("Posi_Id")%>&type=posi','设置角色权限',800,600);return false;">权限</a>
</itemtemplate>
            </asp:TemplateField>
        </Columns>
    </cc1:GridView>
</asp:Content>
