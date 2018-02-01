$(function () {
    //检索我的课程
    $("#search-myExam").blur(function () {
        var str = $.trim($(this).val());
        if (str.length < 1) {
            $("#myExam .examItemBox").show();
            return;
        }
        //检索
        $("#myExam .examItemBox").each(function (index, element) {
            var examname = $(this).find(".itemName span").text();
            if (examname.indexOf(str) < 0) {
                $(this).hide();
            } else {
                $(this).show();
            }
        });
    });
    //当在课程检索输入框中敲回车时
    $("#search-myExam").keydown(function (event) {
        switch (event.keyCode) {
            case 13:
                $(this).blur();
                break;
        }
    });
});

