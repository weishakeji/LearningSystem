$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            account: {},      //当前账户
            interface: {},        //当前支付接口的配置项
            moneyaccount: {},        //资金流水的记录

            //地址栏参数
            params: {
                'money': $api.querystring('money'),
                'paiid': $api.querystring('paiid'),
                'code': $api.querystring('code')
            },
            checked: false,  //参数是否能校验通过

            loading: true,
            loading_income: false       //页面跳转中的预载
        },
        mounted: function () {
            var th = this;
            th.checked = th.checkcode(); //校验传递过来的参数
            if (!th.checked) return;
            th.loading = true;
            $api.get('Account/Current').then(function (req) {
                if (req.data.success) {
                    th.account = req.data.result;
                    if (th.islogin) {
                        th.getPayinterface();
                    } else {
                        th.loading = false;
                    }
                } else {
                    console.error(req.data.exception);
                    throw req.config.way + ' ' + req.data.message;
                }
            }).catch(function (err) {
                th.loading = false;
                console.error(err);
            });

        },
        created: function () {

        },
        computed: {
            //是否登录
            islogin: function () {
                return JSON.stringify(this.account) != '{}' && this.account != null;
            },
            //支付接口是否存在
            ifexist: function () {
                return JSON.stringify(this.interface) != '{}' && this.interface != null && this.interface.Pai_IsEnable == true
                    && this.interface.Pai_InterfaceType != '';
            }
        },
        watch: {

        },
        methods: {
            //校验参数
            checkcode: function () {
                var vcode = $api.storage('weishakeji_pay_vcode');
                var md5 = $api.md5(this.params.money + '_' + this.params.paiid + '_' + vcode);
                return md5 == this.params.code;
            },
            //获取接口对象
            getPayinterface: function () {
                var th = this;
                th.loading = true;
                $api.get('Pay/ForID', { 'id': this.params.paiid }).then(function (req) {
                    th.loading = false;
                    if (req.data.success) {
                        th.interface = req.data.result;
                        if (th.ifexist) {
                            //创建支付流水
                            th.createMoneyAccount();
                        }
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(function (err) {
                    th.loading = false;
                    console.error(err);
                });
            },
            //创建资金流水
            createMoneyAccount: function () {
                var th = this;
                th.loading_income = true;             
                $api.post('Pay/MoneyIncome', { 'money': th.params.money, 'payif': th.interface }).then(function (req) {
                    th.loading_income = false;
                    if (req.data.success) {
                        th.moneyaccount = req.data.result;
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(function (err) {
                    th.loading_income = false;
                    console.error(err);
                });
            }
        }
    });

}, ['Components/btns.js',
    'Components/topayment.js']);
