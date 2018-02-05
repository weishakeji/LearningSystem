<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    Codebehind="ReLogin.aspx.cs" Inherits="Song.Site.Manage.ReLogin" Title="无标题页" %>

<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
<div id="ShowPanel">&nbsp;
                <span id="showtext" style="color: red;"></span>
                <asp:CustomValidator ID="CustomValidator1" runat="server" ValidationGroup="enter"
                    ErrorMessage="验证码错误" OnServerValidate="CustomValidator1_ServerValidate" ControlToValidate="tbCode"
                    Display="Dynamic" SetFocusOnError="True" ValidateEmptyText="True"></asp:CustomValidator>
                <asp:CustomValidator ID="CustomValidator2" runat="server" ControlToValidate="tbPw1"
                    Display="Dynamic" ErrorMessage="密码错误" SetFocusOnError="True" ValidationGroup="enter"></asp:CustomValidator>
            </div>
    <div id="InputPanel">
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
  <tr>
    <td width="100px" class="right">工号/手机号：</td>
    <td> <asp:TextBox ID="tbAccName" runat="server" MaxLength="50" Width="135px" TabIndex="1" Text=""></asp:TextBox>
        <font color="#ff0000">*</font></td>
    
  </tr>
  <tr>
    <td class="right">  密 码：</td>
    <td> <asp:TextBox ID="tbPw1" runat="server" MaxLength="50" TextMode="Password" Width="135px" TabIndex="2"></asp:TextBox>
        <font color="#ff0000">*</font></td>
    
  </tr>
  <tr>
    <td class="right"> 验证码：</td>
    <td> <asp:TextBox ID="tbCode" runat="server" MaxLength="100" Width="60px" TabIndex="3"></asp:TextBox>
        <font color="#ff0000">*</font>
        <img id="codeImg" src="Utility/CodeImg.aspx?len=4&name=relogin"  title="点击更换验证码" /></td>
   
  </tr>
</table>


    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
<cc1:EnterButton verify=true  ID="btnEnter" runat="server" Text="确定" OnClick="btnEnter_Click" ValidationGroup="enter" />
&nbsp;
</asp:Content>
