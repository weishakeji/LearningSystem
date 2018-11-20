$(function () {
    var editurl = "Outline_Edit.aspx?couid=" + $().getPara("couid");
    //新增与编辑按钮事件
    $("input[name$=btnAdd]").click(function () {
        var box = new top.PageBox("新增章节", editurl, 80, 80, null, window.name);
        box.CloseEvent = function () {
            //var href = window.location.href;
            //window.location.href = $().setPara(href, "tmpara", new Date().getTime());
        };
        //box.Open();
        return false;
    });
    //打开窗口，用于编辑、视频、附件的弹窗
    $("a[type=open]").click(function () {
        var txt = $(this).text();
        var url = $(this).attr("href");
        var box = new top.PageBox("章节" + txt, url, 80, 80, null, window.name);
        box.CloseEvent = function () {
            //var href = window.location.href;
            //window.location.href = $().setPara(href, "tmpara", new Date().getTime());
        };
        box.Open();
        return false;
    });
    //编辑状态时的输入框宽度设置
    $("input[name$=tbName]").each(function () {
        var parent = $(this).parent();
        var treeIco = parent.find(".treeIco");
        $(this).width(parent.innerWidth() - treeIco.outerWidth(true) * treeIco.size() - 80
         - parseInt($(this).css("border")) * 3);
    });
});
//导入
function OnInput() {
    var href = String(window.document.location.href);
    var page = "Outline_Input.aspx";
    page += href.indexOf("?") > -1 ? href.substring(href.lastIndexOf("?")) : "";
    var box = new top.PageBox("章节导入", page, 80, 80, null, window.name);
    box.CloseEvent = function (box) {
        //刷新父窗体
        var parent = box.attr("parent");
        window.top.PageBox.Refresh(parent);
    };
    box.Open();
    return false;
}

//导出事件
function OnOutput() {
    var href = String(window.document.location.href);
    var page = "Outline_Export.aspx";
    page += href.indexOf("?") > -1 ? href.substring(href.lastIndexOf("?")) : "";
    var box = new top.PageBox("章节导出", page, 80, 80, null, window.name);
    box.CloseEvent = function (box) {
        //var parent = box.attr("parent");
        //window.top.PageBox.Refresh(parent);
    };
    box.Open();
    return false;
}