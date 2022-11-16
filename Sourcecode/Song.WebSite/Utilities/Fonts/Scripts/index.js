$(function () {
    //统计图标数量
    var list = document.querySelectorAll('.iconfont-list li');
    for (var i = 0; i < list.length; i++) {
        var code = list[i].querySelectorAll('.code')[0];
        var icon = list[i].querySelectorAll('i')[0];
        //console.log(code.innerText);
        icon.innerHTML = '&#x' + code.innerText.substring(1);
    }
    document.querySelector('header span b').innerHTML = list.length;
    //查询
    $("#search").submit(function () {
        //结果显示区域，清空
        var result = $("#result");
        result.html("");
        //查询的字符
        var text = $(this).find("input[type='text']").val();
        if (text == '') return false;
        console.log(text);
        //查询图标
        var items = $(".iconfont-list li");
        var html = "";
        items.each(function () {
            var name = $(this).find("div.name").text();
            var code = $(this).find("div.code").text().replace('\\', '');
            if (name.indexOf(text) > -1 || code.indexOf(text) > -1)
                html += $(this).prop("outerHTML");
        });
        result.html(html);
        return false;
    });
});