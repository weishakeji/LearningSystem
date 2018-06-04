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
                var scene = op.attr("scene");  //支付应用场景
                window.location.href = path + op.val() + "?id=" + $().getPara("id") + "&scene=" + scene;
            }
        }
        //当更改时跳转
        ddl.change(function () {
            var page = $.trim($(this).val());  //当前选中项的value值，即需要转向的页面
            if (page == "") return;
            var href = window.location.href;
            var path = href.substring(0, href.lastIndexOf("/") + 1);
            var option = $(this).find("option[value=" + page + "]");
            var scene = option.attr("scene");  //支付应用场景
            window.location.href = path + $(this).val() + "?id=" + $().getPara("id") + "&scene=" + scene;
        });
    });
//-->
</script>
<asp:DropDownList ID="ddlInterFace" runat="server">
    <asp:ListItem Value="" Selected="True">--选择支付接口--</asp:ListItem>
    <asp:ListItem Value="alipaywap.aspx" scene="alipay">支付宝手机支付</asp:ListItem>
    <asp:ListItem Value="alipayweb.aspx" scene="alipay">支付宝网页直付</asp:ListItem>
    <asp:ListItem Value="weixinpubpay.aspx" scene="weixin,public">微信公众号支付</asp:ListItem>
    <asp:ListItem Value="weixinnativepay.aspx" scene="weixin,native">微信扫码支付</asp:ListItem>
    <asp:ListItem Value="WeixinAppPay.aspx" scene="weixin,native">微信小程序支付</asp:ListItem>
</asp:DropDownList>
