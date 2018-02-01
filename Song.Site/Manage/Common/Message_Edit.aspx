<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    CodeBehind="Message_Edit.aspx.cs" Inherits="Song.Site.Manage.Common.Message_Edit"
    Title="无标题页" %>

<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Src="../Utility/EmplyeeSelectBox.ascx" TagName="EmplyeeSelectBox" TagPrefix="uc1" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <table width="100%" border="0" cellspacing="2" cellpadding="0" class="tableContext">
        <tr>
            <td width="60" class="right">
                内容：
            </td>
            <td>
                <asp:TextBox ID="tbMsg" runat="server" Height="120px" MaxLength="255" TextMode="MultiLine"
                    Width="96%" nullable="false"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="right">
                QQ：
            </td>
            <td>
                <asp:TextBox ID="tbQQ" runat="server" MaxLength="255" Width="96%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="right">
                电话：
            </td>
            <td>
                <asp:TextBox ID="tbPhone" runat="server" MaxLength="255" Width="96%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="right">
                回复：
            </td>
            <td>
                <asp:TextBox ID="tbReply" runat="server" Height="80px" MaxLength="255" TextMode="MultiLine"
                    Width="96%"></asp:TextBox>
            </td>
        </tr>
         <tr>
            <td class="right">
                时间：
            </td>
             
            <td>
               <asp:Label ID="lbCrtTime" runat="server" Text=""></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
    <cc1:EnterButton verify="true" ID="btnAnswer" runat="server" Text="回复" ValidationGroup="enter"
        OnClick="btnAnswer_Click" />
    <cc1:CloseButton ID="CloseButton1" runat="server" /></asp:Content>
