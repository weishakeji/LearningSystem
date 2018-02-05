$(function () {
    loginEvent();
    courseOver();
    if ($().getPara("loyout") == "reg")
        loginLoyoutSet($(".loginBar div:last"));
    else
        loginLoyoutSet($(".loginBar div:first"));
});
//登录框的设置
function loginEvent() {
    $(".loginBar div").click(function(){
       var href = $(this).find("a").attr("href");
        window.location.href = href;
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
//当课程信息，鼠标滑过时
function courseOver(){
    $(".courseList  .item").hover(function(){
        $(this).addClass("courseOver");
        $(this).find(".itemMark").animate({top:-123});
        $(this).find(".itemInfo").animate({top:-123*2});
    },function(){
        $(this).removeClass("courseOver");
        $(this).find(".itemMark").animate({top:-45});
        $(this).find(".itemInfo").animate({top:-123-45});
    });
}
