$(function () {
    cour_tab();
    //课程点击事件
    mui('body').off('tap', '.cour-box').on('tap', '.cour-box', function () {
        var id = $(this).attr("couid");
        var url = "CoursePage.ashx?couid=" + id;
        window.location.href = url;
    });
});
function cour_tab() {
    //课程选项卡方式展示
    mui('body').on('tap', '.cour-bar .cour-tit', function () {
        var action = $(this).attr("action");
        var title = $(this).attr("title");
        //切换选项卡样式
        $(this).parent().find(".cour-tit").removeClass("current");
        $(this).addClass("current");
        $(".cour-context .cour-list").hide();
        $(".cour-context").hide();
        //如果没有登录
        var acid = $("body").attr("accid");
        if (acid == "" || Number(acid) < 1) {
            $(".cour-context").html("<a class='nocour-box' href='login.ashx' type='link'>未登录</a>");
        }
        //如果已经加载过，则不再加载
        var context = $(".cour-context[action=" + action + "]").show();
        if (context.find(".cour-box,.nocour-box").size() > 0) return;
        //添加预载
        context.html($(".cour-area>.loading")[0].outerHTML.replace("{0}", title));
        var loading = context.find(".loading").show();
        //异步加载课程
        cour_tab_post(action, "");
    });
    mui.trigger(document.querySelector('.cour-bar .cour-tit[action=trycou]'), 'tap');
    mui.trigger(document.querySelector('.cour-bar .cour-tit[action=overcou]'), 'tap');
    mui.trigger(document.querySelector('.cour-bar .cour-tit[action=buycou]'), 'tap');

    //mui.trigger(document.querySelector('.current'), 'tap');

}
//异步取课程
function cour_tab_post(action, sear) {
    $.post("/ajax/SelfCourse.ashx", { action: action }, function (data) {
        var d = eval("(" + data + ")");
        try {
            var context = $(".cour-context[action=" + d.action + "]")
            context.find(".loading").remove();
            var sum = d.sum;
            if (d.sum < 1) {
                $(".cour-context[action=" + action + "]").append("<span>没有任何信息</span>");
            } else {
                for (var i = 0; i < d.object.length; i++) {
                    var cour = d.object[i];
                    context.append(buildCourse(cour, action));
                    context.find("img:last").error(function () {
                        var errImg = $(this).attr("default");
                        if (errImg == null) return false;
                        $(this).attr("src", errImg);
                    });
                };
                //显示课程数量
                var tit = $(".cour-bar .cour-tit[action=" + d.action + "]");
                tit.html(tit.attr("title") + "(" + d.object.length + ")");
                //加载学习进度
                getStudyLog();
            }
        }
        catch (err) {
            $(".cour-context:visible").append("加载错误<br/>详情：" + err);
        }
    });
}
//构建课程信息
//cour:课程对象实体
//action:操作类型，如trycou为试用
function buildCourse(cour, action) {
    var defimg = $(".default-img").attr("default"); //默认图片
    var html = "<div class=\"cour-box\" couid=\"" + cour.Cou_ID + "\">";
    html += "<picture>{rec}{free}{limitfree}{complete}<img src='{logo}' default='{defimg}'/></picture><info>{name}{number}{sbjname}<price>{price}</price></info>";
    html = html.rep("{logo}", unescape(cour.Cou_LogoSmall));
    html = html.replace("{name}", "<name>" + unescape(cour.Cou_Name) + "</name>").replace("{sbjname}", "<sbjname>" + unescape(cour.Sbj_Name) + "</sbjname>");
    html = html.replace("{id}", cour.Cou_ID).replace("{defimg}", defimg);
    html = html.replace("{rec}", (cour.Cou_IsRec ? "<rec></rec>" : ""));
    html = html.replace("{free}", (cour.Cou_IsFree ? "<free></free>" : ""));
    html = html.replace("{complete}", "<complete></complete>");
    html = html.replace("{limitfree}", (cour.Cou_IsLimitFree ? "<limitfree></limitfree>" : ""));
    //浏览数、章节数、试题数
    var numstr = '<view>' + cour.Cou_ViewNum + '</view><outline>' + cour.olcount + '</outline><ques>' + cour.quscount + '</ques>';
    html = html.replace("{number}", "<number>" + numstr + "</number>");
    //价格
    var price = "";
    if (cour.Cou_IsFree) {
        price = "<f>免费</f>";
    } else {
        if (cour.Cou_IsLimitFree) {
            var end = cour.Cou_FreeEnd.Format("yyyy年M月d日");
            price = "<l>免费至 <t>" + end + "</t></l>";
        } else {
            if (action == "trycou") {
                price = "<m>试学中</m>";
            } else {
                price = "<m>" + cour.endtime.Format("yyyy年M月d日") + "过期</m>";
            }
        }
    }
    html = html.replace("{price}", price);
    html += "</div>";
    return html;
}
//加载学习进度
function getStudyLog() {
    if (!window.SelfStudyLog && !window.SelfStudyLogLoad) {
        var acid = $("body").attr("accid");
        window.SelfStudyLogLoad = true;
        $.get("/ajax/SelfStudyLog.ashx?id=" + acid, { id: acid }, function (data) {
            if (data == "") return;
            window.SelfStudyLog = eval("(" + data + ")");
            getStudyLog();
        });
    } else {
        var obj = window.SelfStudyLog;
        for (var i in obj) {
            var cou = obj[i];
            var box = $(".cour-box[couid=" + cou.Cou_ID + "]");
            if (Number(cou.complete) > 0) {
				var complete=Math.floor(Number(cou.complete)*100)/100;
                box.find("complete").attr("class", "complete").text("完成学习 " + complete + "%");
            }
        }
    }
}