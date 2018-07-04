$(function () {
    pay_card();    
});
window.onload = function () {
    pay_scene();
	pay_onlineStyle();
};
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
    var pai = $(".payitem:visible").first();
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
//支付接口在不同场景下的显示
function pay_scene() {
    var isweixin = $().isWeixin(); 	//是否处于微信
    var ismini = $().isWeixinApp(); //是否处于微信小程序
    $(".payitem").each(function (index, element) {
        var scene = $(this).attr("scene");
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
}
