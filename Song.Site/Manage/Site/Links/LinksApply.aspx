<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    Codebehind="LinksApply.aspx.cs" Inherits="Song.Site.Manage.Site.Links.LinksApply"
    Title="无标题页" %>

<%@ Register Src="../../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Src="../../Utility/Pager.ascx" TagName="Pager" TagPrefix="uc2" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <div id="header">
        <uc1:toolsBar ID="ToolsBar1" runat="server" WinPath="LinksInfo_Edit.aspx" GvName="GridView1"
            WinWidth="640" WinHeight="480" IsWinOpen="true" OnDelete="DeleteEvent" />
        <div class="searchBox">
            名称：<asp:TextBox ID="tbSear" runat="server" Width="115" MaxLength="10"></asp:TextBox>&nbsp;<asp:DropDownList
                ID="ddlSort" runat="server" Width="150">
            </asp:DropDownList>&nbsp;<asp:Button ID="btnSear" runat="server" Width="100" Text="查询"
                OnClick="btnsear_Click" />
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
<cc1:RowVerify id="btnVerify"  onclick="btnVeri_Click" runat="server" ></cc1:RowVerify> 
</itemtemplate>
                <itemstyle cssclass="center" width="44px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="网站名称">
                <itemstyle cssclass="left"  width="260px" />
                <itemtemplate>
<a href="<%# Eval("Lk_Url")%>" target="_blank" title="<%# Eval("Lk_Tootip")%>"><%# Eval("Lk_Name")%></a>
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="站长QQ">
                <itemstyle cssclass="left" />
                <itemtemplate>
<%# Eval("Lk_QQ")%>
</itemtemplate>
 <itemstyle cssclass="center" width="80px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="申请说明">
                <itemstyle cssclass="left" />
                <itemtemplate>
                <div class="explain">
<%# Eval("Lk_Explain")%></div>
</itemtemplate>
            </asp:TemplateField>
           
          
        </Columns>
    </cc1:GridView>
    <uc2:Pager ID="Pager1" runat="server" Size="15" OnPageChanged="BindData" />
</asp:Content>
