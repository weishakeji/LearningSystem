
$(function () {
    _setFindPwError();
});
function _setFindPwError() {
    var acc = $().getPara("acc");
    if (acc != "") {
        var txt = $("input[name=tbAcc]:visible");
        txt.val(acc);
    }
    //错误信息处理
    var error = $().getPara("error");
    //账号为空
    if (Number(error) == 1) {
        var txt = $("input[name=tbAcc]:visible");
        txt.addClass("errTextbox");
        $("#error-show").text("账号不得为空！");
    }
    //验证码错误
    if (Number(error) == 2) {
        var txt = $("input[name=tbCode]:visible");
        txt.addClass("errTextbox");
        Verify.ShowBox(txt, "验证码错误！");
    }
    //密码错误
    if (Number(error) == 3) {
        var txt = $("input[name=tbPw]:visible");
        txt.addClass("errTextbox");
        Verify.ShowBox(txt, "密码不正确！");
    }
    if (Number(error) == 4) {
        var txt = $("input[name=tbAcc]:visible");
        txt.addClass("errTextbox");
        Verify.ShowBox(txt, "账号不存在！");
    }
    if (Number(error) == 5) {
        var txt = $("input[name=tbAcc]:visible");
        txt.addClass("errTextbox");
        Verify.ShowBox(txt, "账号已经存在！");
    }
    if (Number(error) == 6) {
        var txt = $("input[name=tbAnswer]:visible");
        txt.addClass("errTextbox");
        $("#error-show").text("安全问题的答案不正确！");
    }
}


