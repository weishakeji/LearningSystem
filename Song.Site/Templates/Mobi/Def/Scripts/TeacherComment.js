$(function () {
    Star.Init();
    setSumbitEvent();
});

/*星标相关*/
function Star() { };
//初始化
Star.Init = function () {
    //设置星标个数与宽度
    var star = $("span.star");
    var num = Math.floor(star.parent().width() / 28);
    star.attr("size", num > 10 ? 10 : num);
    star.parent().width(num * 28).css({ "margin-right": "auto", "margin-left": "auto" });
    Star.Set();		//设置星星个数
    Star.Event();	//设置星星的事件
}
//设置星标
Star.Set = function (value) {
    var star = $("span.star");
    if (value != null) {
        star.attr("score", value);
        $("#value").text(value + "分").removeClass("show");
    }
    star.html("");
    //生成星标列
    star.each(function (index, element) {
        //显示多少个星标
        var size = Number($(this).attr("size"));
        if (isNaN(size)) size = 5;
        //当前得分
        var score = Number($(this).attr("score"));
        if (isNaN(score)) return true;
        score = score / (10 / size);
        //生成空的星标
        while (size-- > 0) $(this).append("<i></i>");
        //生成实的星标
        var tm = Math.floor(score);
        $(this).find("i:lt(" + tm + ")").addClass("s1");
        //生成半实的星标
        if (score > tm) $(this).find("i:eq(" + tm + ")").addClass("s0");
    });
}
//评分，即星标的点击事件
Star.Event = function () {
    $("span.star").parent().click(function (e) {
        var coordinate = $().Mouse(e); //鼠标点击的坐标值
        var offset = $(this).offset();
        var x = coordinate.x - offset.left; 	//鼠标相对于星标列的坐标
        //设置星标选中的个数
        var value = Math.ceil(x / $(this).width() * 10);
        Star.Set(value);
    });
}

//提交按钮的事件
function setSumbitEvent() {
    $("#btnSubmit").click(function () {
        //验证是否评分
        var num = Number($("#value").text().replace("分", ""));
        if (isNaN(num) || num < 1) {
            $("#alert").addClass("alert").text("请对该教师进行评分！");
            return false;
        }
        //验证评价
        var txt = $.trim($("#comment").val());
        if (txt.length < 1) {
            $("#alert").addClass("alert").text("评价不可为空！");
            return false;
        }
        if (txt.length < 10) {
            $("#alert").addClass("alert").text("评价不可少于10个字！");
            return false;
        }
        if (txt.length > 200) {
            $("#alert").addClass("alert").text("评价不可大于200字！");
            return false;
        }
        //验证校验码
        var vcode = $.trim($("#tbCode").val());
        if (vcode.length != 4 || !(Number(vcode) || vcode == "0")) {
            $("#alert").addClass("alert").text("请输入四位数字！");
            return false;
        }
        $("#alert").removeClass("alert").text("");
        //先验证验证码
        var vname = $("img.verifyCode").attr("src");
        var rs = new RegExp("(^|)name=([^\&]*)(\&|$)", "gi").exec(vname), tmp;
        vname = tmp = rs ? rs[2] : "";
        var url = window.location.href;
		mui("#btnSubmit").button('loading');
        $.post(url, { action: "vcode", vcode: $("input[name=tbCode]").val(), vname: vname }, function (requestdata) {
            var data = eval("(" + requestdata + ")");
            if (Number(data.success) < 1) {
                $("#alert").addClass("alert").text("验证码不正确！");
				mui("#btnSubmit").button('reset');
            } else {
				mui("#btnSubmit").button('loading');
                $.post(url, { action: "submit", num: num, txt: escape(txt) }, function (requestdata) {
                    var data = eval("(" + requestdata + ")");
                    if (Number(data.success) < 1) {
                        $("#alert").addClass("alert").text(data.msg);
                    } else {
                        $("#alert").removeClass("alert").text("");
						mui("#btnSubmit").button('reset');
                        mui.alert("成功递交评价！", "成功", "关闭", function () {
                            parent.location.reload();
                            parent.PageBox.Close();
                        });
                    }
                });
            }
        });
    });
}
