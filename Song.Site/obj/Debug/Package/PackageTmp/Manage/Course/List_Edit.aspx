<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    CodeBehind="List_Edit.aspx.cs" Inherits="Song.Site.Manage.Course.List_Edit" Title="无标题页" %>

<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<%@ Register Assembly="WeiSha.WebEditor" Namespace="WeiSha.WebEditor" TagPrefix="WebEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td class="right" width="80px">
                课程名称：
            </td>
            <td>
                <asp:TextBox ID="Cou_Name" nullable="false" Width="95%" MaxLength="200" runat="server"></asp:TextBox>
            </td>
            <td width="200" rowspan="3" class="right" valign="top">
                <img src="../Images/nophoto.jpg" name="imgShow" width="200" height="123" id="imgShow"
                    runat="server" /><br />
                <cc1:FileUpload ID="fuLoad" runat="server" fileallow="jpg|bmp|gif|png" Width="200" />
            </td>
        </tr>
        <tr>
            <td class="right">
                所属学科：
            </td>
            <td>
                <asp:DropDownList ID="ddlSubject" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="right">
                课程简介：
            </td>
            <td>
                <WebEditor:Editor ID="tbIntro" runat="server" Width="95%" ThemeType="simple" Height="160">
                </WebEditor:Editor>
            </td>
        </tr>
        <tr>
            <td class="right">
                学习目标：
            </td>
            <td colspan="2">
                <WebEditor:Editor ID="tbTarget" runat="server" Width="99%" ThemeType="simple" Height="180">
                </WebEditor:Editor>
            </td>
        </tr>
        <tr>
            <td class="right">
            </td>
            <td colspan="2">
                <asp:CheckBox ID="cbIsUse" runat="server" Text="是否启用" Checked="true" />
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
    <cc1:EnterButton verify="true" ID="btnEnter" runat="server" Text="确定" OnClick="btnEnter_Click"
        ValidationGroup="enter" />
    <cc1:CloseButton ID="CloseButton1" runat="server" />
</asp:Content>
