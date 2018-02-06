$(function () {
    _ClacInfo();
    _ClacTax();
    _ClacResult();
    setEvent();
});
//设置按钮事件
function setEvent() {
    //清除此次成绩
    $(".btnDelResult").click(function () {
        var txt = "是否删除此次考试成绩？";
        var msg = new MsgBox("确认", txt, 90, 40, "confirm");
        msg.EnterEvent = function () {
            window.location.href = $(".btnDelResult").attr("href");
        };
        msg.Open();
        return false
    });
    //显示所有
    $("#btnAll").append("(" + $(".qitemBox").size() + ")");
    $("#btnAll").click(function () {
        $(".qitemBox").show();
        _setEventLoyout();
    });
    //只显示错误
    $("#btnError").append("(" + $(".qitemBox[ansState=1]").size() + ")");
    $("#btnError").click(function () {
        $(".qitemBox").hide();
        $(".qitemBox[ansState=1]").show();
        _setEventLoyout();
    });
    //只显示正确
    $("#btnSuccess").append("(" + $(".qitemBox[ansState=0]").size() + ")");
    $("#btnSuccess").click(function () {
        $(".qitemBox").hide();
        $(".qitemBox[ansState=0]").show();
        _setEventLoyout();
    });
    //只显示未做的试题
    $("#btnNoans").append("(" + $(".qitemBox[ansState=2]").size() + ")");
    $("#btnNoans").click(function () {
        $(".qitemBox").hide();
        $(".qitemBox[ansState=2]").show();
        _setEventLoyout();
    });
}
function _setEventLoyout() {
    $(".typeBox").each(function () {
        $(this).show();
        if ($(this).find(".qitemBox:visible").size() < 1) {
            $(this).hide();
        } else {
            $(this).show();
        }
    });
}
//计算得分
function _ClacInfo() {
    $("tr.sum td").each(function (index) {
        if (index < 1) return;
        var sum = 0;
        $("tr.itemrow").each(function () {
            var td = $(this).find("td").get(index);
            var num = Number(td.innerText);
            sum += num;
        });
        $(this).html(sum);
    });
}

//计算序号
function _ClacTax() {
    //大题的序号，数字转汉字
    var hanzi = ["一", "二", "三", "四", "五", "六", "七", "八", "九", "十"];
    $(".typeTax").each(function (index) {
        $(this).text(hanzi[index]);
    });
    //试题的序号
    $(".qTax").each(function (index) {
        $(this).text(index + 1);
    });
    $(".qitemBox").each(function (index) {
        $(this).find(".order").each(function (index) {
            $(this).text(String.fromCharCode(65 + index));
        });
    });
}
//计算学员答题情况，例如单选题的选项
function _ClacResult() {
    $(".qitemBox").each(function () {
        var type = parseInt($(this).attr("type"));
        if (type == 1 || type == 2) {
            var result = $(this).find(".resultItem").text().split("、");
            var word = "";
            for (var r in result) {
                if (result[r] == "") continue;
                var tm = $(this).find(".quesSelectBox div[anid=" + result[r] + "]");
                word += tm.find(".order").text() + "、";
            }
            if (word.indexOf("、") > -1 && word.substring(word.length - 1) == "、")
                word = word.substring(0, word.length - 1);
            $(this).find(".resultItem").html(word);
        }
        if (type == 3) {
            var result = $.trim($(this).find(".resultItem").text());
            var txt = result == "0" ? "正确" : "错误";
            $(this).find(".resultItem").html(txt);
        }
    });
}
