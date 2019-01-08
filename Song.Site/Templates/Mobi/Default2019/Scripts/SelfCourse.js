$(function () {
    cour_tab();
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
        var context = $(".cour-context[action=" + action + "]").show();
        //添加预载
        context.html($(".cour-area>.loading")[0].outerHTML.replace("{0}", title));
        var loading = context.find(".loading").show();
        //异步加载课程
        cour_tab_post(action, "");
    });
    mui.trigger(document.querySelector('.current'), 'tap');

}
//异步取课程
function cour_tab_post(action, sear) {
    $.post("/ajax/SelfCourse.ashx", { action: action }, function (data) {
        var d = eval("(" + data + ")");
        try {
			var context = $(".cour-context[action=" + d.action + "]")
			context.find(".loading").remove();
            var sum = d.sum;
			if(d.sum<1){
				 $(".cour-context:visible").append("<span>没有任何信息</span>");
			}else{
				for (var i = 0; i < d.object.length; i++) {
					var cour = d.object[i];
					context.append(buildCourse(cour));
					context.find("img:last").error(function () {
                        var errImg = $(this).attr("default");
                        if (errImg == null) return false;
                        $(this).attr("src", errImg);
                    });
				};
			}
        }
        catch (err) {
            $(".cour-context:visible").append("加载错误<br/>详情：" + err);
        }
    });
}
//构建课程信息
function buildCourse(cour) {
    var defimg = $(".default-img").attr("default"); //默认图片
	var html = "<div class=\"cour-box\">";
    html += "<picture>{rec}{free}{limitfree}<img src='{logo}' default='{defimg}'/></picture><info>{name}{number}{sbjname}<price>{price}</price></info>";
    html = html.rep("{logo}", cour.Cou_LogoSmall);
    html = html.replace("{name}", "<name>" + cour.Cou_Name + "</name>").replace("{sbjname}", "<sbjname>" + cour.Sbj_Name + "</sbjname>");
    html = html.replace("{id}", cour.Cou_ID).replace("{defimg}", defimg);
    html = html.replace("{rec}", (cour.Cou_IsRec ? "<rec></rec>" : ""));
    html = html.replace("{free}", (cour.Cou_IsFree ? "<free></free>" : ""));
    html = html.replace("{limitfree}", (cour.Cou_IsLimitFree ? "<limitfree></limitfree>" : ""));
	//浏览数、章节数、试题数
	var numstr='<view>' + cour.Cou_ViewNum + '</view><outline>' + cour.olcount + '</outline><ques>' + cour.quscount + '</ques>';
	html = html.replace("{number}", "<number>"+numstr+"</number>");
    //价格
    var price = "";
    if (cour.Cou_IsFree) {
        price = "<f>免费</f>";
    } else {
        if (cour.Cou_IsLimitFree) {
            var end = cour.Cou_FreeEnd.Format("yyyy年M月d日");
            price = "<l>免费至 <t>" + end + "</t></l>";
        } else {
            price = "<m>" + cour.endtime.Format("yyyy年M月d日") + "过期</m>";
        }
    }
    html = html.replace("{price}", price);
	html+="</div>";
    return html;
}