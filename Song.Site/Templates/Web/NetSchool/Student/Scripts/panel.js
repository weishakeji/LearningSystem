//菜单树的状态，为1是展开首个，为2时全部打开，其它值为关闭树形
var treestate = 24;
$(function () {
    _init($("#menuBox"), 1);
    //树形菜单的初始展开状态
    var mmitem = $("body");
    if (treestate == 1) mmitem = $("#menuBox").find(">.mmitem:first");
    if (treestate == 2) mmitem = $("#menuBox").find(".mmitem");
    mmitem.each(function () {
        var mmbox = $(this).next(".mmBox[mmtype!=node]");
        if (mmbox.size() > 0 && mmbox.find(">.mmitem").size() > 0) {
            $(this).click();
        }
    });
    _setAdminPage($().getPara("mmid"));
    //学员中心(左上方）的点击事件
    $(".menuTitle").click(function () {
        var mmid = $(this).attr("mmid");
        _setAdminPage(mmid);
        return false;
    });
});
//mmbox:菜单区域
//level:层级
function _init(mmbox, level) {
    if (mmbox.size() < 1) return;
    //菜单项的处理
    mmbox.find(">.mmitem").each(function () {
        //禁用超链接，改为上级元素事件
        $(this).find("a").click(function () {
            $(this).parent().click();
            return false;
        });
        //下级菜单区
        var mmbox = $(this).next(".mmBox[mmtype!=node]");
        if (mmbox.size() > 0 && mmbox.find(">.mmitem").size() > 0) {
            //下级菜单缩进
            mmbox.find(">.mmitem").css("padding-left", (15 * level) + "px");
            //树形展开
            $(this).click(function () {
                mmbox.toggle();
                if (mmbox.is(":hidden")) $(this).removeClass("open");
                if (mmbox.is(":visible")) $(this).addClass("open");
            }).append("<div class=\"rightIco\">&nbsp;</div>");

        } else {
            //如果没有下级菜单，或当前菜单为”node"，则跳转
            var link = $(this).find("a");
            $(this).click(function () {
                var mmid = $(this).attr("mmid");
                var url = $().setPara(window.location.href, "mmid", mmid);
                try {
                    history.pushState({}, "", url); //更改地址栏信息
                } catch (e) {
                    window.location.href = url;
                    return;
                }
                _setAdminPage(mmid);
            });
        }
        //递归
        _init(mmbox, level + 1);
    });
}
//设置右侧的内容区域
function _setAdminPage(mmid) {
    var id = Number(mmid);
    if (id < 1) {return; }
    var mm = $("#menuBox .mmitem[mmid=" + id + "]");
    alert(mm.text());
}
