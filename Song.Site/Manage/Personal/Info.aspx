<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    CodeBehind="Info.aspx.cs" Inherits="Song.Site.Manage.Personal.Info" Title="无标题页" %>

<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <script type="text/javascript" src="../CoreScripts/hanzi2pinyin.js"></script>
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td width="120" class="right">
                姓名：
            </td>
            <td>
                <asp:TextBox ID="tbName" runat="server" MaxLength="20" Width="80px"></asp:TextBox>
            </td>
            <td colspan="2" rowspan="8">
                <img src="../Images/nophoto.jpg" name="imgShow" width="150" height="200" id="imgShow"
                    runat="server" /><br />
                <cc1:FileUpload ID="fuLoad" runat="server" fileallow="jpg|bmp|gif|png" Width="150" />
            </td>
        </tr>
        <tr>
            <td width="120" class="right">
                拼音缩写：
            </td>
            <td>
                <asp:TextBox ID="tbNamePinjin" runat="server" MaxLength="10" Width="50px"></asp:TextBox>
                <span id="namePinjin" style="display: none">多音选择：<span id="duoyin"></span></span>
            </td>
        </tr>
        <tr>
            <td class="right">
                性别：
            </td>
            <td>
                <asp:RadioButtonList ID="rbSex" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                    <asp:ListItem Selected="True" Value="1">男</asp:ListItem>
                    <asp:ListItem Value="2">女</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <td class="right">
                登录帐号：
            </td>
            <td>
                <asp:Label ID="lbAcc" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="right">
                员工工号：
            </td>
            <td>
                <asp:Label ID="lbEmpCode" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="right">
                所在院系：
            </td>
            <td>
                <asp:Label ID="lbDepart" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="right">
                岗位：
            </td>
            <td>
                <asp:Label ID="lbPosi" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="right">
                所在班组：
            </td>
            <td>
                <asp:Label ID="lbTeam" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="right">
                &nbsp;
            </td>
            <td colspan="3">
            </td>
        </tr>
        <tr>
            <td class="right">
                电话：
            </td>
            <td colspan="3">
                <asp:TextBox ID="tbTel" runat="server" MaxLength="50"></asp:TextBox>&nbsp;<asp:CheckBox
                    ID="cbIsOpenTel" runat="server" Text="在单位内公开" ToolTip="公开后，在单位通讯录中可见" />
            </td>
        </tr>
        <tr>
            <td class="right">
                移动电话：
            </td>
            <td colspan="3">
                <asp:TextBox ID="tbMobile" runat="server" MaxLength="50"></asp:TextBox>&nbsp;<asp:CheckBox
                    ID="cbIsOpenMobile" runat="server" Text="在单位内公开" ToolTip="公开后，在单位通讯录中可见" />
            </td>
        </tr>
        <tr>
            <td class="right">
                Email：
            </td>
            <td colspan="3">
                <asp:TextBox ID="tbEmail" runat="server" MaxLength="50"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="right">
                QQ：
            </td>
            <td colspan="3">
                <asp:TextBox ID="tbQQ" runat="server" MaxLength="50"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="right">
                微信：
            </td>
            <td colspan="3">
                <asp:TextBox ID="tbWeixin" runat="server" MaxLength="100"></asp:TextBox>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
    <cc1:EnterButton verify="true" ID="btnEnter" runat="server" Text="确定" OnClick="btnEnter_Click"
        ValidationGroup="enter" />
    <%--<Song:DeleteButton ID="DeleteButton1" runat="server" OnClick="btnDelete_Click" />--%>
    <cc1:CloseButton ID="CloseButton1" runat="server" Visible="false" />
</asp:Content>
