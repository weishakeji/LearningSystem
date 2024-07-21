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
        <link href="/Utilities/License/Styles/Index.css" rel="stylesheet" type="text/css"/>
      </head>

      <body>

        <header>
        系统于
          <xsl:for-each select="Platform/License/InitTime">
            <span>
              <xsl:value-of select="text()" />
            </span>
          </xsl:for-each>
        开始运行，已经运行
          <xsl:for-each select="Platform/License/RunTime">
            <span>
              <xsl:value-of select="text()" />
            </span>
          </xsl:for-each>
          <div class="context">
            <div class="card">
              <div class="verInfo">
                <div class="title">
                  <div class="titleLeft">
                    <xsl:for-each select="Platform/License/Edition">
                      <xsl:value-of select="text()" />
                    </xsl:for-each>

                  </div>
                  <div class="titleRight">
                    <div class="spanVersion">
                    Version                      
                      <xsl:value-of select="/Platform/AppDetails/item[@remark='内部版本号']"/>
                    </div>
                    <div class="spanVersion">
                      <xsl:value-of select="/Platform/AppDetails/item[@remark='发布时间']"/>                   
                    </div>
                    <div class="spanStatus">
                      <xsl:value-of select="/Platform/AppDetails/item[@remark='发布状态']"/>
                    </div>
                  </div>
                </div>

                <div class="licInfo">

                  <xsl:if test="Platform/License/VersionLevel != '0'">

                    <xsl:for-each select="Platform/License/Type">
                      <span>授权类型：
                        <xsl:value-of select="text()" />
                      </span>
                    </xsl:for-each>

                    <span>（
                      <xsl:for-each select="Platform/License/Serial">
                        <xsl:value-of select="text()" />
                      </xsl:for-each>:
                      <xsl:for-each select="Platform/License/Port">
                        <xsl:value-of select="text()" />
                      </xsl:for-each>）
                    </span>

                    <span>授权时效：
                      <xsl:for-each select="Platform/License/StartTime">
                        <xsl:value-of select="substring(text(), 1, 11)" />
                      </xsl:for-each>
                    至
                      <xsl:for-each select="Platform/License/EndTime">
                        <xsl:value-of select="substring(text(), 1, 11)" />
                      </xsl:for-each>
                    </span>

                  </xsl:if>
                  <xsl:if test="Platform/License/VersionLevel = '0'">
                  未授权版本！
                  </xsl:if>
                  <a href="?action=refresh">刷新</a>
                </div>
              </div>
              <div class="card-cont">
                <dl class="limititems">
                  <xsl:for-each select="Platform/License/Limititems/*">
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
                <xsl:for-each select="Platform/Activationcode/*">
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
                  <xsl:for-each select="Platform/Editions/level[index='1']/*">
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

                <xsl:for-each select="Platform/Editions/*">
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