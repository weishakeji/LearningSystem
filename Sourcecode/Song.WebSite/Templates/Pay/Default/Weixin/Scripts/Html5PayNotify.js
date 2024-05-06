$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            pid: $api.querystring('pi'),    //接口id
            serial: $api.querystring('serial'), //流水号
            referrer: decodeURIComponent($api.querystring('referrer')), //来源页 

            interface: {},       //支付接口
            moneyAccount: {},       //账单

            success: false,      //支付是否成功
            orderquery: {},      //订单查询的条件
            loading: true
        },
        mounted: function () {
            this.getPayinterface();
        },
        created: function () {

        },
        computed: {
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
                ).then(([pi, acc]) => {
                    //获取结果
                    th.interface = pi.data.result;
                    th.moneyAccount = acc.data.result;
                    //支付回调是否成功
                    th.success = th.moneyAccount.Ma_IsSuccess;
                    if (th.success) {
                        window.setTimeout(function () {
                            window.location.href = th.referrer;
                        }, 1000);
                    } else {
                        //订单查询的条件
                        th.orderquery['serial'] = th.serial;
                        th.orderquery['appid'] = th.interface.Pai_ParterID;
                        let config = $api.xmlconfig.tojson(th.interface.Pai_Config);
                        th.orderquery['mchid'] = config["MCHID"];    //商户id
                        th.orderquery['paykey'] = config["Paykey"];  //支付密钥
                        window.setTimeout(function () {
                            th.call_succeeded();
                        }, 1000);
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loading = false);
            },
            goback: function () {
                window.location.href = this.referrer;
            },
            //验证是否成功
            call_succeeded: function (serial) {
                var th = this;
                $api.get('Pay/WxOrderQuery', th.orderquery).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        if (result['trade_state'] == 'SUCCESS') {
                            window.setTimeout(function () {
                                window.location.href = th.referrer;
                            }, 1000);
                        } else {
                            window.setTimeout(function () {
                                th.call_succeeded();
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

}, ['/Utilities/Components/avatar.js']);
