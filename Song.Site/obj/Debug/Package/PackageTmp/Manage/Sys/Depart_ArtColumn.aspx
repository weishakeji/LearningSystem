<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true" 
CodeBehind="Depart_ArtColumn.aspx.cs" Inherits="Song.Site.Manage.Sys.Depart_ArtColumn" Title="权限管理" %>
<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
  <script type="text/javascript" src="Scripts/Purview/Node.js"></script>
  <script type="text/javascript" src="Scripts/Purview/StateTree.js"></script>
  <!--顶部条 -->
  <div id="contTop">
    <div id="treeName"> 设置 <span id="typeName"></span>
      &#8220;<asp:Literal ID="ltName" runat="server"></asp:Literal>&#8221;
     
      的文章管理权限：
      <asp:Label ID="lbName" runat="server" Text=""></asp:Label>
    </div>
    <!--预载进度条 -->
    <div id="loadingBar"> <img src="../Images/loading/load_016.gif" alt=""/></div>
  </div>
  <!-- 菜单区域 -->
  <div id="TreeAreaPanel"> </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
  <input name="btnEnter" type="button" value="确定" class="Button" />
  <input name="CloseButton" type="button" value="关闭" class="Button"/>
</asp:Content>
