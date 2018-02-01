<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true" CodeBehind="View.aspx.cs" Inherits="Song.Site.Manage.Personal.View" Title="无标题页" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
  <table width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr>
      <td width="100" class="right">员工姓名：</td>
      <td><asp:Label ID="lbName" runat=server></asp:Label>
        (
        <asp:Label ID="lbAcc" runat=server></asp:Label>
        )</td>
    </tr>
    <tr>
      <td class="right">员工工号：</td>
      <td><asp:Label ID="lbEmpCode" runat=server></asp:Label></td>
    </tr>
    <tr>
      <td class="right">所在院系：</td>
      <td><asp:Label ID="lbDepart" runat=server></asp:Label></td>
    </tr>
    <tr>
      <td class="right">所在工作组：</td>
      <td><asp:Label ID="lbGroup" runat=server></asp:Label></td>
    </tr>
    <tr>
      <td class="right">职务：</td>
      <td><asp:Label ID="lbPosi" runat=server></asp:Label></td>
    </tr>
    <tr>
      <td class="right">Email：</td>
      <td><asp:Label ID="lbEmail" runat=server></asp:Label></td>
    </tr>
    <tr>
      <td class="right">入职时间：</td>
      <td><asp:Label ID="lbRegTime" runat=server></asp:Label></td>
    </tr>
  </table>
</asp:Content>
