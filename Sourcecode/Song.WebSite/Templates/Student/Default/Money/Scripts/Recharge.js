$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            account: {},     //当前登录账号
            platinfo: {},
            organ: {},
            config: {},      //当前机构配置项   

            paypis: [],       //支付接口的列表
            form: {
                money: '',      //要充值的金额
                payid: 0,
            },
            rules: {
                money: [
                    { required: true, message: '不得为空', trigger: 'blur' },
                    {
                        validator: async function (rule, value, callback) {
                            var money = Number(value);
                            money = isNaN(money) ? 0 : money;
                            if (money <= 0) {
                                return callback(new Error('金额不得小于零!'));
                            }
                            money = Math.floor(money * 100) / 100;
                            if (money <= 0) {
                                return callback(new Error('金额不得小于一分钱!'));
                            } else {
                                vapp.form.money = money;
                            }
                        }, trigger: 'blur'
                    }
                ]
            },
            paypanel: false,         //支付确认的面板是否显示
            payurl: '',              //转向支付平台的路径与参数
            isopenpay:false,        //是否打开支付窗口

            loading_init: true,
            loading_up: false
        },
        mounted: function () {
            var th = this;
            $api.bat(
                $api.get('Account/Current'),
                $api.cache('Platform/PlatInfo:60'),
                $api.get('Organization/Current'),
                $api.get('Pay/ListEnable', { 'platform': 'web' })
            ).then(axios.spread(function (account, platinfo, organ, payi) {
                th.loading_init = false;
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
                th.paypis = th.pay_scene(payi.data.result);
                for (let i = 0; i < th.paypis.length; i++) {
                    th.paypis[i].Pai_Config = $api.xmlconfig.tojson(th.paypis[i].Pai_Config);
                }
                th.setCurrentpay();    //设置或获取当前支付接口，默认是第一个
                console.log(th.paypis);
            })).catch(function (err) {
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
        },
        watch: {
        },
        methods: {
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
                            this.form.payid = currid;
                            $api.storage(key, this.paypis[i].Pai_ID);
                            current = this.paypis[i];
                        }
                    }
                    if (current == null) {
                        current = this.paypis[0];
                        this.form.payid = this.paypis[0].Pai_ID;
                        $api.storage(key, this.paypis[0].Pai_ID);
                    }
                    return current;
                }

            },
            //支付接口在不同场景下的显示
            pay_scene: function (paypis) {
                var isweixin = $api.isWeixin(); 	//是否处于微信
                var ismini = $api.isWeixinApp(); //是否处于微信小程序
                for (let i = 0; i < paypis.length; i++) {
                    const pi = paypis[i];
                    var scene = pi.Pai_Scene;
                    var arr = scene.split(",");
                    //如果处在微信中
                    if (isweixin) {
                        if (arr[0] != "weixin" || arr[1] == "h5") paypis.splice(i, 1);
                        if (ismini && arr[1] != "mini") paypis.splice(i, 1);
                        if (!ismini && arr[1] == "mini") paypis.splice(i, 1);
                    } else {
                        //如果不在微信中，且该接口仅限微信使用，则不显示
                        if (arr[0] == "weixin" && arr[1] != "native") paypis.splice(i, 1);
                    }
                }             
                return paypis;
            },
            //开始进入支付
            payEntry: function (formName) {
                this.$refs[formName].validate((valid) => {
                    if (valid) {
                        //转向支付页面
                        var url = '/pay/PayEntry';
                        //校验码
                        var vcode = new Date().getTime();
                        $api.storage('weishakeji_pay_vcode', vcode);
                        var md5 = $api.md5(this.form.money + '_' + this.form.payid + '_' + vcode);
                        //console.log(md5);
                        url = $api.url.set(url, {
                            'money': this.form.money,
                            'paiid': this.form.payid,
                            'code': md5,
                            'random': Math.random()
                        });
                        console.log(url);
                        this.paypanel = true;
                        this.payurl = url;

                    } else {
                        console.log('error submit!!');
                        return false;
                    }
                });
                //window.location.href = url;
            },
            //打开支付窗口
            open_paywindow: function () {
                if (!window.top || !window.top.vapp) return;
                this.isopenpay=true;
                var url = this.payurl;              
                var obj = {
                    'url': url,
                    'ico': 'e62d', 'min': false,
                    'title': '在线充值',
                    'width': '400px',
                    'height': '500px'
                }
                window.top.vapp.open(obj,function(sender,event){
                    //alert(3);
                    console.log(sender);
                    window.top.vapp.shut(window.name);
                });
            },
            fresh_window:function(){
                window.location.reload();
            }
        }
    });

});
