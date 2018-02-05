<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    Codebehind="User_Password.aspx.cs" Inherits="Song.Site.Manage.Site.User_Password"
    Title="无标题页" %>

<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
用户姓名：  
  <asp:Label ID="lbName" runat=server></asp:Label>(<asp:Label ID="lbAcc" runat=server></asp:Label>)
    <div id="trPw1" runat="server">
        重置密码：
        <asp:TextBox nullable="false" ID="tbPw1" runat="server" MaxLength="100"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbPw1"
            ErrorMessage="*不得为空" ValidationGroup="enter"></asp:RequiredFieldValidator></div>
    <div id="trPw2" runat="server">
        再次输入：
        <asp:TextBox ID="tbPw2" runat="server" MaxLength="100"></asp:TextBox>
        <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="tbPw2"
            ControlToValidate="tbPw1" ErrorMessage="*两次密码输入不同" ValidationGroup="enter"></asp:CompareValidator></div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
    <cc1:EnterButton verify=true  ID="btnEnter" runat="server" Text="确定" OnClick="btnEnter_Click" ValidationGroup="enter" />
    <cc1:CloseButton ID="CloseButton1" runat="server" />
</asp:Content>
