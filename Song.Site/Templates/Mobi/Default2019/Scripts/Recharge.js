$(function () {
    //充值码的二维码
    var code = $().getPara("code");
    var pw = $().getPara("pw");
    if ($.trim(code) != "" && $.trim(pw) != "") {
        $("#tbCard").val(code + "-" + pw);
    }
    $('#btn_camera').on('tap', function () {
        $("#upload_qrcode").click();
    });
    qrcode_Decode();

    //充值卡充值 
    pay_card();
});
window.onload = function () {
    pay_scene();
    pay_onlineStyle();
};
//充值卡充值
function pay_card() {
    $("form#formPayCard").submit(function () {
        //判断是否登录
        var nologin = $("#nologin");
        if (nologin.size() > 0) {
            window.Verify.ShowBox(nologin.find("a"), "请登录");
			var msg=new MsgBox("提示", "未登录，点击“确定”进入登录界面。"
						+"<br/><br/><second>6</second>秒后关闭消息", 90, 200, "confirm");
				msg.EnterEvent=function(){					
					window.location.href="login.ashx";
				}
				msg.Open();
            return false;
        }
        //开始充值
        var form = $(this);
        var card = form.find("#tbCard").val();
        if ($.trim(card) == "") return false;
        mui('#btnPayCard').button('loading');
        $.post(window.location.href, { action: "paycard", card: card }, function (data) {
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
//二维码解析
function qrcode_Decode() {
    $("#upload_qrcode").change(function (e) {
		setLoading($("#btn_camera img"),"loading");
        var files = e.target.files;
        if (files && files.length > 0) {
            var form = new FormData();
            form.append('file', files[0]);
            form.append('action', 'decode_qrcode');
            $.ajax({
                url: window.location.href,
                type: 'POST',
                data: form,                    // 上传formdata封装的数据
                dataType: 'Text',
                cache: false,                      // 不缓存
                processData: false,                // jQuery不要去处理发送的数据
                contentType: false,                // jQuery不要去设置Content-Type请求头
                success: function (data) {
                    var code = $().getPara(data, "code");
                    var pw = $().getPara(data, "pw");
                    if ($.trim(code) != "" && $.trim(pw) != "") {
                        $("#tbCard").val(code + "-" + pw);
						mui.toast("二维码解析成功", { duration: 2000, type: 'div' });
                    }else{
						mui.toast("无法解析二维码", { duration: 2000, type: 'div' });
					}
					setLoading($("#btn_camera img"),"normal");
                }
            });
        }
    });
}
//设置图片预载状态
//state: loading,或normal
function setLoading(el,state){
	var file=el.attr(state);
	var path=el.attr("src");
	if(path.indexOf("/")>-1){
		path=path.substring(0,path.lastIndexOf("/")+1);
	}
	el.attr("src",path+file);
}
/*******
支付接口
*/
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
		scene = scene.replace(/\s+/g,"");
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
