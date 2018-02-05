$(function () {
    _clacNumber();
    //提示信息
    $("input[name$=btnEnter]").click(function () {
        if (!_veriCount()) {
            alert("请填写试题数量，即该有多少道题。");
            return false;
        }
        if (!_veriPer()) {
            alert("各题型所占总分的比例，合计必须等于100%。");
            return false;
        }
        var isNull = false;
        $("input[name$=Count]").each(function () {
            var num = $(this).val() == null ? 0 : Number($(this).val());
            var p = $(this).parent().find("input[name$=Score]");
            var per = p.val() == null ? 0 : Number(p.val());
            if ((num != 0) ^ (per != 0)) {
                isNull = true;
                var tit = $.trim($(this).parent().prev().text());
                alert(tit + "题数如果为空，则不要设置百分比，反之亦然。");
                return false;
            }
        });
        if (isNull) return false;
        _clacNumber();
    });
    $("input[name$=Score]").change(function () { _clacNumber(); });
    $("input[name$=tbTotal]").change(function () {
        var num = Number($(this).val());
        num = Math.floor(num * 0.6);
        $("input[name$=tbPassScore_]").val(num);
        _clacNumber();
    });
});

//计算验证：是否设置有题
function _veriCount() {
    //选中了多道题
    var count = 0;
    $("input[name$=Count]").each(function () {
        var num = $(this).val() == null ? 0 : Number($(this).val());
        count += num;
    });
    return count > 0;
}
//计算验证：各题型百分比是否正确
function _veriPer() {
    //选中了多道题
    var count = 0;
    $("input[name$=Score]").each(function () {
        var num = $(this).val() == null ? 0 : Number($(this).val());
        count += num;
    });
    return count == 100;
}
//计算验证：未设置题数，则不要设置比例
function _veriNull() {
    //选中了多道题
    var count = 0;
    $("input[name$=Count]").each(function () {
        var num = $(this).val() == null ? 0 : Number($(this).val());
        if (num == 0) {
            var tit = $(this).parent().prev().text();
            alert(tit);
        }
    });
    return count == 100;
}
//计算各分数
function _clacNumber() {
    //总分
    var total = Number($("input[name$=tbTotal]").val());
    //各类题型的百分比输入框
    var score = new Array();
    $("input[name$=Score]").each(function () {
        if ($.trim($(this).val()).length > 0) {
            score.push($(this));
        }
        $(this).parent().find("input[name$=Number]").val("");
        $(this).parent().find("span[id$=Number]").text("0");
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
        th.parent().find("span[id$=Number]").text(num);
    }
}