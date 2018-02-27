<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    CodeBehind="ProfitSharing_Edit.aspx.cs" Inherits="Song.Site.Manage.Sys.ProfitSharing_Edit"
    Title="无标题页" %>

<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <table width="100%" border="0" cellspacing="2" cellpadding="0" class="tableContext">
        <tr>
            <td width="80" class="right">
                分润方案：
            </td>
            <td>
                <asp:TextBox nullable="false" ID="tbName" runat="server" Width="80%" lenlimit="50"
                    group="enter|add"></asp:TextBox><asp:CheckBox ID="cbIsUse" runat="server" Checked="True"
                        Text="启用" />
            </td>
        </tr>
        <tr>
            <td class="right" valign="top">
                说明：
            </td>
            <td>
                <asp:TextBox ID="tbIntro" runat="server" Height="30px" MaxLength="255" TextMode="MultiLine"
                    Width="99%"></asp:TextBox>
            </td>
        </tr>
    </table>
    <fieldset>
        <legend>分润比例设置</legend>
        <div class="GridViewArea">
            <asp:GridView ID="gvProfit" runat="server" Width="99%" AutoGenerateColumns="False"
                ShowFooter="True" ShowHeader="True">
                <EditRowStyle CssClass="editRow" />
                <Columns>
                    <asp:TemplateField HeaderText="移动">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbUp" OnClick="lbUp_Click" ToolTip="上移" runat="server">&#8593; </asp:LinkButton>
                            <asp:LinkButton ID="lbDown" OnClick="lbDown_Click" ToolTip="下移" runat="server">&#8595;</asp:LinkButton>
                        </ItemTemplate>
                        <ItemStyle CssClass="center noprint" Width="60px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="级别">
                        <ItemTemplate>
                            <span style="display:none"><%# Eval("Ps_Level","{0}")%></span>
                             <%# Container.DataItemIndex   + 1 %>
                        </ItemTemplate>
                        <ItemStyle CssClass="center noprint" Width="60px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="资金比例">
                        <ItemTemplate>
                            <span class="moneyratio"><%# Eval("Ps_Moneyratio","{0}")%></span> %
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="tbMoneyEdit" runat="server" nullable="false" datatype="uint" Text='<%# Eval("Ps_Moneyratio", "{0}")%>'
                                group="edit" Width="40"></asp:TextBox>
                        </EditItemTemplate>
                        <FooterTemplate>
                            合计：<span class="moneySum">0</span> %</FooterTemplate>
                        <FooterStyle CssClass="center" />
                        <ItemStyle CssClass="center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="卡券比例">
                        <ItemTemplate>
                            <span class="couponratio"><%# Eval("Ps_Couponratio", "{0}")%></span> %
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="tbCouponEdit" runat="server" nullable="false" datatype="uint" Text='<%# Eval("Ps_Couponratio", "{0}")%>'
                                group="edit" Width="40"></asp:TextBox>
                        </EditItemTemplate>
                        <FooterTemplate>
                            合计：<span class="couponSum">0</span> %</FooterTemplate>
                        <FooterStyle CssClass="center" />
                        <ItemStyle CssClass="center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="状态">
                        <ItemTemplate>
                            <cc1:StateButton ID="sbUse" OnClick="sbUse_Click" runat="server" TrueText="[使用]"
                                FalseText="[禁用]" State='<%# Eval("Ps_IsUse","{0}")=="True"%>'></cc1:StateButton>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:CheckBox ID="cbIsUse" runat="server" Text="启用" Checked='<%# Eval("Ps_IsUse")%>' />
                        </EditItemTemplate>
                        <ItemStyle CssClass="center noprint" Width="60px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="操作">
                        <ItemTemplate>
                            <cc1:RowDelete ID="btnDel" OnClick="btnDel_Click" runat="server"></cc1:RowDelete>
                            <cc1:RowEdit ID="btnEdit" OnClick="btnEdit_Click" IsJsEvent="false" runat="server">
                            </cc1:RowEdit>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Button ID="btnEditEnter" runat="server" CssClass="editBtn" group="edit" verify="true"
                                OnClick="btnEditEnter_Click" Text="确定" />
                            <asp:Button ID="btnEditBack" runat="server" CssClass="backBtn" OnClick="btnEditBack_Click"
                                Text="返回" />
                        </EditItemTemplate>
                        <HeaderStyle CssClass="center noprint" />
                        <ItemStyle CssClass="center noprint" Width="100px" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <asp:Panel ID="plAddProfit" runat="server" CssClass="plAddProfit">
            资金分润(%)：<asp:TextBox ID="tbMoneyAdd" runat="server" nullable="false" group="add"
                numlimit="0-100" datatype="uint" Width="40"></asp:TextBox>
            卡券分润(%)：<asp:TextBox ID="tbCouponAdd" runat="server" nullable="false" datatype="number"
                numlimit="0-100" group="add" Width="60"></asp:TextBox>
            <asp:Button ID="btnAAEnter" runat="server" CssClass="editBtn" group="add" verify="true"
                Text="添加" OnClick="btnAddEnter_Click" />
        </asp:Panel>
    </fieldset>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
    <cc1:EnterButton verify="true" ID="btnEnter" runat="server" Text="确定" OnClick="btnEnter_Click"
        group="enter" ValidationGroup="enter" />
    <%--<Song:DeleteButton ID="DeleteButton1" runat="server" OnClick="btnDelete_Click" />--%>
    <cc1:CloseButton ID="CloseButton1" runat="server" />
</asp:Content>
