$(function () {
    $("#cour_add").click(function () {
        var href = $(this).attr("href");
        var width = Number($(this).attr("wd"));
        var height = Number($(this).attr("hg"));
        var box = new top.PageBox("学习卡的课程", href, width, height, null, window.name);
        box.CloseEvent = function () {
            $(".courses dd").remove();
            var courses = $.cookie("card_add");
            alert(courses[0].id);
        }
        box.Open();
        return false;
    });
});