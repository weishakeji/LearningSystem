$(function () {
    var editurl = "Outline_Edit.aspx?couid=" + $().getPara("couid");
    //新增与编辑按钮事件
    $("input[name$=btnAdd]").click(function () {
        var box = new top.PageBox("新增章节", editurl, 80, 80, null, window.name);
        box.CloseEvent = function () {
            //var href = window.location.href;
            //window.location.href = $().setPara(href, "tmpara", new Date().getTime());
        };
        box.Open();
        return false;
    });
    //打开窗口，用于编辑、视频、附件的弹窗
    $("a[type=open]").click(function () {
        var txt = $(this).text();
        var url = $(this).attr("href");
        var box = new top.PageBox("章节"+txt, url, 80, 80, null, window.name);
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
