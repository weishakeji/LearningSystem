<%@ Page Language="C#" AutoEventWireup="true" Codebehind="return_url.aspx.cs" Inherits="Com.Alipaywap.return_url" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>页面跳转同步通知页面</title>
     
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="initial-scale=1,maximum-scale=1,user-scalable=no" />
    <meta name="viewport" content="width=device-width, initial-scale=1,maximum-scale=1.0, user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <meta name="format-detection" content="telephone=yes" />
    <meta name="format-detection" content="email=no" />
    <script type="text/javascript" src="/Utility/CoreScripts/jquery.js"></script>
    <script type="text/javascript" src="/Utility/CoreScripts/Extend.js"></script>
    <style type="text/css">
        html
        {
            background-color:#fff;
        }
         body
        {
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
        

    </style>
</head>
<body>
 <div class="show-tit" id="name">
 正在返回，稍候...
 </div>
    <script type="text/javascript">
        $(function () {
            var return_para = "<%= return_para %>";
            var default_returl = '/Mobile/recharge.ashx';
            var returl = $.cookie('recharge_returl');  //充值后的返回
            if (returl == '' || returl == null) returl = default_returl;
            //转向
            returl += "?" + return_para;
            window.location.href = returl;

        });         
    </script>
</body>
</html>