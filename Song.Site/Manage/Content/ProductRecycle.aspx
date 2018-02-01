<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    Codebehind="ProductRecycle.aspx.cs" Inherits="Song.Site.Manage.Content.Recycle"
    Title="无标题页" %>

<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Src="../Utility/Pager.ascx" TagName="Pager" TagPrefix="uc2" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <div id="header">
        <uc1:toolsBar ID="ToolsBar1" runat="server" WinPath="Product_Preview.aspx" GvName="GridView1"
            WinWidth="800" WinHeight="600" IsWinOpen="true" OnDelete="DeleteEvent" OnRecover="RecoverEvent"
            AddButtonVisible="false" ModifyButtonVisible="false" IsBatchReco="true" RecoverButtonVisible="true"
            DelShowMsg="注：\n产品将彻底删除，删除后无法恢复。" />
        <div class="searchBox">
            标题：<asp:TextBox ID="tbSear" runat="server" Width="115" MaxLength="10"></asp:TextBox>&nbsp;<cc1:DropDownTree
                ID="ddlColumn" runat="server" Width="100" IdKeyName="Ps_Id" ParentIdKeyName="Ps_PatId"
                TaxKeyName="Ps_Tax">
            </cc1:DropDownTree><asp:Button ID="btnSear" runat="server" Width="100" Text="查询"
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
<cc1:RowRecover id="btnRecover" runat="server"  onclick="btnRecover_Click" ></cc1:RowRecover> 
</itemtemplate>
                <itemstyle cssclass="center" width="44px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="标题">
                <itemstyle cssclass="left" />
                <itemtemplate>
 <%# Eval("Pd_Name", "{0}")%> 
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="所在分类">
                <itemstyle cssclass="center" width="120px" />
                <itemtemplate>              
<%# Eval("Ps_Name")%>
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="产品咨询">
                <itemstyle cssclass="center" width="80px" />
                <itemtemplate>
             <a href="#" onclick="OpenWin('Product_Message.aspx?id=<%# Eval("Pd_Id")%>','<%# Eval("Pd_Name","咨询：{0}")%>',80,80);return false;">查看</a>

</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="创建时间">
                <itemstyle cssclass="center" width="140px" />
                <itemtemplate>
               
<%# Eval("Pd_CrtTime", "{0}")%>
</itemtemplate>
            </asp:TemplateField>
        </Columns>
    </cc1:GridView>
    <uc2:Pager ID="Pager1" runat="server" Size="15" OnPageChanged="BindData" />
</asp:Content>
