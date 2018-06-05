$(function () {
    pay_card();
    pay_onlineStyle();
});

//充值卡充值
function pay_card() {
    $("form#formPayCard").submit(function () {
        var form = $(this);
        var card = form.find("#tbCard").val();
        if ($.trim(card) == "") return false;
		mui('#btnPayCard').button('loading');
        $.post("Recharge.ashx", { action: "paycard", card: card }, function (data) {
            //充值成功
            if (data == "1") {
                mui.toast('充值成功！', { duration: 500, type: 'div' });
                //弹出确认框
                mui.alert("充值成功", function (e) {
                    document.location.href = "Recharge.ashx";
                });
            } else {
                mui.toast(data, { duration: 2000, type: 'div' });
                $("#card-error").text(data);
            }
			mui('#btnPayCard').button('reset');
        });
        return false;
    });
}


//设置接口的样式
function pay_onlineStyle() {
    //初始样式，默认取第一个
    var pai = $(".payitem:first");
	$("input[name=paiid]").val(pai.attr("paiid"));
    $(".payitem").find(".ico").hide();
    pai.find(".ico").show();
    $("#pay-title #pay-api").text(pai.attr("painame"));
    $("#pay-title #pay-img").attr("src", pai.find("img").attr("src"));
    //当点击接口时
    mui('body').on('tap', '.payitem', function () {
		$("input[name=paiid]").val($(this).attr("paiid"));
        $(".payitem").find(".ico").hide();
        $(this).find(".ico").show().focus();
        $("#pay-title #pay-api").text($(this).attr("painame"));
        $("#pay-title #pay-img").attr("src", $(this).find("img").attr("src"));
    });
}
