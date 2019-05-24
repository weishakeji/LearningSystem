<%@ Page Language="C#" AutoEventWireup="true" Codebehind="return_url.aspx.cs" Inherits="Alipayweb_return_url" %>

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
        
        .accinfo
        {
            margin-right: auto;
            margin-left: auto;
            margin-top: 30px;
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
        a[class*="btn"]
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
</head>
<body>
    
</body>
</html>