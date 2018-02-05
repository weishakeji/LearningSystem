<%@ Page Language="C#" MasterPageFile="~/Manage/PageWin.Master" AutoEventWireup="true"
    Codebehind="Video_Edit.aspx.cs" Inherits="Song.Site.Manage.Content.Video_Edit"
    Title="无标题页" %>

<%@ MasterType VirtualPath="~/Manage/PageWin.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<%@ Register Assembly="WeiSha.WebEditor" Namespace="WeiSha.WebEditor" TagPrefix="WebEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">

    <script language="javascript" src="../Utility/datepicker/WdatePicker.js" type="text/javascript"></script>

    <script language="javascript" src="scripts/tab.js" type="text/javascript"></script>

    <div id="tabTitleBox">
        <div class="titItem curr" tab="tab1">
            基本信息</div>
        <div class="titItem" tab="tab2">
            详细信息</div>
        <div class="titItem" tab="tab3">
            图片介绍</div>
        <div class="titItem" tab="tab4">
            发布设置</div>
        <div class="titItem" tab="tab5">
            自定义信息</div>
    </div>
    <div id="tabContextBox">
        <div class="contItem" tab="tab1">
            <%--flash视频--%>

            <script src="Scripts/AC_RunActiveContent.js" type="text/javascript"></script>

            <div id="flashShowBox" runat="server" class="showBox">
                <div id="videoError" runat="server" class="videoError" visible="false">
                    <span>视频不存在，可能是路径设置不正确！</span><br />
                    <br />
                    请参考如下意见：
                    <br />
                    1、视频文件建议放置到网站根目录
                    <asp:Label ID="lbDir1" runat="server" Text=""></asp:Label>
                    之下；<br />
                    2、“视频地址”需要输入完整路径名；
                </div>
                <div id="videoBox" runat="server" class="videoBox">

                    <script type="text/javascript">
AC_FL_RunContent( 'type','application/x-shockwave-flash','width','400px','height','300px','data','images/flvplayer.swf?file=<%= flv %>','movie','images/flvplayer?file=<%= flv %>&showfsbutton=true&autostart=true','wmode','transparent','quality','high','allowfullscreen','true' ); //end AC code
                    </script>

                    <noscript>
                        <object type="application/x-shockwave-flash" width="400px" height="300px" data="images/flvplayer.swf?file=<%= flv %>">
                            <param name="movie" value="images/flvplayer.swf?file=<%= flv %>&showfsbutton=true&autostart=true" />
                            <param name="wmode" value="transparent" />
                            <param name="quality" value="high" />
                            <param name="allowfullscreen" value="true" />
                        </object>
                    </noscript>
                </div>
            </div>
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="80" class="right">
                        视频名称：</td>
                    <td>
                        <asp:TextBox ID="tbName" runat="server" Width="95%" MaxLength="255" nullable="false"  group="ent"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="right">
                        视频地址：</td>
                    <td>
                        <asp:TextBox ID="tbVideo" nullable="false" runat="server" group="ent" Width="95%" MaxLength="255"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="right">
                        栏目分类：</td>
                    <td>
                        <asp:DropDownList ID="ddlColumn" runat="server" Width="150">
                        </asp:DropDownList></td>
                </tr>
                <tr>
                    <td class="right">
                    </td>
                    <td>
                        <asp:CheckBox ID="cbIsShow" runat="server" Checked="True" Text="是否显示" />
                        <asp:CheckBox ID="cbIsRec" runat="server" Checked="True" Text="是否推荐" />
                        <asp:CheckBox ID="cbIsHot" runat="server" Checked="True" Text="热点" Visible="false" />
                        <asp:CheckBox ID="cbIsTop" runat="server" Checked="True" Text="置顶" Visible="false" /></td>
                </tr>
                  <tr>
                    <td class="right">
                        标签：</td>
                    <td>
                       <asp:TextBox ID="tbLabel" runat="server" Width="95%" MaxLength="255"></asp:TextBox></td>
                </tr>
            </table>
        </div>
         <div class="contItem" tab="tab2" style="display: none">
            <WebEditor:Editor ID="tbDetails" runat="server" Height="450px" Width="100%"> </WebEditor:Editor>
        </div>
        <!--图片管理-->
        <div class="contItem" tab="tab3" style="display: none">
            <div>
                <cc1:FileUpload ID="fuLoad" runat="server" nullable="false"  fileallow="jpg|bmp|gif|png" group="upfile"
                    Width="400" />
                <asp:Button ID="btnUpFile" runat="server" Text="上传" group="upfile" verify="true"
                    OnClick="btnUpFile_Click" />
            </div>
            <div class="imgList">
                <asp:Repeater ID="rptPict" runat="server">
                    <HeaderTemplate>
                        <dl>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <dd class="imgBox" style="width: 146px">
                            <div class="picShow" style="width: 140px; height: 140px;background-image:url(<%# Eval("Pic_FilePathSmall", "{0}") %>)"
                                imgid="<%# Eval("Pic_Id") %>">
                                <div style="display:<%# Eval("Pic_IsCover","{0}")=="True" ? "block" : "none" %>" class="cover" >封面</div></div>
                            <div class="control">
                                <asp:LinkButton ID="lbCover" runat="server" CommandArgument='<%# Eval("Pic_Id") %>'
                                    OnClick="lbCover_Click">设为封面</asp:LinkButton>
                                <asp:LinkButton ID="lbDel" runat="server" OnClientClick="return confirm('您确定删除吗？')"
                                    CommandArgument='<%# Eval("Pic_Id")%>' OnClick="lbDel_Click">删</asp:LinkButton>
                                <cc1:StateButton ID="sbUse" OnClick="sbShow_Click" runat="server" TrueText="显" FalseText="隐"
                                    State='<%# Eval("Pic_IsShow","{0}")=="True"%>' CommandArgument='<%# Eval("Pic_Id")%>'></cc1:StateButton>
                            </div>
                        </dd>
                    </ItemTemplate>
                    <FooterTemplate>
                        </dl>
                    </FooterTemplate>
                </asp:Repeater>
            </div>
        </div>
        <div class="contItem" tab="tab4" style="display: none">
            <!--发布设置-->
            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td width="80px" class="right">
                        Keywords：</td>
                    <td>
                        <asp:TextBox ID="tbKeywords" runat="server" MaxLength="255" Width="95%"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="right" valign="top">
                        Description：</td>
                    <td>
                        <asp:TextBox ID="tbDescr" runat="server" MaxLength="255" TextMode="MultiLine" lenlimit="250"
                            Width="95%" Height="100px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="right">
                        上线时间：</td>
                    <td>
                        <asp:TextBox ID="tbPushTime" runat="server" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})"
                            MaxLength="255" Width="160"></asp:TextBox></td>
                </tr>
            </table>
        </div>
        <div class="contItem" tab="tab4" style="display: none">
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBtn" runat="server">
    <cc1:EnterButton verify="true" ID="btnEnter" runat="server" Text="确定" OnClick="btnEnter_Click"
        ValidationGroup="enter" group="ent"/>
    <%--<Song:DeleteButton ID="DeleteButton1" runat="server" OnClick="btnDelete_Click" />--%>
    <cc1:CloseButton ID="CloseButton1" runat="server" />
</asp:Content>
