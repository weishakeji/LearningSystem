<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    Codebehind="Download.aspx.cs" Inherits="Song.Site.Manage.Content.Download"
    Title="无标题页" %>

<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Src="../Utility/Pager.ascx" TagName="Pager" TagPrefix="uc2" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <div id="header">
        <uc1:toolsBar ID="ToolsBar1" runat="server" WinPath="Download_Edit.aspx" GvName="GridView1"
            WinWidth="800" WinHeight="600" IsWinOpen="true" OnDelete="DeleteEvent" DelShowMsg="注：\n1、资源并未直接删除，可以从回收站恢复。" />
        <div class="searchBox">
            名称：<asp:TextBox ID="tbSear" runat="server" Width="115" MaxLength="10"></asp:TextBox>&nbsp;<asp:CheckBox ID="cbIsHot" runat="server" Text="热点" />
            <asp:CheckBox ID="cbIsTop" runat="server" Text="置顶" />
            <asp:CheckBox ID="cbIsRec" runat="server" Text="推荐" /><asp:Button ID="btnSear" runat="server" Width="100" Text="查询"
                OnClick="btnsear_Click" />
        </div>
    </div>
    <cc1:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" SelectBoxKeyName="SelectBox"
        ShowSelectBox="True">
        <EmptyDataTemplate>
            没有满足条件的资源信息！
        </EmptyDataTemplate>
        <Columns>
               <asp:TemplateField HeaderText="操作">
                <itemtemplate>
<cc1:RowDelete id="btnDel" onclick="btnDel_Click" runat="server"></cc1:RowDelete> 
<cc1:RowEdit id="btnEdit" runat="server" ></cc1:RowEdit> 
</itemtemplate>
                <itemstyle cssclass="center" width="44px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="ID">
                <itemstyle cssclass="center" width="50px" />
                <itemtemplate>
<%# Eval("Dl_Id","{0}")%>
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="资源名称">
                <itemstyle cssclass="left" />
                <itemtemplate>
              <a href="<%# Eval("Dl_FilePath", "{0}")%>" target=_blank><%# Eval("Dl_Name", "{0}")%> </a>
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="文件大小">
                <itemstyle cssclass="center" width="80px" />
                <itemtemplate>              
<span class="diSize"><%# Eval("Dl_Size")%></span>
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="创建时间">
                <itemstyle cssclass="center" width="150px" />
                <itemtemplate>              
<%# Eval("Dl_CrtTime")%>
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="显示">
                <itemstyle cssclass="center" width="60px" />
                <itemtemplate>
<cc1:StateButton id="sbShow" onclick="sbShow_Click" runat="server" TrueText="显示" FalseText="隐藏" State='<%# Eval("Dl_IsShow","{0}")=="True"%>'></cc1:StateButton> 
</itemtemplate>
            </asp:TemplateField>
        </Columns>
    </cc1:GridView>
    <uc2:Pager ID="Pager1" runat="server" Size="15" OnPageChanged="BindData" />
</asp:Content>
