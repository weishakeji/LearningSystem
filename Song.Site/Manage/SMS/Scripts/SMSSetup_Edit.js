$(function () {
    ajaxLoaddata();
});
//异步加载数据
function ajaxLoaddata() {

    $(".smsCount").text("查询中...");
    $.post(window.location.href, { action: "getnumber" }, function (requestdata) {
        var data = eval("(" + requestdata + ")");
        var span = $(".smsCount")
        if (data.num == -1) {
            span.text("查询失败，详情："+data.desc);
        } else {
            span.text(data.num);
        }
    });

}