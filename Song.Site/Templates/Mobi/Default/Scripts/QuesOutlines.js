$(function () {
    //获取历史记录的章节的题数
    var loglink = $("a.log");
    var li = $("li[olid=" + loglink.attr("olid") + "]");
    var count = li.find("a").attr("count");
    loglink.attr("href", $().setPara(loglink.attr("href"), "count", count));

    //按钮导航
    mui('body').on('tap', '.mm-item', function () {
        if ($(this).attr("href") != "")
            document.location.href = $(this).attr("href");
        return false;
    });

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
        $(".sum").text(sum);
    })();
    //
    clac();
});

//计算练习的题数
function clac() {
    var couid = $().getPara("couid");
    //记录总共练习多少道
    var count = 0;
    $(".outline").each(function (index) {
        var olid = $(this).attr("olid");
        var keyname = state.name.get("QuesExercises", couid, olid);
        var data = state.read(keyname);
        //计算答题信息
        var ret = state.clac(data.items);
        var sum = Number($(this).find(".num").text());
        $(this).find(".ansnum").text(ret.ansnum > sum ? sum : ret.ansnum);   //已经做了多少题
        //正确率（除以整体数量）
        var per = ret.sum>0 ? Math.floor(ret.correct / ret.sum * 10) : 0;
        $(this).find("b").attr("per",per).addClass("per" + (per>0 ? per : 0));
        //$(this).find("b").addClass("per" + index);
        count += ret.ansnum;
    });
    $(".ansSum").text(count);
}
