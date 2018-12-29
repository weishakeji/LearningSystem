$(function () {
    //自动计算图片的高度，以方便显示圆形
    $(".thPhoto").each(function (index, element) {
        var wd = $(this).innerWidth();
        $(this).height(wd * 0.95);
    });
});