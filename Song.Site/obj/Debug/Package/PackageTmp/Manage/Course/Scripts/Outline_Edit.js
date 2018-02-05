//初始化方法
$(function () {
    navLoyoutInit();
    setTreeEvent();
});

//初始化
function navLoyoutInit() {
    //树形下拉菜单
    var ddlOutline = $("select[name$=ddlOutline]");
    //下拉菜单的选项
    var option = ddlOutline.find("option");
    //当前id
    var id = $().getPara("couid");
    //当前导航项的父级id
    var pid = ddlOutline.find("option[selected=selected]").attr("value");
    pid = typeof (pid) == "undefined" ? "0" : pid;
    ddlOutline.attr("defPid", pid);
    //
    _setChild(id, option);
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
    var ddlOutline = $("select[name$=ddlOutline]");
    var pid = ddlOutline.attr("defPid");
    ddlOutline.change(function () {
        var cid = $(this).attr("value");
        if (cid == "-1") {
            alert("请勿选择自身或自身的下级作为父级。");
            ddlOutline.find("option").removeAttr("selected");
            ddlOutline.find("option[value=" + pid + "]").attr("selected", "selected");
        }
    });
}

