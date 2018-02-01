$(function(){
    setLoyout();
});
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
    setTimeout("setNum()",1000);
    setLoginBox("#loginBoxBack","center",171,231);
    setLoginBox("#loginBox","center",170,230);
}
function setNum(){
    var div=$(".num");
    var wd =document.documentElement.clientWidth;
    div.width(wd-(1920-wd)/2);
    //alert(div.html());
    //div.css({left:0});
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
