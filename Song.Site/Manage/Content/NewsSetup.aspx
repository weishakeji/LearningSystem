<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true" CodeBehind="NewsSetup.aspx.cs" Inherits="Song.Site.Manage.Content.NewsSetup" Title="无标题页" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
  <fieldset>
  <legend>常用</legend>
  最多置顶数：
  <asp:TextBox ID="tbMaxTop" runat="server" Text="10" MaxLength="3" Width="60px"></asp:TextBox>
  <br />
  最多推荐数：
  <asp:TextBox ID="tbMaxRec" runat="server" Text="10" MaxLength="3" Width="60px"></asp:TextBox>
  <br />
  <asp:CheckBox ID="cbIsVerify" runat="server" Text="新增文章是否需要审核" />
  <br />
  <asp:CheckBox ID="cbIsReVeri" runat="server" Text="文章修改后是否需要再次审核" />
  <br />
  常用的文章来源：<br />
  <asp:TextBox ID="tbSource" runat="server" Text="" MaxLength="250" Width="300px"></asp:TextBox>
  <br />
  <asp:Button ID="btnEnter" runat="server" Text="保存常用参数" OnClick="btnEnter_Click" ValidationGroup="enter" />
  </fieldset>
  <fieldset>
  <legend>二维码</legend>
  <table width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr>
      <td><strong>资讯文章的二维码</strong><br />
        图片宽高：
        <asp:TextBox ID="tbArtQrWH" runat="server" MaxLength="3"
            Width="50px"></asp:TextBox>
        px <br />
        信息模板：<br />
        <asp:TextBox ID="tbArtQrTextTmp" runat="server" Height="133px" TextMode="MultiLine" Width="290px"></asp:TextBox><br />
<asp:Button ID="btnArtEnter" runat="server" Text="保 存" OnClick="btnArtEnter_Click" />
<asp:Button ID="btnArtBuild" runat="server" Text="保存并批量生成" OnClick="btnArtBuild_Click" />
</td>
      <td><strong>资讯专题的二维码</strong><br />
        图片宽高：
        <asp:TextBox ID="tbSpecQrWH" runat="server" MaxLength="3"
            Width="50px"></asp:TextBox>
        px <br />
        信息模板：<br />
        <asp:TextBox ID="tbSpecQrTextTmp" runat="server" Height="133px" TextMode="MultiLine" Width="290px"></asp:TextBox><br />
<asp:Button ID="btnSpecEnter" runat="server" Text="保 存" OnClick="btnSpecEnter_Click" />
<asp:Button ID="btnSpecBuild" runat="server" Text="保存并批量生成" OnClick="btnSpecBuild_Click" />
</td>
    </tr>
  </table>
  </fieldset>
</asp:Content>
