$(function(){
	//单选框只能选中一个
	$("input[name$=cbAns]").click(function(){		
		var txt=$(this).parents("tr").find("input[id$=itemTxt]"); 
		if($.trim(txt.val())=="")
		{
			alert("请勿选择空白选项作为答案项目。");
			return false;
		}
		$("input[name$=cbAns]").removeAttr("checked");
		$(this).prop("checked","checked");
	});
	//提交按钮
	$("input[id$=btnEnter]").click(function(){
		//验证题干是否录入
		if(Number($(".count").text())<1)
		{
			alert("题干不得为空！");
			return false;
		}
		//是否设置了正确答案
		if(Number($(".anscount").text())<1)
		{
			alert("请填写答案！");
			return false;
		}
		return true;
	});
    _setEvent();
});
//设置一些事件
function _setEvent() {
    $(".wrongInfo").hover(function () {
        var box = $("#wrongInfoBox");
        var off = $(this).offset();
        box.css("position", "absolute").css("z-index", "20001");
        box.css({ width: 150, height: 200 });
        box.css({ left: off.left, top: off.top + $(this).height() });
        box.css("background-color", "#FFFFCC");
        box.css("padding", "5px");
        box.show();
    }, function () {
        $("#wrongInfoBox").hide();
    });
}

