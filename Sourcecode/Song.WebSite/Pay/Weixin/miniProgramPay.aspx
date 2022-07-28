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
        
        body
        {
            text-align: center;
        }
        .txt
        {
            margin-right: auto;
            margin-left: auto;
            margin-top: 50px;
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
    
    <div class="txt">
        正在调取支付模块！</div>
    <script type="text/javascript">
        
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
                    window.setTimeout(function () {
                        window.location.href = "miniProgramResult.aspx?serial=" + serial;
                    },1000); 
                }
            });
            //setTimeout('clearWord( )', 3000);
            
        }
        $("#btn").click(goPaymini);
        window.onload = goPaymini;
    </script>
    
</body>
</html>
