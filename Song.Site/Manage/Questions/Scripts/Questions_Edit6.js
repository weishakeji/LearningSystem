$(function () {
    _inputVerifyEvent();
    _setEvent();
    $("div[type=addbar] a").click(function () {
        var values = $("form").serialize();
        alert(values);
        $.get(window.location.href, function () {
        });
        var type = $(this).attr("href");
        var name = $(this).text();
        var win = new window.top.PageBox("公共题干下级试题：" + name, "/manage/Questions/Questions_Edit" + type + ".aspx", 1000, 600, null, window.name);
        /*win.CloseEvent = function (box, pbox) {
        var iframe = pbox.find("iframe");
        if (iframe.size() > 0) {
        iframe.attr("src", iframe.get(0).contentWindow.location.href);
        } 
        }*/
        win.Open();
        return false;
    });
});
//输入验证
function _inputVerifyEvent(){
    //提交按钮
    $("input[id$=btnEnter]").click(function(){
        //验证题干是否录入
        if(Number($(".count").text())<1){
            alert("题干不得为空！");
            return false;
        }
    });
}


//设置一些事件
function _setEvent(){
	$(".wrongInfo").hover(function(){
		var box=$("#wrongInfoBox");
		var off=$(this).offset();
		box.css("position","absolute").css("z-index","20001");
		box.css({width:150,height:200});
		box.css({left:off.left,top:off.top+$(this).height()});
		box.css("background-color","#FFFFCC");
		box.css("padding","5px");
		box.show();
	},function(){
		$("#wrongInfoBox").hide();
	});
}
