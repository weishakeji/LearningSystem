$(function () {
    //隐藏一些列;
    _hideColumn("ID");
    _hideColumn("姓名");
    _hideColumn("身份证");
    $(".GridView").find("td").addClass("center");
    $(".examAvg").text(_clacAvg());
    //当点击成绩时，可以查看回顾
    $(".GridView").find("td:contains('$')").each(function () {
        var val = $(this).text();
        //取成绩与id
        if (val.indexOf("$") > -1) {
            var score = val.substring(0, val.lastIndexOf("$"));
            var exrid = val.substring(val.lastIndexOf("$") + 1);
            $(this).html(score).attr({ "exrid": exrid, "title": "点击查看成绩详情" }).css("cursor", "pointer");
            $(this).click(function () {
                var exrid = $(this).attr("exrid");
                var href = "/ExamReview.ashx?id=" + exrid;
                var title = "考试成绩查看";
                new top.PageBox(title, href, 80, 80, null, window.name).Open();
                return false;
            });
        }
    });
});
//计算平均分
function _clacAvg() {
    var tm = 0;
    var span = $(".examItem span");
    span.each(function () {
        var num = Number($(this).text());
        tm += num;
    });
    var avg = tm / span.size();
    return Math.round(avg * 100) / 100;
}

//隐藏某一行
function _hideColumn(clName) {
    var gv = $(".GridView");
    var index;
    gv.find("th").each(function (i) {
        if ($.trim($(this).text()) == clName) {
            index = i;
        }
    });
    gv.find("tr").each(function () {
        $(this).children().each(function (i) {
            if (i == index) {
                $(this).hide();
                return false;
            }
        })
    })
}