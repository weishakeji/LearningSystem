<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    CodeBehind="StudentSort.aspx.cs" Inherits="Song.Site.Manage.Admin.Sort" Title="学生分类" %>
<%@ MasterType VirtualPath="~/Manage/ManagePage.Master" %>
<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Src="../Utility/Pager2.ascx" TagName="Pager" TagPrefix="uc2" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <div id="header">
        <uc1:toolsBar ID="ToolsBar1" runat="server" WinPath="StudentSort_Edit.aspx" GvName="GridView1"
            WinWidth="640" WinHeight="480" OnDelete="DeleteEvent" />
        <asp:Panel ID="searchBox" CssClass="searchBox" runat="server">
            名称：<asp:TextBox ID="tbName" runat="server" Width="100" MaxLength="10"></asp:TextBox>&nbsp;<asp:Button
                ID="btnSear" runat="server" Width="60" Text="查询" OnClick="btnsear_Click" /></asp:Panel>
    </div>
    <cc1:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" SelectBoxKeyName="SelectBox"
        ShowSelectBox="True">
        <EmptyDataTemplate>
            没有满足条件的信息！
        </EmptyDataTemplate>
        <Columns>
            <asp:TemplateField HeaderText="序号">
                <ItemStyle CssClass="center" Width="40" />
                <ItemTemplate>
                    <%# Container.DataItemIndex   + 1 %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="操作">
                <ItemTemplate>
                    <cc1:RowDelete ID="btnDel" OnClick="btnDel_Click" runat="server"></cc1:RowDelete>
                    <cc1:RowEdit ID="btnEdit" runat="server"></cc1:RowEdit>
                </ItemTemplate>
                <ItemStyle CssClass="center" Width="46px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="移动">
                <ItemStyle CssClass="center" />
                <HeaderStyle Width="100px" />
                <ItemTemplate>
                    <asp:LinkButton ID="lbUp" OnClick="lbUp_Click" runat="server" Enabled='<%# Container.DataItemIndex!=0 %>'>上移</asp:LinkButton>
                    <asp:LinkButton ID="lbDown" OnClick="lbDown_Click" runat="server" Enabled='<%# Container.DataItemIndex+1< ((Song.Entities.StudentSort[])GridView1.DataSource).Length %>'>下移</asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="分类名称">
                <ItemStyle CssClass="left" />
                <ItemTemplate>
                    <%# Eval("sts_Name")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="默认">
                <ItemStyle CssClass="center" Width="60px" />
                <ItemTemplate>
                    <cc1:StateButton ID="sbDefault" OnClick="sbDefault_Click" runat="server" TrueText="默认组"
                        FalseText="普通" State='<%# Eval("Sts_IsDefault","{0}")=="True"%>'></cc1:StateButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="使用">
                <ItemStyle CssClass="center" Width="60px" />
                <ItemTemplate>
                    <cc1:StateButton ID="sbUse" OnClick="sbUseClick" runat="server" TrueText="使用" FalseText="禁用"
                        State='<%# Eval("sts_IsUse","{0}")=="True"%>'></cc1:StateButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </cc1:GridView>
    <uc2:Pager ID="Pager1" runat="server" Size="20" OnPageChanged="BindData" />
</asp:Content>
