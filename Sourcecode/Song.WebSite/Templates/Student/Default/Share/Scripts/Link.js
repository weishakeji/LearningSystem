$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            account: {},
            loading_init: true
        },
        mounted: function () {
            var th = this;
            th.loading_init = true;
            $api.get('Account/Current').then(function (req) {
                th.loading_init = false;
                if (req.data.success) {
                    th.account = req.data.result;
                    th.$nextTick(function () {
                        th.buildQrcode();
                    });
                } else {
                    console.error(req.data.exception);
                    throw req.data.message;
                }
            }).catch(function (err) {
                Vue.prototype.$alert(err);
                console.error(err);
            });
        },
        created: function () {

        },
        computed: {

        },
        watch: {
        },
        methods: {
            //生成分享链接
            shareurl: function () {
                var url = window.location.origin;
                url = $api.url.set(url, { 'sharekeyid': this.account.Ac_ID });
                return url;
            },
            //复制到粘贴板
            copytext: function (val, title) {
                this.copy(val, 'textarea').then(function (th) {
                    th.$message({
                        message: '复制 “' + title + '” 到粘贴板',
                        type: 'success'
                    });
                });
            },
            buildQrcode: function () {
                //生成课程二维码
                var url = this.shareurl();
                //未使用的，生成二维码
                new QRCode($dom("#qrcode")[0], {
                    text: url,
                    width: 150,
                    height: 150,
                    colorDark: "#000000",
                    colorLight: "#ffffff",
                    render: "img",
                    correctLevel: QRCode.CorrectLevel.H
                });
            }
        }
    });

}, ['/Utilities/Scripts/qrcode.js']);
