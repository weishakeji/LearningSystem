<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <xsl:template match="/">
    <html lang="zh">

      <head>
        <meta charset="utf-8" />
        <meta name="viewport" content="initial-scale=1,maximum-scale=1,user-scalable=no" />
        <meta name="viewport" content="width=device-width, initial-scale=1,maximum-scale=1.0, user-scalable=no" />
        <title>授权信息</title>
        <script src="/Utilities/Scripts/jquery.js"></script>
        <script src="/Utilities/Scripts/axios_min.js"></script>
        <style>
        html,
        body {
          margin: 0px;
          padding: 0px;
          text-align: center;
          background-color: #eee;
          width: 100%;
          height: 100%;
          overflow: hidden;
        }

        dl,
        dd {
          margin: 0px;
          padding: 0px;
        }

        body,
        p,
        div,
        * {
          line-height: 25px;
          font-family: "Microsoft Yahei", "微软雅黑", Tahoma, Arial, Helvetica, STHeiti;
        }

        a {
          text-decoration: none;
          color: rgb(57, 57, 255);
        }

        *::-webkit-scrollbar {
          /*滚动条整体样式*/
          width: 3px;
          /*高宽分别对应横竖滚动条的尺寸*/
          height: 1px;
        }

        *::-webkit-scrollbar-thumb {
          /*滚动条里面小方块*/
          border-radius: 3px;
          -webkit-box-shadow: inset 0 0 5px rgba(133, 131, 131, 0.2);
          background: #808080;
        }

        *::-webkit-scrollbar-track {
          /*滚动条里面轨道*/
          -webkit-box-shadow: inset 0 0 5px rgba(133, 131, 131, 0.2);
          border-radius: 3px;
          background: #EDEDED;
        }

        header {
          background: #666;
          color: #CCC;
          width: calc(100% - 3px);
          height: 100px;
          line-height: 30px;
          position: absolute;
          z-index: 3;
          top: 0px;
          left: 0px;
          box-shadow: 0 2px 12px 0 rgba(165, 159, 159, 0.8);

        }

        header>span {
          color: #CCFF66;
        }

        header .card {
          margin-top: 0px !important;
        }

        .verInfo {
          background-color: #999;
          min-height: 80px;
          max-width: 780px;
          margin-right: auto;
          margin-left: auto;
          padding: 10px;
          box-sizing: border-box;
          -moz-box-sizing: border-box;
          -webit-box-sizing: border-box;
          position: relative;
        }

        section {
          position: absolute;
          width: 100%;
          height: calc(100% - 295px);
          overflow-x: hidden;
          overflow-y: scroll;
          top: 200px;
          text-align: center;
          padding-bottom: 50px;
        }

        .context {
          margin-left: auto;
          max-width: 820px;
          margin-right: auto;
        }

        .title {
          line-height: 50px;
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
          font-weight: bold;
          color: #ff0000;
          height: 50px;
          line-height: 40px;
          width: calc(100% - 220px);
          float: left;
          text-align: left;
          text-shadow: 2px 2px 5px #fff;
        }

        .title .titleRight {
          height: 50px;
          width: 200px;
          float: right;
          text-align: right;
        }

        .licInfo {
          width: 100%;
          text-align: left;
          position: absolute;
          left: 10px;
          bottom: 0px;
          font-size: 12px;
          color: #cecece;
        }

        .licInfo span {
          margin-right: 10px;
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
          margin: 10px 20px 20px 20px;
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

        .Activationcode {
          counter-reset: show-list;
          list-style-type: none;
          counter-reset: sectioncounter;
        }

        .Activationcode dd {
          margin-bottom: 10px;

        }

        .Activationcode dd:before {
          content: counter(sectioncounter) "、 ";
          counter-increment: sectioncounter;
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

        dl.card-cont dd {
          width: 100%;
        }

        dl.card-cont dd * {
          line-height: 30px;
        }

        dl.card-cont dd div:first-child {
          float: left;
          width: 80px;

        }

        table.card-cont {
          width: 100%;
          border: none;
        }

        table.card-cont td {
          text-align: center;
line-height:35px;
        }

        table.card-cont th,
        table.card-cont td {
          border: none;
        }
        </style>
      </head>

      <body>
        <header>
        系统于
          <xsl:for-each select="LicenseInfo/license/InitTime">
            <span>
              <xsl:value-of select="text()" />
            </span>
          </xsl:for-each>
        开始运行，已经运行
          <xsl:for-each select="LicenseInfo/license/RunTime">
            <span>
              <xsl:value-of select="text()" />
            </span>
          </xsl:for-each>
          <div class="context">
            <div class="card">
              <div class="verInfo">
                <div class="title">
                  <div class="titleLeft">
                    <xsl:for-each select="LicenseInfo/license/Edition">
                      <xsl:value-of select="text()" />
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
                   
                      <xsl:for-each select="LicenseInfo/Versions/ReleaseDate">
                        <xsl:value-of select="text()" />
                      </xsl:for-each>
                    </div>
                    <div class="spanStatus">

                      <xsl:for-each select="LicenseInfo/Versions/Stage">
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
                  <a href="?action=refresh">刷新</a>
                </div>
              </div>
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
          </div>
        </header>
        <section>
          <div class="context">


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
              <dl class="card-cont Activationcode">
                <xsl:for-each select="LicenseInfo/Activationcode/*">
                  <dd>
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
                    <br />
                    <xsl:value-of select="text()" />
                  </dd>

                </xsl:for-each>
              </dl>
            </div>
            <div class="card intro">
              <div class="card-tit">各版本功能对比</div>
              <table border="1" class="card-cont">
                <tr>
                  <xsl:for-each select="LicenseInfo/Editions/level[index='1']/*">
                    <th>
                      <xsl:if test="local-name() = 'index'">
                      #
                      </xsl:if>
                      <xsl:if test="local-name() != 'index'">
                        <xsl:value-of select="local-name()" />
                      </xsl:if>
                    </th>

                  </xsl:for-each>
                </tr>

                <xsl:for-each select="LicenseInfo/Editions/*">
                  <tr>
                    <xsl:for-each select="*">
                      <td>
                        <xsl:value-of select="text()" />
                      </td>
                    </xsl:for-each>
                  </tr>
                </xsl:for-each>

              </table>
            </div>
          </div>
        </section>
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
      <script src="/Utilities/Scripts/api.js"></script>
    </html>
  </xsl:template>

</xsl:stylesheet>