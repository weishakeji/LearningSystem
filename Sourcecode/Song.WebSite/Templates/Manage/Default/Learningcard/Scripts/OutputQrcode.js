$ready(function () {
    window.vue = new Vue({
        el: '#app',
        data: {
            id: $api.querystring('id'),
            cardset: {},
            courses: [],
            cards: [],
            organ: {},       //学习卡
            loading: false
        },
        watch: {
            'cards': function (nl, ov) {
                //数据变动，且渲染完成后执行
                this.$nextTick(function () {
                    var imgused = $dom(".qrcode-area>img#used").attr("src");
                    var imgdisable = $dom(".qrcode-area>img#disable").attr("src");
                    var url = window.location.origin + "/mobi/Card/learn?code={code}&pw={pw}";
                    for (var i = 0; i < vue.cards.length; i++) {
                        var card = vue.cards[i];
                        var dd = $dom(".qrcode-area dl dd#card_" + card.Lc_ID);
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
            this.loading = true;
            $api.bat(
                $api.get('Learningcard/SetForID', { 'id': this.id }),
                $api.get('Learningcard/SetCourses', { 'id': this.id }),
                $api.get('Learningcard/Cards', { 'lsid': this.id })
            ).then(axios.spread(function (cardset, courses, cards) {
                //判断结果是否正常
                for (var i = 0; i < arguments.length; i++) {
                    if (arguments[i].status != 200)
                        console.error(arguments[i]);
                    var data = arguments[i].data;
                    if (!data.success && data.exception != null) {
                        console.error(data.exception);
                        throw data.message;
                    }
                }
                vue.loading = false;
                //获取结果
                vue.cardset = cardset.data.result;
                vue.courses = courses.data.result;
                vue.cards = cards.data.result;
                $api.get('Organization/ForID', { 'id': vue.cardset.Org_ID }).then(function (req) {
                    if (req.data.success) {
                        vue.organ = req.data.result;
                    } else {
                        $api.get('Organization/Current').then(function (req) {
                            if (req.data.success) {
                                vue.organ =req.data.result;                        
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
            })).catch(function (err) {
                console.error(err);
            });
        },
        computed: {},
        methods: {},
        mounted: function () {
            this.$nextTick(function () {

            });
        }
    });
}, ['/Utilities/Scripts/qrcode.js']);