$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            account: {},     //当前登录账号
            platinfo: {},
            org: {},
            config: {},      //当前机构配置项       
            loading: false,
            loading_init: true,

            param: {},           //数据
            point_input: '',      //要兑换的积分数
            tips: '',             //提示信息
            checked: false           //输入校验是否通过
        },
        mounted: function () {
            var th = this;
            $api.get('Point/Param').then(function (req) {
                if (req.data.success) {
                    th.param = req.data.result;
                } else {
                    console.error(req.data.exception);
                    throw req.config.way + ' ' + req.data.message;
                }
            }).catch(function (err) {
                alert(err);
                console.error(err);
            });
        },
        created: function () {

        },
        computed: {
            //是否登录
            islogin: (t) => { return !$api.isnull(t.account); }
        },
        watch: {
            'account': {
                handler: function (nv, ov) {
                    if ($api.isnull(nv)) return;
                }, immediate: true
            },
            'point_input': function (nv, ov) {
                if (nv == '') {
                    this.tips = '';
                    this.checked = false;
                    return;
                }
                var num = parseInt(nv);
                if (isNaN(num)) {
                    this.tips = '请输入数字';
                    this.checked = false;
                    return;
                }
                if (num > this.account.Ac_Point) {
                    this.tips = '最多兑换 <b>' + this.account.Ac_Point + '</b>个积分';
                    this.point_input = this.account.Ac_Point;
                    this.checked = false;
                    return;
                }
                var result = Math.floor(num / this.param.PointConvert);
                this.tips = '可兑换 <b>' + result + '</b>个卡券';
                this.checked = true;
            }
        },
        methods: {
            login: function () {
                var url = $api.url.set(this.commonaddr('signin'), {});
                window.navigateTo(url);
            },
            myself: function () {
                var url = $api.url.set("/mobi/account/myself", {});
                window.navigateTo(url);
            },
            godetails: function () {
                var url = $api.url.set("details", {});
                window.navigateTo(url);
            },
            //计算示例
            calc_demo: function (point, ratio) {
                //计算兑换结果
                var result = Math.floor(this.account.Ac_Point / this.param.PointConvert);
                return result;
            },
            //兑换按钮事件
            btnExchange: function () {
                var msg = this.tips;
                if (this.point_input == '') msg = '请输入要兑换的积分数';
                if (!this.checked) {
                    this.$dialog.alert({
                        message: msg,
                    }).then(() => {

                    });
                } else {
                    this.exchange();
                }
            },
            //具体事件
            exchange: function () {
                var th = this;
                this.loading = true;

                var num = parseInt(this.point_input);
                if (isNaN(num)) return;
                if (num > this.account.Ac_Point) return;
                var coupon = Math.floor(num / this.param.PointConvert);
                if (coupon <= 0) return;
                $api.get('Point/Exchange', { 'coupon': coupon }).then(function (req) {
                    th.loading = false;
                    if (req.data.success) {
                        var result = req.data.result;
                        if (result) {
                            th.$dialog.alert({
                                message: '操作成功',
                            }).then(() => {
                                window.location.reload();
                            });
                        }
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    alert(err);
                    console.error(err);
                });
            }
        }
    });

}, ['../Components/page_header.js']);
