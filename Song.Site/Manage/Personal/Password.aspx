<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true" CodeBehind="Password.aspx.cs" Inherits="Song.Site.Manage.Personal.Password" Title="无标题页" %>
<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
<p>员工姓名：  
  <asp:Label ID="lbName" runat=server></asp:Label>(<asp:Label ID="lbAcc" runat=server></asp:Label>)</p>
  <p>
  原密码：<asp:TextBox nullable="false" ID="tbOldPw" runat="server" MaxLength="100" TextMode="Password"></asp:TextBox>
  <asp:Label ID="lbShow" runat=server Text="原密码不正确" Visible=False ForeColor="Red"></asp:Label></p>
  <br />
    <p>
        新密码：
        <asp:TextBox nullable="false" ID="tbPw1" runat="server" MaxLength="100"  sametarget="tbPw2" TextMode="Password"></asp:TextBox>
        </p>
    <p>
        再输入：
        <asp:TextBox ID="tbPw2" runat="server" MaxLength="100"  TextMode="Password"></asp:TextBox>
        </p>
            
            <p></p>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
 <cc1:EnterButton verify=true  ID="btnEnter" runat="server" Text="确定" OnClick="btnEnter_Click" ValidationGroup="enter" />
    <%--<Song:DeleteButton ID="DeleteButton1" runat="server" OnClick="btnDelete_Click" />--%>
    <cc1:CloseButton ID="CloseButton1" runat="server" />
</asp:Content>
