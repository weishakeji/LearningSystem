$(function () {
    var tbCode = $("input[name$=tbCode]");
    var search = tbCode.size() > 0 ? $.trim(tbCode.val()) : "";
    $(".code").each(function () {
        var code = $(this).text();
        var regexp = new RegExp("" + search + "", "gm");
        code = code.replace(regexp, "<b>" + search + "</b>");
        $(this).html(code);
    });
});

