$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            account: {},     //当前登录账号
            platinfo: {},
            organ: {},
            config: {},      //当前机构配置项   

            paypis: [],       //支付接口的列表

            input_code: '',      //要输入的充值卡卡号
            showhelp: false,     //显示帮助信息 

            recharge_val: '',        //要充值的金额
            currentpay_id: 0,        //当前支付接口的id

            loading_init: true,
            loading_up: false
        },
        mounted: function () {
            var th = this;
            $api.bat(
                $api.get('Account/Current'),
                $api.cache('Platform/PlatInfo:60'),
                $api.get('Organization/Current'),
                $api.get('Pay/ListEnable', { 'platform': 'mobi' })
            ).then(axios.spread(function (account, platinfo, organ, payi) {
                vapp.loading_init = false;
                //判断结果是否正常
                for (var i = 0; i < arguments.length; i++) {
                    if (arguments[i].status != 200)
                        console.error(arguments[i]);
                    var data = arguments[i].data;
                    if (!data.success && data.exception != null) {
                        console.error(data.message);
                    }
                }
                //获取结果
                th.account = account.data.result;
                th.platinfo = platinfo.data.result;
                th.organ = organ.data.result;
                //机构配置信息
                th.config = $api.organ(th.organ).config;
                //支付接口
                th.paypis = payi.data.result;
                var isweixin = $api.isWeixin(); 	//是否处于微信
                var ismini = $api.isWeixinApp(); //是否处于微信小程序
                for (let i = 0; i < th.paypis.length; i++) {
                    th.paypis[i].Pai_Config = $api.xmlconfig.tojson(th.paypis[i].Pai_Config);
                }
                th.setCurrentpay();    //设置或获取当前支付接口，默认是第一个
                console.log(th.paypis);
            })).catch(function (err) {
                console.error(err);
            });
            this.init_code();
        },
        created: function () {
            //当文件选择输入框变更时
            $("#upload_qrcode").change(function (e) {
                var files = e.target.files;
                if (files && files.length > 0) {
                    console.log(files[0]);
                    var url = window.getObjectURL(files[0]);
                    qrcode.decode(url);

                    qrcode.callback = function (imgMsg) {
                        var txt = imgMsg;
                        var code = $api.url.get(txt, 'code');
                        var pw = $api.url.get(txt, 'pw');
                        console.log(code + '-' + pw);
                        vapp.input_code = code + '-' + pw;
                        vapp.$toast.success('卡号解析正常');
                    }
                }
            });

        },
        computed: {
            //是否登录
            islogin: function () {
                return JSON.stringify(this.account) != '{}' && this.account != null;
            },
        },
        watch: {
            'search_code': function (nv, ov) {
                console.log(nv);
            }
        },
        methods: {
            //页面跳转
            gourl: function (page) {
                var url = $api.url.set(page, {});
                window.location.href = url;
            },
            //初始化充值卡号，当扫码时，会带参数跳转到这里
            init_code: function () {
                var code = $api.querystring('code');
                var pw = $api.querystring('pw');
                if (code != '' && pw != '') {
                    this.input_code = code + '-' + pw;
                    var url = window.location.href;
                    url = $api.url.set(url, {
                        'code': null,
                        'pw': null
                    });
                    console.log(url);
                    window.history.pushState({}, '', url);
                }
            },
            //使用充值卡
            usecode: function () {
                if ($api.trim(this.input_code) == '') {
                    this.$toast.fail('卡号不得为空');
                    return;
                }
                var th = this;
                this.validator(null, this.input_code, function (p) {
                    //if (p == null) return;
                    var type = $api.getType(p);
                    if (type == 'Error') {
                        th.$notify({
                            type: 'danger',
                            message: p.message,
                            position: 'bottom',
                        });
                    } else {
                        th.loading_up = true;
                        $api.post('Coupon/Recharge', { 'code': th.input_code }).then(function (req) {
                            th.loading_up = false;
                            if (req.data.success) {
                                var result = req.data.result;
                                th.$toast.success('成功充值' + result.Ca_Value + '个卡券');
                                window.setTimeout(function () {
                                    th.input_code = '';
                                    var url = window.location.href;
                                    url = $api.url.set(url, {
                                        'code': null,
                                        'pw': null
                                    });
                                    window.location.href = url;
                                }, 3000);
                            } else {
                                console.error(req.data.exception);
                                throw req.data.message;
                            }
                        }).catch(function (err) {
                            th.loading_up = false;
                            //Vue.prototype.$alert(err);
                            th.$notify({
                                type: 'danger',
                                message: err,
                                position: 'bottom',
                            });
                            console.error(err);
                        });
                    }
                });
            },
            //验证充值码
            validator: function (rule, value, callback) {
                if (value.indexOf('-') < 0) {
                    return callback(new Error('格式为“充值卡-密码”，破折号不可缺少'));
                } else {
                    var arr = value.split('-');
                    if (!(/^\d+(\.\d+)?$/.test($api.trim(arr[0])))) {
                        return callback(new Error('卡号必须为数字'));
                    }
                    if (arr.length > 1) {
                        if (!(/^\d+(\.\d+)?$/.test($api.trim(arr[1])))) {
                            return callback(new Error('密码必须为数字'));
                        }
                    }
                }
                return callback();
            },
            //打开二维码图片
            openqrcode: function () {
                var upload = $("#upload_qrcode");
                upload.click();
            },
            //*** 在线支付 */
            //设置或获取当前支付接口
            setCurrentpay: function (paiid) {
                var key = 'weisha_payid_current';
                var currid = paiid == null ? Number($api.storage(key)) : paiid;

                if (this.paypis.length > 0) {
                    var current = null;
                    for (let i = 0; i < this.paypis.length; i++) {
                        this.paypis[i].current = false;
                        if (this.paypis[i].Pai_ID == currid) {
                            this.currentpay_id = currid;
                            $api.storage(key, this.paypis[i].Pai_ID);
                            current = this.paypis[i];
                        }
                    }
                    if (current == null) {
                        current = this.paypis[0];
                        this.currentpay_id = this.paypis[0].Pai_ID;
                        $api.storage(key, this.paypis[0].Pai_ID);
                    }
                    return current;
                }

            },
            //开始进入支付
            payEntry: function () {
                if (this.recharge_val == '') {
                    this.$toast.fail('请输入金额');
                    return;
                }
                var money = Number(this.recharge_val);
                money = isNaN(money) ? 0 : money;
                if (money <= 0) {
                    this.$toast.fail('金额不得小于零');
                    return;
                }
                //转向支付页面
                var url = '/pay/PayEntry.html';
                //校验码
                var vcode = new Date().getTime();
                $api.storage('weishakeji_pay_vcode', vcode);
                var md5 = $api.md5(money + '_' + this.currentpay_id + '_' + vcode);
                //console.log(md5);
                url = $api.url.set(url, {
                    'money': money,
                    'paiid': this.currentpay_id,
                    'code': md5
                });
                console.log(url);
                window.location.href = url;
            }
        },
        filters: {
        }
    });
}, ['../Components/page_header.js',
    '/Utilities/Scripts/jquery.js',
    '/Utilities/Scripts/reqrcode.js']);


window.getObjectURL = function (file) {
    var url = null;
    if (window.createObjectURL != undefined) { // basic
        url = window.createObjectURL(file);
    } else if (window.URL != undefined) { // mozilla(firefox)
        url = window.URL.createObjectURL(file);
    } else if (window.webkitURL != undefined) { // webkit or chrome
        url = window.webkitURL.createObjectURL(file);
    }
    return url;
}