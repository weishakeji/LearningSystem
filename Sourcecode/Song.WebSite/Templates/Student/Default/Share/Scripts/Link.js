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
            copy: function (val, title) {
                navigator.clipboard.writeText(val)
                    .then(() => {
                        vapp.$message({
                            message: '复制 “' + title + '” 到粘贴板',
                            type: 'success'
                        });
                    })
                    .catch(err => {
                        if (textbox == null) textbox = 'input';
                        var oInput = document.createElement('textarea');
                        document.body.appendChild(oInput);
                        oInput.value = val;
                        oInput.select(); // 选择对象
                        document.execCommand("Copy"); // 执行浏览器复制命令           
                        oInput.style.display = 'none';
                    });

            },
            buildQrcode: function () {
                //生成课程二维码
                var url = this.shareurl();
                jQuery('#qrcode').qrcode({
                    render: "canvas", //也可以替换为table
                    width: 150,
                    height: 150,
                    foreground: "#666",
                    background: "#FFF",
                    text: url
                });
            }
        }
    });

}, ['/Utilities/Scripts/jquery.qrcode.min.js']);
