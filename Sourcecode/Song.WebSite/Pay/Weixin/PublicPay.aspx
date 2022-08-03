<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PublicPay.aspx.cs" Inherits="Song.Site.Pay.Weixin.PublicPay" %>

<!doctype html>
<html>

<head runat="server">
    <title>微信公众号支付</title>
    <meta name="viewport" content="initial-scale=1,maximum-scale=1,user-scalable=no" />
    <meta name="viewport" content="width=device-width, initial-scale=1,maximum-scale=1.0, user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <meta name="format-detection" content="telephone=yes" />
    <meta name="format-detection" content="email=no" />
    <script type="text/javascript" src="/Utilities/Scripts/jquery.js"></script>
     <script type="text/javascript" src="/Utilities/Scripts/axios_min.js"></script>
     <script type="text/javascript" src="/Utilities/Scripts/api.js"></script>
    <style type="text/css">
        .accinfo {
            margin-right: auto;
            margin-left: auto;
            margin-top: 30px;
        }

        body {
            text-align: center;
            margin: 0px;
            padding-top: 20px;
        }

        .show-tit {
            font-size: 16px;
            line-height: 40px;
            height: 40px;
            font-weight: bold;
            text-align: center;
            margin-top: 20px;
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

        .footer {
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

        .footer a {
            color: #333;
            text-decoration: none;
            display: block;
            line-height: 45px;
        }
    </style>
</head>

<body>
    <form id="form1" runat="server">
        <div class="accinfo">
            <div class="show-tit" id="name">
                <%= acc.Ac_Name %>
            </div>
            <div class="img-line">
                <img src="<%= path %><%= acc.Ac_Photo %>" id="photo" default="/Utilities/images/head1.jpg" /></div>
            <div class="show-tit" id="Div1">
                正在支付...
            </div>
        </div>
        <div class="footer">
            <a href="/" id="btnBacklink">如无法正常返回，请点击</a></div>
    </form>
    <script type="text/javascript">


        //调用微信JS api 支付
        function jsApiCall() {
            var apipara = '<%=wxJsApiParam%>';
            if (apipara == "") return;
            apipara = <%=wxJsApiParam %>;
            WeixinJSBridge.invoke('getBrandWCPayRequest', apipara, function (res) {
                //WeixinJSBridge.log(res.err_msg);
                var msg = "code:" + res.err_code + "\n";
                msg += "desc:" + res.err_desc + "\n";
                msg += "msg:" + res.err_msg + "\n";
                //alert(msg);
                //支付成功
                var returl = "";                //返回的地址
                var default_returl = '/mobi/account/myself';
                returl = $api.storage('recharge_returl');  //充值后的返回
                if (returl == '' || returl == null) returl = default_returl;
                $("#btnBacklink").attr('href', returl);
                $("#btnBacklink").click();
                if (res.err_msg == "get_brand_wcpay_request:ok") {
                    window.location.href = returl;
                }
                //支付取消
                if (res.err_msg == "get_brand_wcpay_request:cancel") {
                    window.location.href = returl;
                }
                //支付失败
                if (res.err_msg == "get_brand_wcpay_request:fail") {
                    alert("支付失败！");
                    window.location.href = returl;
                }
            });
        }
        (function callpay() {
            if (typeof WeixinJSBridge == "undefined") {
                if (document.addEventListener) {
                    document.addEventListener('WeixinJSBridgeReady', jsApiCall, false);
                }
                else if (document.attachEvent) {
                    document.attachEvent('WeixinJSBridgeReady', jsApiCall);
                    document.attachEvent('onWeixinJSBridgeReady', jsApiCall);
                }
            }
            else {
                jsApiCall();
            }
            return false;
        })();
        $("img").error(function () {
            var errImg = $(this).attr("default");
            if (errImg == null) return false;
            $(this).attr("src", errImg);
        });
    </script>
</body>

</html>