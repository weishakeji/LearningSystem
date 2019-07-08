$(function () {
    _initLoyout();
    $("input[name$=cbAns]").click(function () {
        var tr = $(this).parents("tr");
        var num = tr.find("span.itemTxtCount");
        if (Number($.trim(num.text())) < 1) {
            alert("请勿选择空白选项作为答案项目。");
            return false;
        }
        //$("input[value=rbAns]").removeAttr("checked");
        //$(this).prop("checked", "checked");
    });
    //提交按钮
    $("input[id$=btnEnter]").click(function () {
        //验证题干是否录入
        if (Number($(".count").text()) < 1) {
            alert("题干不得为空！");
            return false;
        }
        //验证选项是否为空
        var itemNoNumm = 0;
        $("span.itemTxtCount").each(function () {
            var num = Number($(this).text());
            itemNoNumm += num;
        });
        if (itemNoNumm < 1) {
            alert("请填写试题的选择项！");
            return false;
        }
        //是否设置了正确答案
        if ($("input[name$=cbAns]:checked").size() < 2) {
            alert("多选题至少要有两个答案！");
            return false;
        }
        return true;
    });
    _setEvent();
});
//初始布局
function _initLoyout() {
    $(".quesRight").height($(".pageWinContext").height());
}
//设置一些事件
function _setEvent() {
    $(".wrongInfo").hover(function () {
        var box = $("#wrongInfoBox");
        var off = $(this).offset();
        box.css("position", "absolute").css("z-index", "20001");
        box.css({ width: 150, height: 200 });
        box.css({ left: off.left, top: off.top + $(this).height() });
        box.css("background-color", "#FFFFCC");
        box.css("padding", "5px");
        box.show();
    }, function () {
        $("#wrongInfoBox").hide();
    });
}
