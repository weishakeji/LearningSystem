$(function () {
    //查看下级会员详情
    mui('body').on('tap', '.btnFriend', function () {
        new PageBox("我的朋友", "Subordinates.ashx", 100, 100, "url").Open();
    });
    //帮助说明
    mui('body').on('tap', '.btnHelp', function () {
        var txt = $(".help-box .explain").get(0).outerHTML;
        var msg = new MsgBox("说明", txt, 80, 80, "msg");
        msg.ShowCloseBtn = false;
        msg.OverEvent = function () {
            //var href = form.find("input[name=from]").val();
            //window.location.href = $().setPara(href, "sharekeyid", data.acid);
        };
        msg.Open();
    });
	//生成分享二维码
	var sharekeyid=$().getPara("sharekeyid");
	jQuery('#qrcode').qrcode({
		render: "canvas", //也可以替换为table
		width: 75,
		height: 75,
		foreground: "#333",
		background: "#FFF",
		text: $().getHostPath()+"default.ashx?sharekeyid="+sharekeyid
	});
});
