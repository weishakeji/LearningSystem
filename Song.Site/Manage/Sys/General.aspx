<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    CodeBehind="General.aspx.cs" Inherits="Song.Site.Manage.Sys.General" Title="无标题页" %>

<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <script type="text/javascript" src="../CoreScripts/iColorPicker.js"></script>
    <table width="100%" border="0" cellspacing="2" cellpadding="0" class="tableContext">
        <tr>
            <td width="80" class="right">
                系统名称：
            </td>
            <td>
                <asp:TextBox ID="tbSysName" runat="server" Width="90%" MaxLength="100"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="right" valign="top">
                日志：
            </td>
            <td>
                <asp:CheckBox ID="cbLogin" runat="server" Text="是否记录登录信息" />
                记录<asp:TextBox ID="tbLoginTimeSpan" runat="server" MaxLength="2" Width="30px"></asp:TextBox>天之内的登录信息<br />
                <asp:CheckBox ID="cbWork" runat="server" Text="是否记录操作信息" />
                记录<asp:TextBox ID="tbWorkTimeSpan" runat="server" MaxLength="2" Width="30px"></asp:TextBox>天之内的操作信息<br />
            </td>
        </tr>
        <tr>
            <td class="right" valign="top">
                云平台：
            </td>
            <td>
                <asp:RadioButtonList ID="rblMultiOrgan" runat="server">
                    <asp:ListItem Value="0" Text="启用多机构，当前系统支持多机构入驻，不同机构可以设置不同二级域名"></asp:ListItem>
                    <asp:ListItem Value="1" Text="采用单机构，只采用默认机构作为唯一机构，不再受跨域问题影响"></asp:ListItem>
                </asp:RadioButtonList>
                （此处勿轻易切换，会导致用户登录与注销的异常情况，需清空浏览器缓存）
            </td>
        </tr>
        <tr>
            <td class="right" valign="top">
            </td>
            <td>
                <cc1:Button ID="btnEnter" runat="server" verify="true" Text="确 定" OnClick="btnEnter_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
