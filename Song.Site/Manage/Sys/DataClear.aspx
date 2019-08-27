<%@ Page Language="C#" MasterPageFile="~/Manage/ManagePage.Master" AutoEventWireup="true"
    Codebehind="DataClear.aspx.cs" Inherits="Song.Site.Manage.Sys.DataClear" Title="数据库备份" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <fieldset>
        <legend>内容数据清理</legend>
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td width="80" class="right">
                    资讯：</td>
                <td width="250">
                    <asp:LinkButton ID="lbtnNewsArticle" runat="server" OnClick="lbtnNewsArticle_Click">资讯内容</asp:LinkButton>
                    &nbsp;
                    <asp:LinkButton ID="lbtnNewsSort" runat="server">资讯分类</asp:LinkButton>
                    &nbsp;
                    <asp:LinkButton ID="lbtnSpec" runat="server">资讯专题</asp:LinkButton></td>
                <td width="80" class="right">
                    文章：
                </td>
                <td>
                    <asp:LinkButton ID="lbtnArticle" runat="server">文章内容</asp:LinkButton>
                    &nbsp;
                    <asp:LinkButton ID="lbtnArticleSort" runat="server">文章分类</asp:LinkButton></td>
            </tr>
            <tr>
                <td class="right">
                    图片：</td>
                <td>
                    <asp:LinkButton ID="lbtnPicture" runat="server">图片内容</asp:LinkButton>
                    &nbsp;
                    <asp:LinkButton ID="lbtnPicAlbum" runat="server">图片相册</asp:LinkButton>
                    &nbsp;
                    <asp:LinkButton ID="lbtnPicSort" runat="server">图片分类</asp:LinkButton></td>
                <td class="right">
                    视频：
                </td>
                <td>
                    <asp:LinkButton ID="lbtnVideo" runat="server">视频内容</asp:LinkButton>
                    &nbsp;
                    <asp:LinkButton ID="lbtnVideoAlbum" runat="server">视频相册</asp:LinkButton>
                    &nbsp;
                    <asp:LinkButton ID="lbtnVideoSort" runat="server">视频分类</asp:LinkButton></td>
            </tr>
            <tr>
                <td class="right">
                    产品：</td>
                <td>
                    <asp:LinkButton ID="lbtnProduct" runat="server">产品内容</asp:LinkButton>
                    &nbsp;
                    <asp:LinkButton ID="lbtnSort" runat="server">产品分类</asp:LinkButton></td>
                <td class="right">
                    下载：
                </td>
                <td>
                    <asp:LinkButton ID="lbtnDownload" runat="server">下载内容</asp:LinkButton>
                    &nbsp;
                    <asp:LinkButton ID="lbtnDownloadSort" runat="server">下载分类</asp:LinkButton></td>
            </tr>
        </table>
    </fieldset>
    <fieldset>
        <legend>网站信息清理</legend>
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td width="80" class="right">
                    基本信息：</td>
                <td width="250">
                    <asp:LinkButton ID="lbtnNotice" runat="server">网站公告</asp:LinkButton>
                    &nbsp;<asp:LinkButton ID="lbtnSite" runat="server">网站基本信息</asp:LinkButton>
                </td>
                <td width="80" class="right">
                    友情链接：
                </td>
                <td>
                    <asp:LinkButton ID="lbtnLink" runat="server">链接信息</asp:LinkButton>
                    &nbsp;
                    <asp:LinkButton ID="lbtnLinkSort" runat="server">链接分类</asp:LinkButton></td>
            </tr>
            <tr>
                <td class="right">
                    留言板：</td>
                <td>
                    <asp:LinkButton ID="lbtnMessageBook" runat="server">留言信息</asp:LinkButton>
                </td>
                <td class="right">
                    在线调查：
                </td>
                <td>
                    <asp:LinkButton ID="lbtnVote" runat="server">在线调查</asp:LinkButton>
                </td>
            </tr>
            <tr>
                <td class="right">
                    网站会员：</td>
                <td>
                    <asp:LinkButton ID="lbtnUser" runat="server">会员信息</asp:LinkButton>
                    &nbsp;
                    <asp:LinkButton ID="lbtnUserSort" runat="server">会员分类</asp:LinkButton></td>
                <td class="right">
                </td>
                <td>
                </td>
            </tr>
        </table>
    </fieldset>
    <fieldset>
        <legend>企业信息清理</legend>
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td width="80" class="right">
                    通讯录：</td>
                <td width="250">
                    <asp:LinkButton ID="lbtnAddressBook" runat="server" OnClick="lbtnAddressBook_Click">通讯录</asp:LinkButton>
                </td>
                <td width="80" class="right">
                    企业信息：
                </td>
                <td>
                    <asp:LinkButton ID="lbtnEnterprise" runat="server">企业基本信息</asp:LinkButton>&nbsp;
                    <asp:LinkButton ID="LinkEnterPriseIntro" runat="server">单位介绍</asp:LinkButton>
                </td>
            </tr>
            <tr>
                <td class="right">
                    组织机构：</td>
                <td>
                    <asp:LinkButton ID="lbtnDepart" runat="server">院系信息</asp:LinkButton>
                    &nbsp;
                    <asp:LinkButton ID="LinkButton2" runat="server">岗位信息</asp:LinkButton>
                    &nbsp;
                    <asp:LinkButton ID="LinkButton3" runat="server">工作组</asp:LinkButton>
                    &nbsp;
                    <asp:LinkButton ID="LinkButton4" runat="server">员工</asp:LinkButton></td>
                <td class="right">
                </td>
                <td>
                </td>
            </tr>
        </table>
    </fieldset>
    <fieldset>
        <legend>系统数据清理</legend>
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td width="80" class="right">
                    上传文件：</td>
                <td width="250">
                    <asp:LinkButton ID="lbtnUpload" runat="server">Upload文件夹清理</asp:LinkButton>
                </td>
                <td width="80" class="right">
                    统计：
                </td>
                <td>
                    <asp:LinkButton ID="lbtnCount" runat="server">访问量清零</asp:LinkButton>
                    &nbsp;
                    <asp:LinkButton ID="lbtnLog" runat="server">操作日志清零</asp:LinkButton></td>
            </tr>
              <tr>
                <td class="right">
                    垃圾清理：</td>
                <td>
                    <asp:LinkButton ID="LinkButton1" runat="server">压缩包</asp:LinkButton>
                    &nbsp;
                    <asp:LinkButton ID="LinkButton5" runat="server">辅助文件</asp:LinkButton></td>
                <td class="right">
                    
                </td>
                <td>
                   </td>
            </tr>
        </table>
    </fieldset>
</asp:Content>
