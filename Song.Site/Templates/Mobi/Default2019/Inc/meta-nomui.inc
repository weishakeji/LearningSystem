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
<script type="text/javascript" src="/Utility/CoreScripts/jquery.js?ver={$version}"></script>
<script type="text/javascript" src="/Utility/CoreScripts/artTemplate.js?ver={$version}"></script>
<script type="text/javascript" src="../CoreScripts/Extend.js?ver={$version}"></script>
<script type="text/javascript" src="../CoreScripts/PageBox.js?ver={$version}"></script>
<!--核心Css文件-->
<link rel="stylesheet" href="../CoreStyles/public.css?ver={$version}"/>
<link rel="stylesheet" href="../CoreStyles/PageBox.css?ver={$version}"/>
<!--公共的Js文件-->
<script type="text/javascript" src="../CoreScripts/public.js?ver={$version}"></script>
<script type="text/javascript">
  //当前登录的账号ID
  var currLoginACID = Number("{$Account.Ac_id}");
  window.onload=function(){
    ToAllLinkAddAccoutsID(currLoginACID);
  }
</script>
<!--后面就是各个页面自己的了-->


