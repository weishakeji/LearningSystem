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
    <script src="http://res.wx.qq.com/open/js/jweixin-1.2.0.js" type="text/javascript"></script>
</head>
<body>
<script type="text/javascript">
    if (window.__wxjs_environment === 'miniprogram') {
        wx.miniProgram.navigateTo({
            url: '/pages/wxpay/wxpay?orderid=' + id
        })
    }
</script>
</body>
</html>
