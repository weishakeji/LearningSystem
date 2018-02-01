<%@ Page Title="" Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    CodeBehind="Accounts_Export.aspx.cs" Inherits="Song.Site.Manage.Sys.Accounts_Export" %>

<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <br />
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td class="right" width="80">
                机构：
            </td>
            <td>
            <div class="selectBtn"><a class="all">全选</a><a class="invert">反选</a><a class="cancel">取消</a></div>
                <asp:CheckBoxList ID="cblOrg" runat="server">
                </asp:CheckBoxList>
            </td>
        </tr>
        <tr>
            <td class="right">
                &nbsp;
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="right">
                &nbsp;
            </td>
            <td>
                <cc1:Button ID="btnExport" CssClass="Button" runat="server" Text="导出到Excel" OnClick="btnExport_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
