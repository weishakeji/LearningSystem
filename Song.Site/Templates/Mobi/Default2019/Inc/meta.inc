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
<script type="text/javascript" src="/Utility/CoreScripts/verify.js?ver={$version}"></script>
<script src="../CoreScripts/mui.min.js?ver={$version}" type="text/javascript"></script>
<script type="text/javascript" src="../CoreScripts/Extend.js?ver={$version}"></script>
<script type="text/javascript" src="../CoreScripts/Base64.js?ver={$version}"></script>
<script type="text/javascript" src="../CoreScripts/PageBox.js?ver={$version}"></script>
<!--核心Css文件-->

<link rel="stylesheet" href="../CoreStyles/mui.min.css?ver={$version}"/>
<link rel="stylesheet" href="../CoreStyles/public.css?ver={$version}"/>
<link rel="stylesheet" href="../CoreStyles/PageBox.css?ver={$version}"/>
<!--APICloud的接口文件，当用apicloud打包app时，需要用到-->
<script src="../APICloud/common.js?ver={$version}"></script>
<script src="../APICloud/api.js?ver={$version}"></script>
<script src="../APICloud/dataSource.js?ver={$version}"></script>
<!--公共的Js文件-->
<script type="text/javascript" src="../CoreScripts/public.js?ver={$version}"></script>
<script type="text/javascript" src="../CoreScripts/mui-fuction.js?ver={$version}"></script>
<script type="text/javascript">
  //当前登录的账号ID
  var currLoginACID = Number("{$Account.Ac_id}");
  window.onload=function(){
  ToAllLinkAddAccoutsID(currLoginACID);
  }
</script>
    <script type="text/javascript">
        //当前模板的路径
        var tmpath = "{$TempPath}";
        //如果是免费版，弹出广告
        $(function () {
            var license = $("html").attr("license");
            if (license != null && license == "false") {
                var initdate = $().cookie("initdate");
                if (initdate == null) {
                    $().cookie("initdate", new Date().getTime());
                    return;
                } else {
                    initdate = new Date(Number(initdate));
                    initdate.setDate(initdate.getDate() + 3);
                    if (initdate < new Date()) {
                        new PageBox("当前版本为免费版", "http://www.weishakeji.net/ad/lv0.html", 100, 100, "url").Open();
                    }
                }

            }
        });
    </script>
<!--后面就是各个页面自己的了-->


