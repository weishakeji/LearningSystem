<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    Codebehind="ProductMessage_Edit.aspx.cs" Inherits="Song.Site.Manage.Content.ProductMessage_Edit"
    Title="无标题页" %>
<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebEditor" Namespace="WeiSha.WebEditor" TagPrefix="WebEditor" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
  <table width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr>
      <td valign="top"><table width="100%" border="0" cellspacing="0" cellpadding="0">
          
          <tr>
            <td width="230" align="center" valign="top"><div id="picShowBox"> <img src="../../Images/nophoto.gif" name="imgShow" width="200" height="150" id="imgShow" runat=server /></div>
            <asp:Label ID="lbName" runat="server" Font-Bold="True" ></asp:Label>
               <br />属性：
              <asp:Label ID="lbIsUse" runat="server" Text="禁用" ForeColor="Red" ></asp:Label>
              <asp:Label ID="lbIsNew" runat="server" Text="新产品" ></asp:Label>
              <asp:Label ID="lbIsRec" runat="server" Text="推荐" ></asp:Label>
            </td>
            <td valign="top">所属分类：
              <asp:Label ID="lbColumn" runat="server" ></asp:Label>
              <br />
              型号：
              <asp:Label ID="lbModel" runat="server" ></asp:Label>
              <br />
              编号：
              <asp:Label ID="lbCode" runat="server" ></asp:Label>
              <br />
              上市时间：
              <asp:Label ID="lbPushTime" runat="server" ></asp:Label>
              <br />
              价格：
              <asp:Label ID="lbPrise" runat="server" ></asp:Label>
              元<br />

              重量：
              <asp:Label ID="lbWeight" runat="server" ></asp:Label>
              千克　　　 <br />
              库存：
              <asp:Label ID="lbStocks" runat="server" ></asp:Label>
              <asp:Label ID="lbUnit" runat="server" ></asp:Label>
              <br />
              关键字：
              <asp:Label ID="lbKeywords" runat="server" ></asp:Label>
              <br />
              编辑人员：
              <asp:Label ID="lbAccName" runat="server" ></asp:Label>
            </td>
          <tr>
            <td colspan="2"> 简要说明：<br />
             <p> <asp:Label ID="lbIntro" runat="server" ></asp:Label>
              </p><br />
</td>
          </tr>
          <tr>
            <td colspan="2"> 详细说明：<br />
              <asp:Label ID="lbDetails" runat="server" ></asp:Label></td>
          </tr>
        </table></td>
        <!--右侧为留言回复 -->
      <td width="400" valign="top">主题：<asp:TextBox ID="tbText" runat="server" MaxLength="200" Width="276px"></asp:TextBox><br />
          内容：
          <br />
          <asp:TextBox ID="tbContext" runat="server" Rows="5" TextMode="MultiLine" Width="99%"></asp:TextBox><br />
          留言时间：<asp:Label ID="lbCrtTime" runat="server" Text=""></asp:Label>　　IP：<asp:Label ID="lbIP" runat="server" Text=""></asp:Label><br />
          电话：<asp:Label ID="lbPhone" runat="server" Text=""></asp:Label><br />
          邮箱：<asp:Label ID="lbEmail" runat="server" Text=""></asp:Label><br />
          地址：<asp:Label ID="lbAddress" runat="server" Text=""></asp:Label><br />
          回复：<br />
          <asp:TextBox ID="tbAnswer" runat="server" MaxLength="255" Rows="5" TextMode="MultiLine" Width="99%"></asp:TextBox><br />
          <asp:CheckBox ID="cbIsShow" runat="server"  Text="是否在网站显示"/></td>
    </tr>
  </table>
 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
  <cc1:EnterButton verify="true" ID="btnEnter" runat="server" Text="确定" OnClick="btnEnter_Click"
        ValidationGroup="enter" />
    <cc1:DeleteButton ID="DeleteButton1" runat="server"  OnClick="btnDelete_Click"/>
    <cc1:CloseButton ID="CloseButton1" runat="server" />
</asp:Content>
