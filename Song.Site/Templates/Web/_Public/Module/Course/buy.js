$(function () {
    var css = $("script[href]").attr("href");
    $().loadcss(css);
});
window.onload = function () {
    pay_onlineStyle();
};
$(function () {
    _btnEvent();
    _selectPrice();
});
//选择价格
function _selectPrice() {
    //价格选项的点击事件
    $(".priceItem").click(function () {
        $(this).parent().find(".priceItem").removeClass("priceSelected");
        $(this).addClass("priceSelected");
        //记录当前选中的购买项
        $.cookie('cpid', $.trim($(this).attr("cpid")));
        //购买的计算
        var buyclac = clacMoney();
        $(this).parent().find('.txt-row').hide();
        $(this).find(".txt-row").text('购买需要：卡券' + buyclac.need.coupon + '个，资金' + buyclac.need.money + '元');
        if (buyclac.money < buyclac.need.money) {
            $(this).find(".txt-row").append('，还需充值' + buyclac.recharge + '元').addClass('needRecharge');
            $(this).after($("#payInterface").show());
            $("#payInterface .rechargeValue span").text(buyclac.recharge);
            $("input[name=money]").val(buyclac.recharge);
            $("#needRecharge b").text(buyclac.recharge);

        } else {
            $(this).find(".txt-row").removeClass('needRecharge');
            $("#payInterface").hide();
        }
        $(this).find(".txt-row").show();
    });
    //取之前点击的项目（如果没有取第一个）
    var first = $(".priceItem[cpid=" + $.cookie('cpid') + "]");
    if (first.size() < 1) first = $(".priceItem").first();
    first.click();
    //计算日均价格
    (function averageday() {
        $(".alt").each(function (index, element) {
            var span = Number($(this).attr("span"));    //时间数
            var unit = $.trim($(this).attr("unit"));    //时间单位
            var price = Number($(this).attr("price"));      //价格
            //平均每天多少钱
            var average = price / getday(span, unit);
            if (average > 0.01)
                average = Math.floor(average * 100) / 100 + "元";
            else
                average = Math.floor(average * 10000) / 100 + "分";
            $(this).find("b").text(average);
        });
        //计算课程计算项的天数
        function getday(span, unit) {
            var day = span;
            if (unit == "日") day = span;
            if (unit == "周") day = span * 7;
            if (unit == "月") day = span * 30;
            if (unit == "年") day = span * 365;
            return day;
        }
    })();
}
//计算购买课程时所需的资金
function clacMoney() {
    var selected = $(".priceSelected");
    var obj = {
        span: $(".priceSelected .alt").attr("span"),        //当前选中的购买项的数量
        unit: $(".priceSelected .alt").attr("unit"),        //当前选中的购买项的日期单位
        mprice: Number(selected.find(".mprice").html()),      //选中项的资金价格
        cprice: Number(selected.find(".cprice").html()),      //选中项的卡券价格
        money: Number($("#money").text()),       //余额
        coupon: Number($("#coupon").text()),        //卡券
        need: {
            money: 0,   //如果购买，需要的资金数
            coupon: 0   //需要的卡券数
        },
        recharge: 0,     //需要充值的数钱
        //是否能够买得起
        pass: false,
        passclac: function () {
            var t = this;
            return t.money >= t.mprice || t.money >= (t.mprice - (t.coupon > t.cprice ? t.cprice : t.coupon));
        }
    };
    obj.need.coupon = obj.coupon > obj.cprice ? obj.cprice : obj.coupon;   //消耗的卡券数
    obj.need.money = obj.mprice - obj.need.coupon;
    obj.recharge = obj.need.money - obj.money;
    obj.pass = obj.passclac();
    return obj;
}
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
        var tm = money >= mprice || money >= (mprice - (coupon > cprice ? cprice : coupon));
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
        data: {
            veriCode: code, cpid: cpid, couid: couid, return_url: return_url,
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

/*******
支付接口
*/
//设置接口的样式
function pay_onlineStyle() {
    //支付接口在不同场景下的显示
    (function pay_scene() {
        var isweixin = $().isWeixin(); 	//是否处于微信
        var ismini = $().isWeixinApp(); //是否处于微信小程序
        /*$(".payitem").each(function (index, element) {
            var scene = $(this).attr("scene");
            scene = scene.replace(/\s+/g, "");
            if ($.trim(scene) == "") return true;
            var arr = scene.split(",");
            //如果处在微信中
            if (isweixin) {
                if (arr[0] != "weixin" || arr[1] == "h5") $(this).remove();
                if (ismini && arr[1] != "mini") $(this).remove();
                if (!ismini && arr[1] == "mini") $(this).remove();
            } else {
                //如果不在微信中，且该接口仅限微信使用，则不显示
                if (arr[0] == "weixin" && arr[1] != "h5") $(this).remove();
            }
        });*/
        //如果没有可供使用的支付接口
        $("#nopay").css("display", $(".payitem").size() < 1 ? "block" : "none");
    })();
    //当点击接口时
    $(".payitem").click(function () {
        $("input[name=paiid]").val($(this).attr("paiid"));
        $(".payitem").removeClass("paySelected").find(".rechargeValue").hide();
        $(this).addClass("paySelected").find(".rechargeValue").show();
        $("#pay-title #pay-api").text($(this).attr("painame"));
        $("#pay-title #pay-img").attr("src", $(this).find("img").attr("src"));
    });
    //初始样式，默认取第一个
    var pai = $(".payitem").first();
    if (pai.size() > 0) pai.click();
}
