$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            tag: $api.querystring('tag'),
            type: $api.querystring('type'),
            appid: $api.querystring('appid'),
            redirect_uri: $api.querystring('redirect_uri')
        },
        mounted: function () {
            this.showQrcode();
        },
        created: function () {
        },
        computed: {},
        watch: {},
        methods: {
            //显示二维码
            showQrcode: function (obj) {
                var obj = new WxLogin({
                    self_redirect: true,
                    id: "login_container",
                    appid: this.appid,
                    scope: "snsapi_login",
                    redirect_uri: this.redirect_uri,
                    state: this.tag + ',' + this.type,
                    style: "",
                    href: ""
                });
            }
        }
    });

}, ['https://res.wx.qq.com/connect/zh_CN/htmledition/js/wxLogin.js']);
