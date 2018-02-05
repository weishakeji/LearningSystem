<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true" CodeBehind="InnerEmail_Edit.aspx.cs" Inherits="Song.Site.Manage.Common.InnerEmail_Edit" Title="无标题页" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
  <p>发件人：</p>
  <p>抄送：</p>
  <p>暗送：</p>
  <p>邮件主题：</p>
  <p>邮件内容：</p>
  <p>附件：</p>
  <p>重要度：一般 重要 非常重要</p>
  <p>&nbsp;</p>
  <p>立即发送 保存到草稿箱 关闭</p>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
<cc1:EnterButton verify=true  ID="Button1" runat="server" Text="立即发送" OnClick="btnEnter_Click" ValidationGroup="enter" />
    <asp:Button verify=true  ID="btnEnter" runat="server" Text="保存到草稿箱" OnClick="btnEnter_Click" ValidationGroup="enter" />
   
    <cc1:CloseButton ID="CloseButton1" runat="server" />
</asp:Content>
