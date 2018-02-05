<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    Codebehind="Product.aspx.cs" Inherits="Song.Site.Manage.Content.Product"
    Title="无标题页" %>

<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Src="../Utility/Pager.ascx" TagName="Pager" TagPrefix="uc2" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <div id="header">
        <uc1:toolsBar ID="ToolsBar1" runat="server" WinPath="Product_Edit.aspx" GvName="GridView1"
            WinWidth="800" WinHeight="600" IsWinOpen="true" OnDelete="DeleteEvent" DelShowMsg="注：\n1、产品并未接直接删除，可以从回收站还原。" />
        <div class="searchBox">
            标题：<asp:TextBox ID="tbSear" runat="server" Width="115" MaxLength="10"></asp:TextBox>&nbsp;
            <asp:CheckBox ID="cbIsRec" runat="server" Text="推荐" state="true" />
            <asp:CheckBox ID="cbIsNew" runat="server" Text="最新" state="true" />
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
            <asp:TemplateField HeaderText="操作">
                <itemtemplate>
<cc1:RowDelete id="btnDel" onclick="btnDel_Click" runat="server"></cc1:RowDelete> 
<cc1:RowEdit id="btnEdit" runat="server" ></cc1:RowEdit> 
</itemtemplate>
                <itemstyle cssclass="center" width="44px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="排序">
                <itemstyle cssclass="center" />
                <headerstyle width="70px" />
                <itemtemplate>
<asp:LinkButton id="lbUp" onclick="lbUp_Click" runat="server"  Enabled='<%# Container.DataItemIndex!=0 %>'>上移</asp:LinkButton> 
<asp:LinkButton id="lbDown" onclick="lbDown_Click" runat="server"  Enabled='<%# Container.DataItemIndex+1< ((Song.Entities.Product[])GridView1.DataSource).Length %>'>下移</asp:LinkButton>

</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="ID">
                <itemstyle cssclass="center" width="40px" />
                <itemtemplate>
<%# Eval("Pd_Id","{0}")%>
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="产品名称">
                <itemstyle cssclass="left" />
                <itemtemplate>              
<%# Eval("Pd_Name", "{0}")%> <span class="crtTime" title="创建时间">[ <%# Eval("Pd_CrtTime", "{0:yyyy-MM-dd}")%> ]</span>
</itemtemplate>
            </asp:TemplateField>
          
            <asp:TemplateField HeaderText="新产品">
                <itemstyle cssclass="center" width="60px" />
                <itemtemplate>
<cc1:StateButton id="sbNew" onclick="sbNew_Click" runat="server" TrueText="非新" FalseText="新产品" State='<%# Eval("Pd_IsNew","{0}")=="False"%>'></cc1:StateButton> 
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="推荐">
                <itemstyle cssclass="center" width="60px" />
                <itemtemplate>
<cc1:StateButton id="sbRec" onclick="sbRec_Click" runat="server" TrueText="非荐" FalseText="推荐" State='<%# Eval("Pd_IsRec","{0}")!="True"%>'></cc1:StateButton> 
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="使用">
                <itemstyle cssclass="center" width="60px" />
                <itemtemplate>
<cc1:StateButton id="sbUse" onclick="sbUse_Click" runat="server" TrueText="使用" FalseText="禁用" State='<%# Eval("Pd_IsUse","{0}")=="True"%>'></cc1:StateButton> 
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="产品咨询">
                <itemstyle cssclass="center" width="80px" />
                <itemtemplate>
                <a href="#" onclick="OpenWin('Product_Message.aspx?id=<%# Eval("Pd_Id")%>','<%# Eval("Pd_Name","咨询：{0}")%>',80,80);return false;">查看</a> 
                </itemtemplate>
            </asp:TemplateField>
        </Columns>
    </cc1:GridView>
    <uc2:Pager ID="Pager1" runat="server" Size="15" OnPageChanged="BindData" />
</asp:Content>
