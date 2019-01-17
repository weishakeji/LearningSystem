$(function () {
    //参加考试
    $(".btnStart").click(function () {
        var examid = $(this).attr("examid");
        document.location.href = $().setPara("examing.ashx", "id", examid);
    });
    //查看成绩
    mui('body').on('tap', '.btnRow a', function (event) {
        var href = $(this).attr("href");
        var name = $(this).attr("title");
        new PageBox("成绩：《" + name + "》", href, 100, 100, window.name, "url").Open();
    });
});