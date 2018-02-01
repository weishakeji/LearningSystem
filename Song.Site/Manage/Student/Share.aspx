<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Share.aspx.cs" Inherits="Song.Site.Manage.Student.Share"
    MasterPageFile="~/Manage/Student/Parents.Master" %>
<%@ MasterType VirtualPath="~/Manage/Student/Parents.Master" %>
<%@ Register Assembly="WeiSha.WebControl" Namespace="WeiSha.WebControl" TagPrefix="cc1" %>
<%@ Register Assembly="WeiSha.WebEditor" Namespace="WeiSha.WebEditor" TagPrefix="WebEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
<script src="../CoreScripts/clipboard.min.js"></script>
   <a href="<%=shareurl %>" target="_blank"><%=shareurl %></a> <span class="copy" data-clipboard-text="<%=shareurl %>" id="btn">复制链接</span>
    <script>
        var btn = document.getElementById('btn');
        var clipboard = new Clipboard(btn); //实例化

        //复制成功执行的回调，可选
        clipboard.on('success', function (e) {
            //console.log(e);
            alert("成功复制分享链接到剪贴板");
        });

        //复制失败执行的回调，可选
        clipboard.on('error', function (e) {
            //console.log(e);
            alert("未能复制分享链接到剪贴板，请手工复制");
        });
    </script>
   <br />
    <br /><br /><br />
    分享二维码：<br />
   <img src="data:image/JPG;base64,<%=shareimg %>" />
</asp:Content>
