<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <xsl:template match="/Platform">
    <html lang="zh">
      <head>
        <meta charset="utf-8" />
        <meta name="viewport" content="initial-scale=1,maximum-scale=1,user-scalable=no" />
        <meta name="viewport" content="width=device-width, initial-scale=1,maximum-scale=1.0, user-scalable=no" />
        <title>授权信息</title>
        <script type="text/javascript" src="/Utilities/Scripts/axios_min.js"></script>
        <script type="text/javascript" src="/Utilities/Scripts/vue.min.js"></script>
        <script type="text/javascript" src="/Utilities/Scripts/utils.js"></script>
        <script src="/Utilities/Scripts/api.js"></script>
        <script type="text/javascript" src="/Utilities/License/Scripts/Index.js"></script>
        <link type="text/css" rel="stylesheet" href="/Utilities/Fonts/icon.css"/>
        <link href="/Utilities/License/Styles/Index.css" rel="stylesheet" type="text/css"/>
      </head>

      <body>
        <div id="vapp">
          <header>
            <div class="topbar">
              <span class="title"><xsl:value-of select="AppDetails/item[@remark='团队介绍']"/></span>
               <!--右侧，联系方式与返回首页-->
              <div class="right">
                <span copyright="tel"></span>
                <a href="?action=refresh" class="fresh">刷新授权</a>
                <a copyright="website" class="brand"><xsl:value-of select="AppDetails/item[@remark='商标']"/></a>
                <a href="/" target="_blank"  class="home">首页</a>
              </div>
            </div>
            <section>             
                <span class="product">
                  <span><xsl:value-of select="AppDetails/item[@remark='产品名称']"/></span>
                  <span class="ver">ver <xsl:value-of select="AppDetails/item[@remark='版本号']"/></span>
                </span>  
                <span class="edition">
                  <xsl:value-of select="License/Edition"/>    
                  <xsl:if test="License/EdtionLevel = '0'">
                    <span class="stage">
                      - <xsl:value-of select="AppDetails/item[@remark='发布状态']"/>
                    </span>
                  </xsl:if> 
                  <xsl:if test="License/Label != ''">
                    <span class="label">（ <xsl:value-of select="License/Label"/> Edition ）</span>
                  </xsl:if> 
                </span>
            </section>
          </header>
          <!--头部结束，产品介绍，内容开始-->
          <div class="content">
            <p copyright="intro" class="tip"></p>
            <p copyright="detail" class="tip"></p>
          </div>
          <!--发行相关的信息-->
          <div class="release">
            <span title="内部版本号"><xsl:value-of select="AppDetails/item[@remark='内部版本号']"/></span>
            <span title="发布时间"><xsl:value-of select="AppDetails/item[@remark='发布时间']"/></span>
            <span title="初始运行"><xsl:value-of select="License/InitTime"/></span>
            <span title="已经运行"><xsl:value-of select="License/RunTime"/></span>
          </div>
          <!--中间内容-->
          <div class="context">           
            <div class="card lic" remark="授权信息">
              <div class="tit">
                    <!--社区版-->
                    <xsl:if test="License/EdtionLevel = '0'">社区版 
                      <span class="stage">
                          - <xsl:value-of select="AppDetails/item[@remark='发布状态']"/>
                      </span>
                    </xsl:if>
                     <!--商业版-->
                    <xsl:if test="License/EdtionLevel != '0'">
                        <xsl:value-of select="License/Edition"/>
                    </xsl:if>
                    <xsl:if test="License/Label != ''">
                        <span class="label">（ <xsl:value-of select="License/Label"/> Edition ）</span>
                    </xsl:if>  
              </div>
              <div class="cont">
                  <!--社区版-->
                  <xsl:if test="License/EdtionLevel = '0'">  
                    <xsl:if test="DataDetails/@dbType != 'SQLite'">
                        <alert large="true">
                          社区版仅限使用SQLite数据库，实际数据库 
                          <b><xsl:value-of select="DataDetails/@dbType"/></b>
                        </alert>             
                      </xsl:if>
                      <xsl:if test="AppDetails/item[@remark='发布状态'] = 'Trial'">
                        <xsl:if test="License/Trial != '0'">
                            <alert large="true"> 
                              发行状态为Trial，允许试用 
                              <xsl:value-of select="License/Trial"/>天，
                              已运行 <b><xsl:value-of select="License/RunTime"/></b>
                            </alert>  
                        </xsl:if>                  
                      </xsl:if>
                  </xsl:if>
                   <!--商业版-->
                  <xsl:if test="License/EdtionLevel != '0'">
                      <!--授权主题与时限-->                     
                      <p>
                          <xsl:variable name="lictype" select="License/Type"/>
                          授权主题：<xsl:value-of select="$lictype" />->
                          <xsl:value-of select="License/Serial" />
                          <xsl:if test="$lictype = 'IP' or $lictype = 'Domain' or $lictype = 'Root'">:
                              <xsl:value-of select="License/Port" />
                          </xsl:if>                          
                      </p>
                      <p>
                          授权时效：
                          <xsl:value-of select="License/StartTime" /> 至
                          <xsl:value-of select="License/EndTime" />
                      </p>
                      <!--账户数量超出允许的数量时的提示-->
                      <xsl:variable name="accCount" select="number(DataDetails/Accounts)"/>
                      <xsl:variable name="entCount" select="number(License/LimitItems/item[@entity])"/>
                      <xsl:if test="$accCount > $entCount">                
                        <alert large="true">当前版本限制 <xsl:value-of select="License/LimitItems/item[@entity]" /> 个账号，
                        已经存在账号数 <xsl:value-of select="DataDetails/Accounts" /> 个，
                        超出 <xsl:value-of select=" DataDetails/Accounts -License/LimitItems/item[@entity]" /> 个账号，
                        请删除多余的账号，或联系客服增加账号数量。
                        </alert>
                      </xsl:if>
                  </xsl:if>    
                  <!--公共内容-当前版本的限制数量-->
                  <p>当前版本所能承载的最大数据量，如下：</p>
                  <dl class="limitItems">
                        <xsl:for-each select="License/LimitItems/*">
                          <xsl:variable name="entity" select="@entity"/>
                          <xsl:variable name="value" select="text()"/>
                          <xsl:variable name="name" select="@name"/>
                          <dd entity="{$entity}" value="{$value}" name="{$name}">                            
                            <xsl:if test="text() = '0'">（不限）</xsl:if>
                            <xsl:if test="text() != '0'">
                              <xsl:value-of select="text()" />
                            </xsl:if> 
                          </dd>
                        </xsl:for-each>
                  </dl>
              </div>
            </div>
            <!--开源协议-->
            <div class="card protocol">
                <div class="tit">开源协议</div>
                <div class="cont">
                  <p>《<span copyright="product"></span>》采用双协议，社区版采用<span copyright="openlicense"></span>开源协议，商业版需要购买商业授权。</p>
                  <dl>
                    <dt class="free">社区版</dt>
                    <dd>
                      没有功能限制，没有学员注册数量限制，没有并发数限制。
                    </dd>
                    <dd>社区版使用SQLite数据库，安装部署简单。</dd>
                    <dd><alert>SQLite作为文件型数据库，在高并发、大数据场景下表现不佳，仅供交流学习，不建议用于商业用途。</alert></dd>
                  </dl>
                  <dl>
                    <dt class="business">商业版</dt>
                    <dd>与社区版功能相同，采用PostgreSQL数据库，且不同版本对注册量有一定限制（在线人数、并发量等没有限制）。</dd>
                    <dd>商业授权为永久授权，一次购买永久有效。</dd>
                  </dl>                 
                  <p class="opensourse">开源地址：
                    <a target="_blank" copyright="gitee">Gitee</a>
                    <a target="_blank" copyright="github">GitHub</a>
                    <a target="_blank" copyright="gitcode">Gitcode</a>
                  </p>
                </div>
            </div>
            <!--说明信息-->
            <div class="card explain">
              <div class="tit">说明</div>
                <div class="cont">
                  <p>1、如果当前版本无法满足您的需求，请联系<a target="_blank" copyright="taobao">在线销售（淘宝店）</a>，或致电 <span copyright="tel"></span>
                  </p>
                  <p>2、将下述激活码发给客服人员，客服将反馈给您授权文件，将其放置在站点根目录即可。</p>                                             
                  <p>3、主域授权的后缀仅限：<xsl:value-of select="License/LimitDomain"/>。</p>
                  <img copyright="weixinqr" width="120px" />
                </div>
            </div>
            <!-- 激活码  START-->
             <div class="card code">
              <div class="tit">激活码</div>
              <dl class="cont">
                <xsl:for-each select="Activationcode/*">
                  <dd>
                    <xsl:if test="local-name() = 'CPU'">CPU</xsl:if>
                    <xsl:if test="local-name() = 'HardDisk'">硬盘</xsl:if>
                    <xsl:if test="local-name() = 'IP'">IP</xsl:if>
                    <xsl:if test="local-name() = 'Domain'">域名</xsl:if>
                    <xsl:if test="local-name() = 'Root'">根域</xsl:if>
                    : <span><xsl:value-of select="@value"/></span>
                    <div><xsl:value-of select="text()" /></div>
                  </dd>
                </xsl:for-each>
              </dl>
            </div>
             <!-- 激活码  END  -->
          </div>

          <!-- 各版本功能对比  START-->
          <div class="card edition">
            <div class="tit">各版本详情</div>
            <table border="1" class="editions cont">
              <thead> 
                <tr>
                  <xsl:for-each select="Editions/level[index='1']/*">
                    <th>
                      <xsl:if test="local-name() = 'index'">#</xsl:if>
                      <xsl:if test="local-name() != 'index'">
                        <xsl:value-of select="local-name()" />
                      </xsl:if>
                    </th>
                  </xsl:for-each>
                </tr>
              </thead>
              <tbody>
                <xsl:for-each select="Editions/*">
                  <tr>
                    <xsl:for-each select="*">
                      <xsl:variable name="item" select="local-name()"/>
                      <td item="{$item}">
                        <xsl:if test="text() = '0'">-</xsl:if>
                        <xsl:if test="text() != '0'">
                          <xsl:value-of select="text()" />
                        </xsl:if>
                        <xsl:if test="local-name() = '版本'"> Edition</xsl:if>
                      </td>
                    </xsl:for-each>
                  </tr>
                </xsl:for-each>                                  
              </tbody>
            </table>
          </div>
          <!-- 各版本功能对比  END  -->
        <footer>
          <div class="copyright">Copyright © <a copyright="website" target="_blank"><span copyright="company"></span></a> All Rights Reserved.</div>
          <div><xsl:value-of select="AppDetails/item[@remark='核心开发者']"/></div>
        </footer>
       
        </div>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>