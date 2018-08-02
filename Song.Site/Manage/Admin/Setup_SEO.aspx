<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    CodeBehind="Setup_SEO.aspx.cs" Inherits="Song.Site.Manage.Admin.Setup_SEO" Title="无标题页" %>

<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Src="../Utility/Pager.ascx" TagName="Pager" TagPrefix="uc2" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <table width="100%" border="0" cellspacing="2" cellpadding="0" class="tableContext">
        <tr>
            <td width="100" class="right">
                Keywords：
            </td>
            <td>
                <asp:TextBox ID="Org_Keywords" runat="server" MaxLength="1000" Width="98%" TextMode="MultiLine"
                    Height="40"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="right">
                Description：
            </td>
            <td>
                <asp:TextBox ID="Org_Description" runat="server" MaxLength="1000" Width="98%" TextMode="MultiLine"
                    Height="40"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="right">
                附属代码：<br />
                （例如流量统计代码）
            </td>
            <td>
                <asp:TextBox ID="Org_Extracode" runat="server" MaxLength="1000" Width="98%" TextMode="MultiLine"
                    Height="100"></asp:TextBox>
            </td>
        </tr>
        <td width="80" class="right">
        </td>
        <td>
            <cc1:Button ID="btnSeo" runat="server" Text="保存" OnClick="btnSeo_Click" />
        </td>
        </tr>
    </table>
</asp:Content>
