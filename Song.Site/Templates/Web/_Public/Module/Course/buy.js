$(function () {
    _btnEvent();
    _selectPrice();
    //计算日均价格
    $(".alt").each(function (index, element) {
        var span = Number($(this).attr("span"));
        var unit = $.trim($(this).attr("unit"));
        var price = Number($(this).attr("price"));
        var day = span;
        if (unit == "日") day = span;
        if (unit == "周") day = span * 7;
        if (unit == "月") day = span * 30;
        if (unit == "年") day = span * 365;
        var result = Math.floor(price / day * 100) / 100;
        $(this).find("b").text(result);
    });
});
//按钮事件
function _btnEvent() {
    //免费课程的学习按钮
    $("#btnStudy").click(function () {
        //BuySubmit(0, 0);
        //return false;
    });
    //确定按钮
    $("#btnBuyStudy").click(function () {
        var moneySpan = $("#money"); //学员资金余额
        var couponSpan = $("#coupon"); //学员卡券余额
        if (moneySpan.size() < 1) {
            var txt = "您还没有登录，请登录后购买。";
            var msg = new MsgBox("未登录", txt, 400, 300, "confirm");
            msg.EnterEvent = function () {
                window.location.href = "/student/index.ashx";
            };
            msg.Open();
            return false
        }
        //是否选项费用项
        var selected = $(".priceSelected");
        if (selected.size() < 1) {
            new MsgBox("提示", "请选择学习费用的选项。", 400, 300, "alert").Open();
            return false;
        }
        //判断产品价格不得低于余额
        var money = Number(moneySpan.html()); //余额
        var coupon = Number(couponSpan.html()); //卡券余额
        var mprice = Number($(this).attr("mprice"));    //价格，资金
        var cprice = Number($(this).attr("cprice"));    //价格，卡券
        //满足条件的判断
        var tm=money >= mprice || money>=(mprice-(coupon>cprice ? cprice: coupon) );
        //资金余额不足
        if (!tm) {
            var msg = new MsgBox("提示", "你的余额不足，是否充值？", 400, 300, "confirm");
            msg.EnterEvent = function () {
                $(".btnRecharge").get(0).click();
            };
            msg.Open();
            return false
        }
        if (!Verify.IsPass($("form.money-area"))) return false;
        BuySubmit(1, 0);
        return false;
    });
}
//选择价格
function _selectPrice() {
    //价格选项的点击事件
    $(".priceItem").click(function () {
        $(this).parent().find(".priceItem").removeClass("priceSelected");
        $(this).parent().find(".priceItem span.ico").html("&#xf00c6;");
        //选中事件
        $(this).addClass("priceSelected");
        $(this).find("span.ico").html("&#xe667;");
        //如果登录，则显示验证码
        var moneySpan = $("#money");
        if (moneySpan.size() > 0) {
            $(".verifyInfo").show();
        }
        //设置价格在购买按钮上，取值时方便
        var selected = $(".priceSelected");
        var mprice = Number(selected.find(".mprice").html()); //选中项的资金价格
        var cprice = Number(selected.find(".cprice").html()); //选中项的卡券价格
        $("#btnBuyStudy").attr("mprice", mprice).attr("cprice", (isNaN(cprice) ? 0 : cprice));
    });
    $(".priceItem:eq(0)").click();
}

//提交购买操作
//isfree:是否免费购买，免费请写0
//istry:是否试用，不试用请写0
function BuySubmit(isfree, istry) {
    var urlPath = "/Ajax/CourseBuySubmit.ashx?timestamp=" + new Date().getTime();
    var code = $.trim($(".verify").val());         //验证码
    var cpid = $(".priceSelected").attr("cpid");  //价格项的id
    var couid = $().getPara("couid");    //课程id
    var return_url = "CourseStudy.ashx";            //成功后，跳转的页面
    $.ajax({
        type: "POST", url: urlPath, dataType: "text",
        data: { veriCode: code, cpid: cpid, couid: couid, return_url: return_url,
            isfree: isfree, istry: istry
        },
        //开始，进行预载
        beforeSend: function (XMLHttpRequest, textStatus) {
            window.isSubmit = true;
            var msg = new MsgBox("提交", "正在处理交易，请稍等……", 400, 300, "loading");
            msg.ShowCloseBtn = false;
            msg.Open();
        },
        //加载出错
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            var msg = new MsgBox("服务器故障", "无法获取响应<br/><br/>错误信息：" + errorThrown, 400, 300, "alert");
            msg.Open();
        },
        //加载成功！
        success: function (data) {
            try {
                var result = eval("(" + data + ")");
                if (result.status == 0) {
                    var msg = new MsgBox("操作成功", "操作成功，点击确定开始学习", 400, 300, "confirm");
                    msg.EnterEvent = function () {
                        window.location.href = result.return_url + "?couid=" + result.couid;
                    };
                    msg.Open();
                }
                var error = "";
                if (result.status == 1) error = "您还未登录！";
                if (result.status == 2) error = "验证码不正确！";
                if (result.status == 3) error = "价格数据不存在，请刷新界面再提交！";
                if (result.status == 4) error = "当前课程不存在，请刷新界面再提交！";
                if (result.status == 5) error = "余额不足，请充值！";
                if (result.status == 6) error = "数据异常！";
                if (result.status == 7) error = "当前课程并不是免费的！";
                if (result.status == 7) error = "当前课程不允许试用！";
                if (result.status != 0) {
                    var errinfo = "原因：" + error;
                    errinfo += "<br/>说明：" + result.errinfo;
                    errinfo += "<br/>详情：<a href=\'" + result.logfile + "' target='_blank'>点击查看</a>";
                    var msg = new MsgBox("发生错误", errinfo, 400, 300, "alert");
                    msg.OverEvent = function () {
                        MsgBox.Close();
                    };
                    msg.Open();
                }
                window.isSubmit = false;
            } catch (e) {
                alert(data);
            }
        }
    });
}