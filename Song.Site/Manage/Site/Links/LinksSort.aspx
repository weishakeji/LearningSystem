<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    Codebehind="LinksSort.aspx.cs" Inherits="Song.Site.Manage.Site.Links.LinksSort"
    Title="无标题页" %>

<%@ Register Src="../../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Src="../../Utility/Pager.ascx" TagName="Pager" TagPrefix="uc2" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <div id="header">
        <uc1:toolsBar ID="ToolsBar1" runat="server" WinPath="LinksSort_Edit.aspx" GvName="GridView1"
            WinWidth="600" WinHeight="450" IsWinOpen="true" OnDelete="DeleteEvent" DelShowMsg="注：\n删除友情链接分类后，该分类下的链接项也会被会部删除，且无法恢复。" />
        <div class="searchBox">
            名称：<asp:TextBox ID="tbSear" runat="server" Width="115" MaxLength="10"></asp:TextBox>&nbsp;<asp:Button
                ID="btnSear" runat="server" Width="100" Text="查询" OnClick="btnsear_Click" />
        </div>
    </div>
    <cc1:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" SelectBoxKeyName="SelectBox"
        ShowSelectBox="True">
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
            <asp:TemplateField HeaderText="操作">
                <itemtemplate>
<cc1:RowDelete id="btnDel" onclick="btnDel_Click" runat="server"></cc1:RowDelete> 
<cc1:RowEdit id="btnEdit" runat="server" ></cc1:RowEdit> 
</itemtemplate>
                <itemstyle cssclass="center" width="44px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="移动">
                <itemstyle cssclass="center" />
                <headerstyle width="60px" />
                <itemtemplate>
<asp:LinkButton id="lbUp" onclick="lbUp_Click" runat="server"  Enabled='<%# Container.DataItemIndex!=0 %>'>上移</asp:LinkButton> 
<asp:LinkButton id="lbDown" onclick="lbDown_Click" runat="server"  Enabled='<%# Container.DataItemIndex+1< ((Song.Entities.LinksSort[])GridView1.DataSource).Length %>'>下移</asp:LinkButton>

</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="ID">
                <itemstyle cssclass="center" width="50px" />
                <itemtemplate>
<%# Eval("Ls_Id","{0}")%>
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="名称">
                <itemstyle cssclass="center" width="200" />
                <itemtemplate>
<span title="<%# Eval("Ls_Tootip")%>"><%# Eval("Ls_Name")%></span>
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="显示方式">
                <itemstyle cssclass="center" width="60px" />
                <itemtemplate>
<%# Convert.ToBoolean(Eval("Ls_IsImg")) ? "图片" : ""%> <%# Convert.ToBoolean(Eval("Ls_IsText")) ? "文字" : ""%>
</itemtemplate>
            </asp:TemplateField>
           <asp:TemplateField HeaderText="显示">
                <itemstyle cssclass="center" width="68px" />
                <itemtemplate><cc1:StateShow id="sbShow" onclick="sbShow_Click" runat="server" State='<%# Eval("Ls_IsShow","{0}")=="True"%>'></cc1:StateShow> 
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="使用">
                <itemstyle cssclass="center" width="60px" />
                <itemtemplate>
<cc1:StateUse id="sbUse" onclick="sbUse_Click" runat="server" State='<%# Eval("Ls_IsUse","{0}")=="True"%>'></cc1:StateUse> 
</itemtemplate>
            </asp:TemplateField>
        </Columns>
    </cc1:GridView>
    <uc2:Pager ID="Pager1" runat="server" Size="15" OnPageChanged="BindData" />
</asp:Content>
