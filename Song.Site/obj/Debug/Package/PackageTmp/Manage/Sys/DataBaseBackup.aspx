<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    Codebehind="DataBaseBackup.aspx.cs" Inherits="Song.Site.Manage.Sys.DataBaseBackup"
    Title="数据库备份" %>

<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
  <div id="header">  <uc1:toolsBar ID="ToolsBar1" runat="server" WinPath="DataBaseBackup_Edit.aspx" AddButtonOpen="false"
         WinWidth="600" WinHeight="400"  RecoverButtonVisible="true" IsBatchReco="false" GvName="GridView1" OnDelete="DeleteEvent" OnRecover="RecoverEvent"
        OnAdd="AddEvent" DelShowMsg="注：系统备份无法删除！" /></div>
    <cc1:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
        SelectBoxKeyName="SelectBox" ShowSelectBox="True">
        <EmptyDataTemplate>
            没有任何数据库备份！
        </EmptyDataTemplate>
        <Columns>
            <asp:TemplateField HeaderText="序号">
                <itemstyle cssclass="center" width="40" />
                <itemtemplate>
<%# Container.DataItemIndex   +  1 %>
</itemtemplate>
            </asp:TemplateField>
             <asp:TemplateField HeaderText="操作">
                <itemtemplate>
<cc1:RowDelete id="btnDel" onclick="btnDel_Click" runat="server"></cc1:RowDelete> 
<cc1:RowRecover id="btnRecover" runat="server"  onclick="btnRecover_Click" ></cc1:RowRecover> 
</itemtemplate>
                <itemstyle cssclass="center" width="44px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="类别">
                <itemstyle cssclass="center" />
                <itemtemplate>
<%# Eval("type","<span style=\"color:red\">{0}</span>")%>
</itemtemplate>
                <footerstyle width="100px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="备份名称">
                <itemstyle cssclass="left" />
                <itemtemplate>
<%# Eval("file")%>
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="备份时间">
                <itemstyle cssclass="center" />
                <itemtemplate>
<%# Eval("time")%>
</itemtemplate>
                <footerstyle width="200px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="文件大小">
                <itemstyle cssclass="center" />
                <itemtemplate>
<%# Eval("size","{0} Kb")%>
</itemtemplate>
                <footerstyle width="100px" />
            </asp:TemplateField>
        
        </Columns>
    </cc1:GridView>
</asp:Content>
