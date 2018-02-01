<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    Codebehind="AddressBook.aspx.cs" Inherits="Song.Site.Manage.Common.AddressBook"
    Title="无标题页" %>

<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Src="../Utility/Pager.ascx" TagName="Pager" TagPrefix="uc2" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
     <div id="header"> <uc1:toolsBar ID="ToolsBar1" runat="server" WinPath="AddressBook_Edit.aspx" GvName="GridView1"
        WinWidth="640" WinHeight="480" IsWinOpen="true" OnDelete="DeleteEvent"  InputButtonVisible="true" />
    <div class="searchBox">
        分类：<asp:DropDownList ID="ddlTpye" runat="server" Width="140">
        </asp:DropDownList>
        姓名：<asp:TextBox ID="tbSear" runat="server" Width="115" MaxLength="10"></asp:TextBox>&nbsp;
        <asp:Button ID="btnSear" runat="server" Width="100" Text="查询" OnClick="BindData" />
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
            <asp:TemplateField HeaderText="性别">
                <itemstyle cssclass="center" width="40" />
                <itemtemplate>
<%# Eval("Adl_Sex", "{0}") == "0" ? "未知" : (Eval("Adl_Sex", "{0}") == "1" ? "男" : "女")%>
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="名称">
                <itemstyle cssclass="center" />
                <itemtemplate>
<%# Eval("Adl_Name")%>
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="分类">
                <itemstyle cssclass="center" />
                <itemtemplate>
<%# Eval("Ads_Name")%>
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="所在单位">
                <itemstyle cssclass="center" />
                <itemtemplate>
<%# Eval("Adl_Company")%>
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="办公电话">
                <itemstyle cssclass="center" width="100" />
                <itemtemplate>
<%# Eval("Adl_CoTel")%>
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="移动电话">
                <itemstyle cssclass="center" width="100" />
                <itemtemplate>
                <a href="#" onclick="OpenWin('../sms/SMSSendSingle.aspx?id=<%# Eval("Adl_Id") %>','给（<%# Eval("Adl_Name") %>）发送短信',640,365);return false;" title="点击发送短信"><%# Eval("Adl_MobileTel")%></a>

</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Email">
                <itemstyle cssclass="center" width="150" />
                <itemtemplate>
<%# Eval("Adl_Email")%>
</itemtemplate>
            </asp:TemplateField>
        </Columns>
    </cc1:GridView>
    <uc2:Pager ID="Pager1" runat="server" Size="15" OnPageChanged="BindData" />
</asp:Content>
