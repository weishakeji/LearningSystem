<%@ Page Title="" Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    CodeBehind="OtherLogin.aspx.cs" Inherits="Song.Site.Manage.Sys.OtherLogin" %>
    <%@ MasterType VirtualPath="~/Manage/ManagePage.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <fieldset>
        <legend>QQ登录</legend>
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td width="80">
                </td>
                <td>
                    <asp:CheckBox ID="cbIsQQLogin" runat="server" Text="是否启用QQ登录" Checked="True" />
                    <asp:CheckBox ID="cbIsQQDirect" runat="server" Text="是否允许QQ直接注册" ToolTip="" Checked="True" />
                </td>
            </tr>
            <tr>
                <td class="right">
                    AppID:
                </td>
                <td class="left">
                    <asp:TextBox ID="tbQQAppid" runat="server" nullable="false"  group="qq" Width="100"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="right">
                    AppKey:
                </td>
                <td class="left">
                    <asp:TextBox ID="tbQQAppkey" runat="server" nullable="false"  group="qq" Width="300"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="right">
                    回调域:
                </td>
                <td class="left">
                    <asp:TextBox ID="tbQQReturl" runat="server" begin="http://|https://" nullable="false"  group="qq" Width="200"></asp:TextBox>/qqlogin.ashx
                </td>
            </tr>
            <tr>
                <td class="right">
                    说明:</td>
                <td class="left">
                    （回调域默认取自db.config中设置项）
                </td>
            </tr>
            <tr>
                <td class="right">
                    </td>
                <td class="left">
                    <cc1:Button ID="btnQQlogin" runat="server" Text="确定" 
                        onclick="btnQQlogin_Click" verify="true"  group="qq"/>
                </td>
            </tr>
        </table>
    </fieldset>
    <fieldset>
        <legend>微信登录</legend>
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td width="80">
                </td>
                <td>
                    <asp:CheckBox ID="cbIsWeixinLogin" runat="server" Text="是否启用微信登录" />
                    <asp:CheckBox ID="cbIsWeixinDirect" runat="server" Text="是否允许微信直接注册" ToolTip="" Checked="True" />
                </td>
            </tr>
            <tr>
                <td class="right">
                    AppID:
                </td>
                <td class="left">
                    <asp:TextBox ID="tbWeixinAppid" runat="server" nullable="false" group="weixin" Width="180"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="right">
                    AppSecret:
                </td>
                <td class="left">
                    <asp:TextBox ID="tbWeixinSecret" runat="server"  nullable="false" group="weixin"  Width="300"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="right">
                    回调域:
                </td>
                <td class="left">
                 <asp:TextBox ID="tbWeixinReturl"  begin="http://|https://" runat="server" nullable="false"  group="weixin" Width="200"></asp:TextBox>/weixinlogin.ashx                 
                </td>
            </tr>
            <tr>
                <td class="right">
                    说明:</td>
                <td class="left">
                    （回调域默认取自db.config中设置项）
                </td>
            </tr>
            <tr>
                <td class="right">
                    </td>
                <td class="left">
                    <cc1:Button ID="btnWeixnLogin" runat="server" Text="确定" onclick="btnWeixnLogin_Click" verify="true" group="weixin"  />
                </td>
            </tr>
        </table>
    </fieldset>
</asp:Content>
