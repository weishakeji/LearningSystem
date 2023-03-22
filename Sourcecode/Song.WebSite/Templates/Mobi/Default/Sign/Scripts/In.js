$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            account: {},
            organ: {},
            config: {},
            plate: {},
            //来源页
            referrer: decodeURIComponent($api.querystring('referrer')),

            loading: false
        },
        mounted: function () {
            var th = this;
            //平台信息
            $api.bat(
                $api.get('Account/Current'),
                $api.cache('Platform/PlatInfo:60'),
                $api.get('Organization/Current')
            ).then(axios.spread(function (acc, platinfo, organ) {
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
                th.account = acc.data.result;
                th.platinfo = platinfo.data.result;
                th.organ = organ.data.result;
                //机构配置信息
                th.config = $api.organ(th.organ).config;
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
            }
        },
        watch: {
            'account': function (nv, ov) {

            },
            'config': function (nv, ov) {

            }
        },
        methods: {
            //已经登录
            logged: function (account) {
                this.account = account;
                var th = this;
                window.setTimeout(function () {
                    if (th.referrer != '') window.location.href = th.referrer;
                }, 200);
            },
            //退出登录
            logout: function () {
                this.$dialog.confirm({
                    message: '是否确定退出登录？',
                }).then(function () {
                    $api.loginstatus('account', '');
                    this.account = {};
                    window.setTimeout(function () {
                        window.location.href = '/mobi/';
                    }, 500);
                }).catch(function () { });
            }
        }
    });

}, ['/Utilities/Components/avatar.js',
    '/Utilities/OtherLogin/config.js',      //第三方登录的配置项
    '/Utilities/Components/Sign/Login.js'
]);
