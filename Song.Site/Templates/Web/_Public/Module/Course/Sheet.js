$(function () {
    tabEvent();
	//生成课程二维码
	var couid=$().getPara("id");
	jQuery('#course-qrcode').qrcode({
		render: "canvas", //也可以替换为table
		width: 75,
		height: 75,
		foreground: "#666",
		background: "#FFF",
		text: $().getHostPath()+"Mobile/Course.ashx?id="+couid
	});
});

//选项卡切换
function tabEvent() {
    $(".tabArea").each(function (index, element) {
        $(this).find(".tab-bar>div").click(function () {
            $(this).parent().find(">div").removeClass("titOver");
            $(this).parent().parent().find(".tab-context>div").hide();
            $(this).addClass("titOver");
            var index = $(this).parent().find(">div").index(this);
            $(this).parent().parent().find(".tab-context>div:eq(" + index + ")").show();
        });
        //默认第一个显示
        //$(this).find(".tab-bar>div:eq(0)").click();
    });
}

$(function () {
    initEvent();   
});
//页面初始事件
function initEvent() {
    //点击选择学习的按钮
    var btn = $("a[state=noSelected]");
    $("a[state=noSelected]").click(function () {
        var url = $(this).attr("href");
        window.msgUrl = url;
        //当前学员是否通过审核
        var isPass = $(this).attr("isPass") == "False" ? false : true;
        if (isPass) {
            var txt = "您是否确认学习当前课程？";
            var msg = new MsgBox("选择课程", txt, 400, 300, "confirm");
            msg.EnterEvent = function () {
                window.location.href = window.msgUrl;
            };
            msg.Open();
        } else {
            var txt = "您还没有通过审核，无法进行课程学习。<br/>如果您确认已经通过审核，请重新登录。";
            var msg = new MsgBox("学员未通过审核", txt, 400, 200, "alert");
            msg.Open();
        }
        return false;
    });
}
