<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PayInterfacerType.ascx.cs"
    Inherits="Song.Site.Manage.Pay.PayInterfacerType" %>
<script type="text/javascript">
<!--
    $(function () {
        var id = Number("<%= id %>");
        var ddl = $("select[name$=ddlInterFace]");
        //当前选中的直接跳转
        var op = ddl.find("option[selected]");
        if (op.size() == 1) {
            var href = window.location.href;
            var page = href.substring(href.lastIndexOf("/") + 1).toLowerCase();
            if (page.indexOf("?") > -1) page = page.substring(0, page.indexOf("?"));
            if ($.trim(op.val()) != "" && page != op.val().toLowerCase()) {
                var path = href.substring(0, href.lastIndexOf("/") + 1);
                window.location.href = path + op.val() + "?id=" + $().getPara("id");
            }
        }
        //当更改时跳转
        ddl.change(function () {
            if ($.trim($(this).val()) == "") return;
            var href = window.location.href;
            var page = href.substring(href.lastIndexOf("/")).toLowerCase();
            if (page.indexOf("?") > -1) page = page.substring(0, page.indexOf("?"));
            if ($.trim($(this).val()) != "" && page != $(this).val().toLowerCase()) {
                var path = href.substring(0, href.lastIndexOf("/") + 1);
                window.location.href = path + $(this).val() + "?id=" + $().getPara("id");
            }
        });
    });
//-->
</script>
<asp:DropDownList ID="ddlInterFace" runat="server">
    <asp:ListItem Value="" Selected="True">--选择支付接口--</asp:ListItem>
    <asp:ListItem Value="alipaywap.aspx">支付宝手机支付</asp:ListItem>
    <asp:ListItem Value="alipayweb.aspx">支付宝网页直付</asp:ListItem>
    <asp:ListItem Value="weixinpubpay.aspx">微信公众号支付</asp:ListItem>
</asp:DropDownList>
