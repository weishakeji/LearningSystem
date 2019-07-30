$(function () {
    selectedInit();
    subjectEvent();
    //搜索以及分页导航
    var search = $().getPara("search");
    $("input[name=search]").val(search);
    var paras = $().getPara();
    if (paras != null && paras.length > 0) {
        $("#pagerBar a").each(function (index, element) {
            var href = $(this).attr("href");
            for (var i = 0; i < paras.length; i++) {
                if (paras[i].key == "index") continue;
                href = $().setPara(href, paras[i].key, paras[i].value);
            }
            $(this).attr("href", href);
        });
    }
});


//各种选择按钮的事件
function subjectEvent() {
    $(".selectItem a").click(function () {
        var attr = $(this).attr("attr");
        var val = $(this).attr(attr);
        //要清除的参数
        var clear = $(this).attr("clear");
        var url = buildUrl(attr, val, clear);
        window.document.location.href = url;
        return false;
    });
}
//初始化
function selectedInit() {
    var strs = $().getPara();
    if (strs.length < 1) {
        $("#SelectedBar").hide();
    } else {
        //参数数组      
        $("#Selected").html("");
        for (var i = 0; i < strs.length; i++) {
            if (strs[i].key == "search") {
                var tm = "<div class=\"selectedItem\" clear=\"search\">";
                tm += "<div class='name'>查询-" + strs[i].value + "</div><div class='close' attr='" + strs[i].key + "'>&nbsp;</div>";
                tm += "</div> ";
                $("#Selected").append(tm);
                break;
            }
            var item = $(".selectItem a[" + strs[i].key + "=" + strs[i].value + "]");
            if (item.size() < 1) continue;
            //主题，如专业、类型的文字信息
            var theme = item.parent().parent().prev().text();
            var txt = item.text();
            var clear = item.attr("clear");
            var tm = "<div class=\"selectedItem\" clear=\"" + clear + "\">";
            tm += "<div class='name'>" + theme + txt + "</div><div class='close' attr='" + strs[i].key + "'>&nbsp;</div>";
            tm += "</div> ";
            $("#Selected").append(tm);
        }
        //选中的专业在鼠标经过时
        $("#Selected .selectedItem").hover(function () {
            $(this).addClass("selectedItemOver");
            $(this).find(".close").addClass("closeOver");
        }, function () {
            $(this).removeClass("selectedItemOver");
            $(this).find(".close").removeClass("closeOver");
        });
        $("#Selected .close").click(function () {
            var clear = $(this).parent().attr("clear");
            var url = buildUrl($(this).attr("attr"), null, clear);
            window.document.location.href = url;
        });
    }
    //如果没有任何选择项，则不显示已选择的区域
    if ($("#Selected div").size() < 1) {
        $("#SelectedBar").hide();
    }
}
//生成地址
function buildUrl(attr, val, clear) {
    //先去除指定属性
    var strs = $().getPara();
    var clr = clear == null ? new Array() : clear.split(",");
    for (var i = 0; i < strs.length; i++) {
        for (var j = 0; j < clr.length; j++) {
            if (strs[i].key == clr[j]) {
                strs.splice(i, 1);
                break;
            }
        }
    }
    //重新生成Url
    url = String(window.document.location.href);
    var baseUrl = url.indexOf("?") > -1 ? url.substring(0, url.lastIndexOf("?")) : url;
    if (strs.length > 0) {
        baseUrl += "?";
        for (var i = 0; i < strs.length; i++) {
            if (i != 0) baseUrl += "&";
            baseUrl += strs[i].key + "=" + strs[i].value;
        }
    }
    if (val != null)
        baseUrl = $().setPara(baseUrl, attr, val);
    return baseUrl;
}
