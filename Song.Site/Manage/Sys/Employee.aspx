<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    Codebehind="Employee.aspx.cs" Inherits="Song.Site.Manage.Sys.Employee" Title="员工列表" %>

<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Src="../Utility/Pager.ascx" TagName="Pager" TagPrefix="uc2" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <div id="header">
        <uc1:toolsBar ID="ToolsBar1" runat="server" WinPath="Employee_Edit.aspx" GvName="GridView1"
            WinWidth="800" WinHeight="600" OnDelete="DeleteEvent" DelShowMsg="注：\n管理员角色的员工，无法删除，需要先去除管理员身份。" InputButtonVisible="true" />
        <div class="searchBox">
            <cc1:DropDownTree ID="ddlDepart" runat="server" Width="150" IdKeyName="dep_id"
                ParentIdKeyName="dep_PatId" TaxKeyName="dep_Tax">
            </cc1:DropDownTree>
            <asp:DropDownList ID="ddlIsUse" runat="server">
                <asp:ListItem Value="null">--所有员工--</asp:ListItem>
                <asp:ListItem Value="true">在职</asp:ListItem>
                <asp:ListItem Value="false">离职</asp:ListItem>
            </asp:DropDownList>
            姓名：<asp:TextBox ID="tbSear" runat="server" Width="115" MaxLength="10"></asp:TextBox>&nbsp;<asp:Button ID="btnSear" runat="server" Width="100"
                    Text="查询" OnClick="btnsear_Click" />
        </div>
    </div>
    <cc1:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" SelectBoxKeyName="SelectBox"
        ShowSelectBox="True">
        <EmptyDataTemplate>
            没有满足条件的员工信息！
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
                <itemstyle cssclass="center" width="46px" />
            </asp:TemplateField>
       <%--     <asp:TemplateField HeaderText="ID">
                <itemstyle cssclass="center" width="60px" />
                <itemtemplate>
<%# Eval("acc_id","{0}")%> 
</itemtemplate>
            </asp:TemplateField>--%>
            <asp:TemplateField HeaderText="在职">
                <itemstyle cssclass="center" width="60px" />
                <itemtemplate>
<%# Eval("acc_isUse","{0}")=="True" ? "在职" : "离职"%> 
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="姓名/电话">
                <itemstyle cssclass="left" width="160px" />
                <itemtemplate>
                <%# Eval("acc_Name")%><%# Eval("Posi_Id","{0}") == superid ? "<span style=\"color:Red\" title=\"超级管理员\">*</span>" : ""%>
                 <span class="mobileTel" title="员工移动电话"><%# Eval("acc_MobileTel")%></span>

</itemtemplate>
            </asp:TemplateField>
           
           <%-- <asp:TemplateField HeaderText="所在院系">
                <itemstyle cssclass="center" />
                <itemtemplate>
               
<%# Eval("Dep_CnName", "{0}")%>
</itemtemplate>
            </asp:TemplateField>--%>
            <asp:TemplateField HeaderText="岗位">
                <itemstyle cssclass="center" />
                <itemtemplate>
               
<%# Eval("Posi_Name", "{0}")%>
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="班组">
                <itemstyle cssclass="center" />
                <itemtemplate>
               
<%# Eval("Team_Name", "{0}")%>
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="公开手机">
                <itemstyle cssclass="center" width="100px" />
                <itemtemplate>
               
<cc1:StateButton id="sbOpenMobile" onclick="sbOpenMobile_Click" runat="server" TrueText="公开" FalseText="不公开" State='<%# Eval("Acc_IsOpenMobile","{0}")=="True"%>'></cc1:StateButton> 
</itemtemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="密码">
                <itemstyle cssclass="center" width="60px" />
                <itemtemplate>
                <a href="#" onclick="OpenWin('Employee_Password.aspx?id=<%# Eval("acc_id") %>','重置密码',400,250);return false;">修改</a>
</itemtemplate>
            </asp:TemplateField>
            <%-- <asp:TemplateField HeaderText="查看详细">
                <itemstyle cssclass="center" width="60px" />
                <itemtemplate>
                <a href="#" onclick="OpenWin('../user/userview.aspx?id=<%# Eval("acc_id") %>','查看',400,300);return false;">查看</a>
</itemtemplate>
            </asp:TemplateField>--%>
        </Columns>
    </cc1:GridView>
    <br />
    <uc2:Pager ID="Pager1" runat="server" Size="15" OnPageChanged="BindData" />
</asp:Content>
