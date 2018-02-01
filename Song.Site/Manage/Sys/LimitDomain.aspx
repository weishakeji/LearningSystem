<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    CodeBehind="LimitDomain.aspx.cs" Inherits="Song.Site.Manage.Sys.LimitDomain"
    Title="二级域名" %>

<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Src="../Utility/Pager.ascx" TagName="Pager" TagPrefix="uc2" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <div id="header">
        <uc1:toolsBar ID="ToolsBar1" runat="server" WinPath="LimitDomain_Edit.aspx" GvName="GridView1"
            WinWidth="600" WinHeight="400" OnDelete="DeleteEvent"/>
             <div class="searchBox">
          
            域名：<asp:TextBox ID="tbSear" runat="server" Width="115" MaxLength="10"></asp:TextBox>&nbsp;<asp:Button ID="btnSear" runat="server" Width="100"
                    Text="查询" OnClick="btnsear_Click" />
        </div>
    </div>
    <cc1:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" SelectBoxKeyName="SelectBox"
        ShowSelectBox="True">
        <EmptyDataTemplate>
            没有满足的信息！
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
                <ItemStyle CssClass="center" Width="46px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="域名">
                <ItemStyle CssClass="center"  width="100px" />
                <ItemTemplate>
                    <%# Eval("LD_Name", "{0}")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="简介">
                <ItemStyle CssClass="center" />
                <ItemTemplate>
                    <%# Eval("LD_Intro", "{0}")%>
                </ItemTemplate>
            </asp:TemplateField>
             <asp:TemplateField HeaderText="使用">
                <itemstyle cssclass="center" width="60px" />
                <itemtemplate>
<cc1:StateButton id="sbIsUse" onclick="sbIsUse_Click" runat="server" TrueText="使用" FalseText="禁用" State='<%# Eval("LD_IsUse","{0}")=="True"%>'></cc1:StateButton> 
</itemtemplate>
            </asp:TemplateField>
        </Columns>
    </cc1:GridView>
    <br />
    <uc2:Pager ID="Pager1" runat="server" Size="15" OnPageChanged="BindData" />
</asp:Content>
