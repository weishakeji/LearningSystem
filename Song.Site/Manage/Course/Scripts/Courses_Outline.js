$(function () {
    $(".titItem:first").click();
    _initEvent();
    _contentsLoyout();
    playVideo();
    setTreeEvent();
});
//当窗口大变化时
$(window).resize(
	function () {

	}
);
//界面始初化布局
function _contentsLoyout() {
    //弹出窗口
    $("a[btnType=openwin]").click(function () {
        var href = $(this).attr("href");
        var title = $(this).attr("title");        
        new top.PageBox(title, href, 640, 480,null, window.name).Open();
        return false;
    });
    $("input[btnType=openwin]").click(function () {
        var a = $(this).parents("tr").find("a[btnType=openwin]");
        var href = a.attr("href");
        var title = a.attr("title");
        new top.PageBox(title, href, 640, 480, null, window.name).Open();
        return false;
    });

    //树形下拉菜单
    var ddlTree = $("select[id$=ddlOutline]");
    //当前导航项的父级id
    var pid = ddlTree.find("option[selected=selected]").attr("value");
    pid = typeof (pid) == "undefined" ? "0" : pid;
    ddlTree.attr("defPid", pid);
    //
    _setChild($().getPara("olid"), ddlTree.find("option"));
}
function _initEvent() {
    //章节切换的事件
    var ol = $(".outlineName");
    ol.click(function () {
        var rightbox = $(".rightBox");
        if (rightbox.size() > 0) {
            if (!confirm("切换章节之前，请保存当前章节的内容编辑。\n是否继续切换？")) {
                return false;
            }
        }
    });
    //章节编辑提交的事件
    $("input[name$=btnEnter]").hover(function () {
        $(".titItem:first").click();
    });
    //新增章节的按钮
    $("input.btnAdd").click(function () {
        var href = "Courses_Outline.aspx?couid=";
        var id = $().getPara("couid");
        window.location.href = href + id;
        return false;
    });
}
function playVideo() {
    $("a.video").click(function () {
        var href = $(this).attr("href");
        player(href);
        return false;
    });
}
function player(file) {
    //alert(uploaderPath);
    var width = 500;
    var height = 400;
    var str = "<object classid='clsid:D27CDB6E-AE6D-11cf-96B8-4445535411111'  codebase='http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=7,0,19,0'";
    str += "  width=" + width + " height=" + height + " >";
    str += "<param name='movie' value='" + uploaderPath + "flvplayer.swf?vcastr_file=" + file + "' />";
    str += "<param name='quality' value='high' />";
    str += "<param name='allowFullScreen' value='true' />";
    str += "<param name='FlashVars' value='vcastr_file=" + file + "&IsAutoPlay=1&IsContinue=1' />";
    str += "<embed src='" + uploaderPath + "flvplayer.swf?vcastr_file=" + file + "' allowfullscreen='true'";
    str += " flashvars='vcastr_file=" + file + "&IsAutoPlay=1&IsContinue=1' quality='high' pluginspage='http://www.macromedia.com/go/getflashplayer' ";
    str += " type='application/x-shockwave-flash'  width=" + width + " height=" + height + " />";
    str += "</object>";
    $("#divPlayer").html(str);
}

//设置当前导航以及下级导航不可以选择
function _setChild(currid, option) {
    option.each(function (index, element) {
        if ($(this).val() == currid) {
            $(this).attr("style", "background-color: #cccccc;");
            $(this).attr("value", -1);
            //取子级
            option.each(function () {
                var pid = $(this).attr("pid");
                if (pid == currid) {
                    _setChild($(this).val(), option);
                }
            });
        }
    });
}
//选择父级导航更改时的事件
function setTreeEvent() {
    //树形下拉菜单
    var ddlTree = $("select[id$=ddlOutline]");
    var pid = ddlTree.attr("defPid");
    ddlTree.change(function () {
        var cid = $(this).attr("value");
        if (cid == "-1") {
            alert("请勿选择自身或自身的下级作为父级。");
            ddlTree.find("option").removeAttr("selected");
            ddlTree.find("option[value=" + pid + "]").attr("selected", "selected");
        }
    });
}
