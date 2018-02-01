<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    CodeBehind="LinksSort_Edit.aspx.cs" Inherits="Song.Site.Manage.Site.Links.LinksSort_Edit"
    Title="无标题页" %>

<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <table width="100%" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td width="40px" class="right">
                名称：
            </td>
            <td>
                <asp:TextBox nullable="false" ID="Ls_Name" runat="server" MaxLength="100" Width="90%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="right">
                状态：
            </td>
            <td>
                <asp:CheckBox ID="Ls_IsUse" runat="server" Checked="True" Text="是否启用" />
            </td>
        </tr>
        <tr>
            <td class="right">
                说明：
            </td>
            <td>
                <asp:TextBox ID="Ls_Tootip" runat="server" Height="60px" MaxLength="255" TextMode="MultiLine"
                    Width="90%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="right">
            </td>
            <td>
                <asp:CheckBox ID="Ls_IsImg" runat="server" Checked="True" Text="是否显示图片链接" />
            </td>
        </tr>
        <tr>
            <td class="right">
            </td>
            <td>
                <asp:CheckBox ID="Ls_IsText" runat="server" Checked="True" Text="是否显示文字链接" />
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
