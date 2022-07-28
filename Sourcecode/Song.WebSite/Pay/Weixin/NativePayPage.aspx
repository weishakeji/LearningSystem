<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NativePayPage.aspx.cs"
    Inherits="WxPayAPI.NativePayPage" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <meta http-equiv="content-type" content="text/html;image/gif;charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <script type="text/javascript" src="/Utility/CoreScripts/jquery.js"></script>
    <script type="text/javascript" src="/Utility/CoreScripts/Extend.js"></script>
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

        .show-tit {
            font-size: 14px;
            line-height: 40px;
            height: 40px;
            text-align: center;
            margin-top: 20px;
            color: #666;
        }

        .total_fee {
            font-size: 14px;
            line-height: 40px;
            height: 40px;
            color: #f00;
            text-align: center;
        }

        .total_fee span {
            font-size: 16px;
            font-weight: bold;
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
            font-size: 16px;
            line-height: 50px;
            height: 50px;
            color: #06f;
            text-align: center;
            display: none;
            width: 400px;
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
        <div class="show-tit" id="name">
            <%= acc.Ac_Name %>
        </div>
        <div class="img-line">
            <img src="<%= accphoto %>" id="photo" default="/Utility/images/head1.jpg" /></div>
        <div class="show-tit">
            请用微信扫描下面二维码</div>
        <div class="total_fee">
            支付金额：<span><%= (Convert.ToDouble(total_fee)/100).ToString("0.0") %></span>元</div>
    </div>
    <asp:Image ID="Image2" runat="server" Style="width: 200px; height: 200px;" />
    <div class="success">
        <b></b><a href="#" id="close">支付完成，点击关闭，<i>3</i>秒后关闭</a></div>
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
        var returl = $.cookie('recharge_returl');
        $("a#close").attr("href", returl);
        $("a#close").click(function () {
            var returl = $.cookie('recharge_returl');  //充值后的返回
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
    window.onbeforeunload = onunload_handler;
    window.onunload = onunload_handler;
    function onunload_handler() {
        var returl = $.cookie('recharge_returl');  //充值后的返回
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