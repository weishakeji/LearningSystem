$(function () {

    //取消课程学习的按钮事件
    $(".item a.selected").click(function () {

        if (confirm("是否确定终止该课程的学习？")) {
            return true;
        }
        return false;
    });
    //初始样式
    $(".item").each(function () {
        var isFree = $(this).attr("isfree") == "True" ? true : false;
        var isTry = $(this).attr("istry") == "True" ? true : false;
        if (isFree) $(this).addClass("isFree");
        if (isTry) $(this).addClass("isTry");
    });

});