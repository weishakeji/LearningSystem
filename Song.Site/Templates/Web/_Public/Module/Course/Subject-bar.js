$(function () {
    selectedInit();
    subjectEvent();
	//搜索以及分页导航
	$("input[name=search]").val(decodeURI($().getPara("search")));
	var paras=$().getPara();
	if(paras!=null && paras.length>0){
		$("#pagerBar a").each(function(index, element) {
			var href=$(this).attr("href");
            for(var i=0;i<paras.length;i++){
				if(paras[i].key=="index")continue;
				href=$().setPara(href,paras[i].key,paras[i].value);
			}
			$(this).attr("href",href);
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
    var strs = GetRequest();
    if (strs.length < 1) {
        $("#SelectedBar").hide();
    } else {
        //参数数组
        var strs = GetRequest();
        $("#Selected").html("");
        for (var i = 0; i < strs.length; i++) {
			if(strs[i][0]=="search"){
				 var tm = "<div class=\"selectedItem\" clear=\"" + clear + "\">";
				tm += "<div class='name'>查询-" +  unescape(strs[i][1]) + "</div><div class='close' attr='" + strs[i][0] + "'>&nbsp;</div>";
				tm += "</div> ";
				$("#Selected").append(tm);
				break;
			}
            var item = $(".selectItem a[" + strs[i][0] + "=" + strs[i][1] + "]");
            if (item.size() < 1) continue;
            //主题，如专业、类型的文字信息
            var theme = item.parent().parent().prev().text();
            var txt = item.text();
            var clear = item.attr("clear");
            var tm = "<div class=\"selectedItem\" clear=\"" + clear + "\">";
            tm += "<div class='name'>" + theme + txt + "</div><div class='close' attr='" + strs[i][0] + "'>&nbsp;</div>";
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
	if($("#Selected div").size()<1){
		$("#SelectedBar").hide();
	}
}
//生成地址
function buildUrl(attr, val, clear) {
    //当前页面的上级路径，因为子页面没有写路径，默认与本页面同路径
    var url = String(window.document.location.href);
    url = url.replace(/#/ig, "");
    if (url.indexOf("?") > -1) url = url.substring(0, url.lastIndexOf("?"));
    //参数数组
    var strs = GetRequest();
    var para = new Array();
    var isHave = false;
    for (var i = 0; i < strs.length; i++) {
        if (strs[i][0] == attr) {
            strs[i][1] = val;
            isHave = true;
        }
        if (!(strs[i][0] == attr && val == null)) {
            para.push(strs[i]);
        }
    }
    if (!isHave && val != null) para.push(new Array(attr, val));
    if (para.length < 1) return url;
    url += "?";
    //要清除的数据
    var clr = clear == null ? new Array() : clear.split(",");
    for (var i = 0; i < para.length; i++) {
        var tmIsHave = false;
        for (var j = 0; j < clr.length; j++) {
            if (para[i][0] == clr[j]) {
                tmIsHave = true;
                break;
            }
        }
        if (tmIsHave) continue;
        url += para[i][0] + "=" + para[i][1];
        if (i < para.length - 1) url += "&";
    }
    if (url.substring(url.length - 1) == "?") url = url.substring(0, url.length - 1);
    if (url.substring(url.length - 1) == "&") url = url.substring(0, url.length - 1);
    return url;
}
//获取参数
function GetRequest() {
    var url = location.search; //获取url中"?"符后的字串
    var theRequest = new Array();
    if (url.indexOf("?") != -1) {
        var str = url.substr(1);
        strs = str.split("&");
        for (var i = 0; i < strs.length; i++) {
            theRequest.push(new Array(strs[i].split("=")[0], strs[i].split("=")[1]));
        }
    }
    return theRequest;
}