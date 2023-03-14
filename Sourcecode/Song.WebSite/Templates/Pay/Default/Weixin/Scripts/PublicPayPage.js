$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            //微信授权code，通过它获取access_token和openid
            code: $api.querystring('code'),
            //自己传递的参数被回调回来了，面值、流水号
            state: $api.querystring('state'),

            account: {},      //当前账户
            orgin: {},           //当前机构
            interface: {},       //支付接口
            moneyAccount: {},       //账单

            total_fee: 0,  //充值的钱数
            serial: '',   //流水号，即商户订单号
            orgid: 0,  //机构id
            mchid: '',   //商户ID（微信支付中心的商户ID)
            paykey: '',   //商户支付密钥
            appid: '',    //公众号appid
            secret: '',   //公众号secret
            notify_url: '',   //回调地址
            token: '',
            openid: '',

            loading: true,

        },
        mounted: function () {
            this.initData();
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
            //初始化一些数据
            initData: function () {
                var state = eval('({' + decodeURIComponent(this.state) + '})');
                var th = this;
                th.loading = true;
                $api.bat(
                    $api.get('Pay/Interface', { 'id': state.pi }),
                    $api.get('Pay/MoneyAccount', { 'serial': state.serial })
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
                    if (th.ifexist) {
                        th.appid = th.interface.Pai_ParterID;  //绑定支付的APPID（必须配置）
                        th.secret = th.interface.Pai_Key;      //公众帐号secert（仅JSAPI支付的时候需要配置）
                        //接口配置项
                        let config = $api.xmlconfig.tojson(th.interface.Pai_Config);
                        th.mchid = config["MCHID"];    //商户id
                        th.paykey = config["Paykey"];  //支付密钥
                        //回调地址
                        th.notify_url =  th.interface.Pai_Returl;
                        if (th.notify_url=='') th.notify_url = $api.url.host();                       
                        th.notify_url += "Pay/Weixin/PublicPayNotify";
                        console.log(th.notify_url);
                    }
                    if (th.ismaccount) {
                        th.orgid = th.moneyAccount.Org_ID;
                        $api.get('Account/ForID', { 'id': th.moneyAccount.Ac_ID }).then(function (req) {
                            if (req.data.success) account = req.data.result;
                        }).catch(err => console.error(err));
                    }
                })).catch(err => console.error(err))
                    .finally(() => th.loading = false);
            }
        }
    });

}, ['/Utilities/Components/avatar.js']);
