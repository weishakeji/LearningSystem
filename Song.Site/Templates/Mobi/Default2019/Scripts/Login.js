$(function () {
    OtherLogin.init(); //第三方登录
    //返回按钮
    mui('body').on('tap', '#back', function () {
        if (this.href == "") {
            window.history.go(-1);
        } else {
            document.location.href = this.href;
        }
    });
    //注册
    $("#btnRegister").click(function () {
        document.location.href = $(this).attr("href");
    });
    //是否自动登录的切换
    try {
        document.getElementById("cbSign-switch").addEventListener("toggle", function (event) {
            if (event.detail.isActive) {
                $("#cbSign").attr("checked", "checked");
                //console.log("你启动了开关");
            } else {
                $("#cbSign").removeAttr("checked", "checked");
                //console.log("你关闭了开关");  
            }
        });
    } catch (e) {

    }
});
$(function () {
    Event_init();
    //当提交时，按钮进入预载状态
    setInterval(function () {
        var obj = $("*[disabled=disabled]");
        if (obj.size() < 1) return;
        var vtxt = obj.val();
        var ltxt = obj.attr("loading-txt");
        if (vtxt.indexOf(".") < 0) {
            obj.val(ltxt + ".");
        } else {
            var tm = vtxt.substring(vtxt.indexOf("."));
            obj.val(tm.length < 3 ? vtxt + "." : ltxt);
        }
    }, 200);
    //当短信发送后，倒计时数秒
    setInterval(_mobiLogin_smsSendWaiting, 1000);
});


/*
*
*  登录相关事件与方法
*
*/
//事件初始始化
function Event_init() {
    //登录事件
    $("form").submit(function () {
        try {
            var name = $(this).attr("name");
            var func = eval("Event_" + name);
            var btn = $(this).find("input[type=submit][name=btnSubmit]");
            btn.attr("loading-txt", "正在登录").attr("disabled", "disabled").addClass("disabled");
            //提交的地址
            var url = window.location.href;
            url = url.indexOf("?") > -1 ? url.substring(0, url.lastIndexOf("?")) : url;
            func($(this), url);
        } catch (e) {
            alert("error:" + e.message);
        }
        return false;
    });
    //获取验证短信
    mui('body').on('tap', '#getSms', _mobiLogin_smsSend);
}
//账号登录
function Event_accLogin(form, url) {
    //先验证验证码
    var vname = form.find("img.verifyCode").attr("src");
    var rs = new RegExp("(^|)name=([^\&]*)(\&|$)", "gi").exec(vname), tmp;
    vname = tmp = rs ? rs[2] : "";
    $.post(url, { action: "accloginvcode", vcode: form.find("input[name=tbCode]").val(), vname: vname }, function (requestdata) {
        var data = eval("(" + requestdata + ")");
        if (Number(data.success) < 1) {
            Verify.ShowBox(form.find("input[name=tbCode]"), "验证码不正确！");
            var btn = form.find("input[type=submit][name=btnSubmit]");
            btn.val("登录").removeAttr("disabled", "disabled").removeClass("disabled");
        } else {
            //提交验证账号与密码
            var acc = form.find("input[name=tbAcc]").val();
            var pw = form.find("input[name=tbPw]").val();
            var sign = form.find("input[name=cbSign]").is(':checked'); 	//是否免登录
            $.post(url, { action: "acclogin", acc: acc, pw: pw, sign: sign }, function (requestdata) {
                var data = eval("(" + requestdata + ")");
                if (Number(data.success) < 1) {
                    Verify.ShowBox(form.find("input[name=tbPw]"), "密码不正确！");
                    form.find("img.verifyCode").click();
                    var btn = form.find("input[type=submit][name=btnSubmit]");
                    btn.val("登录").removeAttr("disabled", "disabled").removeClass("disabled");
                } else {
                    if (Boolean(data.sign)) {
                        $.storage("accid", data.acid);
                        $.storage("accpw", data.pw);
                    }
                    var txt = "亲爱的 <b>" + data.name + "</b>，您已经成功登录。<br/><br/>将在<second>5</second>秒后将返回来源页。";
                    var msg = new MsgBox("登录成功", txt, 80, 40, "msg");
                    msg.ShowCloseBtn = false;
                    msg.OverEvent = function () {
                        var href = form.find("input[name=from]").val();
                        window.location.href = $().setPara(href, "sharekeyid", data.acid);
                    };
                    msg.Open();
                }
            });
        }
    });
    return false;
}


//手机验证登录
function Event_mobiLogin(form, url) {
    //登录验证
    _mobiLogin_veri(form, url);
}
//短信发送
function _mobiLogin_smsSend() {
    if (Number($(this).attr("num")) > 0) return;
    var group = $(this).attr("group");
    if (!Verify.IsPass($(this).parents("form"), group)) return;
    var vcode = $("form[name=mobiLogin]").find("input[type=number][name=tbCode]").val();
    //先验证验证码
    var vname = $("form[name=mobiLogin] img.verifyCode").attr("src");
    var rs = new RegExp("(^|)name=([^\&]*)(\&|$)", "gi").exec(vname), tmp;
    vname = tmp = rs ? rs[2] : "";
    var phone = $("form[name=mobiLogin] input[name=tbPhone]").val(); //手机号
    //
    $("#getSms").attr("state", "waiting").text("验证中...").css("cursor", "default");
    $.post(window.location.href, { action: "getSms", vcode: vcode, vname: vname, phone: phone }, function (requestdata) {
        var data = eval("(" + requestdata + ")");
        var state = Number(data.state); //状态值
        if (Number(data.success) < 1) {
            //不成功
            if (state == 1) Verify.ShowBox($("form[name=mobiLogin]").find("input[name=tbCode]"), "验证码不正确！");
            if (state == 2) Verify.ShowBox($("form[name=mobiLogin]").find("input[name=tbPhone]"), "手机号不存在！");
            if (state == 3) {
                var txt = "短信发送失败，请与管理员联系。<br/><br/>可能原因：<br/>1、短信接口未开放，或设置不正确。<br/>2、短信账户余额不足。";
                txt += "<br/><br/>详情：" + data.desc;
                new MsgBox("发送失败", txt, 80, 60, "alert").Open();
            }
        } else {
            if (state == 0) {
                $("#getSms").attr("state", "waiting").attr("num", 60).css("cursor", "default");
                $("#getSms").attr("send", "true"); //表示已经发送过
            }
        }
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
        obj.attr("num", num);
    } else {
        obj.text("获取短信").attr("state", "");
    }
}
//手机号登录的验证
function _mobiLogin_veri(form, url) {
    //先验证验证码
    var vname = form.find("img.verifyCode").attr("src");
    var rs = new RegExp("(^|)name=([^\&]*)(\&|$)", "gi").exec(vname), tmp;
    vname = tmp = rs ? rs[2] : "";
    var phone = form.find("input[name=tbPhone]").val(); //手机号
    var sms = form.find("input[name=tbSms]").val(); //用户填写的短信验证码
    var sign = form.find("input[name=cbSign]").is(':checked'); 	//是否免登录
    $.post(url, { action: "mobilogin",
        vcode: form.find("input[name=tbCode]").val(),
        vname: vname,
        phone: phone,
        sms: sms,
        sign: sign
    },
	function (requestdata) {
	    var data = eval("(" + requestdata + ")");
	    var state = Number(data.state); //状态值
	    if (Number(data.success) < 1) {
	        if (state == 1) Verify.ShowBox($("form[name=mobiLogin]").find("input[name=tbCode]"), "验证码不正确！");
	        if (state == 3) Verify.ShowBox($("form[name=mobiLogin]").find("input[name=tbSms]"), "短信验证失败！");
	        form.find("img.verifyCode").click();
	        var btn = form.find("input[type=submit][name=btnSubmit]");
	        btn.val("登录").removeAttr("disabled", "disabled").removeClass("disabled");

	    } else {
	        if (Boolean(data.sign)) {
	            $.storage("accid", data.acid);
	            $.storage("accpw", data.pw);
	        }
	        var txt = "亲爱的 <b>" + data.name + "</b>，您已经成功登录。<br/><br/>将在<second>5</second>秒后将返回来源页。";
	        var msg = new MsgBox("登录成功", txt, 400, 200, "msg");
	        msg.ShowCloseBtn = false;
	        msg.ShowCloseBtn = false;
	        msg.OverEvent = function () {
	            var href = form.find("input[name=from]").val();
	            window.location.href = $().setPara("default.ashx", "sharekeyid", data.acid);
	        };
	        msg.Open();
	    }
	});
}
//格式化字符串
String.prototype.format = function (args) {
    var result = this;
    for (var i = 0; i < arguments.length; i++) {
        result = result.replace(eval('/\\{' + i + '\\}/g'), arguments[i]);
    }
    return result;
}
/*第三方登录*/
function OtherLogin() { }
OtherLogin.init = function () {
    //qq登录
    mui('body').on('tap', 'a[tag=qqlogin]', function () {
        var appid = $(this).attr("appid"); //appid
        var returl = OtherLogin.prefix($(this).attr("returl")) + "/qqlogin.ashx"; //回调域
        var orgid = $(this).attr("orgid"); 	//当前机构id
        var target = "https://graph.qq.com/oauth2.0/authorize?";
        target += "client_id=" + appid + "&response_type=code&scope=all&redirect_uri=" + returl + "&state=" + orgid;
        //window.location.href = target;
        new PageBox("QQ登录", target, 100, 100, "url").Open();
    });
    //微信登录
    mui('body').on('tap', 'a[tag=weixinlogin]', function () {
        //判断是否处于微信中
        var ua = window.navigator.userAgent.toLowerCase();
        if (ua.match(/MicroMessenger/i) != 'micromessenger') {
            var txt = "此功能仅在微信中使用<br/><second>5</second>秒后关闭";
            var msg = new MsgBox("无法使用", txt, 60, 200, "alert");
            msg.Open();
        } else {
            var appid = $(this).attr("appid"); //appid
            var returl = OtherLogin.prefix($(this).attr("returl")) + "/mobile/weixinpublogin.ashx"; //回调域	
            var orgid = $(this).attr("orgid"); 	//当前机构id
            var target = "https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_userinfo&state={2}#wechat_redirect";
            target = target.format(appid, encodeURIComponent(returl), orgid);
            window.location.href = target;
        }
    });
}
//获取前缀，主要用来判断是http还是https
OtherLogin.prefix = function (returl) {
    var arr = "http://|https://".split("|");
    var ispass = false;
    for (var t in arr) {
        if (arr[t].length > returl.length) continue;
        if (returl.indexOf(arr[t]) == 0) {
            ispass = true;
            break;
        }
    }
    return !ispass ? "http://" + returl : returl;
}