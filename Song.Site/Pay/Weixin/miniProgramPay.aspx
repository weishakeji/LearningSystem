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
</head>
<body>
    <div>
        wxjs:<span id="wxjs"></span></div>
    <div id="btn">
        按钮</div>
    <script type="text/javascript">
        $("#wxjs").text(window.__wxjs_environment);
        $("#btn").click(function () {
            //if (window.__wxjs_environment === 'miniprogram') {
            wx.miniProgram.navigateBack();
            wx.miniProgram.postMessage({ data: 'foo' })
            wx.miniProgram.navigateTo({
                url: '/pages/wxpay/pay'
            });
            //}
        });
    </script>
    正在打开支付界面！
</body>
</html>
