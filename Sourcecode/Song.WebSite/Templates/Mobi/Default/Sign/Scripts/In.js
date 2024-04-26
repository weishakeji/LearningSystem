$ready(function () {
    window.vapp = new Vue({
        el: '#vapp',
        data: {
            account: {},
            organ: {},
            config: {},
            plate: {},
         

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
                th.loading_init = false;
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
            successful: function (account) {
                this.account = account;
                var th = this;
                window.setTimeout(function () {
                    let referrer = $api.querystring('referrer');
                    if ($api.isnull(referrer)) referrer = $api.storage('singin_referrer');
                    if ($api.isnull(referrer) || referrer == 'undefined') referrer = '/mobi';
                    window.navigateTo(decodeURIComponent(referrer));
                }, 200);
            },
            //退出登录
            logout: function () {
                this.$dialog.confirm({
                    message: '是否确定退出登录？',
                }).then(function () {
                    $api.login.out('account', function () {
                        window.setTimeout(function () {
                            window.navigateTo('/mobi/');
                        }, 500);
                    });
                    this.account = {};
                }).catch(function () { });
            }
        }
    });

}, ['/Utilities/Components/avatar.js',
    '/Utilities/OtherLogin/config.js',      //第三方登录的配置项
    '/Utilities/Components/Sign/Login.js'
]);
