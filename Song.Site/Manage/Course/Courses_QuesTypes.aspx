<%@ Page Title="" Language="C#" MasterPageFile="~/Manage/Course/CourseEdit.Master"
    AutoEventWireup="true" CodeBehind="Courses_QuesTypes.aspx.cs" Inherits="Song.Site.Manage.Course.Courses_QuesTypes" %>

<%@ MasterType VirtualPath="~/Manage/Course/CourseEdit.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td class="right" width="90px">
                课程名称：
            </td>
            <td>
                <asp:Label ID="lbName" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="right" valign="top">
                题型：
            </td>
            <td>
                <cc1:GridView ID="gvPrice" runat="server" EnableTheming="false" Width="97%" AutoGenerateColumns="False"
                    ShowFooter="false" ShowSelectBox="false">
                    <Columns>
                        <asp:TemplateField HeaderText="序号">
                            <ItemTemplate>
                                <%# Container.DataItemIndex   + 1 %>
                            </ItemTemplate>
                            <ItemStyle CssClass="center" Width="40px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="移动">
                            <ItemTemplate>
                                <asp:LinkButton ID="lbUp" OnClick="lbUp_Click" ToolTip="上移" runat="server">&#8593; </asp:LinkButton>
                                <asp:LinkButton ID="lbDown" OnClick="lbDown_Click" ToolTip="下移" runat="server">&#8595;</asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle CssClass="center" Width="40px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="基础类型">
                            <ItemTemplate>
                                <%# Eval("Qt_TypeName", "{0}")%>
                            </ItemTemplate>
                            <ItemStyle CssClass="center" Width="100px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="试题类型名称">
                            <ItemTemplate>
                                <%# Eval("Qt_Name", "{0}")%>
                            </ItemTemplate>
                            <FooterStyle CssClass="center" />
                            <ItemStyle CssClass="left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="状态">
                            <ItemTemplate>
                                <cc1:StateButton ID="sbUse" OnClick="sbUse_Click" runat="server" TrueText="[使用]"
                                    FalseText="[禁用]" State='<%# Eval("Qt_IsUse","{0}")=="True"%>'></cc1:StateButton>
                            </ItemTemplate>
                            <ItemStyle CssClass="center" Width="50px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="操作">
                            <ItemTemplate>
                                <cc1:RowDelete ID="btnDel" OnClick="btnDel_Click" runat="server"></cc1:RowDelete>
                                <cc1:RowEdit ID="btnEdit" OnClick="btnEdit_Click" CommandArgument='<%# Eval("Qt_ID")%>'
                                    IsJsEvent="false" runat="server"></cc1:RowEdit>
                            </ItemTemplate>
                            <HeaderStyle CssClass="center noprint" />
                            <ItemStyle CssClass="center noprint" Width="50px" />
                        </asp:TemplateField>
                    </Columns>
                </cc1:GridView>
            </td>
        </tr>
    </table>
    <%--编辑区--%>
    <fieldset>
        <legend>
            <asp:Label ID="lbTypeTitle" runat="server" Text="新增"></asp:Label>题型类型</legend>
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td class="right" width="80px">
                    题型名称：
                </td>
                <td>
                    <asp:TextBox ID="tbName" runat="server" group="add" nullable="false" MaxLength="280"
                        Width="95%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="right">
                    基础类型：
                </td>
                <td>
                    <asp:DropDownList ID="ddlBaseType" runat="server" group="add" novalue="-1">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="right">
                </td>
                <td>
                    <asp:CheckBox ID="cbIsUse" Checked="true" runat="server" Text="是否启用" />
                    <span style="display: none">
                        <asp:Label ID="lbTypeID" runat="server" Text="0"></asp:Label></span>
                </td>
            </tr>
            <tr>
                <td class="right" valign="top">
                    说明：
                </td>
                <td>
                    <asp:TextBox ID="tbIntro" runat="server" Width="95%" TextMode="MultiLine" Height="80"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="right">
                </td>
                <td>
                    <asp:Button ID="btnAddEnter" runat="server" CssClass="editBtn" group="add" verify="true"
                        Text=" 添 加 " OnClick="btnAddEnter_Click" />
                    <asp:Panel ID="plEditBox" runat="server" Visible="false">
                        <asp:Button ID="btnEditEnter" runat="server" CssClass="editBtn" group="add" verify="true"
                            Text=" 编 辑 " OnClick="btnEditEnter_Click" />
                        <asp:Button ID="btnBack" runat="server" CssClass="editBtn" group="add" verify="true"
                            Text=" 返 回 " OnClick="btnBack_Click" />
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </fieldset>
</asp:Content>
