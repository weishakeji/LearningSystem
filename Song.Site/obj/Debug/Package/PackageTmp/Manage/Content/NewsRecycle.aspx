<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    Codebehind="NewsRecycle.aspx.cs" Inherits="Song.Site.Manage.Content.NewsRecycle"
    Title="无标题页" %>

<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Src="../Utility/Pager.ascx" TagName="Pager" TagPrefix="uc2" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
     <div id="header"> <uc1:toolsBar ID="ToolsBar1" runat="server" WinPath="News_Preview.aspx" GvName="GridView1"
        WinWidth="80" WinHeight="80" IsWinOpen="true" OnDelete="DeleteEvent" OnRecover="RecoverEvent"
        AddButtonVisible="false" ModifyButtonVisible="false" IsBatchReco="true" RecoverButtonVisible="true"
        DelShowMsg="注：\n新闻将彻底删除，删除后无法恢复。" />
    <div class="searchBox">
        标题：<asp:TextBox ID="tbSear" runat="server" Width="115" MaxLength="10"></asp:TextBox>&nbsp;<cc1:DropDownTree ID="ddlColumn" runat="server" Width="150" IdKeyName="Nc_Id" ParentIdKeyName="Nc_PatId"
        TaxKeyName="Nc_Tax"></cc1:DropDownTree><asp:Button ID="btnSear" runat="server" Width="100"
                Text="查询" OnClick="btnsear_Click" />
   </div></div>
    <cc1:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
        SelectBoxKeyName="SelectBox" ShowSelectBox="True">
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
<cc1:RowRecover id="btnRecover" runat="server"  onclick="btnRecover_Click" ></cc1:RowRecover> 
</itemtemplate>
                <itemstyle cssclass="center" width="44px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="标题">
                <itemstyle cssclass="left" />
                <itemtemplate>
  <span title="图片新闻" style="color:Blue"><%# Eval("Na_IsImg","{0}")=="True" ? "[图]" : ""%></span>
                <span title="热点新闻" style="color:Red"><%# Eval("Na_IsHot", "{0}") == "True" ? "[热]" : ""%></span>
                <span title="推荐新闻" style="color:Green"><%# Eval("Na_IsRec", "{0}") == "True" ? "[荐]" : ""%></span>
                <span title="置顶新闻" style="color:Red"><%# Eval("Na_IsTop", "{0}") == "True" ? "[顶]" : ""%></span>
<a href="#" onclick="OpenWin('News_Preview.aspx?id=<%# Eval("Na_Id")%>','预览',80,80);return false;"><%# Eval("Na_Title", "{0}")%> 
</a>
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="作者">
                <itemstyle cssclass="center" width="80px" />
                <itemtemplate>
             <%# Eval("Na_Author", "{0}") %>  

</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="创建时间">
                <itemstyle cssclass="center" width="140px" />
                <itemtemplate>
               
<%# Eval("Na_CrtTime", "{0}")%>
</itemtemplate>
            </asp:TemplateField>
        </Columns>
    </cc1:GridView>
    <uc2:Pager ID="Pager1" runat="server" Size="15" OnPageChanged="BindData" />
</asp:Content>
