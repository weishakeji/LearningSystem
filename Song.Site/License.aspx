<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="License.aspx.cs" Inherits="Song.Site.License" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>授权管理与版本信息</title>
    <style type="text/css">
<!--
body {
	margin: 0px;
	padding: 0px;
}
* {
	font-size: 14px;
}
#header {
	position: fixed;
	background: #666;
	width: 100%;
	height: 110px;
	line-height: 30px;
	z-index: 9998;
	_width: 100%;
	_position: absolute;
	_top: 0px;
	top: 0px;
}
#header .top {
	width: 780px;
	margin-right: auto;
	margin-left: auto;
	color: #CCC;
}
#header .top span{	
	color: #CCFF66;
}
#header .verInfo {
	background-color: #ccc;
	height: 80px;	
}
.title {
	line-height: 50px;
	font-weight: bold;
	color: #333333;
	height: 50px;
	margin-left: auto;
	width: 780px;
	margin-right: auto;
}
.title .titleLeft {
	font-family: "Microsoft Yahei", "微软雅黑", Tahoma, Arial, Helvetica, STHeiti;
	font-size: 28px;
	line-height: 50px;
	font-weight: bold;
	color: #333333;
	height: 50px;
	margin-left: 0px;
	min-width: 300px;
	float: left;
	margin-right:10px;
}
.title .titleLeft span {
	font-family: "Microsoft Yahei", "微软雅黑", Tahoma, Arial, Helvetica, STHeiti;
	font-size: 28px;
	line-height: 50px;
	font-weight: bold;
	color: #333333;
}
.title .titleRight {
	height: 50px;
	padding-top: 12px;
}
.licInfo {
	margin-left: auto;
	width: 780px;
	margin-right: auto;
	background-color: #CCC;
}
.licInfo span {
	margin-right: 10px;
}
.context {
	margin-bottom: 10px;
	margin-left: auto;
	width: 780px;
	margin-right: auto;
	margin-top: 120px;
	
}
.spanVersion {
	font-size: 13px !important;
	height: 15px;
	line-height: 15px;
	font-family: "Microsoft Yahei", "微软雅黑", Tahoma, Arial, Helvetica, STHeiti;
}
.explain {
	line-height: 20px;
	text-align: left;
}
.licTable td {
	padding: 10px;
}
.infoBox {
	margin-top: 20px;
	margin-bottom: 20px;
}
.limitBox {
	display: table;
}
.tit {
	font-weight: bold;
	margin-bottom: 5px;
}
.limitBox .context {
	width: 750px;
}
.limitItem {
	line-height: 25px;
	float: left;
	height: 25px;
	width: 150px;
	margin-left: 1px;
}
.licInfoBox {
	margin-left: 20px;
	color: #930;
	background-image: url(<%= copyright["weixinqr"] %>);
	background-repeat: no-repeat;
	background-position: right top;
	min-height:260px;
}
.blue {
	color: #0066FF;
	font-weight: bold;
	font-size: 15px;
}
a {
	color: #0066FF;
	text-decoration: underline;
	margin-right: 20px;
	margin-left: 20px;
}
.right {
	text-align: right;
}
#refresh
{
	color: #06C;
	cursor: pointer;
}
.gv {
	width: 780px;
	margin: 10px;
}
.gv th {
	background-color: #eee;
}
.gv td, .gv th {
	text-align: center;
	line-height: 25px;
}
#footer {
	position: fixed;
	bottom: 0;
	background: #000;
	width: 100%;
	height: 30px;
	line-height: 30px;
	z-index: 9999;
	opacity: .80;
	filter: alpha(opacity=80);
	_bottom: auto;
	_width: 100%;
	_position: absolute;
}
#footer a {
	color: #fff;
	font-size: 13px;
	letter-spacing: 2px;
	margin-right: 0px;
	margin-left: 0px;
}
.copyright {
	font-family: arial;
	margin-left: 10px;
	color: #FFF;
	text-align: center;
}
-->
    </style>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="header">
        <div class="top">
            系统于
            <asp:Label ID="lbInitDate" runat="server" CssClass="s" Text=""></asp:Label>
            开始运行，已经正常运行
            <asp:Label ID="lbRunTime" runat="server" CssClass="s"  Text=""></asp:Label>
        </div>
        <div class="verInfo">
            <div class="title">
                <div class="titleLeft">
                    当前系统副本为
                    <asp:Label ID="lbVersion" runat="server" ForeColor="red" Text=""></asp:Label>
                </div>
                <div class="titleRight">
                    <div class="spanVersion">
                        Version
                        <asp:Literal ID="ltVersion" runat="server"></asp:Literal>
                    </div>
                    <div class="spanVersion">
                        <%= AssemblyTitle()%></div>
                </div>
            </div>
            <div class="licInfo" id="licInfo" runat="server" >
                <span>授权类型：
                    <asp:Literal ID="ltLicType" runat="server" Text=""></asp:Literal>
                </span><span>授权对象：
                    <asp:Literal ID="ltLicTarget" runat="server" Text=""></asp:Literal>
                </span><span>授权时效：
                    <asp:Literal ID="ltStartTime" runat="server" Text=""></asp:Literal>
                    至
                    <asp:Literal ID="ltEndTime" runat="server" Text=""></asp:Literal>
                </span>
                <span id="refresh">刷新</span>
            </div>
        </div>
    </div>
    <div class="context">
        <div class="limitBox">
            <div class="tit">
                以下为当前版本所能承载的最大数据量：</div>
            <div>
                <asp:Repeater ID="rptLimit" runat="server">
                    <ItemTemplate>
                        <div class="limitItem">
                            <%# Eval("Key") %>
                            :
                            <%# Convert.ToInt32(Eval("Value")) == 0 ? "不限" : Eval("Value")%>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
        <hr />
        <asp:Panel ID="plLicInfoBox" runat="server">
            <div class="licInfoBox">
                <asp:Literal ID="ltLicInfo" runat="server">系统根目录下不存在授权文件license.txt；<br />
                或license.txt中无内容。</asp:Literal>
            </div>
            <hr />
        </asp:Panel>
        <div class="explain">
            <ol>
                说明：<br />
                <li>如果上述版本的功能无法满足您的需求，升级请联系<a class="blue" href="<%= copyright["taobao"]%>"
                    target="_blank">在线销售（淘宝店）</a> </li>
                <li>升级方法：将下述激活码发给客服人员，客服将反馈给您授权文件，将其放置在站点根目录即可。</li>
                <li>授权说明：主域名授权仅限<asp:Label ID="lbRootLimit" runat="server" ForeColor="green"></asp:Label>。</li>
            </ol>
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="120" class="right">
                        激活码类型：
                    </td>
                    <td>
                        <asp:RadioButtonList ID="rblActivType" runat="server" RepeatDirection="Horizontal"
                            RepeatLayout="Flow">
                            <asp:ListItem Value="1" Enabled="false">CPU串号</asp:ListItem>
                            <asp:ListItem Value="2" Enabled="false">硬盘串号</asp:ListItem>
                            <asp:ListItem Value="3">IP</asp:ListItem>
                            <asp:ListItem Value="4">域名</asp:ListItem>
                            <asp:ListItem Selected="True" Value="5">主域名</asp:ListItem>
                        </asp:RadioButtonList>
                        <span style="color:Red;" title="来自db.config配置">（当前设置的主域：<%= mainname%>）</span>
                    </td>
                </tr>
                <tr>
                    <td class="right">
                        激活码：
                    </td>
                    <td>
                        <asp:Label ID="lbActivCode" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <hr />
        <div>
            <div class="tit">
                以下为各版本功能对比，表格中数字为各版本最大承载数量：</div>
            <div class="verbox">
                <asp:GridView ID="gvLimit" runat="server" CssClass="gv">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <%# Container.DataItemIndex   + 1 %></ItemTemplate>
                            <ItemStyle Width="40" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
        <p>&nbsp;
            
        </p>
    </div>
    </form>
    <div id="footer">
        <div class="copyright">
            Copyright &copy; 2014-2020 <a href="<%= copyright["url"] %>" target="_blank"><%= copyright["en"]%></a>
            All rights reserved
        </div>
    </div>
</body>
<script type="text/javascript" src="/Utility/CoreScripts/jquery.js"></script>
<script type="text/javascript">
	//切换激活码
    $("input[id^=rblActivType]").click(function () {
		var type=$(this).val();
        $("span[id=lbActivCode]").html("<span style='color:red;'>正在加载，请稍候……</span>");
         $.post("license.aspx", { type: type, action: "code" }, function (requestdata) {
			  $("span[id=lbActivCode]").text(requestdata);
		 });
    });
	//刷新授权信息
	$("#refresh").click(function(){	
         $.post("license.aspx", { action: "refresh" }, function (requestdata) {
			  window.location.href="license.aspx";
		 });
	});
</script>
</html>
