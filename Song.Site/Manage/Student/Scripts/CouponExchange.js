$(function () {
	//积分
    var pointsum=Number($("#point").text());
	$("#pointSum").text(pointsum);
	//比率
	var ratio=Number($("#ratio").text());
	ratio=ratio==null ? 0 : ratio;
	//计算兑换结果
	var result=Math.floor(pointsum/ratio);
	$("#result").text(result);
	$("input[name$=tbNumber]").attr("numlimit",result);
});
