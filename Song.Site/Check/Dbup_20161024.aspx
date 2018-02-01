<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dbup_20161024.aspx.cs"
    Inherits="Song.Site.Check.Dbup_20161024" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>试题结构升级</title>
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
        #dl3
        {
            color: #060;
            display: none;
        }
        #loading
        {
            color: Red;
        }
    </style>
    <script type="text/javascript" src="/Utility/CoreScripts/jquery.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="tit">
        试题结构升级
    </div>
    <div>
        说明：<br />
        原试题结构，试题题干与选项是分开存放的。<br />
        这种设计符合数据库设计范式，但执行效率偏低。<br />
        偏低的原因是因为，当抽取试题后，还要再分步取试题选项内容。<br />
        例如一个试卷有一百道题，那么加载试题时，会读取数据库101次。
    </div>
    <hr />
    <div id="loading" style="display: none">
        正在升级中，请稍候……</div>
    <div>
        <dl>
            <dt>第一步，升级数据库结构</dt>
            <dd>
                此举为调整试题的数据库结构</dd>
            <dd>
                <input type="button" name="btnStruct" value="升级数据库结构" id="btnStruct" />
            </dd>
        </dl>
        <dl>
            <dt>第二步，整理试题数据</dt>
            <dd>
                此举将原来的试题数据整理成新的格式</dd>
            <dd>
                <input type="button" name="btnDataTras" value="开始升级" id="btnDataTras" />
            </dd>
            <dd>
                进度：共有试题 <span id="ltSumCount">0</span> 道，正在处理第 <span id="ltCurr">0</span> 道</dd>
        </dl>
        <dl id="dl3">
            <dt>第三步，完成</dt>
            <dd>
                升级完成！</dd>
            <dd>
            </dd>
        </dl>
    </div>
    </form>
    <script type="text/javascript">
        $("input[type=button]").click(function () {
            var name = $(this).attr("name");
            var func = eval(name + "_click");
            $("#loading").show();
            func(name);
        });
        //修改结构
        function btnStruct_click(action) {
            $.post(window.location.href, { action: action }, function (requestdata) {
                if (requestdata == "1") {
                    alert("数据库结构升级完成！");
                } else {
                    alert(requestdata);
                }
                $("#loading").hide();
            });
        }
        //升级数据
        function btnDataTras_click(action) {
            var size = 100; //每页取多少条
            var index = 1; //索引页   
            var sumcount = 0; //总记录数         
            btnDataTras_click_request(size, index, sumcount,0, action);
        }
        function btnDataTras_click_request(size, index, sumcount,orgid, action) {            
            index = size * index < sumcount ? ++index : index;
            $.post(window.location.href, { size: size, index: index, orgid: orgid, action: action }, function (requestdata) {
                var data = eval("(" + requestdata + ")");
                sumcount = data.sumcount;
                orgid = data.orgid;
                $("#ltSumCount").html(sumcount);
                $("#ltCurr").html(size * index);
                if (size * index >= sumcount) {
                    alert("试题数据升级完成");
                    $("#loading").hide();
                } else {
                    btnDataTras_click_request(size, index, sumcount, orgid, action);
                }
            });
        }
    </script>
</body>
</html>
