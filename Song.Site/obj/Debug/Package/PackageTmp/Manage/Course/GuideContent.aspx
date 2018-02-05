<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    CodeBehind="GuideContent.aspx.cs" Inherits="Song.Site.Manage.Course.GuideContent" %>

<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Src="../Utility/Pager.ascx" TagName="Pager" TagPrefix="uc2" %>
<%@ Register Assembly="WeiSha.WebEditor" Namespace="WeiSha.WebEditor" TagPrefix="WebEditor" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <asp:Panel ID="plEditArea" runat="server" CssClass="plEditArea" Visible="false">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td class="right" width="60px">
                    标题：
                </td>
                <td>
                    <asp:TextBox nullable="false" ID="tbTitle" group="ent" runat="server" MaxLength="255"
                        Width="95%"></asp:TextBox><asp:Label ID="lbID" runat="server" Text="" Visible="false"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="right">
                    栏目：
                </td>
                <td>
                    <cc1:DropDownTree ID="ddlTree" runat="server" Width="200" IdKeyName="Gc_ID" ParentIdKeyName="Gc_PID"
                        TaxKeyName="Gc_Tax">
                    </cc1:DropDownTree>
                    &nbsp;
                    <asp:CheckBox ID="cbIsShow" runat="server" Text="是否显示" state="true" Checked="true" />
                    <asp:CheckBox ID="cbIsTop" runat="server" Text="置顶" state="true" />
                    <asp:CheckBox ID="cbIsRec" runat="server" Text="推荐" state="true" />
                    <asp:CheckBox ID="cbIsHot" runat="server" Text="热点" state="true" />
                </td>
            </tr>
        </table>
        <div class="contItem" tab="tab2">
            <WebEditor:Editor ID="tbDetails" runat="server" Height="450px" Width="99%"> </WebEditor:Editor>
        </div>
        <table width="100%" border="0" cellspacing="0" cellpadding="0" style="margin-top:10px;">
            <tr>
                <td class="right" width="10px">
                </td>
                <td>
                    <cc1:Button ID="btnAddEnter" runat="server" Text="确 定" group="ent" verify="true"
                        OnClick="btnAddEnter_Click" />
                    &nbsp;
                    <cc1:Button ID="btnAddBack" runat="server" Text="返 回" OnClick="btnAddBack_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="plListArea" runat="server" CssClass="plListArea">
        <div id="header">
            <uc1:toolsBar ID="ToolsBar1" runat="server" OnAdd="AddEvent" GvName="GridView1" WinWidth="980"
                WinHeight="90" OnDelete="DeleteEvent" DelShowMsg="" isWinOpen=true/>
            <div class="searchBox" id="searchBox">
                <span id="spanColName" runat="server">栏目：《<asp:Label ID="lbColunms" runat="server"
                    Text=""></asp:Label>》</span> 标题：<asp:TextBox ID="tbSear" runat="server" Width="115"
                        MaxLength="10"></asp:TextBox>&nbsp;<asp:Button ID="btnSear" runat="server" Width="100"
                            Text="查询" OnClick="btnsear_Click" />
            </div>
        </div>
        <cc1:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" SelectBoxKeyName="SelectBox"
            ShowSelectBox="True" ShowFooter="false">
            <EmptyDataTemplate>
                没有满足条件的信息！
            </EmptyDataTemplate>
            <Columns>
                <asp:TemplateField HeaderText="序号">
                    <ItemTemplate>
                        <%# Container.DataItemIndex   + 1 %>
                    </ItemTemplate>
                    <ItemStyle CssClass="center" Width="40px" />
                </asp:TemplateField>
               
                <asp:TemplateField HeaderText="标题">
                    <ItemTemplate>
                        <%# Eval("Gu_Title")%>
                    </ItemTemplate>
                    <ItemStyle CssClass="left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="使用">
                    <ItemTemplate>
                        <cc1:StateButton ID="sbUse" OnClick="sbUse_Click" runat="server" TrueText="使用" FalseText="禁用"
                            State='<%# Eval("Gu_IsShow","{0}")=="True"%>'></cc1:StateButton>
                    </ItemTemplate>
                    <HeaderStyle CssClass="center noprint" />
                    <ItemStyle CssClass="center noprint" Width="60px" />
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="操作">
                    <ItemTemplate>
                        <cc1:RowDelete ID="btnDel" OnClick="btnDel_Click" runat="server"></cc1:RowDelete>
                        <cc1:RowEdit ID="btnEdit" IsJsEvent="false" OnClick="btnEdit_Click" CommandArgument='<%# Eval("Gu_ID")%>'
                            runat="server"></cc1:RowEdit>
                    </ItemTemplate>
                    <HeaderStyle CssClass="center noprint" />
                    <ItemStyle CssClass="center noprint" Width="44px" />
                </asp:TemplateField>
            </Columns>
        </cc1:GridView>
        <uc2:Pager ID="Pager1" runat="server" Size="15" OnPageChanged="BindData" />
    </asp:Panel>
</asp:Content>
