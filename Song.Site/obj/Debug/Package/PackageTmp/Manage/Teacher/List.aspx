<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    CodeBehind="List.aspx.cs" Inherits="Song.Site.Manage.Teacher.List" Title="教师列表" %>

<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Src="../Utility/Pager.ascx" TagName="Pager" TagPrefix="uc2" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <div id="header">
        <uc1:toolsBar ID="ToolsBar1" runat="server" WinPath="List_Edit.aspx" AddButtonVisible="true"
            GvName="GridView1" WinWidth="900" WinHeight="600" OnDelete="DeleteEvent" InputButtonVisible="true"
            OutputButtonVisible="true" OnOutput="outputEvent" />
        <div class="searchBox">
            分组：<asp:DropDownList ID="ddlSort" runat="server" DataTextField="Ths_Name" DataValueField="Ths_ID">
            </asp:DropDownList>
            &nbsp;姓名：<asp:TextBox ID="tbSear" runat="server" Width="115" MaxLength="10"></asp:TextBox>&nbsp;<asp:Button
                ID="btnSear" runat="server" Width="100" Text="查询" OnClick="btnsear_Click" />
        </div>
    </div>
    <cc1:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" SelectBoxKeyName="SelectBox"
        ShowSelectBox="True">
        <EmptyDataTemplate>
            没有满足条件的教师信息！
        </EmptyDataTemplate>
        <Columns>
            <asp:TemplateField HeaderText="序号">
                <ItemStyle CssClass="center" Width="40" />
                <ItemTemplate>
                    <%# Container.DataItemIndex   + Pager1.Size*(Pager1.Index-1) + 1 %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="操作">
                <ItemTemplate>
                    <cc1:RowDelete ID="btnDel" OnClick="btnDel_Click" runat="server"></cc1:RowDelete>
                    <cc1:RowEdit ID="btnEdit" runat="server"></cc1:RowEdit>
                </ItemTemplate>
                <ItemStyle CssClass="center" Width="46px" />
            </asp:TemplateField>
            <%-- <asp:TemplateField HeaderText="ID">
                <ItemStyle CssClass="center" Width="60px" />
                <ItemTemplate>
                    <%# Eval("Th_id","{0}")%>
                </ItemTemplate>
            </asp:TemplateField>--%>
            <asp:TemplateField HeaderText="账号">
                <ItemStyle CssClass="center" Width="60px" />
                <ItemTemplate>
                    <%# Eval("Th_accName","{0}")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="姓名/电话">
                <ItemStyle CssClass="left" />
                <ItemTemplate>
                    <%# Eval("Th_Name")%>
                    <%--<span class="mobileTel" title="员工移动电话">
                        <%# Eval("Th_PhoneMobi")%></span>--%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="职称">
                <ItemStyle CssClass="center" />
                <ItemTemplate>
                    <%# Eval("Ths_Name")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="启用">
                <ItemStyle CssClass="center" Width="60px" />
                <ItemTemplate>
                    <cc1:StateButton ID="sbUse" OnClick="sbUse_Click" runat="server" TrueText="启用" FalseText="禁用"
                        State='<%# Eval("Th_IsUse","{0}")=="True"%>'></cc1:StateButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="审核">
                <ItemStyle CssClass="center" Width="60px" />
                <ItemTemplate>
                    <cc1:StateButton ID="sbPass" OnClick="sbPass_Click" runat="server" TrueText="通过" FalseText="未审"
                        State='<%# Eval("Th_IsPass","{0}")=="True"%>'></cc1:StateButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="显示" Visible=false>
                <ItemStyle CssClass="center" Width="60px" />
                <ItemTemplate>
                    <cc1:StateButton ID="sbShow" OnClick="sbShow_Click" runat="server" TrueText="显示" FalseText="隐藏"
                        State='<%# Eval("Th_IsShow","{0}")=="True"%>'></cc1:StateButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="密码">
                <ItemStyle CssClass="center" Width="60px" />
                <ItemTemplate>
                    <a href="#" onclick="OpenWin('Teacher_Password.aspx?id=<%# Eval("Th_id") %>','重置密码',400,300);return false;">
                        修改</a>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </cc1:GridView>
    <br />
    <uc2:Pager ID="Pager1" runat="server" Size="15" OnPageChanged="BindData" />
</asp:Content>
