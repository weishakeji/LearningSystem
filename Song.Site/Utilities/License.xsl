<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <xsl:template match="/">
    <html>

      <head>
        <meta charset="utf-8" />
        <meta name="viewport" content="initial-scale=1,maximum-scale=1,user-scalable=no" />
        <meta name="viewport" content="width=device-width, initial-scale=1,maximum-scale=1.0, user-scalable=no" />
        <title>授权信息</title>
        <script src="/Utilities/Scripts/jquery.js"></script>
        <style>
          body {
          margin: 0px;
          padding: 0px;
          text-align: center;
          background-color: #eee;
          }
          dl,dd{
          margin:0px;
          padding: 0px;
          }
          body,
          p,
          div,
          * {
          font-size: 16px;
          line-height: 25px;
          font-family: "Microsoft Yahei", "微软雅黑", Tahoma, Arial, Helvetica, STHeiti;
          }

          a {
          text-decoration: none;
          color: blue;
          }

          #header {
          background: #666;
          width: 100%;
          min-height: 160px;
          line-height: 30px;
          z-index: 9998;

          top: 0px;
          box-shadow: 0 2px 12px 0 rgba(165, 159, 159, 0.8);

          }

          #header .top {
          max-width: 780px;
          margin-right: auto;
          margin-left: auto;
          color: #CCC;
          padding: 10px;
          }

          #header .top span {
          color: #CCFF66;
          }

          #header .verInfo {
          background-color: #999;
          min-height: 100px;
          max-width: 780px;
          margin-right: auto;
          margin-left: auto;
          padding: 10px;       
          box-sizing: border-box;
          -moz-box-sizing: border-box;
          -webit-box-sizing: border-box;
          position: relative;
          }

          .title {
          line-height: 50px;
          font-weight: bold;
          color: #333333;
          min-height: 50px;
          width: 100%;
          margin-left: auto;
          max-width: 780px;
          margin-right: auto;
          display: table;
          }

          .title .titleLeft {
          font-size: 28px;
          line-height: 50px;
          font-weight: bold;
          color: #333333;
          height: 50px;
          margin-left: 0px;
          width: 300px;
          float: left;
          }

          .title .titleLeft span {

          font-size: 28px;
          line-height: 50px;
          font-weight: bold;
          color: #333333;
          text-shadow: 2px 2px 5px #fff;
          }

          .title .titleRight {
          height: 50px;
          width: 300px;
          float: left;
          }

          .licInfo {
          width: 100%;
          text-align: left;
          margin-left:10px;
          }

          .licInfo span {
          margin-right: 10px;
          }

          .context {
          margin-bottom: 100px;
          margin-left: auto;
          max-width: 820px;
          margin-right: auto;
          margin-top: 20px;

          }

          .spanVersion {
          font-size: 15px !important;
          height: 20px;
          line-height: 25px;
          }

          .explain {
          line-height: 20px;
          text-align: left;
          }

          #footer {
          position: fixed;
          bottom: 0;
          background: #000;
          width: 100%;
          height: 50px;
          z-index: 9999;
          opacity: .80;
          filter: alpha(opacity=80);
          }

          #footer a {
          color: #eee;
          font-size: 16px;
          letter-spacing: 2px;
          margin-right: 0px;
          margin-left: 0px;
          }

          .copyright {
          line-height: 50px;
          font-family: arial;
          margin-left: 10px;
          color: #eee;
          text-align: center;
          }

          /*卡片盒的样式*/
          .card {
          background-color: #FFFFFF;
          margin: 0px;
          padding: 0px;
          text-align: left;
          box-shadow: 0 2px 12px 0 rgba(165, 159, 159, 0.8);
          border: 1px solid #d5d7dc;
          color: #303133;
          -webkit-transition: .3s;
          transition: .3s;
          border-radius: 4px;
          overflow: hidden;
          margin-top: 20px;
          margin: 20px;
          }

          .card .card-tit {
          padding: 15px;
          border-bottom: 1px solid #d5d7dc;
          -webkit-box-sizing: border-box;
          box-sizing: border-box;
          font-size: 16px;
          font-weight: bold;
          }

          .card .card-cont {
          padding: 15px;
          padding-left: 30px;
          }

          /*当前版本承载量*/
          dl.limitItems {
          display: table;
          }

          dl.limitItems dd {
          float: left;
          margin-left: 0px;
          width: 120px;
          margin-right: 20px;
          }

          /*说明*/
          .intro p {
          margin: 0px;
          line-height: 30px;
          word-break: break-all;


          }
          dl.card-cont dd{
          width:100%;
          }
          dl.card-cont dd *{
          line-height:30px;
          }
          dl.card-cont dd div:first-child{
          float:left;
          width:80px;

          }
        </style>
      </head>

      <body id="app">
        <div id="header">
          <div class="top">

          系统于
            <xsl:for-each select="LicenseInfo/license/InitDate">
              <span>
                <xsl:value-of select="text()" />
              </span>
            </xsl:for-each>
          开始运行，已经正常运行
            <xsl:for-each select="LicenseInfo/license/RunTimeSpan">
              <span>
                <xsl:value-of select="text()" />
              </span>
            </xsl:for-each>
          </div>
          <div class="verInfo">
            <div class="title">
              <div class="titleLeft">
              当前系统副本为

                <xsl:for-each select="LicenseInfo/license/VersionName">
                  <span id="lbVersion" style="color:Red;">
                    <xsl:value-of select="text()" />
                  </span>
                </xsl:for-each>

              </div>
              <div class="titleRight">
                <div class="spanVersion">
                Version
                  <xsl:for-each select="LicenseInfo/Versions/Version">
                    <xsl:value-of select="text()" />
                  </xsl:for-each>
                </div>
                <div class="spanVersion">
                ReleaseDate
                  <xsl:for-each select="LicenseInfo/Versions/ReleaseDate">
                    <xsl:value-of select="text()" />
                  </xsl:for-each>
                </div>
              </div>
            </div>

            <div class="licInfo">

              <xsl:if test="LicenseInfo/license/VersionLevel != '0'">

                <xsl:for-each select="LicenseInfo/license/Type">
                  <span>授权类型：
                    <xsl:value-of select="text()" />
                  </span>
                </xsl:for-each>

                <span>（
                  <xsl:for-each select="LicenseInfo/license/Serial">
                    <xsl:value-of select="text()" />
                  </xsl:for-each>:
                  <xsl:for-each select="LicenseInfo/license/Port">
                    <xsl:value-of select="text()" />
                  </xsl:for-each>）
                </span>

                <span>授权时效：
                  <xsl:for-each select="LicenseInfo/license/StartTime">
                    <xsl:value-of select="substring(text(), 1, 11)" />
                  </xsl:for-each>
                至
                  <xsl:for-each select="LicenseInfo/license/EndTime">
                    <xsl:value-of select="substring(text(), 1, 11)" />
                  </xsl:for-each>
                </span>

              </xsl:if>
              <xsl:if test="LicenseInfo/license/VersionLevel = '0'">
              未授权版本！
              </xsl:if>
            </div>
          </div>
        </div>
        <!-- 头部结束-->
        <div class="context">
          <div class="card">
            <div class="card-tit">当前版本所能承载的最大数据量</div>
            <div class="card-cont">
              <dl class="limitItems">
                <xsl:for-each select="LicenseInfo/license/LimitItems/*">
                  <dd>
                    <xsl:value-of select="@value" />
：
                    <xsl:if test="text() = '0'">
                    不限
                    </xsl:if>
                    <xsl:if test="text() != '0'">
                      <xsl:value-of select="text()" />
个
                    </xsl:if>
                  </dd>
                </xsl:for-each>
              </dl>
            </div>
          </div>
          <div class="card intro">
            <div class="card-tit">说明</div>
            <div class="card-cont">
              <p>1、如果上述版本的功能无法满足您的需求，升级请联系<a href="https://shop35387540.taobao.com/" target="_blank" copyright="taobao">在线销售（淘宝店）</a>
              </p>
              <p>2、升级方法：将下述激活码发给客服人员，客服将反馈给您授权文件，将其放置在站点根目录即可。</p>
              <p>3、授权说明：主域名授权仅限.net、.com、.org、.cn、.me、.site、.co、.cc、.info、.net.cn、.com.cn、.org.cn。</p>
            </div>
          </div>
          <div class="card intro">
            <div class="card-tit">激活码</div>
            <dl class="card-cont">
              <xsl:for-each select="LicenseInfo/Activationcode/*">
                <dd>
                  <div class="left">
                    <xsl:if test="local-name() = 'CPU'">
                CPU
                    </xsl:if>
                    <xsl:if test="local-name() = 'HardDisk'">
                硬盘
                    </xsl:if>
                    <xsl:if test="local-name() = 'IP'">
                IP
                    </xsl:if>
                    <xsl:if test="local-name() = 'Domain'">
                域名
                    </xsl:if>
                    <xsl:if test="local-name() = 'Root'">
                根域
                    </xsl:if>
                  </div>

                  <div class="right">
                    <xsl:value-of select="text()" />
                  </div>
                </dd>
              </xsl:for-each>           

            </dl>
          </div>

        </div>

        <div id="footer">
          <div class="copyright">
          Copyright © 2014-2021
            <a href="http://www.weishakeji.net" target="_blank" copyright="url">
              <span copyright="powerby"></span>
            </a>
          . All rights reserved
          </div>
        </div>
      </body>
      <script type="text/javascript">
      $.get('/api/v1/Copyright/Info', function (req) {
        var d = eval("(" + req + ")");
        var copyright = d.result;
        /*for (var t in copyright) {
          copyright[t] = unescape(copyright[t]);
        }*/
        $("*[copyright]").each(function (index, element) {
          var name = $(this).get(0).tagName.toLowerCase(); //html元素的标签名         
          var val = $(this).attr("copyright"); 	//copyright的值，对应json的属性
          for (var attr in copyright) {
            if (attr == val) {
              var txt = unescape(copyright[attr]);
              switch (name) {
                case "a":
                  $(this).attr("href", txt);
                  break;
                case "img":
                  $(this).attr("src", txt);
                  break;
                default:
                  $(this).text(txt);
              }
            }
          }
        });
      })
      </script>

    </html>
  </xsl:template>

</xsl:stylesheet>