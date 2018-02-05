<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true" CodeBehind="EmpGroup_Edit.aspx.cs" Inherits="Song.Site.Manage.Sys.EmpGroup_Edit" Title="无标题页" %>
<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
  <table width="100%" border="0" cellspacing="2" cellpadding="0" class="tableContext">
    <tr>
      <td width="80" class="right"> 组名称：</td>
      <td><asp:TextBox nullable="false" ID="tbName" runat="server"></asp:TextBox>
        <asp:Label ID="lbIsSytem" runat="server" Text="(系统组不允许修改名称)"></asp:Label>
      </td>
    </tr>
    <tr>
      <td class="right"> 状态：</td>
      <td><asp:CheckBox ID="cbIsUse" runat="server" Checked="True" Text="是否启用" />
      </td>
    </tr>
    <tr>
      <td class="right" valign="top">说明： </td>
      <td><asp:TextBox ID="tbIntro" runat="server" Height="120px" MaxLength="255" TextMode="MultiLine"
        Width="99%"></asp:TextBox>
      </td>
    </tr>
  </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
  <cc1:EnterButton verify=true  ID="btnEnter" runat="server" Text="确定" OnClick="btnEnter_Click" ValidationGroup="enter" />
  <%--<Song:DeleteButton ID="DeleteButton1" runat="server" OnClick="btnDelete_Click" />--%>
  <cc1:CloseButton ID="CloseButton1" runat="server" />
</asp:Content>
