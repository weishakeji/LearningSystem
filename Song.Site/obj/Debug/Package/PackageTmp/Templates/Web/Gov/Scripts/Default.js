$(function () {
    loginEvent();
    loginLoyoutSet($(".loginBar div:first"));
    isLoginBox();
    teacherOver();
    courseTabs();
});

//登录框的设置
function loginEvent() {
    $(".loginBar div").click(function(){
        loginLoyoutSet($(this));
    });
}
//设置登录框的布局
function loginLoyoutSet(obj){
    //标题的标示与隐藏
    $(".loginBar div").attr("class","loginTitle");
    obj.attr("class","loginTitleCurr");
    //登录区域的显示与隐藏
    $(".loginArea").hide();
    var tag=obj.attr("tag");
    var curr=$(".loginArea[tag="+tag+"]");
    curr.show();
    curr.find("input:first").focus();
}
//当前登录的对象，优先显示出来
function isLoginBox(){
    $(".isLoginBox:first").each(function(){
        var tag=$(this).parent().attr("tag");
        //登录区的显示
        $(".loginArea").hide();
        $(".loginArea[tag="+tag+"]").show();
        //登录框标题样式
        $(".loginBar div").attr("class","loginTitle");
        $(".loginBar div[tag="+tag+"]").attr("class","loginTitleCurr");
    });
}

//当月教师信息，鼠标滑过时
function teacherOver(){
    $(".itemTeacher").hover(function(){
        $(this).find(".mark").fadeIn(100);
        $(this).find(".thInfo").slideDown(100);
    },function(){
        $(this).find(".mark").fadeOut(100);
        $(this).find(".thInfo").slideUp(100);
    });
	$(".itemTeacher").click(function(){
        var href=$(this).attr("href");
        window.document.location.href=href;
    });
}
//课程切换
function courseTabs() {
    $("div[type='tab']").each(function (index) {
        $(this).find("*[type='area'][index=1]").show();
        $(this).find("*[type='title'][index=1]").addClass("titleOver");
        //选项卡
        $(this).find("*[type='title']").hover(function () {
            //隐藏所有内容区
            $(this).parents("*[type='tab']").find("*[type='area']").hide();
            $(this).parents("*[type='tab']").find("*[type='title']").removeClass("titleOver");
            //设置当前效果
            var index = $(this).attr("index");
            $(this).parents("*[type='tab']").find("*[type='area'][index=" + index + "]").show();
            $(this).addClass("titleOver");
        });
    });
}