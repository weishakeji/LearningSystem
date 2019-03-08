$(function () {
    //生成学员学习证明的二维码
    $(".qrcode").each(function () {
        var acid = $(this).attr("acid");
        jQuery($(this)).qrcode({
            render: "canvas", //也可以替换为table
            width: 75,
            height: 75,
            foreground: "#000",
            background: "#FFF",
            text: $().getHostPath() + "Mobile/certify.ashx?acid=" + acid
            //text:acid
        });
    });

});