<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    CodeBehind="Accounts_Edit.aspx.cs" Inherits="Song.Site.Manage.Sys.Accounts_Edit"
    Title="无标题页" %>

<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <script language="javascript" src="../Utility/datepicker/WdatePicker.js" type="text/javascript"></script>
    <script type="text/javascript" src="../CoreScripts/hanzi2pinyin.js"></script>
    <table width="100%" border="0" cellspacing="2" cellpadding="0" class="tableContext">
        <tr>
            <td width="80" class="right">
                登录账号：
            </td>
            <td>
                <asp:TextBox nullable="false" ID="tbAccName" runat="server" MaxLength="100" Enabled="false"></asp:TextBox>
            </td>
            <td width="150" rowspan="7" valign="top">
                <img src="../Images/nophoto.jpg" name="imgShow" width="150" height="200" id="imgShow"
                    runat="server" /><br />
                <cc1:FileUpload ID="fuLoad" runat="server" fileallow="jpg|bmp|gif|png" Width="150" />
            </td>
        </tr>
        <tr>
            <td class="right">
                姓名：
            </td>
            <td>
                <asp:TextBox ID="tbName" runat="server" MaxLength="4" pinyin="tbNamePinjin" nullable="false"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="right">
                拼音缩写：
            </td>
            <td>
                <asp:TextBox ID="tbNamePinjin" runat="server" MaxLength="10" Width="50px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="right">
                所在机构：
            </td>
            <td>
                <asp:Label ID="lbOrgin" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="right">
                性别：
            </td>
            <td>
                <asp:RadioButtonList ID="rbSex" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                  <asp:ListItem  Selected="True" Value="0">未知</asp:ListItem>
                    <asp:ListItem Value="1">男</asp:ListItem>
                    <asp:ListItem Value="2">女</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <td class="right">
                身份证：
            </td>
            <td>
                <asp:TextBox ID="tbIdCard" runat="server" MaxLength="160"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="right">
                年龄：
            </td>
            <td colspan="2">
                <asp:TextBox ID="tbAge" runat="server" datatype="uint" MaxLength="3" Width="50px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="right">
                电子信箱：
            </td>
            <td colspan="2">
                <asp:TextBox ID="tbEmail" MaxLength="80" Width="50%" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="right">
                固定电话：
            </td>
            <td colspan="2">
                <asp:TextBox ID="tbTel" MaxLength="50" Width="50%" runat="server"></asp:TextBox>
                <asp:CheckBox ID="cbIsOpenTel" runat="server" Text="是否公开" />
            </td>
        </tr>
        <tr>
            <td class="right">
                移动电话1：
            </td>
            <td colspan="2">
                <asp:TextBox ID="tbMobleTel1" MaxLength="50" Width="50%" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="right">
                移动电话2：
            </td>
            <td colspan="2">
                <asp:TextBox ID="tbMobleTel2" MaxLength="50" Width="50%" runat="server"></asp:TextBox>
                <asp:CheckBox ID="cbIsOpenMobile" runat="server" Text="是否公开" />
            </td>
        </tr>
        <tr>
            <td class="right">
                Q Q：
            </td>
            <td colspan="2">
                <asp:TextBox ID="tbQQ" MaxLength="50" Width="50%" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="right">
                微信：
            </td>
            <td colspan="2">
                <asp:TextBox ID="tbWeixin" MaxLength="100" Width="50%" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
    <cc1:EnterButton verify="true" ID="btnEnter" runat="server" Text="确定" OnClick="btnEnter_Click"
        ValidationGroup="enter" />
    <cc1:CloseButton ID="CloseButton1" runat="server" />
</asp:Content>
