<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <xsl:template match="/">

    <html>

    <head>
      <meta charset="utf-8" />
      <meta name="viewport" content="initial-scale=1,maximum-scale=1,user-scalable=no" />
      <meta name="viewport" content="width=device-width, initial-scale=1,maximum-scale=1.0, user-scalable=no" />
      <title>版权信息</title>
      <style>
        body,
        p,
        div,
        * {
        font-size: 16px;
        line-height: 25px;
        }

        a {
        text-decoration: none;
        color: blue;
        }

        .context {
        max-width: 980px;
        padding: 20px;
        margin: 0px auto 0px auto;

        }

        .box {
        position: relative;
        padding: 30px 20px 20px 20px;
        margin: 0px 0px 10px 0px;
        box-shadow: 2px 1px #ddd;
        word-wrap : break-word;
        }

        .box span {
        display: block;
        position: absolute;
        height: 30px;
        line-height: 30px;
        font-size: 14px;
        color: #999;
        left: 10px;
        top: 0px;
        }

        .compay div {
        font-size: 25px;
        line-height: 40px;
        text-shadow: 2px 2px #ccc;
        font-weight: bold;
        }

        .readme {
        border: solid 1px #999;
        background-color: #ccc;
        box-shadow: 2px 2px #ccc;
        }
      </style>
    </head>

    <body>
      <div class="context">

        <xsl:for-each select="Copyright/*">
          <div>
            <xsl:attribute name="class">
              <xsl:text>box </xsl:text>
              <xsl:value-of select="name()" />
            </xsl:attribute>
            <span class="remark">&#8727;
              <xsl:value-of select="@remark" />
            </span>

            <!--如果是图片-->
            <xsl:if test="./@type = 'image'">
              <div>
                <xsl:value-of select="child::text()" disable-output-escaping="yes" />
              </div>
              <image src="">
                <xsl:attribute name="src">
                  <xsl:text></xsl:text>
                  <xsl:value-of select="child::text()" />
                </xsl:attribute>
              </image>
            </xsl:if>
            <!--如果是超链接-->
            <xsl:if test="./@type = 'link'">
              <a href="" target="_blank">
                <xsl:attribute name="href">
                  <xsl:text></xsl:text>
                  <xsl:value-of select="child::text()" />
                </xsl:attribute>
                <div>
                  <xsl:value-of select="child::text()" disable-output-escaping="yes" />
                </div>
              </a>
            </xsl:if>
            <!--如果是文本-->
            <xsl:if test="./@type = 'text'">
              <div>
                <xsl:value-of select="child::text()" disable-output-escaping="yes" />
              </div>
            </xsl:if>
          </div>
        </xsl:for-each>
      </div>
    </body>

    </html>
  </xsl:template>

</xsl:stylesheet>