$(function () {
    //搜索字符高亮显示
    var tbCode = $("input[name$=tbCode]");
    var search = tbCode.size() > 0 ? $.trim(tbCode.val()) : "";
    $(".code").each(function () {
        var code = $(this).text();
        var regexp = new RegExp("" + search + "", "gm");
        code = code.replace(regexp, "<b>" + search + "</b>");
        $(this).html(code);
    });
    //回滚事件
    $("a[tag=btnGoBack]").click(function () {
        var href = $(this).attr("href");
        var code = $(this).attr("code");    //学习卡，卡号
        var pw = $(this).attr("pw");    //学习卡，卡号
        new top.PageBox("回滚学习卡："+code+"-"+pw, href+"?code="+code+"&pw="+pw, 640, 480, null, window.name).Open();
        return false;
    });
});

