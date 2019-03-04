<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="miniProgramResult.aspx.cs"
    Inherits="WxPayAPI.miniProgramResult" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>微信html5支付回调</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="initial-scale=1,maximum-scale=1,user-scalable=no" />
    <meta name="viewport" content="width=device-width, initial-scale=1,maximum-scale=1.0, user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <meta name="format-detection" content="telephone=yes" />
    <meta name="format-detection" content="email=no" />
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
            color: #FFFFFF;
            }

        .footer
        {
            position: fixed;
            bottom: 50;
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
    <script type="text/javascript" src="/Utility/CoreScripts/jquery.js"></script>
    <script type="text/javascript" src="/Utility/CoreScripts/Extend.js"></script>
</head>
<body>
    <div class="accinfo">
        <div class="show-tit" id="name">
            <asp:Label ID="lbError" runat="server" Text="如果不是主动取消支付，请稍候！" CssClass="lbError" Visible="false"></asp:Label>
            <asp:Label ID="lbSucess" runat="server" Text="支付成功！" CssClass="lbSucess" Visible="false"></asp:Label>
        </div>
        
        <div class="img-line">
            </div>
        <div class="show-tit" id="Div1">
            充值金额：&yen; <%= ((float)total_fee)/100%> 元
        </div>
    </div>
    <div class="footer"><a class="btn-green" id="getBrandWCPayRequest" href="/Mobile/recharge.ashx">返 回</a></div>
     <script type="text/javascript">
         function gourl() {
             window.location.href = $().setPara(window.location.href, "t", new Date().getTime());
         }
         $(function () {
             var lbSucess = $(".lbSucess");
             if (lbSucess.size() < 1) {
                 setTimeout("gourl()", 2000);
             }
         });
         
     </script>
</body>
</html>
