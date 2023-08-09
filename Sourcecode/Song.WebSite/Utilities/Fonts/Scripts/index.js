$(function () {
    //统计图标数量
    let list = document.querySelectorAll('.iconfont-list li');
    for (let i = 0; i < list.length; i++) {
        const code = list[i].querySelectorAll('.code')[0];
        const icon = list[i].querySelectorAll('i')[0];
        icon.innerHTML = '&#x' + code.innerText.substring(1);
    }
    //显示总共有多少个图标
    document.querySelector('header span b').innerHTML = list.length;
    //查询
    $("#search").submit(function () {
        //结果显示区域，清空
        let result = $("#result");
        result.html("");
        $(".iconfont-list li").removeClass('selected');
        $(this).find('span').html('');
        //查询的字符
        let text = $(this).find("input[type='text']").val();
        if (text == '') return false;
        //查询图标
        let items = $(".iconfont-list li");
        let html = "";
        let count = 0;
        items.each(function () {
            const name = $(this).find("div.name").text();
            const code = $(this).find("div.code").text().replace('\\', '');
            if (name.indexOf(text) > -1 || code.indexOf(text) > -1) {
                html += $(this).prop("outerHTML");
                $(this).addClass('selected');
                count++;
            }
        });
        result.html(html);
        $(this).find('span').html('查询到 <b>' + count + '</b> 个图标');
        return false;
    });
    //右上角按钮的点击事件
    $("header div.right span[icon]").click(function () {
        let url = $(this).attr("url");
        buildpanel(url);
    });
});

//创建一个面板
function buildpanel(url) {
    let panel = $('div.panel');
    if (panel.length < 1) panel = $('<div>', { class: "panel" });
    panel.append($('<iframe>', {
        'src': url,
        'frameborder': 0, 'border': 0,
        'marginwidth': 0, 'marginheight': 0
    }));
    $("body").append(panel);
    panel.click(function () {
        $(this).remove();
    });
}