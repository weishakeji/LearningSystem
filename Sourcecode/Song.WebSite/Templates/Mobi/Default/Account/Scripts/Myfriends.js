$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            account: {},     //当前登录账号
            platinfo: {},
            organ: {},
            config: {},      //当前机构配置项        
            datas: {},
            loading_init: true,
            show_help: false,    //显示帮助
            show_share: false,       //显示分享
            //朋友数量
            friends: 0,
            friendsAll: 0,
            //赚取的积分、金额、卡券
            earn: {
                point: 0,
                money: 0,
                coupon: 0
            },
            //积分与卡券的兑换参数
            param: {},
            //分润信息
            profit: null
        },
        mounted: function () {
            $api.bat(
                $api.get('Account/Current'),
                $api.cache('Platform/PlatInfo:60'),
                $api.get('Organization/Current'),
                $api.get('Share/Param')
            ).then(axios.spread(function (account, platinfo, organ, param) {
                vapp.loading_init = false;
                //获取结果
                vapp.account = account.data.result;
                vapp.platinfo = platinfo.data.result;
                vapp.organ = organ.data.result;
                vapp.param = param.data.result;
                //机构配置信息
                vapp.config = $api.organ(vapp.organ).config;
                //
                if (vapp.account != null) {
                    vapp.getfriends(vapp.account.Ac_ID);
                    vapp.getEarn(vapp.account.Ac_ID);
                }
            })).catch(function (err) {
                console.error(err);
            });
            this.getProfit();
        },
        created: function () {
            window.queryQrcode = window.setInterval(function () {
                var qrcode = $dom("#qrcode");
                if (qrcode.length > 0) {
                    var url = window.location.origin + '?sharekeyid=' + vapp.account.Ac_ID;
                    var qrcode = new QRCode("qrcode", {
                        text: url,
                        width: 100,
                        height: 100,
                        colorDark: "#000000",
                        colorLight: "#ffffff",
                        correctLevel: QRCode.CorrectLevel.H
                    });
                    window.clearInterval(window.queryQrcode);
                }
                console.log('没有查找到qrcode区域');
            }, 100);
        },
        computed: {
            //是否登录
            islogin: function () {
                return JSON.stringify(this.account) != '{}' && this.account != null;
            }
        },
        watch: {
        },
        methods: {
            //获取朋友数量
            getfriends: function (acid) {
                $api.bat(
                    $api.get('Share/Friends', { 'acid': acid }),
                    $api.get('Share/FriendAll', { 'acid': acid })
                ).then(axios.spread(function (friends, friendsAll) {
                    vapp.loading_init = false;
                    //获取结果
                    vapp.friends = friends.data.result;
                    vapp.friendsAll = friendsAll.data.result;
                })).catch(function (err) {
                    console.error(err);
                });
            },
            //获取赚数的金额、卡券等
            getEarn: function (acid) {
                $api.bat(
                    $api.get('Share/EarnCoupon', { 'acid': acid }),
                    $api.get('Share/EarnMoney', { 'acid': acid }),
                    $api.get('Share/EarnPoint', { 'acid': acid })
                ).then(axios.spread(function (coupon, money, point) {
                    vapp.loading_init = false;
                    //获取结果
                    vapp.earn.coupon = coupon.data.result;
                    vapp.earn.money = money.data.result;
                    vapp.earn.point = point.data.result;
                })).catch(function (err) {
                    console.error(err);
                });
            },
            //获取分润方案
            getProfit: function () {
                $api.get('ProfitSharing/ThemeCurrent').then(function (req) {
                    if (req.data.success) {
                        vapp.profit = req.data.result;
                        if (vapp.profit == null) return;
                        $api.get('ProfitSharing/ProfitList', { 'tid': vapp.profit.Ps_ID }).then(function (req) {
                            if (req.data.success) {
                                vapp.profit.items = req.data.result;
                            } else {
                                console.error(req.data.exception);
                                throw req.data.message;
                            }
                        }).catch(function (err) {
                            alert(err);
                            console.error(err);
                        });
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });
            },
            showHelp: function () {
                var html = $dom('.help-box').get(1).outHtml();
                this.show_help = true;
                window.setTimeout(function () {
                    $dom("#show_help").html(html);
                }, 200);
            },
            showShare: function () {
                var html = $dom('.share-box').get(1).outHtml();
                this.show_share = true;
                window.setTimeout(function () {
                    $dom("#show_share").html(html);
                }, 200);
            }
        }
    });

}, ['/Utilities/Scripts/qrcode.js']);
