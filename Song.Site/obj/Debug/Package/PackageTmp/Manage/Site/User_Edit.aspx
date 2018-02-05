<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    Codebehind="User_Edit.aspx.cs" Inherits="Song.Site.Manage.Site.User_Edit"
    Title="无标题页" %>
<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
  <script language="javascript" src="../Utility/datepicker/WdatePicker.js" type="text/javascript"></script>
  
  帐号：
  <asp:TextBox nullable="false" ID="tbAccName" runat="server" MaxLength="100"></asp:TextBox>
   <div id=trPw1 runat=server> 初始密码：
    <asp:TextBox ID="tbPw1" runat="server" MaxLength="100"></asp:TextBox>
  </div>
  <div id=trPw2 runat=server> 再次输入：
    <asp:TextBox ID="tbPw2" runat="server" MaxLength="100"></asp:TextBox>
  </div><br/>
  姓名：
  <asp:TextBox ID="tbName" runat="server" MaxLength="100"></asp:TextBox>
    <br />
  性别：<asp:RadioButtonList ID="rbSex" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
      <asp:ListItem Selected="True" Value="1">男</asp:ListItem>
      <asp:ListItem Value="2">女</asp:ListItem>
 
      </asp:RadioButtonList>
    &nbsp;<br />
    <asp:CheckBox ID="cbIsUse" runat="server" Checked="True" Text="是否使用"/>
    <br />
  <br/>
  用户组：
  <asp:DropDownList ID="ddlGroup" runat="server"> </asp:DropDownList>&nbsp;
  

  <br/>
  电子信箱：
  <asp:TextBox ID="tbEmail" MaxLength="80" Width=50% runat="server"></asp:TextBox>
  <br/>
  电　　话：
  <asp:TextBox ID="tbTel" MaxLength="50" Width=50% runat="server"></asp:TextBox>
  <br/>
  手　　机：
  <asp:TextBox ID="tbMobleTel" MaxLength="50" Width=50% runat="server"></asp:TextBox>
  <br/>
  Q　　 Q： 
  <asp:TextBox ID="tbQQ" MaxLength="50" Width=50% runat="server"></asp:TextBox> 
  <br/>
  　MSN： 
  <asp:TextBox ID="tbMsn" MaxLength="50" Width=50% runat="server"></asp:TextBox> 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
  <cc1:EnterButton verify=true  ID="btnEnter" runat="server" Text="确定" OnClick="btnEnter_Click" ValidationGroup="enter" />
  <cc1:CloseButton ID="CloseButton1" runat="server" />
</asp:Content>
