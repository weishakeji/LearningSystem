$(function () {

    //    //初始样式
    //    $(".item").each(function () {
    //        var isFree = $(this).attr("isfree") == "True" ? true : false;
    //        var isTry = $(this).attr("istry") == "True" ? true : false;
    //        if (isFree) $(this).addClass("isFree");
    //        if (isTry) $(this).addClass("isTry");
    //    });
    $(".item").each(function () {
        var couid = $(this).attr("couid");
        $.post(window.location.href, { couid: couid, action: "getstc" }, function (result) {
            var data = eval("(" + result + ")");
            if (data.success == "0") return;
            var data = data.data;
            //
            var row = $(".item[couid=" + data.Cou_ID + "]");
            row.attr("isfree", data.Stc_IsFree);
            row.attr("istry", data.Stc_IsTry);
            if (data.Stc_IsFree) row.addClass("isFree");
            if (data.Stc_IsTry) row.addClass("isTry");
        });
    });

});