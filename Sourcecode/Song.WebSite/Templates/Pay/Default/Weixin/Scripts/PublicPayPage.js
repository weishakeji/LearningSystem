$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            //微信授权code，通过它获取access_token和openid
            code: $api.querystring('code'),
            //自己传递的参数被回调回来了，面值、流水号
            state: $api.querystring('state'),

            account: {},      //当前账户
            organ: {},           //当前机构
            interface: {},       //支付接口
            moneyAccount: {},       //账单

            token: '',
            openid: '',

            error: '',
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
                    if (th.ismaccount && th.ifexist)
                        th.getAccountAndOrgan(th.moneyAccount);
                })).catch(err => console.error(err))
                    .finally(() => th.loading = false);
            },
            //获取支付账号与所在机构
            getAccountAndOrgan: function (moneyAccount, func) {
                var th = this;
                $api.bat(
                    $api.get('Account/ForID', { 'id': moneyAccount.Ac_ID }),
                    $api.get('Organization/ForID', { 'id': moneyAccount.Org_ID })
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
                    th.organ = org.data.result;
                    th.wxJsApiPay(th.interface, th.moneyAccount, th.account, th.organ);
                })).catch(function (err) {
                    //alert(err);
                    console.error(err);
                });
            },
            //获取支付订单所需的参数
            wxJsApiPay: function (interface, moneyacc, acc, org) {
                var th = this;
                var total_fee = Math.floor(moneyacc.Ma_Money * 100);
                var appid = interface.Pai_ParterID;  //绑定支付的APPID（必须配置）
                var secret = interface.Pai_Key;      //公众帐号secert（仅JSAPI支付的时候需要配置）
                //接口配置项
                var config = $api.xmlconfig.tojson(th.interface.Pai_Config);
                var mchid = config["MCHID"];    //商户id
                var paykey = config["Paykey"];  //支付密钥
                //回调地址
                var notify_url = interface.Pai_Returl;
                if (notify_url == '') notify_url = $api.url.host();
                if (notify_url.length > 0 && notify_url.substring(notify_url.length - 1) != '/') notify_url += "/";
                notify_url += "Pay/Weixin/PublicPayNotify";
                //充值的人员信息
                var buyer = acc.Ac_MobiTel1 == '' ? acc.Ac_MobiTel2 : acc.Ac_MobiTel1;
                if (buyer == '') buyer = acc.Ac_AccName;
                //获取token和openid
                $api.get('Pay/WxOpenidAndAccessTokenFromCode', { 'appid': appid, 'secret': secret, 'code': th.code })
                    .then(function (req) {
                        if (req.data.success) {
                            var result = req.data.result;
                            th.token = result['access_token'];
                            th.openid = result['openid'];
                            //document.writeln(JSON.stringify(result));     
                            //生成订单 
                            $api.get('Pay/WxJsApiPay', {
                                'tracetype': 'JSAPI',
                                'body': org.Org_PlatformName, 'serial': moneyacc.Ma_Serial, 'openid': th.openid,
                                'total_fee': total_fee, 'appid': appid, 'mchid': mchid, 'paykey': paykey,
                                'notify_url': notify_url, 'buyer': buyer
                            }).then(function (req) {
                                if (req.data.success) {
                                    var result = req.data.result;
                                    //document.writeln(JSON.stringify(result));
                                    //var url = result.mweb_url + '&redirect_url=' + th.build_notify_url(pi);
                                    // window.location.href = url;
                                    th.jsApiCall(result);
                                } else {
                                    console.error(req.data.exception);
                                    throw req.data.message;
                                }
                            }).catch(function (err) {
                                th.error = err;
                                console.error(err);
                            }).finally(() => th.loading_url = false);
                        } else {
                            throw req.data.message;
                        }
                    }).catch(err => console.error(err));
            },
            //调用微信JS api 支付
            jsApiCall:function(wxJsApiParam){
                var apipara = wxJsApiParam;
                if (apipara==null || apipara == "") return;              
                WeixinJSBridge.invoke('getBrandWCPayRequest', apipara, function (res) {
                    //WeixinJSBridge.log(res.err_msg);
                    var msg = "code:" + res.err_code + "\n";
                    msg += "desc:" + res.err_desc + "\n";
                    msg += "msg:" + res.err_msg + "\n";
                    //alert(msg);
                    //支付成功
                    var returl = "";                //返回的地址
                    var default_returl = '/mobi/account/myself';
                    returl = $api.storage('recharge_returl');  //充值后的返回
                    if (returl == '' || returl == null) returl = default_returl;
                    $dom("#btnBacklink").attr('href', returl);
                    $dom("#btnBacklink").click();
                    if (res.err_msg == "get_brand_wcpay_request:ok") {
                        window.location.href = returl;
                    }
                    //支付取消
                    if (res.err_msg == "get_brand_wcpay_request:cancel") {
                        window.location.href = returl;
                    }
                    //支付失败
                    if (res.err_msg == "get_brand_wcpay_request:fail") {
                        alert("支付失败！");
                        window.location.href = returl;
                    }
                });
            }
        }
    });

}, ['/Utilities/Components/avatar.js']);
