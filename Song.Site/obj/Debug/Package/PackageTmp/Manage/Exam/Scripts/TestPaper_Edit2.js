$(function () {
    //提交事件
    $("form").submit(function () {
        try {
            var func = eval("type" + fromtype() + "_verify");
            return func();
        } catch (e) { }
        return true;
    });
    //当题量填写变动时
    $(".ItemForAll input[name$=tbItemCount]").change(function () { type0_clacNumber(); });
    //当占分比填写变动时
    $(".ItemForAll input[name$=tbItemScore]").change(function () {
        var num = Number($(this).val());
        num = Math.floor(num * 0.6);
        $("input[name$=tbPassScore_]").val(num);
        type0_clacNumber();
    });
    //当更改试题选择范围时
    $("input[type=radio][name$=rblFromType]").click(function () {
        var val = $(this).val();
        $(".ItemForAll").css("display", val == "0" ? "block" : "none");
        $(".ItemForOutline").css("display", val == "1" ? "block" : "none");
    });
    var type = $("input[name$=rblFromType]:checked").val();
    $(".ItemForAll").css("display", type == "0" ? "block" : "none");
    $(".ItemForOutline").css("display", type == "1" ? "block" : "none");
});
//试题选择范围
//return:返回0为当前课程所有试题，返回1为按章节取试题
function fromtype() {
    var type = $("input[name$=rblFromType]:checked");  
    return Number(type.val());
}

/* 
选择当前课程所有试题
*/
function type0_verify() {
    if (!type0_veriCount()) {
        alert("请填写试题数量，即该项取多少道题。");
        $(".ItemForAll input[name$=tbItemCount]:first").focus();
        return false;
    }
    var per = type0_veriPer();
    if (per!=100) {
        alert("各题型所占总分的比例，合计必须等于100%,当前是"+per+"%");
        $(".ItemForAll input[name$=tbItemScore]:first").focus();
        return false;
    }
    var isNull = false;
    $(".ItemForAll input[name$=tbItemCount]").each(function () {
        var num = $(this).val() == null ? 0 : Number($(this).val());
        var p = $(this).parent().find("input[name$=tbItemScore]");
        var per = p.val() == null ? 0 : Number(p.val());
        if ((num != 0) ^ (per != 0)) {
            isNull = true;
            var tit = $.trim($(this).parent().prev().text());
            alert(tit + "题数与所占百分比，必须同时填写。");
            return false;
        }
    });
    if (isNull) return false;
    type0_clacNumber();
    return true;
}
//验证是否填写了试题数量
function type0_veriCount() {
    //选中了多道题
    var count = 0;
    $(".ItemForAll input[name$=tbItemCount]").each(function () {
        var num = $(this).val() == null ? 0 : Number($(this).val());
        count += num;
    });
    return count > 0;
}

//计算验证：各题型百分比是否正确
function type0_veriPer() {
    //选中了多道题
    var count = 0;
    $(".ItemForAll input[name$=tbItemScore]").each(function () {
        var num = $(this).val() == null ? 0 : Number($(this).val());
        count += num;
    });
    return count;
}
//计算各项所占实际分数
function type0_clacNumber() {
    //总分
    var total = Number($("input[name$=tbTotal]").val());
    //各类题型的百分比输入框
    var score = new Array();
    $(".ItemForAll input[name$=tbItemScore]").each(function () {
        if ($.trim($(this).val()).length > 0) {
            score.push($(this));
        }
        $(this).parent().find("input[name$=Number]").val("");
        $(this).parent().find("span[id*=Number]").text("0");
    });
    var tm = total;
    for (var i = 0; i < score.length; i++) {
        var th = score[i];
        var num = 0;
        if ((i + 1) != score.length) {
            var per = th.val() == null ? 0 : Number(th.val());
            num = parseInt(total * per / 100);
            tm = tm - num;
        } else {
            num = tm;
        }
        th.parent().find("input[name$=Number]").val(num);
        th.parent().find("span[id*=Number]").text(num);
    }
}


/*
按章节选取试题 
*/
function type1_verify() {
    var per = type1_veriPer();
    if (per!= 100) {
        alert("各题型所占总分的比例，合计必须等于100%，当前是"+per+"%");
        $(".ItemForOutline input[name$=tbQuesScore]:first").focus();
        return false;
    }
}
//计算验证：各题型百分比是否正确
function type1_veriPer() {
    //选中了多道题
    var count = 0;
    $(".ItemForOutline input[name$=tbQuesScore]").each(function () {
        var num = $(this).val() == null ? 0 : Number($(this).val());
        count += num;
    });
    return count;
}
