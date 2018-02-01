$(function () {
    setInit();
    setPayInteFaceStyle();
	$("input[id$=btnOnlinePay]").click(function(){
		//alert($("input[name$=tbMoney]").val());
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
	submitPayForm();
}
function submitPayForm(){	
	var form=$("form[id=submitPay]");
	if(form.size()<1){
		 $("body").append("<form method='post' action='/pay/PayEntry.ashx?isVerify=false' target='_blank' id='submitPay'></form>");
		 form=$("form[id=submitPay]");
	}
	var money=$("input[name$=tbMoney]").val();
	form.append("<input name='money' type='hidden' id='money' value='"+money+"' />");
	form.append("<input name='vpaycode' type='hidden' id='vpaycode' value='"+$("input[name$=tbVerCode]").val()+"' />");
	var paiid=$("input[name=paiid]").val();
	if($.trim(paiid)==""){
		setInit();
		paiid=$("input[name=paiid]").val();
	}
	form.append("<input name='paiid' type='hidden' id='paiid' value='"+paiid+"' />");
	form.submit();
}