<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    CodeBehind="Accounts.aspx.cs" Inherits="Song.Site.Manage.Sys.Accounts" Title="账户管理" %>

<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Src="../Utility/Pager2.ascx" TagName="Pager" TagPrefix="uc2" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <div id="header">
        <uc1:toolsBar ID="ToolsBar1" runat="server" WinPath="Accounts_Edit.aspx" AddButtonOpen="true"
            AddButtonVisible="false" WinWidth="640" WinHeight="480" GvName="GridView1" OnDelete="DeleteEvent"
            DelButtonVisible="false" OutputButtonVisible="true" />
         <asp:Panel ID="searchBox" CssClass="searchBox" runat="server">
            <asp:DropDownList ID="ddlOrgin" runat="server">
            </asp:DropDownList>
            账号：<asp:TextBox ID="tbAccname" runat="server" Width="80" MaxLength="30"></asp:TextBox>&nbsp;
            姓名：<asp:TextBox ID="tbName" runat="server" Width="80" MaxLength="30"></asp:TextBox>&nbsp;
            手机：<asp:TextBox ID="tbMobitel" runat="server" Width="80" MaxLength="30"></asp:TextBox>&nbsp;
            <asp:Button ID="btnSear" runat="server" Width="100" Text="查询" OnClick="btnsear_Click" />
        </asp:Panel>
    </div>
    <cc1:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" SelectBoxKeyName="SelectBox"
        ShowSelectBox="false">
        <EmptyDataTemplate>
            没有任何账户
        </EmptyDataTemplate>
        <Columns>
            <asp:TemplateField HeaderText="序号">
                <ItemStyle CssClass="center" Width="40" />
                <ItemTemplate>
                    <%# Container.DataItemIndex   + 1 %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="">
                <ItemTemplate>
                    <cc1:RowDelete ID="btnDel" OnClick="btnDel_Click" runat="server" Visible="true">
                    </cc1:RowDelete>
                    <cc1:RowEdit ID="btnEdit" runat="server"></cc1:RowEdit>
                </ItemTemplate>
                <ItemStyle CssClass="center" Width="50px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="账号">
                <ItemStyle CssClass="center" Width="150px" />
                <ItemTemplate>
                    <%# Eval("Ac_Accname")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="姓名">
                <ItemStyle CssClass="center" Width="80px" />
                <ItemTemplate>
                    <%# Eval("Ac_name")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="性别">
                <ItemStyle CssClass="center" Width="60px" />
                <ItemTemplate>
                    <%# Eval("Ac_Sex", "{0}") == "1" ? "男" : ""%>
                    <%# Eval("Ac_Sex", "{0}") == "2" ? "女" : ""%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="手机号1">
                <ItemStyle CssClass="center" Width="80px" />
                <ItemTemplate>
                    <%# Eval("Ac_MobiTel1")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="手机号2">
                <ItemStyle CssClass="center" Width="80px" />
                <ItemTemplate>
                    <%# Eval("Ac_MobiTel2")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="教师">
                <ItemStyle CssClass="center" Width="80px" />
                <ItemTemplate>
                    <%# Eval("Ac_IsTeacher", "{0}") == "True" ? "教师" : ""%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="机构">
                <ItemStyle CssClass="center" />
                <ItemTemplate>
                    <%# GetOrgin(Eval("Org_ID"))%>
                </ItemTemplate>
            </asp:TemplateField>
             <asp:TemplateField HeaderText="启用/审核">
                <ItemStyle CssClass="center" Width="120px" />
                <ItemTemplate>
                    <cc1:StateButton ID="sbUse" OnClick="sbUse_Click" runat="server" TrueText="启用" FalseText="禁用"
                        State='<%# Eval("Ac_IsUse","{0}")=="True"%>'></cc1:StateButton>/<cc1:StateButton ID="sbPass" 
                        OnClick="sbPass_Click" runat="server" TrueText="通过" FalseText="未审"
                        State='<%# Eval("Ac_IsPass","{0}")=="True"%>'></cc1:StateButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </cc1:GridView>
    <uc2:Pager ID="Pager1" runat="server" Size="15" OnPageChanged="BindData" />
</asp:Content>
