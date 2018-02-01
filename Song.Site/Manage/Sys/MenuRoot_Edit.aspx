<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    Codebehind="MenuRoot_Edit.aspx.cs" Inherits="Song.Site.Manage.Sys.MenuRoot_Edit"
    Title="修改数据库备份" %>
<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
  <table width="100%" border="0" cellspacing="2" cellpadding="0" class="tableContext">
    <tr>
      <td width="80" class="right">名称：</td>
      <td><asp:TextBox nullable="false" ID="tbName" runat="server" MaxLength="100"></asp:TextBox>
      </td>
    </tr>
    <tr>
      <td class="right">标记：</td>
      <td><asp:TextBox nullable="false" ID="tbMarker" runat="server" MaxLength="100"></asp:TextBox>
      </td>
    </tr>
    <tr>
      <td class="right"> 样式：</td>
      <td><asp:CheckBox ID="cbIsBold" runat="server" Text="粗体" />
        <asp:CheckBox ID="cbIsItalic" runat="server" Text="斜体" />
      </td>
    </tr>
    <tr>
      <td class="right"> 状态：</td>
      <td><asp:CheckBox ID="cbIsUse" runat="server" Text="是否使用" Checked="True" />
        <asp:CheckBox ID="cbIsShow" runat="server" Text="是否显示" Checked="True" />
      </td>
    </tr>
    <tr>
      <td class="right" valign="top"> 说明：</td>
      <td><asp:TextBox ID="tbIntro" runat="server" Height="60px" MaxLength="255" TextMode="MultiLine"
        Width="300px"></asp:TextBox>
      </td>
    </tr>
  </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
  <cc1:EnterButton verify=true  ID="btnEnter" runat="server" Text="确定" OnClick="btnEnter_Click" ValidationGroup="enter" />
  <%--<Song:DeleteButton ID="DeleteButton1" runat="server" OnClick="btnDelete_Click" />--%>
  <cc1:CloseButton ID="CloseButton1" runat="server" />
</asp:Content>
