$(function () {
    //学习卡使用的按钮事件
    use_card();
    //学习卡领用（暂存的个人名下）
    get_card();
    //当点击学习卡列表中的使用按钮时
    badge_use();
    //查看学习卡详情
    $(".viewinfo").click(function () {
        var href = $(this).attr("href");
        var code = $(this).text();
        var box = new top.PageBox("学习卡：" + code, href, 1000, 80, null, window.name);       
        box.Open();
        return false;
    });

});
//填充学习到输入框，并使用
function set_card_use(card) {
    $("#tbCard").val(card);
    $("#btnUseCard").click();
}
//学习卡充值
function use_card() {
    $("input[name$=btnUse]").click(function () {
        //开始充值
        var form = $(this).parents("form");
        var card = form.find("input[name$=tbCode").val();
        if ($.trim(card) == "") return false;
        $('input[name$=btnUse]').attr({"disabled":"disabled"});
        $.post(window.location.href, { action: "useCode", card: card }, function (result_data) {
            var data = eval(result_data);
            //操作成功
            if (data.state == 1) {                
                var txt = "选修课程" + data.items.length + "个，如下：<br/>";
                for (var i = 0; i < data.items.length; i++) {
                    txt += (i + 1) + "、《" + unescape(data.items[i].Cou_Name) + "》<br/>";
                }
                var msg = new top.MsgBox("学习卡使用成功", txt, 400, 300, "msg");
                msg.ShowCloseBtn = false;
                msg.OverEvent = function () {
                    top.document.location.href = top.document.location.href;
                };
                msg.Open();

            } else {
                var msg = new top.MsgBox("学习卡使用失败", data.info, 400, 300, "msg");
                msg.ShowCloseBtn = false;
                msg.Open();                
                $("#card-error").text(data.info);
            }
            $("input[name$=btnUse]").removeAttr("disabled"); ;
        });
        return false;
    });
}
//填充学习到输入框，并使用
function set_card_get(card) {
    $("#tbCard").val(card);
    $("#btnGetCard").click();
}
//学习卡领用
function get_card() {
    $("input[name$=btnGet]").click(function () {
        //开始充值
        var form = $(this).parents("form");
        var card = form.find("input[name$=tbCode").val();
        if ($.trim(card) == "") return false;
        $('input[name$=btnGet]').attr({"disabled":"disabled"});
        $.post(window.location.href, { action: "getCode", card: card }, function (result_data) {
            var data = eval(result_data);
            //操作成功
            if (data.state == 1) {

                var msg = new top.MsgBox("操作成功", "学习卡放置在个人名下，随时可以使用", 400, 300, "msg");
                msg.ShowCloseBtn = false;
                msg.OverEvent = function () {
                    top.document.location.href = top.document.location.href;
                };
                msg.Open();
            } else {
                var msg = new top.MsgBox("操作失败", data.info, 400, 300, "msg");
                $("#card-error").text(data.info);
            }
             $("input[name$=btnGet]").removeAttr("disabled");
        });
        return false;
    });
}


//当点击学习列表中的使用时
function badge_use() {
    $(".use").click(function () {
        var code = $.trim($(this).prev("a").text());
        $("input[name$=tbCode]").val(code);
    });
}