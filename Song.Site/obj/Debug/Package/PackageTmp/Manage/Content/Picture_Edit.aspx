<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    Codebehind="Picture_Edit.aspx.cs" Inherits="Song.Site.Manage.Content.Picture.PictureInfo_Edit"
    Title="无标题页" %>
<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<%@ Register Assembly="WeiSha.WebEditor" Namespace="WeiSha.WebEditor" TagPrefix="WebEditor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server"> 
  <script language="javascript" src="../Utility/datepicker/WdatePicker.js" type="text/javascript"></script> 
  <script language="javascript" src="scripts/tab.js" type="text/javascript"></script>
  <div id="tabTitleBox">
    <div class="titItem curr" tab="tab1"> 基本信息</div>

    <div class="titItem" tab="tab4"> 发布设置</div>
    <div class="titItem" tab="tab5"> 自定义信息</div>
  </div>
  <div id="tabContextBox">
    <div class="contItem" tab="tab1">
      <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
          <td width="80px" class="right"> 图片名称：</td>
          <td><asp:TextBox nullable="false" ID="tbName" runat="server" Width="95%" MaxLength="255"></asp:TextBox></td>
        </tr>
        <tr>
          <td class="right">栏目分类：</td>
          <td><asp:DropDownList ID="ddlColumn" runat="server" Width="150"> </asp:DropDownList>&nbsp;<asp:CheckBox ID="cbIsShow" runat="server" Checked="True" Text="是否显示" />
            <asp:CheckBox ID="cbIsRec" runat="server" Checked="True" Text="是否推荐" />
            <asp:CheckBox ID="cbIsHot" runat="server" Checked="True" Text="热点" />
            <asp:CheckBox ID="cbIsTop" runat="server" Checked="True" Text="置顶" /></td>
        </tr>
        <tr>
          <td class="right" valign="top">简介：</td>
          <td><asp:TextBox ID="tbIntro" runat="server" MaxLength="255" TextMode="MultiLine" lenlimit="250"
                            Width="95%" Height="100px"></asp:TextBox></td>
        </tr>
        <tr>
          <td class="right">标签：</td>
          <td><asp:TextBox ID="tbLabel" runat="server" Width="95%" MaxLength="255"></asp:TextBox></td>
        </tr>
        <tr>
          <td class="right">上传图片：</td>
          <td> <cc1:FileUpload ID="fuLoad" runat="server" fileallow="jpg|bmp|gif|png" Width="260px" /></td>
        </tr>
        <tr>
          <td class="right"></td>
          <td> <div id="picShowBox"> <img src="../../Images/nophoto.gif" name="imgShow" height="350" id="imgShow" style="width:60%" runat=server /></div></td>
        </tr>
      </table>
      
     
    </div>

    <div class="contItem" tab="tab4"> <!--发布设置-->
      <table width="100%" border="0" cellpadding="0" cellspacing="0">
        <tr>
          <td width="80px" class="right"> Keywords：</td>
          <td><asp:TextBox ID="tbKeywords" runat="server" MaxLength="255" Width="95%"></asp:TextBox></td>
        </tr>
        <tr>
          <td class="right" valign="top"> Description：</td>
          <td><asp:TextBox ID="tbDescr" runat="server" MaxLength="255" TextMode="MultiLine" lenlimit="250"
                            Width="95%" Height="100px"></asp:TextBox></td>
        </tr>
        <tr>
          <td class="right"> 上线时间：</td>
          <td><asp:TextBox ID="tbPushTime" runat="server" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})"
                            MaxLength="255" Width="160"></asp:TextBox></td>
        </tr>
      </table>
    </div>
    <div class="contItem" tab="tab5"> </div>
  </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
  <cc1:EnterButton verify=true  ID="btnEnter" runat="server" Text="确定" OnClick="btnEnter_Click" ValidationGroup="enter" />
  <%--<Song:DeleteButton ID="DeleteButton1" runat="server" OnClick="btnDelete_Click" />--%>
  <cc1:CloseButton ID="CloseButton1" runat="server" />
</asp:Content>
