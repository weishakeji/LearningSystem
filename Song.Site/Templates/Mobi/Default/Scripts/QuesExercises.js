
window.onload = function () {
    //总题数
    var count = Number($("body").attr("quscount"));
    //设置试题宽度
    var wd = $(window).width();
    var hg = document.querySelector(".context").clientHeight;
    $("#quesArea").width(wd * (count == 0 ? 1 : count + 10));
    //设置题型
    var quesTypes = $("body").attr("questype").split(",");
    //设置宽高，试题类型
    $(".quesItem").width(wd).height(hg).each(function (index, element) {
        var type = Number($(this).attr("type"));
        $(this).find(".ques-type").text("【" + $.trim(quesTypes[type - 1]) + "题】");
        if (type == 1 || type == 3) {
            $(this).find(".btnSubmit").hide();
        }
        //收藏图标的状态
        var isCollect = $(this).attr("IsCollect") == "True" ? true : false;
        if (isCollect) {
            $(this).find(".btnFav").addClass("IsCollect");
        }
    });
    //选项的序号，数字转字母
    $(".quesItemsBox").each(function () {
        $(this).find(">div").each(function (index, element) {
            var char = String.fromCharCode(0x41 + index);
            $(this).find("b").after(char + "、");
        });
    });
    //左右滑动切换试题
    finger.init();
};



