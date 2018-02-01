<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dbup_20170301.aspx.cs" Inherits="Song.Site.Check.Dbup_20170301" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <title>课程价格升级</title>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<script type="text/javascript" src="/Utility/CoreScripts/jquery.js"></script>
<style type="text/css">
#form1 {
	width: 640px;
}
.tit {
	font-size: 22px;
	margin-bottom: 10px;
	font-weight: bold;
	font-family: "黑体", "微软雅黑";
	color: #000;
}
*, dd, div {
	font-size: 14px;
	line-height: 22px;
}
#loading{
	color: #060;
}
</style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="tit"> 课程价格记录到课程表
      </div>
      <div> 说明：<br />
      课程价格是可以设置多条的，例如200年1个月，800元1年。<br />  
       这样在设计数据库时，课程的价格与课程分属两个表，如果要显示课程价格，要多一次操作数据库。<br />
        为了提高执行效率，当更改价格时，同步到课程表。<br />       
      <hr />
       <input type="button" name="btnStruct" value="开始升级" id="btnStruct" />
<div id="loading" style="display:none">正在升级中，请稍候……</div>
    </form>
       
<script type="text/javascript">
var size = 10; //每页取多少条
var index = 1; //索引页
var sumcount = 0; //总记录数
$("#btnStruct").click(function(){	
	$(this).hide();
	$("#loading").show();
	 request();
});
function request(){
	index = size * index < sumcount ? ++index : index;
	var url=window.location.href;	
	$.post(url, { size: size, index: index }, function (requestdata) {
        var data = eval("(" + requestdata + ")");
        sumcount = data.sumcount;
		if(size * index >= sumcount){
			alert("升级完成");
			$("#btnStruct").show();
			$("#loading").hide();
			return;
		}
		request();
	});
}
</script>
</body>
</html>
