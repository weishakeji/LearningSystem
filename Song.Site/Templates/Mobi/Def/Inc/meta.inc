<meta charset="utf-8" />
<meta name="viewport" content="initial-scale=1,maximum-scale=1,user-scalable=no"/>
<meta name="viewport" content="width=device-width, initial-scale=1,maximum-scale=1.0, user-scalable=no" />
<meta name="apple-mobile-web-app-capable" content="yes"/>
<meta name="apple-mobile-web-app-status-bar-style" content="black"/>
<meta name="format-detection" content="telephone=yes" />
<meta name="format-detection" content="email=no" />
<meta name="Keywords"  content="{$org.Org_Keywords},微厦科技" />
<meta name="Description" content="{$org.Org_Description},微厦科技" />
<!--核心Js文件-->
<script type="text/javascript" src="/Utility/CoreScripts/jquery.js"></script>
<script type="text/javascript" src="/Utility/CoreScripts/artTemplate.js"></script>
<script type="text/javascript" src="/Utility/CoreScripts/verify.js"></script>
<script src="../CoreScripts/mui.js" type="text/javascript"></script>
<script type="text/javascript" src="../CoreScripts/Extend.js"></script>
<script type="text/javascript" src="../CoreScripts/Base64.js"></script>
<script type="text/javascript" src="../CoreScripts/PageBox.js"></script>
<!--核心Css文件-->
<link rel="stylesheet" type="text/css" href="/Utility/iconfont/iconfont.css"/>
<link rel="stylesheet" href="../CoreStyles/public.css"/>
<link rel="stylesheet" href="../CoreStyles/mui.css"/>
<link rel="stylesheet" href="../CoreStyles/PageBox.css"/>
<!--APICloud的接口文件，当用apicloud打包app时，需要用到-->
<script src="../APICloud/common.js"></script>
<script src="../APICloud/api.js"></script>
<script src="../APICloud/dataSource.js"></script>
<!--公共的Js文件-->
<script type="text/javascript" src="../CoreScripts/public.js"></script>
<script type="text/javascript" src="../CoreScripts/mui-fuction.js"></script>
<script type="text/javascript">
  //当前登录的账号ID
  var currLoginACID = Number("{$Account.Ac_id}");
  window.onload=function(){
    ToAllLinkAddAccoutsID(currLoginACID);
  }
</script>
<script type="text/javascript">
  //当前模板的路径
  var tmpath="{$TempPath}";
</script>
<!--后面就是各个页面自己的了-->


