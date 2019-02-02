<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    CodeBehind="OrganSetup.aspx.cs" Inherits="Song.Site.Manage.Admin.OrganSetup"
    Title="无标题页" %>

<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Src="../Utility/Pager.ascx" TagName="Pager" TagPrefix="uc2" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <table width="100%" border="0" cellspacing="2" cellpadding="0" class="tableContext">
        <tr>
            <td width="100" class="right">
                平台名称：
            </td>
            <td>
                <asp:TextBox nullable="false" ID="Org_PlatformName" group="para" runat="server" MaxLength="200"
                    Width="90%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="right">
                域名：
            </td>
            <td class="domain">
                <asp:Label ID="Org_TwoDomain" runat="server" Text=""></asp:Label>.<asp:Label ID="lbDomain"
                    runat="server" Text=""></asp:Label>:<%= WeiSha.Common.Server.Port%>
            </td>
        </tr>
        <tr>
            <td width="100" class="right">
                标志：
            </td>
            <td>
                <img src="../Images/nophoto.jpg" name="imgShow" id="imgShow" runat="server" style="max-height: 60px;" /><br />
                <cc1:FileUpload ID="fuLoad" runat="server" FileAllow="jpg|bmp|gif|png" group="img"
                    Width="150" />
            </td>
        </tr>
        <tr>
            <td class="right">
                ICP备案号：
            </td>
            <td>
                <asp:TextBox ID="Org_ICP" runat="server" MaxLength="200" Width="50%"></asp:TextBox>
            </td>
        </tr>
          <tr>
            <td class="right">
                手机端：
            </td>
            <td>
                <asp:CheckBox ID="Org_IsOnlyWeixin" runat="server" Text="手机端是否仅限微信中使用" /><br />
                <asp:CheckBox ID="cbIsMobileRemoveMoney" runat="server" Text="手机端隐藏关于“充值收费”等资费相关信息" />
            </td>
        </tr>
        <tr>
            <td width="80" class="right">
            </td>
            <td>
                <cc1:Button ID="btnBase" runat="server" Text="确定" verify="true" group="para" OnClick="btnBase_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
