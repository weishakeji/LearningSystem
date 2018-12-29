$(function () {
	//积分
    var pointsum=Number($("#pointSum").text());
	//$("#pointSum").text(pointsum);
	//比率
	var ratio=Number($("#ratio").text());
	ratio=ratio==null ? 0 : ratio;
	//计算兑换结果
	var result=Math.floor(pointsum/ratio);
	$("#result").text(result);
	$("input[name$=tbNumber]").attr("numlimit",result);
	
	pay_card();
});

//提交
function pay_card() {
    $("form").submit(function () {
        var form = $(this);
        var coupon = form.find("#tbNumber").val();
        if ($.trim(coupon) == "") return false;
        mui('#btnCouponExchange').button('loading');
        $.post(window.location.href, { action: "exchange", coupon: coupon }, function (data) {
			var obj=eval("({"+data+"})");
            //充值成功
            if (obj.state == 1) {
                mui.toast('兑换成功！', { duration: 500, type: 'div' });
                //弹出确认框
                mui.alert("兑换成功", function (e) {
                    document.location.href = "CouponExchange.ashx";
                });
            } else {
                mui.toast(data, { duration: 2000, type: 'div' });
                $("#card-error").text(obj.error);
            }
            mui('#btnCouponExchange').button('reset');
        });
        return false;
    });
}