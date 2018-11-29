<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    CodeBehind="SSOApi.aspx.cs" Inherits="Song.Site.Manage.Site.SSOApi" Title="单点登录" %>

<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Src="../Utility/Pager.ascx" TagName="Pager" TagPrefix="uc2" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <div id="header">
        <uc1:toolsBar ID="ToolsBar1" runat="server" WinPath="SSOApi_Edit.aspx" GvName="GridView1"
            WinWidth="640" WinHeight="80" OnDelete="DeleteEvent" />
        <div class="searchBox">
        </div>
    </div>
    <cc1:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" SelectBoxKeyName="SelectBox"
        ShowSelectBox="True"  IsEncrypt="False">
        <EmptyDataTemplate>
            没有满足条件的信息！
        </EmptyDataTemplate>
        <Columns>
            <asp:TemplateField HeaderText="序号">
                <ItemStyle CssClass="center" Width="40" />
                <ItemTemplate>
                    <%# Container.DataItemIndex   + Pager1.Size*(Pager1.Index-1) + 1 %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="操作">
                <ItemTemplate>
                    <cc1:RowDelete ID="btnDel" OnClick="btnDel_Click" runat="server"></cc1:RowDelete>
                    <cc1:RowEdit ID="btnEdit" runat="server"></cc1:RowEdit>
                </ItemTemplate>
                <ItemStyle CssClass="center" Width="44px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="名称">
                <ItemStyle CssClass="left"/>
                <ItemTemplate>
                    <%# Eval("SSO_Name", "{0}")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="APPID">
                <ItemStyle CssClass="center" Width="250px" />
                <ItemTemplate>
                    <%# Eval("SSO_APPID")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="域">
                <ItemStyle CssClass="center"  />
                <ItemTemplate>
                    <%# Eval("SSO_Domain", "{0}")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="状态">
                <ItemStyle CssClass="center" Width="60px" />
                <ItemTemplate>
                    <cc1:StateButton ID="sbUse" OnClick="sbUse_Click" runat="server" TrueText="启用"
                        FalseText="禁用" State='<%# Eval("SSO_IsUse","{0}")=="True"%>'></cc1:StateButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </cc1:GridView>
    <uc2:Pager ID="Pager1" runat="server" Size="15" OnPageChanged="BindData" />
</asp:Content>
