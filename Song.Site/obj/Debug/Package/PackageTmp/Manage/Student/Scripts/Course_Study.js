$(function(){
    loyoutInit();
    BaseEvent();
});
//基础布局
function loyoutInit() {
    var hg=document.documentElement.clientHeight;
    hg=hg-$(".pageWinBtn").height()-20;
    $("#outlineLeft").height(hg);
    $("#outlineRigt").height(hg);
    $("#outlineFrame").height(hg);
}
//基本操作事件
function BaseEvent(){
    //左侧大纲的事件
    $("#outlineLeft .leftItem").click(function(){
        var target=$(this).find("span").attr("target");
        var href=$(this).find("span").attr("href");
        var frame=$("#"+target)
        frame.attr("src",href);
        //样式
        $("#outlineLeft .leftItem").removeClass("current");
        $(this).parent().addClass("current");
        return false;
    });
    $("#outlineLeft .leftItem").hover(function(){
        $(this).addClass("over");
    },function(){
        $(this).removeClass("over");
    });

}