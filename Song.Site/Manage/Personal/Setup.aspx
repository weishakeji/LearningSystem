<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true" CodeBehind="Setup.aspx.cs" Inherits="Song.Site.Manage.Personal.Setup" Title="无标题页" %>
<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
<script type="text/javascript" src="../CoreScripts/hanzi2pinyin.js"></script>
  <table width="100%" border="0" cellspacing="0" cellpadding="0">
    
     <tr>
      <td width="80" class="right">每日一练：</td>
      <td>
          <asp:CheckBoxList ID="cblSubject" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
          </asp:CheckBoxList>
          <hr />
          注：该项设置用于在首页“每日一练”中的学习范围。
      </td>
     
    </tr>
   
 
  </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
  <cc1:EnterButton verify=true  ID="btnEnter" runat="server" Text="确定" OnClick="btnEnter_Click" ValidationGroup="enter" />
  <%--<Song:DeleteButton ID="DeleteButton1" runat="server" OnClick="btnDelete_Click" />--%>
  <cc1:CloseButton ID="CloseButton1" runat="server" Visible=false />
</asp:Content>
