$(function () {
	loginLoyoutSet();
    //登录界面中的注册按钮事件
    $("#findPw").click(function () {
        new MsgBox("找回密码", "请联系当前系统的管理员（即超级管理员）。<br/>由超级管理员进行密码重置。", 400, 250, "alert").Open();
		return false;
    });
	(function(){
		var error=Number($().getPara("error"));	//错误代码
		error=isNaN(error) ? 0 : error;
		var acc=$().getPara("acc");	//账号
		if(error==1)Verify.ShowBox($("input[type=text][name=tbAcc]"),"登录账号不得为空");
		if(error==2)Verify.ShowBox($("input[type=text][name=tbCode]"),"验证码不正确");
		if(error==3)Verify.ShowBox($("input[type=password][name=tbPw]"),"密码不正确");
		if(acc!="")$("input[type=text][name=tbAcc]").val(acc);
	})();
});
//设置登录框的布局
function loginLoyoutSet() {
    //标题的标示与隐藏
    var offset=$("#bodyTop").offset();
	$(".fixedBox").css("top",offset.top+50);
	$(".fixed-box").width($("#Context").width());
}

//当窗口大变化时
$(window).resize(function () {
    window.windowResizeTemp = window.windowResizeTemp == null ? 0 : window.windowResizeTemp++;
    //if (window.windowResizeTemp % 2 == 0) setLoyout();
});

/*
//设置登录框
function setLoginBox(element, position, distance, top) {
    if (typeof (element) == "string") element = $(element);
    if (element.size() < 1) return;
    //初始化一些值
    position = position != null && position != "" ? position : "left";
    distance = distance != null ? distance : $(window).width() / 2;
    top = top != null ? top : 0;
    //计算浮动窗体在左侧位置（坐标）
    var left = 0;
    if (position == "left") left = distance;
    if (position == "right") left = $(window).width() - distance - element.width();
    if (position == "center") left = distance > 0 ? $(window).width() / 2 + distance : $(window).width() / 2 + distance - element.width();
    //设置浮动窗的属性，主要是设置坐标
    element.css({ position: "absolute",
        "z-index": 100,
        left: left,
        top: top
    });
    element.show();
}
*/