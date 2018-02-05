$(function () {
    //全选反选取消的事件
    $(".selectBtn a").click(function () {
        var cl = $(this).attr("class");
        var checkbox = $("*[id$=cblOrg] input[type=checkbox]");
        checkbox.each(function (index, element) {
            if (cl == "all") $(this).attr("checked", true);
            if (cl == "invert") $(this).attr("checked", !$(this).attr("checked"));
            if (cl == "cancel") $(this).attr("checked", false);
        });
        return;
    });
});
