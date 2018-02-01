<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    Codebehind="Admin_Edit.aspx.cs" Inherits="Song.Site.Manage.Sys.Admin_Edit"
    Title="无标题页" %>

<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">

    <script language="javascript" src="../Utility/datepicker/WdatePicker.js" type="text/javascript"></script>

    <script type="text/javascript" src="../CoreScripts/hanzi2pinyin.js"></script>

    <table width="100%" border="0" cellspacing="2" cellpadding="0" class="tableContext">
  <tr>
            <td width="80" class="right">
                所在机构：</td>
            <td>
                <asp:Label ID="lbOrg" runat="server" Text=""></asp:Label></td>
      </tr>
      
      <tr>
       <td class="right">
                登录账号：</td>
            <td>
                <asp:TextBox nullable="false" ID="tbAccName" runat="server" MaxLength="100" Width="160px"></asp:TextBox></td>
      </tr>
        <tr>
            <td class="right">
                姓名：</td>
            <td>
                <asp:TextBox ID="tbName" runat="server" nullable="false" Width="160px" ></asp:TextBox> </td>
        </tr>
         <tr>
            <td class="right">
                手机号：</td>
            <td>
                <asp:TextBox ID="tbMobile" runat="server" datatype="mobile" MaxLength="15" Width="160px"></asp:TextBox> (手机号可以替代账号登录系统)           </td>
        </tr>
        <tr>
            <td class="right">
                密码：</td>
            <td>
                <asp:TextBox ID="tbPw" runat="server" Width="160px" ></asp:TextBox>(重置或预设机构管理员密码)  </td>
        </tr>
        <tr>
            <td class="right">
                性别：</td>
            <td>
                <asp:RadioButtonList ID="rbSex" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                    <asp:ListItem Selected="True" Value="1">男</asp:ListItem>
                    <asp:ListItem Value="2">女</asp:ListItem>
                </asp:RadioButtonList>            </td>
        </tr>
       
        <tr>
            <td class="right">
                年龄：</td>
            <td >
                <asp:TextBox ID="tbAge" runat="server" datatype="uint" MaxLength="3" Width="50px"></asp:TextBox>            </td>
        </tr>
          <tr>
            <td class="right">
                身份证：</td>
            <td>
                <asp:TextBox ID="tbIdCard" runat="server" MaxLength="160" Width="160px"></asp:TextBox>            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
    <cc1:EnterButton verify="true" ID="btnEnter" runat="server" Text="确定" OnClick="btnEnter_Click"
        ValidationGroup="enter" />
    <cc1:CloseButton ID="CloseButton1" runat="server" />
</asp:Content>
