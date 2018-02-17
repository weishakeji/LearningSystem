$(function () {
    setInit();
    setPayInteFaceStyle();
	//在线充值提交
	$("input[id$=btnOnlinePay]").click(function(){
	    if (Verify.IsPass($("form"))) {
	        $("input[type=hidden][name=vpaycode]").val($("input[type=text][name$=tbVerCode]").val());
	        $("input[type=hidden][name=money]").val($("input[type=text][name$=tbMoney]").val());
	        submitPay();
	    };	
	});
	//当在线充值时，变换提交方式
	$("input[name$=tbVerCode]").click(function(){
		$("form").attr("target","_blank");
		$("form").attr("action","/pay/PayEntry.ashx?isVerify=false");
	});
	//当采用充值码时，再变换回来
	$("input[name$=tbCode]").click(function(){
		$("form").attr("target","_self");
		$("form").attr("action","");
	});
});
//初始化
function setInit() {   
    var paiid = $().getPara("paiid");
    var pi = $(".payInterface .pi[paiid=" + paiid + "]");
    if (pi.size() < 1) pi = $(".payInterface .pi:first");
    pi.addClass("piSelect").attr("select", "true");
    $("input[name=paiid]").val(pi.attr("paiid"));
}
//设置接口的样式
function setPayInteFaceStyle() {
    var pi = $(".payInterface .pi");
    pi.click(function () {
        $(".payInterface .pi").removeClass("piSelect").attr("select", "false");
        $(this).addClass("piSelect").attr("select", "true");
        $("input[name=paiid]").val($(this).attr("paiid"));
    });
}
//提交在线支付
function submitPay() {
    var txt = "充值是否成功？如果充值成功请点击“确定”。";
    var msg = new parent.MsgBox("在线充值", txt, 400, 300, "confirm");
    msg.EnterEvent = function () {
        window.location.href = window.location.href;
        parent.MsgBox.Close();
    };
    msg.Open();	
}
