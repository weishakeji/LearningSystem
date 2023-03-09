$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            pid: $api.querystring('pi'),    //接口id
            serial: $api.querystring('serial'), //流水号

            account: {},      //当前账户
            orgin: {},           //当前机构
            interface: {},       //支付接口
            moneyAccount: {},       //账单

            notify_url: '',      //成功后的回调地址
            pay_url: '',         //支付地址

            loading: true,

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
                $api.get('Pay/Interface', { 'id': th.pid }).then(function (req) {
                    th.loading = false;
                    if (req.data.success) {
                        th.interface = req.data.result;
                        if (th.ifexist) {
                            //获取支付流水
                            th.getMoneyAccount();
                        }
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loading = false);
            },
            //获取支付流水
            getMoneyAccount: function () {
                var th = this;
                th.loading = true;
                $api.get('Pay/MoneyAccount', { 'serial': th.serial }).then(function (req) {
                    th.loading = false;
                    if (req.data.success) {
                        th.moneyAccount = req.data.result;
                        //生成回调地址
                        th.notify_url = th.build_notify_url(th.interface);
                        console.log(th.notify_url);
                        th.pay_url = th.build_pay_url(th.interface, th.account, th.moneyAccount, th.orgin);
                    } else {
                        console.error(req.data.exception);
                        throw req.config.way + ' ' + req.data.message;
                    }
                }).catch(err => console.error(err))
                    .finally(() => th.loading = false);
            },
            //生成回调地址
            build_notify_url: function (pi, account, moneyacc, orgin) {
                let notify_url = pi.Pai_Returl;
                if (notify_url == '') notify_url = window.location.origin;
                let t = notify_url.substring(notify_url.length - 1);
                if (notify_url.length > 0 && notify_url.substring(notify_url.length - 1) != '/') notify_url += "/";
                notify_url += "Pay/Weixin/NativeNotify";
                return notify_url;
            },
            //支付url
            build_pay_url: function (pi, account, moneyacc, orgin) {
                let total_fee = moneyacc.Ma_Money * 100;
                let orgid = moneyacc.Org_ID;
                let appid = pi.Pai_ParterID;  //绑定支付的APPID（必须配置）
                let secret = pi.Pai_Key;      //公众帐号secert（仅JSAPI支付的时候需要配置）
                let notify_url = pi.Pai_Returl;
                //接口配置项
                let config = $api.xmlconfig.tojson(pi.Pai_Config);
                let mchid = config["MCHID"];    //商户id
                let paykey = config["Paykey"];  //支付密钥

                window.NativePay.GetPrePayUrl(orgid);
            }
        }
    });

}, ['/Utilities/Components/avatar.js',
    'Business/NativePay.js']);
