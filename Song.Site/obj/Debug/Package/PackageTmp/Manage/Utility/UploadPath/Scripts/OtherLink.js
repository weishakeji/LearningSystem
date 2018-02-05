$(function () {
    $("form").submit(function () {
        var name = $(this).find("input[name=tbName]").val();
        var url = $(this).find("input[name=tbUrl]").val();
        var height = $(this).find("input[name=tbHeight]").val();
        var width = $(this).find("input[name=tbWidth]").val();
        var span = $(this).find("input[name=tbSpan]").val();
        $.post(window.location.href, { url: url, name: name, height: height, width: width, span: span }, function (requestdata) {
            try {
                var data = eval("(" + requestdata + ")");
                if (data.success == "1") {
                    //调用上级页面方法
                    //alert(data.url);
                    top.setOuterLink(data.name, data.url);
                }
            } catch (err) {
                alert("错误：" + err);
            }
        });
        return false;
    });
});
