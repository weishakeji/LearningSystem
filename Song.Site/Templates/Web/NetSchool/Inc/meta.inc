<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<meta name="Keywords"  content="{$org.Org_Keywords},微厦科技" />
<meta name="Description" content="{$org.Org_Description},微厦科技" />
<!--核心Js文件-->
<script type="text/javascript" src="/Utility/CoreScripts/jquery.js"></script>
<script type="text/javascript" src="/Utility/CoreScripts/artTemplate.js"></script>
<script type="text/javascript" src="/Utility/CoreScripts/Extend.js"></script>
<script type="text/javascript" src="/Utility/CoreScripts/easydrag.js"></script>
<script type="text/javascript" src="/Utility/CoreScripts/PageBox.js"></script>
<script type="text/javascript" src="/Utility/CoreScripts/MessageBox.js"></script>
<script type="text/javascript" src="/Utility/CoreScripts/Verify.js"></script>
<!--核心Css文件-->
<link rel="stylesheet" type="text/css" href="/Utility/iconfont/iconfont.css"/>
<link rel="stylesheet" type="text/css" href="../CoreStyles/public.css"/>
<link rel="stylesheet" type="text/css" href="/Utility/CoreStyles/PageBox.css"/>
<link rel="stylesheet" type="text/css" href="/Utility/CoreStyles/MessageBox.css"/>
<link rel="stylesheet" type="text/css" href="/Utility/CoreStyles/Jme.css"/>
<!--公共的Js文件-->
<script type="text/javascript" src="../CoreScripts/public.js"></script>
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