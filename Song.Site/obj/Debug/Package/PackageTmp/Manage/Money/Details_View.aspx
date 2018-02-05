<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    CodeBehind="Details_View.aspx.cs" Inherits="Song.Site.Manage.Money.Details_View"
    Title="无标题页" %>

<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<%@ Register Assembly="WeiSha.WebEditor" Namespace="WeiSha.WebEditor" TagPrefix="Kind" %>
<%@ Register Src="../Utility/SortSelect.ascx" TagName="SortSelect" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <script language="javascript" src="../Utility/datepicker/WdatePicker.js" type="text/javascript"></script>
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td class="right" width="80">
                学员：
            </td>
            <td>
                <asp:Label ID="Ac_ID" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="right">
            </td>
            <td>
            <div class="type type<%=type %>">
                <asp:Label ID="Ma_Type" runat="server" Text=""></asp:Label>
                >>
                <asp:Label ID="Ma_Monery" runat="server" Text=""></asp:Label>元</div>
            </td>
        </tr>
        <tr>
            <td class="right">
                时间：
            </td>
            <td>
                <asp:Label ID="Ma_CrtTime" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="right">
                流水号：
            </td>
            <td>
                <asp:Label ID="Ma_Serial" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="right">
                来源：
            </td>
            <td>
                <asp:Label ID="Ma_From" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="right">
                来源信息：
            </td>
            <td>
                <asp:Label ID="Ma_Source" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr id="trCode" runat="server" visible="false">
            <td class="right">
                充值码：
            </td>
            <td>
                <asp:Label ID="Rc_Code" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr id="trPayinterface" runat="server" visible="false">
            <td class="right">
                支付平台：
            </td>
            <td>
                <asp:Label ID="Pai_ID" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="right">
                支付账户：
            </td>
            <td>
                <asp:Label ID="Ma_Buyer" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="right">
                收款账户：
            </td>
            <td>
                <asp:Label ID="Ma_Seller" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="right">
                状态：
            </td>
            <td>
                <asp:Label ID="Ma_IsSuccess" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="right">
                说明：
            </td>
            <td>
                <asp:Label ID="Ma_Info" runat="server" Text=""></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
    <cc1:CloseButton ID="CloseButton1" runat="server" />
</asp:Content>
