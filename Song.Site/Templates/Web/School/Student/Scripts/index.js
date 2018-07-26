$(function () {
    courseOver(); //课程列表中的事件
});

//当课程信息，鼠标滑过时
function courseOver() {
    $(".courseList .item").hover(function () {
        $(this).addClass("courseOver");
        $(this).find(".itemMark").animate({ top: -142 });
        $(this).find(".itemInfo").animate({ top: -135 * 2 });
    }, function () {
        $(this).removeClass("courseOver");
        $(this).find(".itemMark").animate({ top: -45 });
        $(this).find(".itemInfo").animate({ top: -135 - 45 });
    });
}
