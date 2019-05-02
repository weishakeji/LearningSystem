<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    CodeBehind="Setup_Accounts.aspx.cs" Inherits="Song.Site.Manage.Admin.Setup_Accounts"
    Title="无标题页" %>

<%@ Register Src="../Utility/toolsBar.ascx" TagName="toolsBar" TagPrefix="uc1" %>
<%@ Register Src="../Utility/Pager.ascx" TagName="Pager" TagPrefix="uc2" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <script type="text/javascript" src="../CoreScripts/iColorPicker.js"></script>
    <table width="100%" border="0" cellspacing="2" cellpadding="0" class="tableContext">
        <tr>
            <td class="right" width="100" valign="top">
                注册相关：
            </td>
            <td>
                <p> <asp:CheckBox ID="cbIsRegTeacher" runat="server" Text="禁止教师注册" />
                </p>
                 <p><asp:CheckBox ID="cbIsRegStudent" runat="server" Text="禁止学生注册" />
                </p>
                <p> <asp:CheckBox ID="cbIsRegSms" runat="server" Text="注册时采用短信验证" /></p>
            </td>
        </tr>
        <tr>
            <td class="right" valign="top">
                是否审核：
            </td>
            <td>
                <p>
                    <asp:CheckBox ID="cbIsVerifyTeahcer" runat="server" Text="启用教师注册审核" />
                </p>
                <p>
                    <asp:CheckBox ID="cbIsVerifyStudent" runat="server" Text="启用学生注册审核" />
                </p>
            </td>
        </tr>
        <tr>
            <td class="right" valign="top">
                登录设置：
            </td>
            <td>
                <p>
                    <asp:CheckBox ID="cbIsLoginForPw" runat="server" Text="启用账号密码登录" />
                </p>
                <p>
                    <asp:CheckBox ID="cbIsLoginForSms" runat="server" Text="启用手机短信验证登录" />
                </p>
                <p>
                    <asp:CheckBox ID="cbIsLoginForWeixin" runat="server" Text="微信中仅限微信登录（禁用账号、手机验证等方式登录）" />
                </p>
                <p style="display:none">
                    <asp:CheckBox ID="cbIsTraningLogin" runat="server" Text="试题练习需要学员登录后才能操作" /></p>
            </td>
        </tr>
        <tr>
            <td width="80" class="right">
            </td>
            <td>
                <cc1:Button ID="btnLogin" runat="server" Text="确定" verify="true" group="para" OnClick="btnLogin_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
