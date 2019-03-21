<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dbup_20190321.aspx.cs"
    Inherits="Song.Site.Check.Dbup_20190321" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>修正课程过期的问题</title>
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
    <script language="javascript" type="text/javascript">
// <![CDATA[

        function btnStruct_onclick() {

        }

// ]]>
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="tit">
        修正课程过期的问题
    </div>
    <div>
        说明：<br />
        修正课程过期的问题。<br />
        <br />
        <hr />
      

        <asp:Button ID="Button1" runat="server" Text="按学员购买课程的记录修正课程学习时效" onclick="Button1_Click" />
         <br />
          <br />
       <asp:Button ID="Button2" runat="server" Text="按学习卡使用记录修正课程学习时效" onclick="Button2_Click" />
        <br />
        <br />

    </div>
    </form>
    
</body>
</html>
