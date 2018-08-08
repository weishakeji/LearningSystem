<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    Codebehind="LinksInfo_Edit.aspx.cs" Inherits="Song.Site.Manage.Site.Links.LinksInfo_Edit"
    Title="无标题页" %>
<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
  <table width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr>
      <td width="80" class="right">链接名称：</td>
      <td><asp:TextBox ID="tbName" runat="server" nullable="false" Width="90%"></asp:TextBox></td>
    <td rowspan="5" width="200"><div class="logoBox">
          <div id="picShowBox"> <img src="../../Images/nophoto.gif" name="imgShow" style="width:100px" id="imgShow" runat=server /></div>
          网站Logo：
          <cc1:FileUpload ID="fuLoad" runat="server" width="100" fileallow="jpg|bmp|gif|png"  />
        </div></td> </tr>
        <tr>
      <td class="right">链接地址：</td>
      <td><asp:TextBox ID="tbUrl" runat="server" nullable="false" Width="90%"></asp:TextBox></td>
    </tr>
    <tr>
      <td class="right"></td>
      <td><asp:CheckBox ID="cbIsShow" runat="server" Checked="True" Text="是否显示" />
        <asp:CheckBox ID="cbIsUse" runat="server" Checked="True" Text="是否使用" /></td>
       
    </tr>
    <tr>
      <td class="right">所在分类：</td>
      <td><asp:DropDownList ID="ddlSort" runat="server" Width="160" novalue="-1"> </asp:DropDownList></td>

    </tr>
       <tr>
      <td class="right">站长信箱：</td>
      <td><asp:TextBox ID="tbEmail" runat="server" MaxLength="100" Width="250px"></asp:TextBox></td>
    </tr>
    
    <tr>
      <td valign="top" class="right">说明：</td>
      <td valign="top" colspan="2"><asp:TextBox ID="tbIntro" runat="server" Height="120px" MaxLength="255" TextMode="MultiLine"
        Width="100%"></asp:TextBox></td>
      
    </tr>
  </table>
  <asp:Panel ID="plApply" runat="server">
    <fieldset>
    <legend>申请信息 -
    <asp:CheckBox ID="cbVerify" runat="server" Checked="True" Text="是否通过审核" />
    </legend>
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
      <tr>
        <td width="80" class="right"> 站长姓名：</td>
        <td><asp:TextBox ID="tbMaster" runat="server" Width="180px"></asp:TextBox></td>
        <td  width="80" class="right"> 站长QQ：</td>
        <td><asp:TextBox ID="tbQQ" runat="server" Width="180px"></asp:TextBox></td>
      </tr>
      <tr>
        <td class="right"> 联系电话：</td>
        <td colspan="3"><asp:TextBox ID="tbMobile" runat="server" Width="180px"></asp:TextBox></td>
      </tr>
      <tr>
        <td class="right"> 申请说明：</td>
        <td colspan="3"><asp:TextBox ID="tbExplain" runat="server" Width="98%" Height="80px" TextMode="MultiLine"></asp:TextBox></td>
      </tr>
    </table>
    </fieldset>
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
  <cc1:EnterButton ID="btnEnter" runat="server" Text="确定" verify="true" OnClick="btnEnter_Click"
        ValidationGroup="enter" />
  <%--<Song:DeleteButton ID="DeleteButton1" runat="server" OnClick="btnDelete_Click" />--%>
  <cc1:CloseButton ID="CloseButton1" runat="server" />
</asp:Content>
