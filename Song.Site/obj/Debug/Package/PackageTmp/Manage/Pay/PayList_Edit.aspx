<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    Codebehind="PayList_Edit.aspx.cs" Inherits="Song.Site.Manage.Pay.PayList_Edit"
    Title="无标题页" %>
<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>

<%@ Register src="PayInterfacerType.ascx" tagname="PayInterfacerType" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
  <table width="100%" border="0" cellspacing="2" cellpadding="0" class="tableContext">
  <tr>
            <td width="80" class="right">
                支付方式：</td>
            <td>
               
               
                <uc1:PayInterfacerType ID="PayInterfacerType1" runat="server" />
               
               
               </td>
      </tr>
      </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">

  <cc1:CloseButton ID="CloseButton1" runat="server" />
</asp:Content>
