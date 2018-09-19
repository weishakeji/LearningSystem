$(function () {
    //获取历史记录的章节的题数
    var loglink = $("a.log");
    var li = $("li[olid=" + loglink.attr("olid") + "]");
    var count = li.find("a").attr("count");
    loglink.attr("href", $().setPara(loglink.attr("href"), "count", count));


    //计算章节序号（树形排序
    (function claxtax(pid, prefix) {
        $(".outline[pid=" + pid + "]").each(function (index, element) {
            var tax = $(this).find(".tax");
            tax.html(prefix + (index + 1) + ".");
            var olid = $(this).attr("olid");
            claxtax(olid, tax.text());
        });
    })(0, "");
    //计算总题数
    (function () {
        var sum = 0;
        $("li[pid=0]").each(function (index, element) {
            sum += Number($(this).find(".count .num").text());
        });
        $(".sum").text(sum + "道");
    })();
    //
    clac();
});

//计算练习的题数
function clac() {
    var couid = $().getPara("couid");
    $(".outline").each(function (index) {
        var olid = $(this).attr("olid");
        var keyname = state.name.get("QuesExercises", couid, olid);
        var data = state.read(keyname);
        //计算答题信息
        var ret = state.clac(data.items);
        var sum = Number($(this).find(".num").text());
        $(this).find(".ansnum").text(ret.ansnum > sum ? sum : ret.ansnum);   //已经做了多少题
        //正确率（除以整体数量）
        var per = Math.floor(ret.correct / ret.sum * 10);
        $(this).find("b").addClass("per" + per);
        //$(this).find("b").addClass("per" + index);
    });
    //alert(couid);
}
