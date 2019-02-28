$(function () {
    //学习卡的二维码
    var code = $().getPara("code");
    var pw = $().getPara("pw");
    if ($.trim(code) != "" && $.trim(pw) != "") {
        $("#tbCard").val(code + "-" + pw);
    }
    $('#btn_camera').on('tap', function () {
        $("#upload_qrcode").click();
    });
    qrcode_Decode();
    //学习卡使用的按钮事件
    use_card();
    //学习卡领用（暂存的个人名下）
    get_card();
    //当点击学习卡列表中的使用按钮时
    badge_use();
    //查看学习卡详情
    mui('body').on('tap', '.view', function () {
        var href = $(this).attr("href");
        var code = $(this).parent().attr("code");
        var pw = $(this).parent().attr("pw");
        var box = new top.PageBox("学习卡：" + code + "-" + pw, href, 100, 100, null, window.name);
        box.CloseEvent = function () {
            //var courses = $.cookie("card_add");
            //alert(courses[0].id);
        }
        box.Open();
    });
	//帮助按钮
	$(".btnHelp").click(function(){
		var txt=document.querySelector(".boxHelp").innerHTML;
		//var txt=$(".boxHelp")[0].outerHTML;
		var msg = new MsgBox("帮助", txt, 80, 80, "msg");
		msg.ShowCloseBtn = false;
		msg.OverEvent = function () {
			//document.location.href = "LearningCard.ashx";
		};
		msg.Open();
	});
});
//填充学习到输入框，并使用
function set_card_use(card){
	 $("#tbCard").val(card);
	 $("#btnUseCard").click();
}
//学习卡充值
function use_card() {
    $("#btnUseCard").click(function () {
        //判断是否登录
        var nologin = $("#nologin");
        if (nologin.size() > 0) {
            window.Verify.ShowBox(nologin.find("a"), "请登录");
            return false;
        }
        //开始充值
        var form = $(this).parents("form");
        var card = form.find("#tbCard").val();
        if ($.trim(card) == "") return false;
        mui('#btnUseCard').button('loading');
        $.post(window.location.href, { action: "useCode", card: card }, function (result_data) {
            var data = eval(result_data);
            //操作成功
            if (data.state == 1) {
                mui.toast('使用成功！', { duration: 500, type: 'div' });
                var txt = "选修课程" + data.items.length + "个，如下：<br/>";
                for (var i = 0; i < data.items.length; i++) {
                    txt += (i + 1) + "、《" + unescape(data.items[i].Cou_Name) + "》<br/>";
                }
                var msg = new MsgBox("学习卡使用成功", txt, 80, 40, "msg");
                msg.ShowCloseBtn = false;
                msg.OverEvent = function () {
                    document.location.href = "LearningCard.ashx";
                };
                msg.Open();

            } else {
                var msg = new MsgBox("学习卡使用失败", data.info, 80, 40, "msg");
                msg.ShowCloseBtn = false;
                msg.Open();
                mui.toast(data.info, { duration: 2000, type: 'div' });
                $("#card-error").text(data.info);
            }
            mui('#btnUseCard').button('reset');
        });
        return false;
    });
}
//填充学习到输入框，并使用
function set_card_get(card){
	 $("#tbCard").val(card);
	 $("#btnGetCard").click();
}
//学习卡领用
function get_card() {
    $("#btnGetCard").click(function () {
        //判断是否登录
        var nologin = $("#nologin");
        if (nologin.size() > 0) {
            window.Verify.ShowBox(nologin.find("a"), "请登录");
            return false;
        }
        //开始充值
        var form = $(this).parents("form");
        var card = form.find("#tbCard").val();
        if ($.trim(card) == "") return false;
        mui('#btnGetCard').button('loading');
        $.post(window.location.href, { action: "getCode", card: card }, function (result_data) {
            var data = eval(result_data);
            //操作成功
            if (data.state == 1) {
                mui.toast('操作成功！', { duration: 500, type: 'div' });
                //弹出确认框
                mui.alert("操作成功", function (e) {
                    document.location.href = "LearningCard.ashx";
                });

            } else {
                mui.toast(data.info, { duration: 2000, type: 'div' });
                $("#card-error").text(data.info);
            }
            mui('#btnGetCard').button('reset');
        });
        return false;
    });
}

//二维码解析
function qrcode_Decode() {
    $("#upload_qrcode").change(function (e) {
        setLoading($("#btn_camera img"), "loading");
        var files = e.target.files;
        if (files && files.length > 0) {
            var form = new FormData();
            form.append('file', files[0]);
            form.append('action', 'decode_qrcode');
            $.ajax({
                url: window.location.href,
                type: 'POST',
                data: form,                    // 上传formdata封装的数据
                dataType: 'Text',
                cache: false,                      // 不缓存
                processData: false,                // jQuery不要去处理发送的数据
                contentType: false,                // jQuery不要去设置Content-Type请求头
                success: function (data) {
                    var code = $().getPara(data, "code");
                    var pw = $().getPara(data, "pw");
                    if ($.trim(code) != "" && $.trim(pw) != "") {
                        $("#tbCard").val(code + "-" + pw);
                        mui.toast("二维码解析成功", { duration: 2000, type: 'div' });
                        var href = "LearningCardView.ashx?code=" + code + "&pw=" + pw;
                        var box = new top.PageBox("学习卡：" + code + "-" + pw, href, 100, 100, null, window.name);                       
                        box.Open();
                    } else {
                        mui.toast("无法解析二维码", { duration: 2000, type: 'div' });
                    }
                    setLoading($("#btn_camera img"), "normal");
                }
            });
        }else{
			 setLoading($("#btn_camera img"), "normal");
		}
    });
}
//设置图片预载状态
//state: loading,或normal
function setLoading(el, state) {
    var file = el.attr(state);
    var path = el.attr("src");
    if (path.indexOf("/") > -1) {
        path = path.substring(0, path.lastIndexOf("/") + 1);
    }
    el.attr("src", path + file);
}

//当点击学习列表中的使用时
function badge_use() {
    mui('body').on('tap', '.badge-use', function () {
        var code = $(this).parent().attr("code");
        var pw = $(this).parent().attr("pw");
        $("#tbCard").val(code + "-" + pw);
        mui.toast('请点击使用按钮！', { duration: 500, type: 'div' });
    });
}