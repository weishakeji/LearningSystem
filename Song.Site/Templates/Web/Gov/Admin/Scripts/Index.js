$(function(){
    setLoyout();
    loginEvent();
    courseOver();
    if($().getPara("loyout")=="reg")
        loginLoyoutSet($(".loginBar div:last"));
    else
        loginLoyoutSet($(".loginBar div:first"));
    //登录界面中的注册按钮事件
    $("a.aRegister").click(function(){
        loginLoyoutSet($(".loginBar div:last"));
    });
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
//当窗口大变化时
$(window).resize(function(){
    window.windowResizeTemp=window.windowResizeTemp==null ? 0 : window.windowResizeTemp++;
    if(window.windowResizeTemp%2==0)setLoyout();
});
function setLoyout(){
    //屏幕宽度
    var wd =document.documentElement.clientWidth;
    var trueWd=Number($("#showBox").attr("trueWidth"));
    $("#showBox").css({left:-(1920-wd)/2});
    $("#showBox").width(1920-(1920-wd)/2);
    setLoginBox("#loginBoxBack","center",171,201);
    setLoginBox("#loginBox","center",170,200);
}

//设置登录框
function setLoginBox(element,position,distance,top){
    if(typeof(element)=="string")element=$(element);
    if(element.size()<1)return;
    //初始化一些值
    position=position!=null && position!="" ? position : "left";
    distance=distance!=null ? distance : $(window).width()/2;
    top=top!=null ? top : 0;
    //计算浮动窗体在左侧位置（坐标）
    var left= 0;
    if(position=="left")left=distance;
    if(position=="right")left=$(window).width()-distance-element.width();
    if(position=="center")left=distance>0 ? $(window).width()/2+distance : $(window).width()/2+distance-element.width();
    //设置浮动窗的属性，主要是设置坐标
    element.css({position: "absolute",
        "z-index": 100,
        left: left,
        top: top});
    element.show();
}
