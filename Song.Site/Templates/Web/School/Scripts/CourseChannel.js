$(function () {
	courseTabs(); 
});

//课程切换
function courseTabs() {
    $("div[type='tab']").each(function (index) {
        $(this).find("*[type='area']:first").show();
        $(this).find("*[type='title']:first").addClass("titleOver");
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
