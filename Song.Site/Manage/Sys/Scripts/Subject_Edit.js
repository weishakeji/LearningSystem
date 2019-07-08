//初始化方法
$(function () {
    navLoyoutInit();
    setTreeEvent();
});

//初始化
function navLoyoutInit() {
    //树形下拉菜单
    var ddlTree = $("select[id$=ddlTree]");
    //下拉菜单的选项
    var option = ddlTree.find("option");   
    //当前导航项的父级id
    var pid = ddlTree.find("option[selected=selected]").attr("value");
    pid = typeof (pid) == "undefined" ? "0" : pid;
    ddlTree.attr("defPid", pid);
    //
    //当前id
    var id = Number(ddlTree.attr("currid"));
    _setChild(id, option);
}
//设置当前导航以及下级导航不可以选择
function _setChild(currid, option) {
    if (currid <= 0) return;
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
    var ddlTree = $("select[id$=ddlTree]");
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

