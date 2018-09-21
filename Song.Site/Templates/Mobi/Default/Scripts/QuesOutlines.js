$(function () {
    //按钮导航
    mui('body').on('tap', '.mm-item', function () {
        if ($(this).attr("href") != "")
            document.location.href = $(this).attr("href");
        return false;
    });
    //
    //计算章节序号（树形排序）
    (function claxtax(pid, prefix) {
        $(".outline[pid=" + pid + "]").each(function (index, element) {
            var tax = $(this).find(".tax");
            tax.html(prefix + (index + 1) + ".");
            var olid = $(this).attr("olid");
            claxtax(olid, tax.text());
        });
    })(0, "");
    //
    //计算总题数
    (function () {
        var sum = 0;
        $("li[pid=0]").each(function (index, element) {
            sum += Number($(this).find("a").attr("count"));
        });
        $(".sum").text(sum);
    })();
    //
    clac();
	clac_succper();
});

//计算练习的题数
function clac() {
    var couid = $().getPara("couid");
    //记录总共练习多少道
    var count = 0;
    var last = null; //最后一次练习的记录
    $(".outline").each(function (index) {
        var olid = $(this).attr("olid");
        var keyname = state.name.get("QuesExercises", couid, olid);
        var data = state.read(keyname);
        if (data.current != null) {
            if (last == null) last = data.current;
            if (last.last < data.current.last) last = data.current;
        }
        //计算答题信息
        var ret = state.clac(data.items);
        var sum = Number($(this).find(".num").text());
        $(this).find(".ansnum").text(ret.ansnum > sum ? sum : ret.ansnum);   //已经做了多少题
        //正确率（除以整体数量）
        var per = ret.sum > 0 ? Math.floor(ret.correct / ret.sum * 10) : 0;
        $(this).find("b").attr("per", per).addClass("per" + (per > 0 ? per : 0));
        //$(this).find("b").addClass("per" + index);
        count += ret.ansnum;
    });
    //已经练习的题数
    $(".ansSum").text(count);
    //"继续练习"按钮的设置
    $(".log").attr("href", function () {
        if (last == null) return "#";
        var href = $(this).attr("href");
        href = $().setPara(href, "couid", last.info.couid);
        href = $().setPara(href, "olid", last.info.olid);
        href = $().setPara(href, "qid", last.qid);
        var li = $("li[olid=" + last.info.olid + "]");
        href = $().setPara(href, "count", li.find("a").attr("count"));
        return href;
    });
}

function clac_succper(){
}
