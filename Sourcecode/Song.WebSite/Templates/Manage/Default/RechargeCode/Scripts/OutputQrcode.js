$ready(function () {
    window.vue = new Vue({
        el: '#app',
        data: {
            id: $api.querystring('id'),
            codeset: {},
            codes: [],
            organ: {},       //学习卡
            loading: false
        },
        watch: {
            'codes': function (nl, ov) {
                var th = this;
                //数据变动，且渲染完成后执行
                this.$nextTick(function () {
                    var imgused = $dom(".qrcode-area>img#used").attr("src");
                    var imgdisable = $dom(".qrcode-area>img#disable").attr("src");
                    var url = window.location.origin + "/mobile/Learningcard.ashx?code={code}&pw={pw}";
                    for (var i = 0; i < th.codes.length; i++) {
                        var card = th.codes[i];
                        var dd = $dom(".qrcode-area dl dd#code_" + card.Rc_ID);
                        var code = dd.attr("code");
                        var pw = dd.attr("pw");
                        if (dd.attr("used")) {
                            //已经使用过的学，显示“已经使用”的图片
                            dd.find(".qrcodeimg").add("img").attr("src", imgused);
                            continue;
                        }
                        if (dd.attr("disable")) {
                            //被禁用的，显示“已经使用”的图片
                            dd.find(".qrcodeimg").add("img").attr("src", imgdisable);
                            continue;
                        }
                        //未使用的，生成二维码
                        new QRCode(dd.find(".qrcodeimg")[0], {
                            text: url.replace("{code}", code).replace("{pw}", pw),
                            width: 200,
                            height: 200,
                            colorDark: "#000000",
                            colorLight: "#ffffff",
                            render: "img",
                            correctLevel: QRCode.CorrectLevel.H
                        });
                    }
                });
            }
        },
        created: function () {
            var th = this;
            this.loading = true;
            $api.bat(
                $api.get('RechargeCode/SetForID', { 'id': this.id }),
                $api.get('RechargeCode/Codes', { 'rsid': this.id })
            ).then(([codeset, codes]) => {
                //获取结果
                th.codeset = codeset.data.result;
                th.codes = codes.data.result;
                $api.get('Organization/ForID', { 'id': th.codeset.Org_ID }).then(function (req) {
                    if (req.data.success) {
                        th.organ = req.data.result;
                    } else {
                        $api.get('Organization/Current').then(function (req) {
                            if (req.data.success) {
                                th.organ = req.data.result;
                            } else {
                                console.error(req.data.exception);
                                throw req.data.message;
                            }
                        }).catch(function (err) {
                            alert(err);
                            console.error(err);
                        });
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });
            }).catch(err => console.error(err))
                .finally(() => th.loading = false);
        },
        computed: {},
        methods: {},
        mounted: function () {
            this.$nextTick(function () {

            });
        }
    });
}, ['/Utilities/Scripts/qrcode.js']);