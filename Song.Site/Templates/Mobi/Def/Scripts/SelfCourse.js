//课程的事件
mui.ready(function () {
    //长按弹出课程详情
    mui('body').off('tap', '.cou-item');
    mui('body').on('tap', '.cou-item', function () {
        var id = $(this).attr("couid");
        var isbuy = $(this).attr("buy");
        if (id == null) return;
        if (isbuy == "true") {
            //如果是已经购买的课程的周日直接进入学习
            window.location.href = "CoursePage.ashx?couid=" + id;
        } else {
            var url = "Course.ashx?id=" + id;
            history.pushState({}, "", $().setPara(window.location.href, "openurl", BASE64.encoder(url))); //更改地址栏信息
            new PageBox("课程详情", url, 100, 100, "url").Open();
        }
    });
    mui('body').off('doubletap', '.cou-item');
    mui('body').on('doubletap', '.cou-item', function () {
        var id = $(this).attr("couid");
        if (id == null) return;
        var url = "Course.ashx?id=" + id;
        history.pushState({}, "", $().setPara(window.location.href, "openurl", BASE64.encoder(url))); //更改地址栏信息
        new PageBox("课程详情", url, 100, 100, "url").Open();
    });
    //向左滑动弹出课程详情
    mui('body').off('slideleft', '.mui-table-view-cell');
    mui('body').on('slideleft', '.mui-table-view-cell', function (event) {
        var id = $(this).attr("couid");
        if (id == null) return;
        var url = "Course.ashx?id=" + id;
        history.pushState({}, "", $().setPara(window.location.href, "openurl", BASE64.encoder(url))); //更改地址栏信息
        new PageBox("课程详情", url, 100, 100, "url").Open();
        mui.swipeoutClose(this);
    });
    //向右滑动，弹出咨询交流
    mui('body').off('slideright', '.mui-table-view-cell');
    mui('body').on('slideright', '.mui-table-view-cell', function (event) {
        var id = $(this).attr("couid");
        if (id == null) return;
        var name = $(this).attr("couname");
        new PageBox("《" + name + "》", "MsgBoards.ashx?couid=" + id, 100, 100, "url").Open();
        mui.swipeoutClose(this);
    });
});

$(function () {
    //计算每个分类下的课程数
    $(".mui-table-view-cell").each(function (index, element) {
        var count = $(this).find(".count");
        if (count.size() < 1) return;
        count.html($(this).find(".cou-item").size());
    });
});