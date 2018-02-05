<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="toolsBar.ascx.cs" Inherits="Song.Site.Manage.Utility.toolsBar" %>
<div class="toolsBar">
<asp:Button ID="btnAdd" runat="server"  Text="新增" OnClick="btnAdd_Click" CssClass="btnAdd toolsBtn" />
<asp:Button ID="btnModify" runat="server" Text="修改" OnClick="btnModify_Click"  CssClass="btnModfiy toolsBtn" Visible="false" />
<asp:Button ID="btnDelete" runat="server" Text="删除" OnClick="btnDelete_Click"  CssClass="btnDelete toolsBtn" />
<asp:Button ID="btnView" runat="server" Text="查看"  Visible="false"  OnClick="btnView_Click"  CssClass="btnView toolsBtn" />
<asp:Button ID="btnVerify" runat="server" Text="审核" Visible="false" OnClick="btnVerify_Click"  CssClass="btnVerify toolsBtn" />
<asp:Button ID="btnRecover" runat="server" Text="还原" Visible="false" OnClick="btnRecover_Click"  CssClass="btnBack toolsBtn" />
<asp:Button ID="btnAnswer" runat="server" Text="回复" Visible="false" OnClick="btnAnswer_Click"  CssClass="btnReturn toolsBtn" />
<asp:Button ID="btnInput" runat="server" Text="导入" Visible="false" CssClass="btnGeneral toolsBtn" />
<asp:Button ID="btnOutput" runat="server" Text="导出" OnClick="btnOutput_Click" Visible="false" CssClass="btnGeneral toolsBtn" />
<asp:Button ID="btnPrint" runat="server" Text="打印" CssClass="btnGeneral toolsBtn"  Visible="false" />

</div>
<script language="JavaScript" type="text/javascript">
//gridview的id
var GridViewId="<%=GvName %>";
//是否弹出窗口
var isWinOpen=<%= IsWinOpen.ToString().ToLower() %>;
//子窗口路径
var ChildPage="<%= WinPath %>";
//导入窗口路径
var InputPath="<%= InputPath %>";
//子窗口宽高
var ChildPageWd=<%= WinWidth %>;
var ChildPageHg=<%= WinHeight %>;  
//页面自身的地址
var SelfPage="<%= this.Page.Server.UrlEncode(this.Page.Request.Url.AbsoluteUri)%>";
//删除时的提示信息
var DelShow="\n<%= DelShowMsg %>";
//是否允许批量还原
var IsBatchReco=<%= IsBatchReco.ToString().ToLower() %>;
</script>