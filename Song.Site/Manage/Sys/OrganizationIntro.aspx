<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true" CodeBehind="OrganizationIntro.aspx.cs" Inherits="Song.Site.Manage.Sys.OrganizationIntro" Title="无标题页" %>
<%@ Register Assembly="WeiSha.WebEditor" Namespace="WeiSha.WebEditor" TagPrefix="WebEditor" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
  <p>
    <WebEditor:Editor ID="tbIntro" runat="server" Height="400px" Width="99%">
    </WebEditor:Editor>
  </p>
  <p style="margin-top:10px;"> 　　　　　
    <cc1:Button ID="BtnEnter" runat="server" Text="确  定" verify="true" OnClick="BtnEnter_Click"  />
  </p>
</asp:Content>
