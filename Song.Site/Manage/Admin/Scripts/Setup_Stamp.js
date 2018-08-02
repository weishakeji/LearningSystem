$(function () {
    //默认方位
    var pos = $("input[type=text][name$=tbPosition]").val();
    $(".position td").each(function () {
        var tag = $(this).attr("tag");
        if (pos == tag) {
            $(this).addClass("curr");
        }
    });
    //点击事件
    $(".position td").click(function () {
        $(this).parents("table").find("td").removeClass("curr");
        $(this).addClass("curr");
        var tag = $(this).attr("tag");
        $("input[type=text][name$=tbPosition]").val(tag);
    });
});