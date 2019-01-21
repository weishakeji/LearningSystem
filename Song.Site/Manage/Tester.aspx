<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Tester.aspx.cs" Inherits="Song.Site.Manage.Tester" %>

<%@ Register Src="Utility/ExcelInput.ascx" TagName="ExcelInput" TagPrefix="uc1" %>
<%@ Register Src="Utility/Uploader.ascx" TagName="Uploader" TagPrefix="uc2" %>
<!DOCTYPE html>
<html>
<script type="text/javascript" src="CoreScripts/jquery.js"></script>
<head runat="server">
    <title>无标题页</title>
    <meta charset="utf-8" />
    <meta name="viewport" content="initial-scale=1,maximum-scale=1,user-scalable=no" />
    <meta name="viewport" content="width=device-width, initial-scale=1,maximum-scale=1.0, user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <meta name="format-detection" content="telephone=yes" />
    <meta name="format-detection" content="email=no" />
    <script type="text/javascript" src="/Utility/CoreScripts/jquery.js"></script>
    <script type="text/javascript" src="/Utility/CoreScripts/Verify.js"></script>
    <script type="text/javascript" src="/Utility/CoreScripts/Extend.js"></script>
</head>
<body>
    <script type="text/javascript">
        //写着玩的
        (function () {
            var vers = ["", "1", "2"];
            var obj = function (version) {
                this.version = version == null ? "1.1" : version;
                this.query = function () {
                    //alert(this.version);
                }
            };
            for (var v in vers) {
                var str = vers[v] == "" ?
             "window.$api = new obj();" :
             "window.$api.v" + vers[v] + "= new obj('" + vers[v] + "')";
                eval(str);
            }
        })();
        $api.query();
        $api.v1.query();
        $api.v2.query();
    </script>
</body>
</html>
