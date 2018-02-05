// JavaScript Document
$(function () {
    var veri = new Verify();
    veri.Init();
    veri.SubmitEvent();
});

/*!
* 主  题：《页面录入验证处理》
* 说  明：所有页面录入验证的处理，包括生成星号标注、非法提示等；
* 功能描述：
* 1、当录入框必填时，在控件后生成红色星号；
* 2、根据控件属性判断需要录入的数据格式，如果非法，在控件下方显示下拉提示，并阻止提交，即form的submit事件
* 3、支持分组验证
*
* 作  者：宋雷鸣 
* 开发时间: 2011年8月5日
*/
function Verify() {
}
//验证界面初始化，如果非空选择，加星号标
Verify.prototype.Init = function () {
    var star = "<span style=\"color:red\">*</span>";
    var tbox = $("input[nullable='false']");
    //tbox.after(star);
    var tbox = $("textarea[nullable='false'][star!='false']");
    tbox.after(star);
    var tbox = $("select[novalue='-1']");
    tbox.after(star);
}
//提交按钮的事件
Verify.prototype.SubmitEvent = function () {
    var btn = $("input[type='submit'][verify='true']");
    btn.click(function () {
        //按钮的验证组，识标码
        var group = $(this).attr("group");
        //所在的form
        var form = $(this).parents("form");
        var isPass = true;
        form.find("*:visible").each(function () {
            isPass = Verify.OperateSingle(group, $(this));
            if (!isPass) return false;
        });
        //如果全部通过，则清除提示框
        if (isPass) $("#VerifyShowBox").remove();
        return isPass;
    }
	);
    //当鼠标点击时
    $("body").mousedown(function () {
        $("#VerifyShowBox").remove();
    });
    //当录入时清除提示框 
    $("body").keydown(function () {
        $("#VerifyShowBox").remove();
    });
}
//以下为静态方法
//验证所有录入控件
Verify.OperateSingle = function (group, control) {
    var isPass = true;
    var name = control.get(0).tagName.toLowerCase();
    if (name == "input" || name == "select" || name == "textarea") {
        //输入框的验证组的识标码
        var tbgroup = control.attr("group");
        //提交按钮的分组码存在，或与输入框分组码不同，则跳过
        if (group == null || group == "" || group == tbgroup) {
            return Verify.IsPass(control);
        }
    }
    return isPass;
}
Verify.IsPass = function (control) {
    //只要一个条件为假，则中断
    if (!Verify.IsNull(control)) return false;
    if (!Verify.Lenth(control)) return false;
    if (!Verify.IsSame(control)) return false;
    if (!Verify.Novalue(control)) return false;
    if (!Verify.DataType(control)) return false;
    if (!Verify.FileLimit(control)) return false;
    if (!Verify.FileAllow(control)) return false;
    if (!Verify.NumberLimit(control)) return false;
    return true;
}
//显示提示
Verify.ShowBox = function (control, showTxt) {
    var offset = control.offset();
    var box = $("#VerifyShowBox");
    if (box.size() < 1) {
        var html = "<div id=\"VerifyShowBox\" class=\"VerifyShowBox\" style=\"display:none\">";
        html += "<div class=\"arrow-up\"> </div>";
        html += "<div class=\"errorShow\"> </div>";
        html += "</div>";
        $("body").append(html);
        box = $("#VerifyShowBox");
    }
    box.css({ "position": "absolute", "z-index": 3000,
        "height": 25, "width": control.width() < 60 ? 60 : control.width() + 2,
        "top": offset.top + control.height() + 5,
        "left": offset.left
    });
    box.find(".arrow-up").css({ "width": "0px", "height": "0px",
        "border-left": "5px solid transparent",
        "border-right": "5px solid transparent",
        "border-bottom": "5px solid #FF3300",
        "font-size": "0px",
        "line-height": "0px",
        "margin-left": "10px"
    });
    box.find(".errorShow").html(showTxt);
    box.find(".errorShow").css({ "border": "1px solid ##FF3300",
        "font-size": "13px", "line-height": "25px",
        "text-indent": "10px", "height": "25px",
        "background-color": "#FF3300",
        "color": "#FFFFFF"
    });
    var alpha = 90;
    box.css("filter", "Alpha(Opacity=" + alpha + ")");
    box.css("display", "block");
    box.css("-moz-opacity", alpha / 100);
    box.css("opacity", alpha / 100);
    box.slideDown("slow");
    control.focus();
    return false;
}
//获取当前输入控件的录入值
Verify.GetValue = function (control) {
    var val = "";
    if (control.size() < 1) return "";
    var name = control.get(0).tagName.toLowerCase();
    var type = control.attr("type").toLowerCase();
    if (name == "input") val = control.val();
    if (name == "select") val = control.val();
    if (name == "textarea") val = control.val();
    return $.trim(val);
}

/*
**********
下面是验证用的具体方法
**********
*/
//验证是否为空
Verify.IsNull = function (control) {
    var nullable = control.attr("nullable");
    if (typeof (limit) == "nullable") return true;
    //取录入的值
    var val = Verify.GetValue(control);
    return nullable == "false" && (val == "" || val == control.attr("deftxt")) ? Verify.ShowBox(control, "不得为空！") : true;
}
//验证录入的长度区间
Verify.Lenth = function (control) {
    var limit = control.attr("lenlimit");
    if (typeof (limit) == "undefined") return true;
    //取录入的值长度
    var len = Verify.GetValue(control).length;
    if (limit.indexOf("-") < 0) {
        if (len > Number(limit)) return Verify.ShowBox(control, " 超出 " + (len - limit) + " 个字符");
    } else {
        var minlen = Number(limit.substring(0, limit.indexOf("-")));
        var maxlen = Number(limit.substring(limit.indexOf("-") + 1));
        if (minlen == maxlen && len != minlen)
            return Verify.ShowBox(control, " 限制 " + minlen + " 个字符");
        if (!(len >= minlen && len <= maxlen))
            return Verify.ShowBox(control, " 请录入 " + minlen + "-" + maxlen + " 个字符");
    }
    return true;
}
//验证录入的长度区间
Verify.NumberLimit = function (control) {
    var isPass = true;
    //验证属性
    var limit = control.attr("numlimit");
    if (limit == null) return isPass;
    //取录入的值
    var val = Verify.GetValue(control);
    var num = Number(val);
    if (num == null) {
        Verify.ShowBox(control, " 请输入整数数字！");
        isPass = false;
        control.focus();
    }
    var maxlen = 0;
    var minlen = 0;
    if (limit.indexOf("-") < 0) {
        maxlen = Number(limit);
    } else {
        minlen = Number(limit.substring(0, limit.indexOf("-")));
        maxlen = Number(limit.substring(limit.indexOf("-") + 1));
    }
    if (!(num >= minlen && num <= maxlen)) {
        Verify.ShowBox(control, " 请录入大于等于 " + minlen + "，小于等于" + maxlen + " 的数字。");
        isPass = false;
        control.focus();
    }
    return isPass;
}
//验证输入是否相同
Verify.IsSame = function (control) {
    var isPass = true;
    //验证属性
    var sametarget = control.attr("sametarget");
    if (sametarget == null) return isPass;
    //取录入的值
    var val = Verify.GetValue(control);
    if (sametarget != null) {
        var same = $("input[name$='" + sametarget + "']:visible");
        if (val != Verify.GetValue(same)) {
            Verify.ShowBox(control, " 两次输入不相同！");
            isPass = false;
            control.focus();
        }
    }
    return isPass;
}
//验证不允许等于某值
Verify.Novalue = function (control) {
    var isPass = true;
    //验证属性
    var novalue = control.attr("novalue");
    if (novalue == null) return isPass;
    //取录入的值
    var val = Verify.GetValue(control);
    if (novalue != null) {
        if (val == novalue) {
            Verify.ShowBox(control, " 请选择具体项！");
            isPass = false;
            control.focus();
        }
    }
    return isPass;
}
//验证数据类型，验证是否为数字、帐号、email、邮编、手机号
Verify.DataType = function (control) {
    var isPass = true;
    var datatype = control.attr("datatype");
    if (datatype == null) return isPass;
    //取录入的值
    var val = Verify.GetValue(control);
    if (val == "") return isPass;
    //验证数字
    if (datatype == "uint") {
        var patrn = /^[0-9]{1,20}$/;
        if (!patrn.exec(val)) {
            Verify.ShowBox(control, " 请输入正整数数字！");
            isPass = false;
            control.focus();
        }
    }
    //验证数字
    if (datatype == "number") {
        if (!(Number(val) || val == "0")) {
            Verify.ShowBox(control, " 请输入数字！");
            isPass = false;
            control.focus();
        }
    }
    //验证电话
    if (datatype == "tel") {
        var patrn = /^[+]{0,1}(\d){1,3}[ ]?([-]?((\d)|[ ]){1,12})+$/;
        if (!patrn.exec(val)) {
            Verify.ShowBox(control, " 请输入正确的电话号码！");
            isPass = false;
            control.focus();
        }
    }
    //验证手机号
    if (datatype == "mobile") {
        var patrn = /^[+]{0,1}(\d){1,1}[ ]?([-]?((\d)|[ ]){10,10})+$/;
        if (!patrn.exec(val)) {
            Verify.ShowBox(control, " 请输入正确的移动电话号码！");
            isPass = false;
            control.focus();
        }
    }
    //邮政编码 
    if (datatype == "zip") {
        var patrn = /^[0-9 ]{6,6}$/;
        if (!patrn.exec(val)) {
            Verify.ShowBox(control, " 请输入正确的邮政编码！");
            isPass = false;
            control.focus();
        }
    }
    //用户帐号
    if (datatype == "user") {
        var patrn = /^[a-zA-Z]{1}([a-zA-Z0-9]|[._]){1,20}$/
        if (!patrn.exec(val)) {
            Verify.ShowBox(control, " 请用字符或数字且字符开头！");
            isPass = false;
            control.focus();
        }
    }
    //电子信箱
    if (datatype == "email") {
        var patrn = /^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$/
        if (!patrn.exec(val)) {
            Verify.ShowBox(control, " 请输入正确的电子信箱！");
            isPass = false;
            control.focus();
        }
    }
    //身份证
    if (datatype == "idcard") {
        var patrn = /(^\d{15}$)|(^\d{18}$)|(^\d{17}(\d|X|x)$)/
        if (!patrn.exec(val)) {
            Verify.ShowBox(control, " 请输入正确的身份证信息！");
            isPass = false;
            control.focus();
        }
    }
    //if (!isPass) control.val("");
    return isPass;
}
//上传文件限制，允许的文件类弄型
Verify.FileAllow = function (control) {
    var isPass = false;
    var filelimit = control.attr("fileallow");
    if (filelimit == null) return true;
    //当前值
    var val = Verify.GetValue(control);
    if (val == "") return true;
    if (val.indexOf(".") > -1) {
        val = val.substring(val.lastIndexOf(".") + 1);
    }
    val = val.toLowerCase();
    var arr = filelimit.split("|");
    for (var s in arr) {
        if (arr[s] == val) {
            isPass = true;
            break;
        }
    }
    if (!isPass) {
        Verify.ShowBox(control, " 上传文件仅限 " + filelimit.replace(/\|/g, "、") + " 格式！");
        control.focus();
    }
    return isPass;
}
//上传文件限制，不允许的文件类型
Verify.FileLimit = function (control) {
    var isPass = true;
    var filelimit = control.attr("filelimit");
    if (filelimit == null) return isPass;
    //当前值
    var val = Verify.GetValue(control);
    if (val == "") return isPass;
    if (val.indexOf(".") > -1) {
        val = val.substring(val.lastIndexOf(".") + 1);
    }
    val = val.toLowerCase();
    var arr = filelimit.split("|");
    for (var s in arr) {
        if (arr[s] == val) {
            isPass = false;
            break;
        }
    }
    if (!isPass) {
        Verify.ShowBox(control, " 不许上传 " + filelimit.replace(/\|/g, "、") + " 格式的文件！");
        control.focus();
    }
    return isPass;
}