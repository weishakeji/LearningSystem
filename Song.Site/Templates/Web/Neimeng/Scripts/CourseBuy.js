$(function () {
    _btnEvent();
    _selectPrice();
});
//按钮事件
function _btnEvent(){
    //免费课程的学习按钮
    $("#btnStudy").click(function () {
        BuySubmit(0,0);
        return false;
    });
    //试用按钮
    $(".aTryout").click(function(){
        var isTry=$(this).attr("IsTry")=="True" ? true : false;
        if(isTry) {
            var txt = "您选择了试用《" + $(this).attr("course") + "》。<br/><br/>";
            txt += "说明：在试用状态只能做“章节练习”和查阅一些资料。";
            var msg = new MsgBox("暂时试用", txt, 400, 300, "confirm");
            msg.EnterEvent = function () {
                BuySubmit(1, 1);
            };
            msg.Open();
        }else{
            new MsgBox("提示", "该课程不允许试用，请登录购买。", 400, 300, "alert").Open();
        }
        return false;
    });
    //确定按钮
    $("#btnBuyStudy").click(function(){
        var moneySpan=$(".money");
        if(moneySpan.size()<1) {
            var txt="您还没有登录，请登录后购买。";
            var msg=new MsgBox("未登录",txt,400,300,"confirm");
            msg.EnterEvent=function(){
                window.location.href="Login.ashx";
            };
            msg.Open();
            return false
        }
        //是否选项费用项
        var selected=$(".priceSelected");
        if(selected.size()<1){
            new MsgBox("提示","请选择学习费用的选项。",400,300,"alert").Open();
            return false;
        }
        //判断产品价格不得低于余额
        var money=Number(moneySpan.html());
        var price=Number(selected.find(".price").html());
        if(money<price){
            var msg=new MsgBox("提示","你的余额不足，是否充值？",400,300,"confirm");
            msg.EnterEvent=function(){
				var btnRecharge=$(".btnRecharge").get(0);
                //window.location.href="MoneyIncom.ashx";
				btnRecharge.click();
            };
            msg.Open();
            return false
        }
        //验证码
        if($.trim($(".verify").val())==""){
            new MsgBox("提示","请输入验证码！",400,300,"alert").Open();
            return false;
        }
        BuySubmit(1,0);
    });
}
//选择价格
function _selectPrice(){
    //价格选项的点击事件
    $(".priceItem").click(function(){
        $(this).parent().find(".priceItem").removeClass("priceSelected");
        $(this).parent().find(".priceItem span.ico").html("&#xf00c6;");
        //选中事件
        $(this).addClass("priceSelected");
        $(this).find("span.ico").html("&#xe667;");
        //如果登录，则显示验证码
        var moneySpan=$(".money");
        if(moneySpan.size()>0) {
            $(".verifyInfo").show();
        }
    });
}

//提交购买操作
//isfree:是否免费购买，免费请写0
//istry:是否试用，不试用请写0
function BuySubmit(isfree,istry) {
    var urlPath = "/Ajax/CourseBuySubmit.ashx?timestamp=" + new Date().getTime();
    var code=$.trim($(".verify").val());         //验证码
    var cpid=$(".priceSelected").attr("cpid");  //价格项的id
    var couid = $().getPara("couid");    //课程id
    var return_url = "CourseStudy.ashx";            //成功后，跳转的页面
    $.ajax({
        type: "POST", url: urlPath, dataType: "text",
        data: { veriCode: code, cpid: cpid, couid: couid, return_url: return_url,
            isfree: isfree, istry: istry
        },
        //开始，进行预载
        beforeSend: function (XMLHttpRequest, textStatus) {
            window.isSubmit = true;
            var msg = new MsgBox("提交", "正在处理交易，请稍等……", 400, 300, "loading");
            msg.ShowCloseBtn = false;
            msg.Open();
        },
        //加载出错
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            var msg = new MsgBox("服务器故障", "无法获取响应<br/><br/>错误信息：" + errorThrown, 400, 300, "alert");
            msg.Open();
        },
        //加载成功！
        success: function (data) {
            try {
                var result = eval("(" + data + ")");
                if (result.status == 0) {
                    var msg = new MsgBox("操作成功", "操作成功，点击确定开始学习", 400, 300, "confirm");
                    msg.EnterEvent = function () {
                        window.location.href = result.return_url+"?couid="+result.couid;
                    };
                    msg.Open();
                }
                var error = "";
                if (result.status == 1) error = "您还未登录！";
                if (result.status == 2) error = "验证码不正确！";
                if (result.status == 3) error = "价格数据不存在，请刷新界面再提交！";
                if (result.status == 4) error = "当前课程不存在，请刷新界面再提交！";
                if (result.status == 5) error = "余额不足，请充值！";
                if (result.status == 6) error = "数据异常！";
                if (result.status == 7) error = "当前课程并不是免费的！";
                if (result.status == 7) error = "当前课程不允许试用！";
                if (result.status != 0) new MsgBox("错误", error, 400, 300, "alert").Open();
                window.isSubmit = false;
            } catch (e) {
                alert(data);
            }
        }
    });
}