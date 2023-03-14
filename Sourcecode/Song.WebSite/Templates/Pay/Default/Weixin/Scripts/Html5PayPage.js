$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            pid: $api.querystring('pi'),    //接口id
            serial: $api.querystring('serial'), //流水号
            money: $api.querystring('money'),  //支付金额，单位：分
            referrer: $api.querystring('referrer'), //来源页

            account: {},      //当前账户
            orgin: {},           //当前机构
            interface: {},       //支付接口
            moneyAccount: {},       //账单

            error: '',             //错误信息

            loading: true,
            loading_url: false //支付地址加载
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
                    if (th.ifexist && th.ismaccount) {
                        th.build_pay_url(th.interface, th.account, th.moneyAccount, th.orgin);
                    }

                })).catch(err => console.error(err))
                    .finally(() => th.loading = false);
            },
            //生成回调地址
            build_notify_url: function (pi) {
                let notify_url = pi.Pai_Returl;
                if (notify_url == '') notify_url = window.location.origin;
                let t = notify_url.substring(notify_url.length - 1);
                if (notify_url.length > 0 && notify_url.substring(notify_url.length - 1) != '/') notify_url += "/";
                notify_url += "Pay/Weixin/Html5PayNotify";
                notify_url = $api.url.set(notify_url, { 'pi': pi.Pai_ID, 'serial': this.serial, 'referrer': this.referrer });
                return encodeURIComponent(notify_url);
            },
            //支付url
            build_pay_url: function (pi, account, moneyacc, orgin) {
                let total_fee = Math.floor(moneyacc.Ma_Money * 100);
                let orgid = moneyacc.Org_ID;
                let appid = pi.Pai_ParterID;  //绑定支付的APPID（必须配置）
                let secret = pi.Pai_Key;      //公众帐号secert（仅JSAPI支付的时候需要配置）
                //回调地址
                let notify_url = this.build_notify_url(pi);
                //接口配置项
                let config = $api.xmlconfig.tojson(pi.Pai_Config);
                let mchid = config["MCHID"];    //商户id
                let paykey = config["Paykey"];  //支付密钥
                //充值的人员信息
                let buyer = account.Ac_MobiTel1 == '' ? account.Ac_MobiTel2 : account.Ac_MobiTel1;
                if (buyer == '') buyer = account.Ac_AccName;
                var th = this;
                th.loading_url = true;
                $api.get('Pay/WxJsApiPay', {
                    'tracetype': 'MWEB',
                    'body': orgin.Org_PlatformName, 'serial': moneyacc.Ma_Serial, 'openid': '',
                    'total_fee': total_fee, 'appid': appid, 'mchid': mchid, 'paykey': paykey,
                    'notify_url': notify_url, 'buyer': buyer
                }).then(function (req) {
                    if (req.data.success) {
                        var result = req.data.result;
                        var url = result.mweb_url + '&redirect_url=' + th.build_notify_url(pi);
                        window.location.href = url;
                    } else {
                        console.error(req.data.exception);
                        throw req.data.message;
                    }
                }).catch(function (err) {
                    th.error = err;
                    console.error(err);
                }).finally(() => th.loading_url = false);
            }
        }
    });

}, ['/Utilities/Components/avatar.js',
    '/Utilities/Scripts/jquery.qrcode.min.js']);
