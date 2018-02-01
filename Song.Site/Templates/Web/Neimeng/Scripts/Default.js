$(function(){
    loginEvent();
    loginLoyoutSet($(".loginBar div:first"));
    isLoginBox();
    teacherOver();
    courseOver();
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

//当课程信息，鼠标滑过时
function courseOver(){
    $(".courseList  .item").hover(function(){
        $(this).addClass("courseOver");
        $(this).find(".itemMark").animate({top:0,height:123});
        $(this).find(".itemInfo").animate({top:0,height:123});
    },function(){
        $(this).removeClass("courseOver");
        $(this).find(".itemMark").animate({top:83,height:40});
        $(this).find(".itemInfo").animate({top:123-40,height:40});
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