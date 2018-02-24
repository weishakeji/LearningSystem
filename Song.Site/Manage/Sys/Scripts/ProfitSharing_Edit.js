$(function () {
    //表格底部显示合计
    $(".moneySum").text(calcMoney(true));
    $(".couponSum").text(calcCoupon(true));    
    //行内的编辑按钮
    $("input[name$=btnEdit]").click(function () {
        var sum = calcMoney();
        //alert(sum);
    });
});

//计算资金比例的总计
function calcMoney(isAll) {
    var ratio = 0;
    $(".moneyratio").each(function () {
        var val = Number($(this).text());
        ratio += val;
    });
    //取所有，例如当gridview处理编辑状态时，录入行的值
    if (isAll) {
        var edit = $("input[name$=tbMoneyEdit]");
        if (edit.size() > 0) {
            var limit = 100 - ratio;
            edit.attr("numlimit","0-"+limit);
            ratio += Number(edit.val());
        }
    }
    //设置新增的录入限制
    $("input[name$=tbMoneyAdd]").attr("numlimit", "0-" + (100 - ratio));
    return ratio;
}
//计算卡券比例的总计
function calcCoupon(isAll) {
    var ratio = 0;
    $(".couponratio").each(function () {
        var val = Number($(this).text());
        ratio += val;
    });
    //取所有，例如当gridview处理编辑状态时，录入行的值
    if (isAll) {
        var edit = $("input[name$=tbCouponEdit]");
        if (edit.size() > 0) {
            var limit = 100 - ratio;
            edit.attr("numlimit", "0-" + limit);
            ratio += Number(edit.val());
        }
    }
    //设置录入限制
    $("input[name$=tbCouponAdd]").attr("numlimit", "0-" + (100 - ratio));
    return ratio;
}