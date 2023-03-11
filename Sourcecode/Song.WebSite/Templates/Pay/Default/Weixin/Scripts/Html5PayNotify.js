$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            pid: $api.querystring('pi'),    //接口id
            serial: $api.querystring('serial'), //流水号
            referrer: $api.querystring('referrer'), //来源页 

            account: {},      //当前账户
            orgin: {},           //当前机构
            interface: {},       //支付接口
            moneyAccount: {},       //账单

            loading: true
        },
        mounted: function () {
            var th = this;
            th.loading = true;
            $api.bat(
                $api.get('Account/Current'),
                $api.get("Organization/Current")
            ).then(axios.spread(function (acc, org) {
                //判断结果是否正常
                for (var i = 0; i < arguments.length; i++) {
                    if (arguments[i].status != 200)
                        console.error(arguments[i]);
                    var data = arguments[i].data;
                    if (!data.success && data.exception != null) {
                        console.error(data.exception);
                        throw arguments[i].config.way + ' ' + data.message;
                    }
                }
                //获取结果
                th.account = acc.data.result;
                th.orgin = org.data.result;
                th.getPayinterface();
            })).catch(err => console.error(err))
                .finally(() => th.loading = false);
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
            },
            //支付流水号是否存在或是否可用（例如未支付完成）
            ismaccount: function () {
                return JSON.stringify(this.moneyAccount) != '{}' && this.moneyAccount != null;
            }
        },
        watch: {

        },
        methods: {
            //获取接口对象和流水号
            getPayinterface: function () {
                var th = this;
                th.loading = true;
                $api.bat(
                    $api.get('Pay/Interface', { 'id': th.pid }),
                    $api.get('Pay/MoneyAccount', { 'serial': th.serial })
                ).then(axios.spread(function (pi, acc) {
                    //判断结果是否正常
                    for (var i = 0; i < arguments.length; i++) {
                        if (arguments[i].status != 200)
                            console.error(arguments[i]);
                        var data = arguments[i].data;
                        if (!data.success && data.exception != null) {
                            console.error(data.exception);
                            throw arguments[i].config.way + ' ' + data.message;
                        }
                    }
                    //获取结果
                    th.interface = pi.data.result;
                    th.moneyAccount = acc.data.result;
                })).catch(err => console.error(err))
                    .finally(() => th.loading = false);
            },

            //验证是否成功
            call_succeeded: function (serial) {
                var th = this;
                $api.get('Pay/MoneyAccount', { 'serial': serial }).then(function (req) {
                    if (req.data.success) {
                        th.moneyAccount = req.data.result;
                        if (!th.moneyAccount.Ma_IsSuccess) {
                            window.setTimeout(function () {
                                th.call_succeeded(serial);
                            }, 1000);
                        } else {
                            window.setTimeout(function () {
                                window.location.href = th.referrer;
                            }, 1000);
                        }
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => console.error(err));
            }
        }
    });

}, ['/Utilities/Components/avatar.js',
    '/Utilities/Scripts/jquery.qrcode.min.js']);
