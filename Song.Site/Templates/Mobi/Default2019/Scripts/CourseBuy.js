window.onload = function () {
    pay_onlineStyle();
};
$(function () {
    _btnEvent();
    _selectPrice();
    //如果充值成功后的返回
    var recharge = $.cookie('courseBuy_state');
    if (recharge == 'recharge') {
        BuySubmit(1, 0);
    }
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
        $(".txt-row").text('购买需要：卡券' + buyclac.need.coupon + '个，资金' + buyclac.need.money + '元');
        if (buyclac.money < buyclac.need.money) {
            $(".txt-row").append('，还需充值' + buyclac.recharge + '元').addClass('needRecharge');
            $("#payInterface").show();
            $("input[name=money]").val(buyclac.recharge);
            $("#needRecharge b").text(buyclac.recharge);

        } else {
            $(".txt-row").removeClass('needRecharge');
            $("#payInterface").hide();
        }
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
        span: selected.find(".alt").attr("span"),        //当前选中的购买项的数量
        unit: selected.find(".alt").attr("unit"),        //当前选中的购买项的日期单位
        mprice: Number(selected.find(".mprice").html()),      //选中项的资金价格
        cprice: Number(selected.find(".cprice").size() > 0 ? selected.find(".cprice").html() : 0),      //选中项的卡券价格
        money: Number($("#money").html()),       //余额
        coupon: Number($("#coupon").html()),        //卡券
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
        BuySubmit(0, 0);
        return false;
    });
    //试用按钮
    $(".aTryout").click(function () {
        var isTry = $(this).attr("IsTry") == "True" ? true : false;
        if (isTry) {
            var txt = "您选择了试用《" + $(this).attr("course") + "》。<br/><br/>";
            txt += "说明：在试用状态只能做“章节练习”和查阅一些资料。";
            var msg = new MsgBox("暂时试用", txt, 90, 60, "confirm");
            msg.EnterEvent = function () {
                BuySubmit(1, 1);
            };
            msg.Open();
        } else {
            new MsgBox("提示", "该课程不允许试用，请登录购买。", 90, 60, "alert").Open();
        }
        return false;
    });
    //确定按钮
    $("#btnBuyStudy").click(function () {
        var moneySpan = $("#money"); //学员资金余额
        var couponSpan = $("#coupon"); //学员卡券余额
        if (moneySpan.size() < 1) {
            var txt = "您还没有登录，请登录后购买。";
            var msg = new MsgBox("未登录", txt, 80, 200, "confirm");
            msg.EnterEvent = function () {
                window.location.href = "Login.ashx";
            };
            msg.Open();
            return false
        }
        //是否选项费用项
        var selected = $(".priceSelected");
        if (selected.size() < 1) {
            new MsgBox("提示", "请选择学习费用的选项。", 80, 200, "alert").Open();
            return false;
        }
        //判断产品价格不得低于余额
        var buyclac = clacMoney();
        //资金余额不足
        if (!buyclac.pass) {
            var txt = "当前课程" + buyclac.span + buyclac.unit + "的学习需要<b>" + buyclac.mprice + "元</b>，";
            txt += "使用卡券抵扣以后仍需要<b>" + buyclac.need.money + "元</b>。<br/>您的余额不足，需要充值<b>{0}元</b>，是否立即充值，并购买课程？";
            if ($(".payitem").size() > 0) {
                var msg = new MsgBox("提示", txt.replace("{0}", buyclac.recharge), 80, 220, "confirm");
                msg.EnterEvent = function () {
                    $.cookie('courseBuy_state', 'recharge');    //购买状态（进入支付环节）
                    $.cookie('recharge_returl', '/Mobile/CourseBuy.ashx');  //支付成功后跳转到的页面
                    $('form').submit()
                };
                msg.OverEvent = function () {
                    $.cookie('courseBuy_state', '');
                    $.cookie('recharge_returl', '');
                };
                msg.Open();
            } else {
				//如果没有支付接口
                new MsgBox("提示", $("#nopay").text(), 80, 200, "alert").Open();
            }
            return false
        }
        if (!Verify.IsPass($("form"))) return false;
        //如果资金充足，则提示是否购买
        var couname = $.trim($(".couname").text());
        var unit = buyclac.unit;
        if (buyclac.unit == "日") unit = '天';
        if (buyclac.unit == "月") unit = '个月';
        var msg = new MsgBox("确认", "是否确定购买该课程《" + couname + "》" + buyclac.span + unit + "的学习时间？", 80, 300, "confirm");
        msg.EnterEvent = function () {
            BuySubmit(1, 0);
        };
        msg.Open();

        return false;
    });
    //返回按钮事件
    mui('body').on('tap', '.btnStudyBack', function (event) {
        window.history.go(-1);
    });
}

//提交购买操作
//isfree:是否免费购买，免费请写0
//istry:是否试用，不试用请写0
function BuySubmit(isfree, istry) {
    var urlPath = "/Ajax/CourseBuySubmit.ashx?timestamp=" + new Date().getTime();
    var cpid = $(".priceSelected").attr("cpid");  //价格项的id
    var couid = Number($().getPara("couid"));    //课程id
    if (couid == null || isNaN(couid) || couid <= 0) {
        couid = $('body').attr('couid');
    }
    var return_url = "CoursePage.ashx";            //成功后，跳转的页面
    $.ajax({
        type: "POST", url: urlPath, dataType: "text",
        data: {
            cpid: cpid, couid: couid, return_url: return_url,
            isfree: isfree, istry: istry
        },
        //开始，进行预载
        beforeSend: function (XMLHttpRequest, textStatus) {
            window.isSubmit = true;
            var msg = new MsgBox("提交", "交易处理中，请稍等……", 90, 160, "loading");
            msg.ShowCloseBtn = false;
            msg.Open();
        },
        //加载出错
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            var msg = new MsgBox("服务器故障", "无法获取响应<br/><br/>错误信息：" + errorThrown, 90, 200, "alert");
            msg.Open();
        },
        //加载成功！
        success: function (data) {
            try {
                var result = eval("(" + data + ")");
                if (result.status == 0) {
                    var msg = new MsgBox("操作成功", "开始学习！<second>3</second>后跳转", 90, 200, "loading");
                    msg.OverEvent = function () {
                        top.location.href = result.return_url + "?couid=" + result.couid;
                    };
                    msg.Open();
                    $.cookie('courseBuy_state', '');    //清除购买状态
                    $.cookie('recharge_returl', '');
                }
                var error = "";
                if (result.status == 1) error = "您还未登录！";
                //if (result.status == 2) error = "验证码不正确！";
                if (result.status == 3) error = "价格数据不存在，请刷新界面再提交！";
                if (result.status == 4) error = "当前课程不存在，请刷新界面再提交！";
                if (result.status == 5) error = "余额不足，请充值！";
                if (result.status == 6) error = "数据异常！";
                if (result.status == 7) error = "当前课程并不是免费的！";
                if (result.status == 7) error = "当前课程不允许试用！";
                if (result.status != 0) {
                    var msg = new MsgBox("错误", error, 90, 200, "alert");
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
        $(".payitem").each(function (index, element) {
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
        });
        //如果没有可供使用的支付接口
         $("#nopay").css("display",$(".payitem").size() < 1 ? "block" : "none");
    })();
    //当点击接口时
    mui('body').on('tap', '.payitem', function () {
        $("input[name=paiid]").val($(this).attr("paiid"));
        $(".payitem").find(".ico").hide();
        $(this).find(".ico").show().focus();
        $("#pay-title #pay-api").text($(this).attr("painame"));
        $("#pay-title #pay-img").attr("src", $(this).find("img").attr("src"));
    });
    //初始样式，默认取第一个
    var pai = $(".payitem").first();
    if (pai.size() > 0) mui.trigger(pai.get(0), 'tap');
}

