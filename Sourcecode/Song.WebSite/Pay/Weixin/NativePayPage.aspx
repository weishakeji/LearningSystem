<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NativePayPage.aspx.cs"
    Inherits="WxPayAPI.NativePayPage" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <meta http-equiv="content-type" content="text/html;image/gif;charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
     <script type="text/javascript" src="/Utilities/Scripts/jquery.js"></script>
    <script type="text/javascript" src="/Utilities/Scripts/jquery.qrcode.min.js"></script>
    <script type="text/javascript" src="/Utilities/Scripts/axios_min.js"></script>
     <script type="text/javascript" src="/Utilities/Scripts/api.js"></script>
    <title>微信扫码支付</title>
    <style type="text/css">
        body {
            font-family: "Microsoft YaHei";
        }

        .accinfo {
            margin-right: auto;
            margin-left: auto;
            margin-top: 30px;
        }

        body {
            text-align: center;
        }
        .name{
            font-size: 25px;
            line-height: 40px;
            height: 40px;
            text-align: center;
            margin-top: 20px;
            color: #666;
        }
        .show-tit {
            font-size: 18px;
            line-height: 40px;
            height: 40px;
            text-align: center;
            margin-top: 20px;
            color: #666;
        }

        .total_fee {
            font-size: 16px;
            line-height: 40px;
            height: 40px;
            color: #f00;
            text-align: center;
        }

        .total_fee span {
            font-size: 20px;
            font-weight: bold;
            margin:0px 10px;
        }

        .img-line img {
            border-radius: 110px;
            height: 106px;
            width: 106px;
            border: 1px solid #eee;
            margin-left: auto;
            margin-right: auto;
            box-shadow: 0 -2px 2px #ccc, 0 6px 1px rgba(0, 0, 0, 0.3);
            border-radius: 106px;
            margin: 3px;
        }

        .success {
            font-size: 18px;
            line-height: 50px;
            height: 50px;
            color: #67C23A;
            text-align: center;
            display: none;
            margin-right: auto;
            margin-left: auto;
        }

        .success a {
            color: #333;
            text-decoration: none;
            line-height: 50px;
            height: 60px;
            font-size: 18px;
        }

        .success b {
            background-image: url(../../Manage/Images/ICO/bigIco1.jpg);
            display: inline-block;
            height: 50px;
            width: 50px;
            float: left;
            background-position: -10px -95px;
        }
        .success i {
            color: #f00;
            font-variant: normal;
        }
    </style>
</head>

<body>
    <div class="accinfo">
        <div class="name" id="name">
            <%= acc.Ac_Name %>
        </div>
        <div class="show-tit">
            请用微信扫描下面二维码</div>
        <div class="total_fee">
            支付金额：<span><%= (Convert.ToDouble(total_fee)/100).ToString("0.00") %></span>元</div>
    </div>
    <asp:Image ID="Image2" runat="server" Style="width: 200px; height: 200px;display:none;" />
     <div id="qrcode"></div>
    <div class="success">
        支付完成</div>
</body>
<script type="text/javascript">
    $(function () {
        //设置学员头像
        $('img').error(function () {
            var def = $(this).attr('default');
            $(this).attr('src', def);
        });
        $("img").each(function () {
            var def = $(this).attr('default');
            if (!this.complete || (typeof this.naturalWidth == "undefined" && this.naturalWidth == 0) || !this.src) {
                $(this).attr("src", def);
            }
        });
    });
    (function () {
        var returl = $api.storage('recharge_returl');
        $("a#close").attr("href", returl);
        $("a#close").click(function () {
            var returl = $api.storage('recharge_returl');  //充值后的返回
            if (returl == '' || returl == null) {
                window.opener = null;
                window.open('', '_self');
                window.close();
            } else {
                //转向来源页
                window.location.href = returl;
            }
            return false;
        });
    })();
    //生成二维码
    (function () {
        var box = $("#qrcode");
        if (box.size() < 1) {
            window.setTimeout(this.qrcode, 200);
        }
        box.each(function () {
            if ($(this).find("img").size() > 0) return;
            var url = $("#Image2").attr("src");
            console.log(url);
            jQuery($(this)).qrcode({
                render: "canvas", //也可以替换为table
                width: 200,
                height: 200,
                foreground: "#000",
                background: "#FFF",
                text: url
            });
            //将canvas转换成img标签，否则无法打印
            var canvas = $(this).find("canvas").hide()[0];  /// get canvas element
            var img = $(this).append("<img/>").find("img")[0]; /// get image element
            img.src = canvas.toDataURL();
        });
    })();
    window.onbeforeunload = onunload_handler;
    window.onunload = onunload_handler;
    function onunload_handler() {
        var returl = $api.storage('recharge_returl');  //充值后的返回
        if (!(returl == '' || returl == null)) {
            //转向来源页
            window.location.href = returl;
        }
    }

    //实时监听充值完成
    var interval = window.setInterval("getPaystate()", 1000);
    function getPaystate() {
        $.post(window.location.href, function (data) {
            if (data == "1") {
                window.clearInterval(interval);
                $(".success").fadeIn(1000);
                window.setInterval(function(){
                    var s=Number($(".success i").text());
                    if(s>0){
                        $(".success i").text(--s);
                    }else{
                        $("a#close").click();
                    }
                }, 1000);
            }
        });
    }
</script>

</html>