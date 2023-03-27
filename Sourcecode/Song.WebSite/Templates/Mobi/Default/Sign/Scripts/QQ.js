$ready(function () {

    window.vapp = new Vue({
        el: '#vapp',
        data: {
            //1为绑定，2为登录
            type: $api.querystring('type'),

            outeruser: {},      //外部用户（第三方登录的用户）      
            binduser: {},        //外部用户绑定的账号   

            openid: '',
            tag: '',         //第三方登录的配置项标识
            loading: true
        },
        mounted: function () {

        },
        created: function () {
        },
        computed: {
            //是否是手机端
            ismobi: function () {
                return $api.ismobi();
            },
            //当前openid是否已经绑定到当前登录账户
            'bound': function () {
                return JSON.stringify(this.binduser) != '{}' && this.binduser != null;
            },
        },
        watch: {
        },
        methods: {
            //加载第三方账号成功
            loaduser: function (user) {
                this.outeruser = user;
                //type:1为登录，2为绑定
                if (this.type == 2 || this.type == '2') {
                    for (k in user) user[k] = encodeURIComponent(user[k]);
                    var url = $api.url.set('/student/OtherLogin/qq', user);
                    window.location.href = url;
                    return;
                }
                this.openid = user.openid;
                this.tag = user.tag;
                this.getuser();

            },
            //获取登录账号与已经绑定的账号
            getuser: function () {
                var th = this;
                th.loading = true;
                $api.get('Account/UserLogin', { 'openid': th.openid, 'type': th.tag }).then(function (req) {
                    if (req.data.success) {
                        th.binduser = req.data.result;
                        th.$refs['login'].success(th.binduser, '手机端', 'QQ登录', '');
                        //如果已经绑定
                        var singin_referrer = $api.storage('singin_referrer');
                        if (singin_referrer != '') window.location.href = singin_referrer;
                        else
                            window.location.href = '/mobi/';
                    } else {
                        throw req.data.message;
                    }
                }).catch(function (err) {                    
                    console.error(err);
                }).finally(() => th.loading = false);

            },
        }
    });

}, ['/Utilities/OtherLogin/qq.js',
    '/Utilities/Components/Sign/Login.js']);
