<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    CodeBehind="GuideColumns.aspx.cs" Inherits="Song.Site.Manage.Course.GuideColumns" %>

<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <fieldset>
        <legend>课程公告栏目-<asp:Button ID="btnAdd" runat="server" Text="新增栏目" OnClick="btnAdd_Click" /></legend>
        <asp:Panel ID="plAddColumn" runat="server" CssClass="plAddColumn" Visible="false">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td class="right" width="60px">
                        名称：
                    </td>
                    <td>
                        <asp:TextBox ID="tbTitle" nullable="false" group="add" runat="server" Width="200"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="right">
                        上级：
                    </td>
                    <td>
                        <cc1:DropDownTree ID="ddlTree" runat="server" Width="200" IdKeyName="Gc_ID" ParentIdKeyName="Gc_PID"
                            TaxKeyName="Gc_Tax">
                        </cc1:DropDownTree>
                    </td>
                </tr>
                <tr>
                    <td class="right">
                    </td>
                    <td>
                        <asp:CheckBox ID="cbIsUse" Text="是否启用" Checked="true" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="right">
                        简介：
                    </td>
                    <td>
                        <asp:TextBox ID="tbIntro" runat="server" Width="200" Height="50" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="right">
                    </td>
                    <td>
                        <asp:Button ID="btnAddEnter" runat="server" Text="确 定" group="add" verify="true"
                            OnClick="btnAddEnter_Click" />
                        &nbsp;
                        <asp:Button ID="btnAddBack" runat="server" Text="返 回" OnClick="btnAddBack_Click" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <cc1:GridView ID="gvColumns" runat="server" AutoGenerateColumns="False" SelectBoxKeyName="SelectBox"
            ShowSelectBox="False" ShowFooter="true">
            <EmptyDataTemplate>
                没有栏目！<br />
                没有栏目依然可以在右侧添加课程公告。<br /><br />
                <a href="GuideContent.aspx?couid=<%=couid %>" class="Gc_Title">[当前课程所有课程公告]</a></span>
            </EmptyDataTemplate>
            <EditRowStyle CssClass="editRow" />
            <Columns>
                <asp:TemplateField HeaderText="名称">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbUp" OnClick="lbUp_Click" ToolTip="上移" runat="server">&#8593; </asp:LinkButton>
                        <asp:LinkButton ID="lbDown" OnClick="lbDown_Click" ToolTip="下移" runat="server">&#8595;</asp:LinkButton>
                        <span class="treeIco">
                            <%# Eval("Tree")%></span> <span title="<%# Eval("Gc_Title", "{0}")%>"><a href="GuideContent.aspx?couid=<%=couid %>&gcid=<%# Eval("Gc_ID", "{0}")%>"
                                class="Gc_Title">
                                <%# Eval("Gc_Title")%></a></span>
                                <cc1:StateButton ID="sbUse" OnClick="sbUse_Click" runat="server" TrueText="用" FalseText="禁"
                        State='<%# Eval("Gc_IsUse","{0}")=="True"%>'></cc1:StateButton>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                            <tr>
                                <td class="right" width="50px">
                                    名称：
                                </td>
                                <td>
                                    <asp:TextBox ID="tbTitle" runat="server" nullable="false" group="edit" Text='<%# Eval("Gc_Title", "{0}")%>'
                                        Width="145"></asp:TextBox>
                                    <span class="currentId" style="display: none">
                                        <%# Eval("Gc_ID", "{0}")%></span>
                                </td>
                            </tr>
                            <tr>
                                <td class="right">
                                    上级：
                                </td>
                                <td>
                                    <cc1:DropDownTree ID="ddlColTree" runat="server" Width="145" IdKeyName="Gc_ID" ParentIdKeyName="Gc_PID"
                                        TaxKeyName="Gc_Tax">
                                    </cc1:DropDownTree>
                                </td>
                            </tr>
                            <tr>
                                <td class="right">
                                </td>
                                <td>
                                    <asp:CheckBox ID="cbIsUse" Text="是否启用" Checked='<%# Eval("Gc_IsUse","{0}")=="True" ? true : false%>' runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="right">
                                    简介：
                                </td>
                                <td>
                                    <asp:TextBox ID="tbIntro" runat="server" Width="96%" Height="50" Text='<%# Eval("Gc_Intro", "{0}")%>' TextMode="MultiLine"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <a href="GuideContent.aspx?couid=<%=couid %>" class="Gc_Title">[当前课程所有课程公告]</a></span>
                    </FooterTemplate>
                    <FooterStyle CssClass="center" />
                    <ItemStyle CssClass="left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="操作">
                    <ItemTemplate>
                        <cc1:RowDelete ID="btnDel" OnClick="btnDel_Click" runat="server"></cc1:RowDelete>
                        <cc1:RowEdit ID="btnEdit" OnClick="btnEdit_Click" IsJsEvent="false" runat="server"
                            CommandArgument='<%# Eval("Gc_PID")%>'></cc1:RowEdit>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:Button ID="btnEditEnter" runat="server" CssClass="editBtn" group="edit" verify="true"
                            OnClick="btnEditEnter_Click" Text="确定" />
                        <br />
                        <asp:Button ID="btnEditBack" runat="server" CssClass="backBtn" OnClick="btnEditBack_Click"
                            Text="返回" />
                    </EditItemTemplate>
                    <HeaderStyle CssClass="center noprint" />
                    <ItemStyle CssClass="center noprint" Width="44px" />
                </asp:TemplateField>
            </Columns>
        </cc1:GridView>
    </fieldset>
</asp:Content>
