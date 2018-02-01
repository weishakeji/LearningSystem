<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    Codebehind="Vote.aspx.cs" Inherits="Song.Site.Manage.Site.Vote" Title="无标题页" %>

<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Src="../Utility/Pager.ascx" TagName="Pager" TagPrefix="uc2" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
   <div id="header"> <uc1:toolsBar ID="ToolsBar1" runat="server" WinPath="Vote_Edit.aspx" GvName="GridView1"
        WinWidth="800" WinHeight="600" IsWinOpen="true" OnDelete="DeleteEvent" />
    <div class="searchBox">
        标题：<asp:TextBox ID="tbSear" runat="server" Width="115" MaxLength="10"></asp:TextBox>&nbsp;&nbsp;<a
            href="#" type="ClearBtn">清空</a><asp:Button ID="btnSear" runat="server" Width="100"
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
<cc1:RowEdit id="btnEdit" runat="server" ></cc1:RowEdit> 
</itemtemplate>
                <itemstyle cssclass="center" width="44px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="类型">
                <itemstyle cssclass="center" width="60px" />
                <itemtemplate>
<%# Eval("Vt_SelectType","{0}")=="1" ? "单选○" : "多选□"%>
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="ID">
                <itemstyle cssclass="center" width="50px" />
                <itemtemplate>
<%# Eval("Vt_Id","{0}")%>
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="名称/调查内容">
                <itemstyle cssclass="left" />
                <itemtemplate>
                <div class="vtName" title="<%# Eval("Vt_Intro","（调查内容：{0}）")%>">
<%# Eval("Vt_Name")%> <%# Eval("Vt_Intro","（调查内容：{0}）")%> </div>
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="创建时间">
                <itemstyle cssclass="center" width="140px" />
                <itemtemplate>
<%# Eval("Vt_CrtTime")%>
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="显示">
                <itemstyle cssclass="center" width="60px" />
                <itemtemplate>
<cc1:StateButton id="sbShow" onclick="sbShow_Click" runat="server" TrueText="显示" FalseText="隐藏" State='<%# Eval("Vt_IsShow","{0}")=="True"%>'></cc1:StateButton> 
</itemtemplate>
            </asp:TemplateField>
        </Columns>
    </cc1:GridView>
      <uc2:Pager ID="Pager1" runat="server" Size="15" OnPageChanged="BindData" />
</asp:Content>
