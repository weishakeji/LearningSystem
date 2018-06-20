<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="miniProgramPay.aspx.cs"
    Inherits="Song.Site.Pay.Weixin.miniProgramPay" %>

<!doctype html>
<html>
<head runat="server">
    <title>微信小程序支付</title>
    <meta name="viewport" content="initial-scale=1,maximum-scale=1,user-scalable=no" />
    <meta name="viewport" content="width=device-width, initial-scale=1,maximum-scale=1.0, user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <meta name="format-detection" content="telephone=yes" />
    <meta name="format-detection" content="email=no" />
    <script type="text/javascript" src="/Utility/CoreScripts/jquery.js"></script>
    <script src="https://res.wx.qq.com/open/js/jweixin-1.3.2.js" type="text/javascript"></script>
    <style type="text/css">
        .accinfo
        {
            margin-right: auto;
            margin-left: auto;
            margin-top: 30px;
        }
        body
        {
            text-align: center;
        }
        .show-tit
        {
            font-size: 16px;
            line-height: 40px;
            height: 40px;
            font-weight: bold;
            text-align: center;
            margin-top: 20px;
        }
        
        .img-line img
        {
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
        a[class*="btn"], #btn
        {
            display: block;
            height: 42px;
            line-height: 42px;
            color: #FFFFFF;
            text-align: center;
            border-radius: 5px;
            text-decoration: none;
        }
        .btn-green
        {
            background-image: -webkit-gradient(linear, left top, left bottom, color-stop(0, #43C750), color-stop(1, #31AB40));
            border: 1px solid #2E993C;
            box-shadow: 0 1px 0 0 #69D273 inset;
            margin-top: 10px;
        }
        
        .footer
        {
            position: fixed;
            bottom: 0;
            background: #eee;
            width: 100%;
            height: 45px;
            line-height: 30px;
            z-index: 9999;
            _bottom: auto;
            _width: 100%;
            _position: absolute;
        }
        .footer a
        {
            color: #333;
            text-decoration: none;
            display: block;
            line-height: 45px;
        }
    </style>
    <script type="text/javascript">
        String.prototype.format = function (args) {
            if (arguments.length < 1) return this;
            var primary = this;
            for (var i = 0; i < arguments.length; i++) {
                primary = primary.replace(eval('/\\{' + i + '\\}/g'), arguments[i]);
            }
            return primary;
        }
    </script>
</head>
<body>
    <div>
        wxjs:<span id="wxjs"></span></div>
    <div id="btn" class="btn-green">
        确认支付</div>
    <script type="text/javascript">
        //$("#wxjs").text(window.__wxjs_environment);
        //小程序ID与密钥
        var appid = "<%= appid %>";
        var secret = "<%= secret %>";
        //商户ID与密码
        var mchid = "<%= mchid %>";
        var paykey = "<%= paykey %>";
        //订单金额与商户订单号
        var total_fee = "<%= total_fee %>";
        var serial = "<%= serial %>";
        var org = "<%= orgid %>";
        //回调域
        var notify_url = encodeURIComponent("<%= notify_url %>");
        //
        //打开小程序页面的方法
        function goPaymini() {
            var pageurl = "/pages/wxpay/pay?appid={0}&secret={1}&mchid={2}&paykey={3}&total_fee={4}&serial={5}&org={6}&notify_url={7}";
            pageurl = pageurl.format(appid, secret, mchid, paykey, total_fee, serial, org, notify_url);
            wx.miniProgram.navigateTo({ url: pageurl, success: function () {
                window.location.href = "Html5PayResultNotify.aspx?serial=" + serial;
            } });
            //setTimeout('clearWord( )', 3000);
            
        }
        $("#btn").click(goPaymini);
        window.onload = goPaymini;
    </script>
    正在打开支付界面！
</body>
</html>
