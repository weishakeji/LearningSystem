<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    Codebehind="InternalLink_Edit.aspx.cs" Inherits="Song.Site.Manage.Site.InternalLink_Edit"
    Title="无标题页" %>
<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<%@ Register Assembly="WeiSha.WebEditor" Namespace="WeiSha.WebEditor" TagPrefix="WebEditor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
  <table width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr>
      <td class="right" width="80">名称：</td>
      <td><asp:TextBox nullable="false" ID="tbName" runat="server" MaxLength="100" Width="200px"></asp:TextBox>
      </td>
    </tr>
    <tr>
      <td class="right">链接：</td>
      <td><asp:TextBox nullable="false" ID="tbUrl" runat="server" MaxLength="100" Width="95%"></asp:TextBox></td>
    </tr>
    <tr>
      <td class="right"> 说明：</td>
      <td><asp:TextBox ID="tbTtl" runat="server" MaxLength="255" Width="95%"></asp:TextBox></td>
    </tr>
    <tr>
      <td class="right"> 打开方式：</td>
      <td><asp:DropDownList ID="ddlTarget" runat="server"> 
          <asp:ListItem Selected="True"> </asp:ListItem>
          <asp:ListItem>_self</asp:ListItem>
          <asp:ListItem>_blank</asp:ListItem>
          <asp:ListItem>_parent</asp:ListItem>
          <asp:ListItem>_top</asp:ListItem>
      </asp:DropDownList></td>
    </tr>
    <tr>
      <td class="right">是否使用：</td>
      <td><asp:CheckBox ID="cbIsUse" runat="server" /></td>
    </tr>
  </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
  <cc1:EnterButton verify="true" ID="btnEnter" runat="server" Text="确定" OnClick="btnEnter_Click"
        ValidationGroup="enter" />
  <%--<Song:DeleteButton ID="DeleteButton1" runat="server" OnClick="btnDelete_Click" />--%>
  <cc1:CloseButton ID="CloseButton1" runat="server" />
</asp:Content>
