$(function () {
    //编辑状态时的输入框
    $("input[name$=tbName]").each(function () {
        var parent = $(this).parent();
        var treeIco = parent.find(".treeIco");
        $(this).width(parent.innerWidth() - treeIco.outerWidth(true)*3
         - parseInt($(this).css("border"))*3);
    });
});
