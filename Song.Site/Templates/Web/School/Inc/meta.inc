<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<meta name="Keywords"  content="{$org.Org_Keywords}" />
<meta name="Description" content="{$org.Org_Description}" />
<!--核心Js文件-->
<script type="text/javascript" src="/Utility/CoreScripts/jquery.js?ver={$version}"></script>
<script type="text/javascript" src="/Utility/CoreScripts/artTemplate.js?ver={$version}"></script>
<script type="text/javascript" src="/Utility/CoreScripts/Extend.js?ver={$version}"></script>
<script type="text/javascript" src="/Utility/CoreScripts/easydrag.js?ver={$version}"></script>
<script type="text/javascript" src="/Utility/CoreScripts/PageBox.js?ver={$version}"></script>
<script type="text/javascript" src="/Utility/CoreScripts/MessageBox.js?ver={$version}"></script>
<script type="text/javascript" src="/Utility/CoreScripts/Verify.js?ver={$version}"></script>
<script type="text/javascript" src="/Utility/CoreScripts/polyfill.min.js?ver={$version}"></script>
<script type="text/javascript" src="/Utility/CoreScripts/axios_min.js?ver={$version}"></script>
<script type="text/javascript" src="/Utility/CoreScripts/vue_min.js?ver={$version}"></script>
<script type="text/javascript" src="/Utility/CoreScripts/api.js?ver={$version}"></script>
<!--核心Css文件-->
<link rel="stylesheet" type="text/css" href="/Utility/iconfont/iconfont.css?ver={$version}"/>
<link rel="stylesheet" type="text/css" href="../CoreStyles/public.css?ver={$version}"/>
<link rel="stylesheet" type="text/css" href="/Utility/CoreStyles/PageBox.css?ver={$version}"/>
<link rel="stylesheet" type="text/css" href="/Utility/CoreStyles/MessageBox.css?ver={$version}"/>
<link rel="stylesheet" type="text/css" href="/Utility/CoreStyles/Jme.css?ver={$version}"/>
<!--公共的Js文件-->
<script type="text/javascript" src="../CoreScripts/public.js?ver={$version}"></script>
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
                        new PageBox("当前版本为免费版", "http://www.weishakeji.net/ad/lv0.html", 60, 80, "url").Open();
                    }
                }

            }
        });
</script>
<!--后面就是各个页面自己的了-->