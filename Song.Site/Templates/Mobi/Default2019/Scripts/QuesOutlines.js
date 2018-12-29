$(function () {
    //按钮导航
    mui('body').on('tap', '.mm-item', function () {
        if ($(this).attr("href") != "")
            document.location.href = $(this).attr("href");
        return false;
    });
    //
    //计算章节序号（树形排序）
    $(".outline").each(function () {
        var xpath = $(this).attr("xpath");
        $(this).find("a").html("<span class='tax'>" + xpath + "</span>" + $(this).find("a").html());
    });
    //
    //计算总题数
    (function () {
        var sum = 0;
        $("li[pid=0]").each(function (index, element) {
            var num = parseInt($(this).attr("count"));
            if (!isNaN(num)) sum += num;
        });
        $(".sum").text(sum);
    })();
    //计算得分（正确率）
    clac();
    clac_succper();
    //章节列表的点击事件
    mui("li.outline").on('tap', 'a', function () {
		var type = $(this).attr("type");
        //如果是未完结的，链接不可用
        if (type == "nofinish") return false;
        //如果是未购买，则提示购买
        if (type == "buy") {
            var msg = new MsgBox("购买课程", "当前章节需要购买后学习，点击“确定”进入课程购买。", 90, 220, "confirm");
            msg.href = this.href;
            msg.EnterEvent = function () {
                window.location.href = msg.href;
                msg.Close(msg.WinId);
            }
            msg.Open();
            return false;
        }
        //读取记录
        var couid = $().getPara("couid");
        var olid = $(this).parent().attr("olid");
        var keyname = state.name.get("QuesExercises", couid, olid);
        var data = state.read(keyname);
        //如果没有storage记录，直接进入链接
        if (data.current == null) {
            window.location.href = this.href;
            return false;
        }
        //答题状态初始化
        var txt = "";
        txt += "点击“重新练习”，将清空本章节历史练习记录，重新计算正确率。";
        var msg = new MsgBox("是否继续上次练习？", txt, 90, 220, "confirm");
        msg.btn.enter = "继续练习";
        msg.btn.cancel = "重新练习";
        //msg.ShowCloseBtn = false;
        msg.href = this.href;
        msg.EnterEvent = function () {
            window.location.href = msg.href;
            msg.Close(msg.WinId);
        }
        msg.CancelEvent = function () {
            var msgclear = new MsgBox("是否确定？", "重新练习将清空本章节的学习记录，重新计算正确率。", 80, 160, "confirm");
            msgclear.btn.enter = "确定";
            msgclear.btn.cancel = "取消";
            msgclear.EnterEvent = function () {
                if (typeof state != "undefined") state.clear(keyname);
                window.location.href = msg.href;
                msgclear.Close(msgclear.WinId);
            }
            msgclear.Open();
        }
        msg.Open();
        return false;
    });
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
        var per = ret.sum > 0 ? Math.floor(ret.correct / ret.sum * 1000) / 10 : 0;
        per = per > 0 ? per : 0;
        $(this).find("b").attr("per", per).addClass("per" + Math.floor(per / 10));
        //$(this).find("b").addClass("per" + index);
        count += ret.ansnum;
    });
    //已经练习的题数
    $(".ansSum").text(count);
    //"继续练习"按钮的设置
    if (last != null) {
        $(".log").attr("href", function () {
            if (last == null) return "#";
            var href = $(this).attr("href");
            href = $().setPara(href, "couid", last.info.couid);
            href = $().setPara(href, "olid", last.info.olid);
            href = $().setPara(href, "qid", last.qid);
            var li = $("li[olid=" + last.info.olid + "]");
            href = $().setPara(href, "count", li.find("a").attr("count"));
            return href;
        }).show();
    }
}
//计算总的通过率
function clac_succper() {
    //var outline = { olid: 0, pid: 0, per: 0, level: 0, islast: true, childs: null };  

    var tree = build_tree($(".outline"), null, 1);
    //生成章节的树形结构
    function build_tree(olitem, parent, level) {
        var arr = new Array();
        var pid = 0;
        if (parent != null) pid = parent.olid;
        var items = olitem.filter("[pid=" + pid + "]");
        items.each(function (index) {
            var obj = { olid: Number($(this).attr("olid")), name: $(this).find("a").text(),
                pid: Number($(this).attr("pid")),
                per: Number($(this).find("b:first").attr("per")), islast: false,
                level: level, isclac: false, childs: null, parent: parent
            };
            var childs = olitem.filter("[pid=" + obj.olid + "]");
            obj.islast = childs.size() <= 0;
            if (childs.size() > 0) obj.childs = build_tree(olitem, obj, level + 1);
            $(".outline[olid=" + obj.olid + "]").attr({ "islast": obj.islast, "level": level });
            arr.push(obj);
        });
        return arr;
    }
    //取最大层深
    //var level = getMaxlevel();    
    //取最大层深
    function getMaxlevel() {
        var level = 0;
        $(".outline").each(function () {
            var lv = Number($(this).attr("level"));
            level = lv > level ? lv : level;
        });
        return level;
    }
    for (var i = getMaxlevel(); i > 0; i--) {
        $(".outline[level=" + i + "]").each(function () {
            var pid = Number($(this).attr("pid"));
            //父对象
            var parent = $(".outline[olid=" + pid + "]");
            if (parent.size() < 1) return true;
            if (parent.attr("isclac") == "true") return true;
            //同级同父对象
            var samelevel = $(".outline[pid=" + pid + "]");
            var per = clac_samelevel($(".outline[pid=" + pid + "]"));
            var parentPer = Number(parent.find("b:first").attr("per"));
            parentPer = parentPer < per ? per : parentPer;
            parent.find("b:first").attr("per", parentPer).attr("isclac", "true");
        });
    }
    //计算同级
    function clac_samelevel(rows) {
        if (rows.size() < 1) return 0;
        var sum = 0, count = 0;
        rows.each(function () {
            var per = Number($(this).find("b:first").attr("per"));
            var c = Number($(this).attr("count"));
            if (c > 0) count++;
            sum += per;
        });
        if (count < 1) return 0;
        return sum / count;
    }
    var average = clac_samelevel($(".outline[pid=0]"));
    $(".cou-rate").attr("rate", average).text(Math.floor(average * 10) / 10 + "%");
}
