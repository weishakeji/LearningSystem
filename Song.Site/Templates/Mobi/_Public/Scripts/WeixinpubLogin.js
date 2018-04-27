//登录成功后返回的地址
var returl = function (acpw, acid) {
    //登录成功后的返回地址
	$.storage("accid", acid);
	$.storage("accpw", acpw);
	setTimeout(gorurl(), 1000);
	function gorurl(){
		window.location.href="default.ashx";
	}
}

//
$(function () {
    //选项卡切换
    $("dl.titbox dd").click(function () {
        var dd = $(this).parent("dl").find("dd");
        dd.removeClass("curr");
        $(this).addClass("curr");
        //获取索引号
        var index = $(this).parent("dl").find("dd").index(this);
        $(".regbox").hide();
        $(".regbox:eq(" + index + ")").show();
    });
    $("dl.titbox dd:eq(0)").click();
    //按钮事件
    setBtnEvent();
    sendSmsEvent(); //发送短信的事件
    //当短信发送后，倒计时数秒
    setInterval(_mobiLogin_smsSendWaiting, 1000);
});


function setBtnEvent() {
    //直接登录
    $("#btnDirect").click(function () {
        $(this).addClass("disabled");       
        var sex = $.trim($("#gender").text()); 	//性别
        var name = $.trim($("#name").text()); 	//姓名
        var photo = $.trim($("#photo").attr("src")); 	//头像
        var url = window.location.href;
        ajax.post(url, { action: "Direct", sex: sex, name: name, photo: photo },
			function (requestdata) {
			    var data = eval("(" + requestdata + ")");
			    if (Number(data.success) == 1) returl(data.acpw, data.acid);
			});

    });
    //新用户注册
    $("#formRegist").submit(function () {
        var isSms = $(this).attr("sms") == "True";      
        var sex = $.trim($("#gender").text()); 	//性别
        var name = $.trim($("#name").text()); 	//姓名
        var photo = $.trim($("#photo").attr("src")); 	//头像
        var mobi = $(this).find("input[name=tbNewAcc]").val();
        //如果不需要短信验证
        if (!isSms) {
            var url = window.location.href;
            ajax.post(url, { action: "register1", sex: sex, name: name, photo: photo,
                mobi: mobi
            }, function (requestdata) {
                var data = eval("(" + requestdata + ")");
                if (Number(data.success) < 0) {
                    //不成功
                    if (data.state == 2) Verify.ShowBox($("form[name=formRegist] input[type=text][name=tbNewAcc]"), "该手机号已经注册！");
                }
                //注册成功
                if (Number(data.success) == 1) returl(data.acpw, data.acid);
            });
        }
        //如果需要短信验证
        if (isSms) {
            //先验证验证码
            var vname = $("form[name=formRegist] img.verifyCode").attr("src");
            var rs = new RegExp("(^|)name=([^\&]*)(\&|$)", "gi").exec(vname), tmp;
            vname = tmp = rs ? rs[2] : "";
            var vcode = $(this).find("input[type=text][name=tbNewCode]").val();
            var sms = $(this).find("input[name=tbNewSms]").val(); //用户填写的短信验证码
            var smsname = $(this).find("#getRegSms").attr("smsname");
            ajax.post(window.location.href, { action: "register2", 
                sex: sex, name: name, photo: photo,
                vname: vname, vcode: vcode,
                sms: sms, mobi: mobi, smsname: smsname
            }, function (requestdata) {
                var data = eval("(" + requestdata + ")");
                if (Number(data.success) < 1) {
                    //不成功
                    if (data.state == 1) Verify.ShowBox($("form[name=formRegist] input[type=text][name=tbNewCode]"), "验证码不正确！");
                    if (data.state == 2) Verify.ShowBox($("form[name=formRegist] input[type=text][name=tbNewAcc]"), "该手机号已经注册！");
                    if (data.state == 3) Verify.ShowBox($("form[name=formRegist] input[type=text][name=tbNewSms]"), "短信验证码错误！");
                }
                //注册成功
                if (Number(data.success) == 1) returl(data.acpw, data.acid);
            });
        }
        return false;
    });
    //绑定已经存在账户
    $("#formBind").submit(function () {
        var isSms = $(this).attr("sms") == "True";
        var isSms = $(this).attr("sms") == "True";        
        var sex = $.trim($("#gender").text()); 	//性别
        var name = $.trim($("#name").text()); 	//姓名
        var photo = $.trim($("#photo").attr("src")); 	//头像
        var mobi = $(this).find("input[name=tbAcc]").val();
        //先验证验证码
        var vname = $("form[name=formBind] img.verifyCode").attr("src");
        var rs = new RegExp("(^|)name=([^\&]*)(\&|$)", "gi").exec(vname), tmp;
        vname = tmp = rs ? rs[2] : "";
        var vcode = $("form[name=formBind]").find("input[type=text][name=tbCode]").val();
        //如果不需要短信验证
        if (!isSms) {
            var pw = $(this).find("input[name=tbPw]").val();    //密码
            ajax.post(window.location.href, { action: "bind1", 
                sex: sex, name: name, photo: photo,
                vname: vname, vcode: vcode, mobi: mobi,pw:pw
            }, function (requestdata) {
                var data = eval("(" + requestdata + ")");
                var form = $("form[name=formBind]");
                if (Number(data.success) < 0) {
                    if (data.state == 1) Verify.ShowBox(form.find("input[type=text][name=tbCode]"), "验证码不正确！");
                    if (data.state == 2) Verify.ShowBox(form.find("input[type=text][name=tbAcc]"), "账号不存在！");
                    if (data.state == 3) Verify.ShowBox(form.find("input[type=password][name=tbPw]"), "登录密码错误！");
                    if (data.state == 4) Verify.ShowBox(form.find("input[type=text][name=tbAcc]"), "该账号已经绑定微信！");
                }
                //绑定成功
                if (Number(data.success) == 1) returl(data.acpw, data.acid);
            });
        }
        //如果需要短信验证
        if (isSms) {
            var sms = $(this).find("input[name=tbSms]").val(); //用户填写的短信验证码
            var smsname = $(this).find("#getSms").attr("smsname");
            ajax.post(window.location.href, { action: "bind2", 
                sex: sex, name: name, photo: photo,
                vcode: vcode, vname: vname, sms: sms, mobi: mobi, smsname: smsname
            }, function (requestdata) {

                var data = eval("(" + requestdata + ")");
                var form = $("#formBind");
                if (Number(data.success) < 1) {
                    //不成功
                    if (data.state == 1) Verify.ShowBox(form.find("input[type=text][name=tbCode]"), "验证码不正确！");
                    if (data.state == 2) Verify.ShowBox(form.find("input[type=text][name=tbAcc]"), "该手机号没有注册！");
                    if (data.state == 3) Verify.ShowBox(form.find("input[type=text][name=tbSms]"), "短信验证码错误！");
                    if (data.state == 4) Verify.ShowBox(form.find("input[type=text][name=tbAcc]"), "该账号已经绑定微信！");
                }
                //注册成功
                if (Number(data.success) == 1) returl(data.acpw, data.acid);
            });
        }
        return false;
    });
}
//发送短信的事件
function sendSmsEvent() {
    $("*[type=getSms]").click(function () {
        if (Number($(this).attr("num")) > 0) return;
        var group = $(this).attr("group");
        if (!Verify.IsPass($(this).parents("form"), group)) return;
        var vcode = $(this).parents("form").find("input[type=text][name$=Code]").val();
        //先验证验证码
        var vname = $(this).parents("form").find("img.verifyCode").attr("src");
        var rs = new RegExp("(^|)name=([^\&]*)(\&|$)", "gi").exec(vname), tmp;
        vname = tmp = rs ? rs[2] : "";
        var phone = $(this).parents("form").find("input[type=text][name$=Acc]").val(); //手机号
        //短信的cookie名称
        var smsname = $(this).attr("smsname");
        var smsbtn = $(this).attr("id"); //点击发送短信的按钮的ID
        $(this).attr("state", "waiting").text("验证中...").css("cursor", "default");
        ajax.post(window.location.href, { action: "getRegSms", vcode: vcode, vname: vname, phone: phone,
            smsname: smsname, smsbtn: smsbtn
        }, function (requestdata) {
            var data = eval("(" + requestdata + ")");
            var state = Number(data.state); //状态值
            var form = $("#" + data.btn).parents("form");
            if (Number(data.success) < 1) {
                //不成功
                if (state == 1) Verify.ShowBox(form.find("input[type=text][name$=Code]"), "验证码不正确！");
                if (state == 2) Verify.ShowBox(form.find("input[type=text][name$=Acc]"), "手机号不存在！");
                if (state == 3) {
                    var txt = "短信发送失败，请与管理员联系。<br/><br/>可能原因：<br/>1、短信接口未开放，或设置不正确。<br/>2、短信账户余额不足。";
                    txt += "<br/><br/>详情：" + data.desc;
                    new MsgBox("发送失败", txt, 400, 250, "alert").Open();
                }
            } else {
                if (state == 0) {
                    $("#getRegSms").attr("state", "waiting").attr("num", 60).css("cursor", "default");
                    $("#getRegSms").attr("send", "true"); //表示已经发送过
                }
            }
            //
        });
    });
}

//短信发送后的等待效果
function _mobiLogin_smsSendWaiting() {
    var obj = $("*[state=waiting]");
    if (obj.size() < 1) return;
    var num = Number(obj.attr("num"));
    var ltxt = obj.attr("waiting");
    if (num > 0) {
        obj.text(ltxt.replace("{num}", num--));
        obj.attr("num", num).addClass("waiting");
    } else {
        obj.text("获取短信").removeClass("waiting");
    }
}