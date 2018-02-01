<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dbup_20170323.aspx.cs" Inherits="Song.Site.Check.Dbup_20170323" %>

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
    <div class="tit"> 修正了试卷题量为零的问题
      </div>
      <div> 说明：<br />
      试卷新增后没有即计算试题量，导致在显示试卷时，显示试题为“0道”。<br />  
           
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
