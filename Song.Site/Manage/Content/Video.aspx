<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    Codebehind="Video.aspx.cs" Inherits="Song.Site.Manage.Content.Video.Info" Title="无标题页" %>

<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Src="../Utility/Pager.ascx" TagName="Pager" TagPrefix="uc2" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <div id="header">
        <uc1:toolsBar ID="ToolsBar1" runat="server" WinPath="Video_Edit.aspx" GvName="GridView1"
            WinWidth="800" WinHeight="600" IsWinOpen="true" OnDelete="DeleteEvent" DelShowMsg="注：\n1、视频分类删除后，该下属视频信息也会被删除，且不可恢复。" />
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
            没有满足条件的视频信息！
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
<%# Eval("Vi_Id","{0}")%>
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="视频名称">
                <itemstyle cssclass="left" />
                <itemtemplate>
              <%# Eval("Vi_Name", "{0}")%> 
</itemtemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="视频宽高">
                <itemstyle cssclass="center" width="80" />
                <itemtemplate>
              <%# Eval("Vi_Width", "{0}")%> × <%# Eval("Vi_Height", "{0}")%> 
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="文件大小">
                <itemstyle cssclass="center" width="80" />
                <itemtemplate>
              <span class="piSize"><%# Eval("Vi_Size", "{0}")%></span>
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="创建时间">
                <itemstyle cssclass="center" width="150px" />
                <itemtemplate>              
<%# Eval("Vi_CrtTime")%>
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="显示">
                <itemstyle cssclass="center" width="60px" />
                <itemtemplate>
<cc1:StateButton id="sbShow" onclick="sbShow_Click" runat="server" TrueText="显示" FalseText="隐藏" State='<%# Eval("Vi_IsShow","{0}")=="True"%>'></cc1:StateButton> 
</itemtemplate>
            </asp:TemplateField>
        </Columns>
    </cc1:GridView>
    <uc2:Pager ID="Pager1" runat="server" Size="15" OnPageChanged="BindData" />
</asp:Content>
