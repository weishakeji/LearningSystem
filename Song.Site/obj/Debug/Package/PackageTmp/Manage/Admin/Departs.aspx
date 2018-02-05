<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    CodeBehind="Departs.aspx.cs" Inherits="Song.Site.Manage.Admin.Departs" %>

<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <div id="header">
        <uc1:toolsBar ID="ToolsBar1" runat="server" GvName="GridView1" WinWidth="640" WinHeight="480"
            WinPath="Departs_Edit.aspx" OnDelete="DeleteEvent" DelShowMsg="如果删除院系，下属专业等也会被删除" />
    </div>
    <cc1:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" SelectBoxKeyName="SelectBox"
        ShowSelectBox="True" ShowFooter="false">
        <EmptyDataTemplate>
            没有满足条件的信息！
        </EmptyDataTemplate>
        <Columns>
            <asp:TemplateField HeaderText="序号">
                <ItemTemplate>
                    <%# Container.DataItemIndex   + 1 %>
                </ItemTemplate>
                <ItemStyle CssClass="center" Width="40px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="操作">
                <ItemTemplate>
                    <cc1:RowDelete ID="btnDel" OnClick="btnDel_Click" runat="server"></cc1:RowDelete>
                    <cc1:RowEdit ID="btnEdit" runat="server"></cc1:RowEdit>
                </ItemTemplate>
                <HeaderStyle CssClass="center noprint" />
                <ItemStyle CssClass="center noprint" Width="44px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="移动">
                <ItemStyle CssClass="center" />
                <HeaderStyle Width="40px" />
                <ItemTemplate>
                    <asp:LinkButton ID="lbUp" OnClick="lbUp_Click" runat="server">&#8593; </asp:LinkButton>
                    <asp:LinkButton ID="lbDown" OnClick="lbDown_Click" runat="server">&#8595;</asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="名称">
                <ItemTemplate>
                    <%# Eval("Dep_cnName")%>
                </ItemTemplate>
                <ItemStyle CssClass="left" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="状态">
                <ItemTemplate>
                    <cc1:StateButton ID="StateButton1" OnClick="sbUse_Click" runat="server" TrueText="使用"
                        FalseText="禁用" ToolTip="禁用将无法再选择" State='<%# Eval("Dep_IsUse","{0}")=="True"%>'></cc1:StateButton>
                    /
                    <cc1:StateButton ID="sbUse" OnClick="sbShow_Click" runat="server" TrueText="显示" FalseText="隐藏"
                        ToolTip="隐藏后将不再从前端展示" State='<%# Eval("Dep_IsShow","{0}")=="True"%>'></cc1:StateButton>
                </ItemTemplate>
                <HeaderStyle CssClass="center noprint" />
                <ItemStyle CssClass="center noprint" Width="80px" />
            </asp:TemplateField>
        </Columns>
    </cc1:GridView>
</asp:Content>
