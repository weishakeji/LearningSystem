$(function () {
    //单选框只能选中一个
    $("input[value=rbAns]").click(function () {
        var tr = $(this).parents("tr");
        var txt = tr.find("td input[type='text']");
        if ($.trim(txt.val()) == "") {
            alert("请勿选择空白选项作为答案项目。");
            return false;
        }
        $("input[value=rbAns]").removeAttr("checked");
        $(this).attr("checked", "checked");
    });
});