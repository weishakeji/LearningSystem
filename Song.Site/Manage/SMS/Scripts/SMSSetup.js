$(function () {
    ajaxLoaddata();
    $(".smsedit").click(function () {
        //var datakey=$(this).parents("tr").attr("datakey");
        $(this).parents("tr").find(".RowEdit").click();
        return false;
    });
    //打开
    $("a[class=open]").click(function () {
        var path = $(this).attr("href");
        if (path == "") return false;
        var tit = $(this).attr("title");
        var wd = Number($(this).attr("width"));
        var hg = Number($(this).attr("height"));
        if (path.length >= 7 && path.substring(0, 7).toLowerCase() == "http://") {
            new top.PageBox(tit, path, wd, hg, window.name).Open();
            return false;
        }
        if (path.length >= 8 && path.substring(0, 8).toLowerCase() == "https://") {
            new top.PageBox(tit, path, wd, hg,window.name).Open();
            return false;
        }
        //当前页面的上级路径，因为子页面没有写路径，默认与本页面同路径
        var url = String(window.document.location.href);
        url = url.substring(0, url.lastIndexOf("/") + 1);
        if (path.substring(0, 1) != "/") {
            path = url + path;
        }
        new top.PageBox(tit, path, wd, hg, window.name).Open();
        return false;
    });
});
//异步加载数据
function ajaxLoaddata() {
    $(".smsCount").each(function (index, element) {
        var remarks = $(this).attr("remarks");
        $(this).text("查询中...");
        $.post(window.location.href, { action: "getnumber", marks: remarks }, function (requestdata) {
            var data = eval("(" + requestdata + ")");
            var span = $(".smsCount[remarks=" + data.marks + "]")
            if (data.num == -1) {
                span.text("查询失败：" + data.desc);
            } else {
                span.text(data.num);
            }
        });
    });
}