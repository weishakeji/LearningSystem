<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    Codebehind="DownloadRecycle.aspx.cs" Inherits="Song.Site.Manage.Content.DownloadRecycle"
    Title="无标题页" %>

<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Src="../Utility/Pager.ascx" TagName="Pager" TagPrefix="uc2" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <div id="header">
        <uc1:toolsBar ID="ToolsBar1" runat="server" WinPath="Download_Preview.aspx" GvName="GridView1"
            WinWidth="80" WinHeight="80" IsWinOpen="true" OnDelete="DeleteEvent" OnRecover="RecoverEvent"
            AddButtonVisible="false" ModifyButtonVisible="false" IsBatchReco="true" RecoverButtonVisible="true"
            DelShowMsg="注：\n资源将彻底删除，删除后无法恢复。" />
        <div class="searchBox">
            名称：<asp:TextBox ID="tbSear" runat="server" Width="115" MaxLength="10"></asp:TextBox>&nbsp;<asp:DropDownList
                ID="ddlColumn" runat="server" Width="150">
            </asp:DropDownList><asp:Button ID="btnSear" runat="server" Width="100" Text="查询"
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
            <asp:TemplateField HeaderText="标题">
                <itemstyle cssclass="left" />
                <itemtemplate>
 <%# Eval("Di_Name", "{0}")%> 
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="所在分类">
                <itemstyle cssclass="center" width="80px" />
                <itemtemplate>              
<%# Eval("Dc_Name")%>
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="作者">
                <itemstyle cssclass="center" width="80px" />
                <itemtemplate>
             <%# Eval("Acc_Name", "{0}") %>  

</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="创建时间">
                <itemstyle cssclass="center" width="120px" />
                <itemtemplate>
               
<%# Eval("Di_CrtTime", "{0}")%>
</itemtemplate>
            </asp:TemplateField>
        </Columns>
    </cc1:GridView>
    <uc2:Pager ID="Pager1" runat="server" Size="15" OnPageChanged="BindData" />
</asp:Content>
