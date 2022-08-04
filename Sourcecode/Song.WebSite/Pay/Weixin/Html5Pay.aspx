<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Html5Pay.aspx.cs" Inherits="Song.Site.Pay.Weixin.Html5Pay" %>

<!doctype html>
<html>
<head runat="server">
    <title>微信Html5支付</title>
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
        html
        {
            background-color:#fff;
        }
        
        .accinfo
        {
            margin-right: auto;
            margin-left: auto;
            margin-top: 30px;
        }
        body
        {
            text-align: center;
             text-align: center;
            margin: 0px;
            padding-top: 20px;
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
        a[class*="btn"] {
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
            background-image:-webkit-gradient(linear, left top, left bottom, color-stop(0, #43C750), color-stop(1, #31AB40));border:1px solid #2E993C;box-shadow:0 1px 0 0 #69D273 inset;
            margin-top:10px;
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
            充值金额：&yen; <%= ((float)total_fee)/100%> 元
        </div>
    </div>
    <div class="operation"><a class="btn-green" id="getBrandWCPayRequest" href="#">确认充值</a></div>
    <div class="footer">
        <a href="#" onclick="self.location='/Mobile/recharge.ashx';">如无法正常返回，请点击</a></div>
    </form>
    <script type="text/javascript">
        String.prototype.format = function (args) {
            if (arguments.length < 1) return this;
            var primary = this;
            for (var i = 0; i < arguments.length; i++) {
                primary = primary.replace(eval('/\\{' + i + '\\}/g'), arguments[i]);
            }
            return primary;
        }
        $('img').error(function () {
            $(this).attr('src', $(this).attr('default'));
        });
        var referrer = document.referrer;       
    </script>
    <script src="//wx.gtimg.com/wxpay_h5/fingerprint2.min.1.4.1.js"></script>
    <script type="text/javascript">

        var piid = "<%= piid %>";
        var serial = "<%= serial %>";
        var returl = encodeURIComponent("<%= notify_url%>");
        var rawurl = encodeURIComponent("<%= notify_url%>?piid=" + piid + "&serial=" + serial);
        //获取code
        var fp = new Fingerprint2();
        //result为浏览器指纹
        fp.get(function (result) {
            //alert(result);
            $.post(window.location.href, { code: result, piid: piid, serial: serial, returl: returl }, 
                function (d) {                
                    if (Number(d) != 0) {
                        $('#getBrandWCPayRequest').attr("href", d.mweb_url + '&redirect_url=' + rawurl);
                    } else {
                        alert("下单失败！");
                    }
                }, "json");
        });
    </script>
</body>
</html>
