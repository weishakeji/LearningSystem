<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NativePayPage.aspx.cs" Inherits="WxPayAPI.NativePayPage" %>

<!DOCTYPE html>


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="content-type" content="text/html;image/gif;charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1" /> 
    <script type="text/javascript" src="/Utility/CoreScripts/jquery.js"></script>

    <title>微信扫码支付</title>
    <style type="text/css">
	body{font-family:"Microsoft YaHei"; } 
.accinfo {
	margin-right: auto;
	margin-left: auto;
	margin-top: 30px;
}
body {
	text-align: center;
}
.show-tit {
	font-size: 14px;
	line-height: 40px;
	height: 40px;
	text-align: center;
	margin-top: 20px;
	color:#666;
}
.total_fee {
	font-size: 14px;
	line-height: 40px;
	height: 40px;
	color:#f00;
	text-align: center;
}
.total_fee span
{
    font-size: 16px;
    font-weight: bold;
}
.img-line img {
	border-radius: 110px;
	height: 106px;
	width: 106px;
	border: 1px solid #eee;
	margin-left: auto;
	margin-right: auto;
	box-shadow: 0 -2px 2px #ccc, 0 6px 1px rgba(0, 0, 0, 0.3);
	border-radius: 106px;
	margin: 3px;
}
.success {
	font-size: 16px;
	line-height: 50px;
	height: 50px;
	color:#06f;
	text-align: center;
	display:no7ne;
	width:200px;
	margin-right:auto;
	margin-left:auto;
}
.success a {
	color: #333;
	text-decoration: none;
	line-height: 50px;
	height: 60px;
	font-size: 18px;
}
.success b{
	background-image: url(../../Manage/Images/ICO/bigIco1.jpg);
	display: inline-block;
	height: 50px;
	width: 50px;
	float:left;
	background-position: -10px -95px;
}
    </style>
</head>
<body>
	 <div class="accinfo">
    <div class="show-tit" id="name"> <%= acc.Ac_Name %> </div>
    <div class="img-line"> <img accphoto="<%= accphoto %>" src="/manage/images/noPhoto.jpg" id="photo" /></div>
    <div class="show-tit"> 请用微信扫描下面二维码</div>
    <div class="total_fee"> 支付金额：<span><%= (Convert.ToDouble(total_fee)/100).ToString("0.0") %></span>元</div>
  </div>
	<asp:Image ID="Image2" runat="server" style="width:200px;height:200px;"/>
	<div class="success"><b></b> <a href="#" id="close">支付完成，请关闭</a></div>
</body>
<script type="text/javascript">
(function(){
	//设置学员头像
	$("#photo").each(function() {
        var accphoto=$(this).attr("accphoto");
		if(accphoto!=""){
			if(accphoto.substring(accphoto.length-1)!="/")
			$(this).attr("src",accphoto);
		}
    });
	$("a#close").click(function(){
		window.opener=null;
		window.open('','_self');
		window.close();
		return false;
	});
})();

//实时监听充值完成
var interval = window.setInterval("getPaystate()",1000);
function getPaystate(){
	$.post(window.location.href, function(data){
		if(data=="1"){
			window.clearInterval(interval);
			$(".success").fadeIn(1000);
		}
	});	
}
</script>
</html>
