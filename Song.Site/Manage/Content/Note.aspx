<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    Codebehind="Note.aspx.cs" Inherits="Song.Site.Manage.Content.Note" Title="无标题页" %>

<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Src="../Utility/Pager.ascx" TagName="Pager" TagPrefix="uc2" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <div id="header">
        <uc1:toolsBar ID="ToolsBar1" runat="server" WinPath="Note_Edit.aspx" GvName="GridView1"
            WinWidth="640" WinHeight="480" IsWinOpen="true" OnDelete="DeleteEvent" AddButtonVisible=false />
        <div id="fieldSear" class="searchBox" runat="server">
            检索：<asp:TextBox ID="tbSear" runat="server" Width="100px" MaxLength="10"></asp:TextBox>
            <asp:Button ID="btnSear" runat="server" Width="100" Text="查询" OnClick="btnsear_Click" />
        </div>
    </div>
    <cc1:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" SelectBoxKeyName="SelectBox"
        ShowSelectBox="True">
        <EmptyDataTemplate>
            没有满足条件的信息！
        </EmptyDataTemplate>
        <Columns>
            <asp:TemplateField HeaderText="序号">
                <itemtemplate>
<%# Container.DataItemIndex   + Pager1.Size*(Pager1.Index-1) + 1 %>
</itemtemplate>
                <itemstyle cssclass="center" width="40px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="操作">
                <itemtemplate>
<cc1:RowDelete id="btnDel" onclick="btnDel_Click" runat="server"></cc1:RowDelete> 
<cc1:RowEdit id="btnEdit" runat="server" ></cc1:RowEdit> 
</itemtemplate>
                <itemstyle cssclass="center" width="44px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="ID">
                <itemtemplate>
<%# Eval("Nn_Id","{0}")%>
</itemtemplate>
                <itemstyle cssclass="center" width="50px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="内容">
                <itemtemplate>
                
<%# Eval("Nn_Details", "{0}")%> 
</itemtemplate>
                <itemstyle cssclass="left" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="显示">
                <itemtemplate>
<cc1:StateButton id="sbShow" onclick="sbShow_Click" runat="server" TrueText="显示" FalseText="隐藏" State='<%# Eval("Nn_IsShow","{0}")=="True"%>'></cc1:StateButton> 
</itemtemplate>
                <itemstyle cssclass="center" width="60px" />
            </asp:TemplateField>
        </Columns>
    </cc1:GridView>
    <uc2:Pager ID="Pager1" runat="server" Size="15" OnPageChanged="BindData" />
</asp:Content>
