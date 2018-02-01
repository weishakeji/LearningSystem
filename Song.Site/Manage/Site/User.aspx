<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    Codebehind="User.aspx.cs" Inherits="Song.Site.Manage.Site.User" Title="员工列表" %>

<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Src="../Utility/Pager.ascx" TagName="Pager" TagPrefix="uc2" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
   <div id="header"> <uc1:toolsBar ID="ToolsBar1" runat="server" WinPath="User_Edit.aspx" GvName="GridView1"
        WinWidth="640" WinHeight="480" OnDelete="DeleteEvent" DelShowMsg="注：\n超级管理员角色的员工，无法删除" />
   <div class="searchBox">
        用户组：<asp:DropDownList ID="ddlGroup" runat="server" Width="150">
        </asp:DropDownList>
        <asp:DropDownList ID="ddlIsUse" runat="server">
            <asp:ListItem Value="null">--所有--</asp:ListItem>
            <asp:ListItem Value="true">使用</asp:ListItem>
            <asp:ListItem Value="false">禁用</asp:ListItem>
        </asp:DropDownList>
        姓名：<asp:TextBox ID="tbSear" runat="server" Width="115" MaxLength="10"></asp:TextBox>&nbsp;<asp:Button ID="btnSear" runat="server" Width="100"
                Text="查询" OnClick="btnsear_Click" />
    </div></div>
    <cc1:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
        SelectBoxKeyName="SelectBox" ShowSelectBox="True">
        <EmptyDataTemplate>
            没有满足条件的用户信息！
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
            <asp:TemplateField HeaderText="用户帐号">
                <itemstyle cssclass="left" />
                <itemtemplate>
                <%# Eval("User_accName","{0}")%>

</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="姓名">
                <itemstyle cssclass="left" />
                <itemtemplate>
            <%# Eval("User_Name")%>

</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="性别">
                <itemstyle cssclass="center" width="60px" />
                <itemtemplate>
            <%# Eval("User_Sex","{0}")=="1" ? "男" : "女"%>

</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="用户组">
                <itemstyle cssclass="center" width="100px" />
                <itemtemplate>
               
<%# Eval("UGrp_Name", "{0}")%>
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="密码">
                <itemstyle cssclass="center" width="60px" />
                <itemtemplate>
                <a href="#" onclick="OpenWin('User_Password.aspx?id=<%# Eval("User_id") %>','重置密码',400,250);return false;">修改</a>
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="查看详细">
                <itemstyle cssclass="center" width="60px" />
                <itemtemplate>
                查看
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="状态">
                <itemstyle cssclass="center" width="60px" />
                <itemtemplate>
<cc1:StateButton id="sbShow" onclick="sbShow_Click" runat="server" TrueText="使用" FalseText="禁用" State='<%# Eval("User_IsUse","{0}")=="True"%>'></cc1:StateButton> 
</itemtemplate>
            </asp:TemplateField>
        </Columns>
    </cc1:GridView>

    <uc2:Pager ID="Pager1" runat="server" Size="15" OnPageChanged="BindData" />
</asp:Content>
