<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dbup_20170710.aspx.cs"
    Inherits="Song.Site.Check.Dbup_20170710" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>账户升级</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <script type="text/javascript" src="/Utility/CoreScripts/jquery.js"></script>
    <style type="text/css">
        #form1
        {
            width: 640px;
        }
        .tit
        {
            font-size: 22px;
            margin-bottom: 10px;
            font-weight: bold;
            font-family: "黑体" , "微软雅黑";
            color: #000;
        }
        *, dd, div
        {
            font-size: 14px;
            line-height: 22px;
        }
        #loading
        {
            color: #060;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="tit">
        账户升级
    </div>
    <div>
        说明：<br />
        合并教师与学员账户。<br />
        <hr />
        <input type="button" name="btnStruct" value="开始升级" id="btnStruct" />
        <div id="loading" style="display: none">
            <span id="showtext">正在升级中，请稍候……</span>
            <br />
            共<span id="accsum">0</span>条；当前正在处理第<span id="acccurr">0</span>条
            </div>
    </div>
    </form>
    <script type="text/javascript">
        var size = 10; //每页取多少条
        var index = 1; //索引页
        var sumcount = 0; //总记录数
        $("#btnStruct").click(function () {
            $(this).hide();
            $("#loading").show();
            request();
        });
        function request() {
            index = size * index < sumcount ? ++index : index;
            var url = window.location.href;
            $.post(url, { size: size, index: index }, function (requestdata) {
                var data = eval("(" + requestdata + ")");
                if (data.success == -1) {
                    alert("请勿重复执行!");
                    $("#btnStruct").show();
                    $("#loading").hide();
                    return;
                }
                if (data.success == 0) {
                    alert("执行错误：" + data.msg);
                    $("#btnStruct").show();
                    $("#loading").hide();
                    return;
                }
                if (data.success == 1) {
                    sumcount = data.sumcount;
                    $("#accsum").html(sumcount);    //总计多少条
                    $("#acccurr").html(size * index >= sumcount ? sumcount : size * index);   //当前第多少条
                    if (size * index >= sumcount) {
                        $("#showtext").text("升级完成");
                        //$("#btnStruct").show();
                        //$("#loading").hide();
                        return;
                    }
                    request();
                }
            });
        }
    </script>
</body>
</html>
