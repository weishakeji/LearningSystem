<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    CodeBehind="OrganLevel_Edit.aspx.cs" Inherits="Song.Site.Manage.Sys.OrganLevel_Edit"
    Title="无标题页" %>

<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <table width="100%" border="0" cellspacing="2" cellpadding="0" class="tableContext">
        <tr>
            <td width="80" class="right">
                名称：
            </td>
            <td>
                <asp:TextBox nullable="false" ID="tbName" runat="server" MaxLength="200" Width="80%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="right">
                标识：
            </td>
            <td>
                <asp:TextBox nullable="false" ID="tbTag" runat="server" MaxLength="50" Width="100"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="right">
                状态：
            </td>
            <td>
                <asp:CheckBox ID="cbIsUse" runat="server" Checked="True" Text="是否启用" />
            </td>
        </tr>
        <tr>
            <td class="right">
                等级：
            </td>
            <td>
                <asp:TextBox nullable="false" ID="tbLevel" runat="server" MaxLength="2" datatype="uint"
                    Width="30"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="right">
                分润方案：
            </td>
            <td>
                <asp:DropDownList ID="ddlProfit" runat="server" DataTextField="Ps_Name" DataValueField="Ps_ID" novalue="-1" alt="请选择分润方案">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="right" valign="top">
                说明：
            </td>
            <td>
                <asp:TextBox ID="tbIntro" runat="server" Height="120px" MaxLength="255" TextMode="MultiLine"
                    Width="99%"></asp:TextBox>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
    <cc1:EnterButton verify="true" ID="btnEnter" runat="server" Text="确定" OnClick="btnEnter_Click"
        ValidationGroup="enter" />
    <%--<Song:DeleteButton ID="DeleteButton1" runat="server" OnClick="btnDelete_Click" />--%>
    <cc1:CloseButton ID="CloseButton1" runat="server" />
</asp:Content>
