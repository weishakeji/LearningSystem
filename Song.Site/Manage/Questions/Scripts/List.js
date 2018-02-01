$(function(){
	//设置题干的长度，不让其换行
	$(".QusTitle").each(function(){		
		$(this).width($(this).parent().width()*0.96);
	});
	//导入
	$("input[id$=btnInput]").click(function(){
			var LocString=String(window.document.location.href);
			if(LocString.indexOf("?")>-1)
			{
				var para=LocString.substring(LocString.lastIndexOf("?"));
				OpenWin('Questions_Input.aspx'+para,'数据导入',800,600);
			}else
			{
				OpenWin('Questions_Input.aspx','数据导入',800,600);
			}
			return false;
});
//导出
$("input[id$=btnOutput]").click(function () {
    var LocString = String(window.document.location.href);
    if (LocString.indexOf("?") > -1) {
        var para = LocString.substring(LocString.lastIndexOf("?"));
        OpenWin('Export.aspx' + para, '试题导出', 800, 600);
    } else {
        OpenWin('Export.aspx', '试题导出', 800, 600);
    }
    return false;
});
    //批量使用
    $("a[id$=lbUse]").click(function(){
        var keys=GetKeyValues(GridViewId);
        if(keys==""){
            alert("请选择要操作的行");
            return false;
        }
    });
    //批量禁用
    $("a[id$=lbNoUse]").click(function(){
        var keys=GetKeyValues(GridViewId);
        if(keys==""){
            alert("请选择要操作的行");
            return false;
        }
    });
});

